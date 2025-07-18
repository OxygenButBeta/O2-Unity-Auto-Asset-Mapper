using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using Object = UnityEngine.Object;

public static class MapGeneratorPostCompileHandler {
    public const string ADDRESSABLE_LABEL_KEY = "MapperLabel";
    public const string MAP_KEY = "PendingMapType";
    public const string PATH_KEY = "Path";
    public const string PENDING_KEYS = "PendingKeys";
    public const string PENDING_ASSET_PATHS = "PendingAssetPaths";

    [InitializeOnLoad]
    public static class PostCompileHandler {
        static PostCompileHandler() {
            EditorApplication.delayCall += TryCreateAssetAfterCompile;
        }

        static void TryCreateAssetAfterCompile() {
            if (!EditorPrefs.HasKey(PENDING_KEYS))
                return;


            var mapTypeName = EditorPrefs.GetString(MAP_KEY);
            EditorPrefs.DeleteKey(MAP_KEY);


            Type type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == mapTypeName && typeof(ScriptableObject).IsAssignableFrom(t));

            if (type == null) {
                Debug.LogWarning($"Type {mapTypeName} not found after compile.");
                return;
            }

            ScriptableObject instance = ScriptableObject.CreateInstance(type);
            var sourcePath = EditorPrefs.GetString(PATH_KEY);

            var assetPath = $"{sourcePath}/{mapTypeName}.asset";

            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();

            var keys = EditorPrefs.GetString(PENDING_KEYS).Split(',');
            var assetPaths = EditorPrefs.GetString(PENDING_ASSET_PATHS).Split(',');

            MethodInfo method = type.GetMethod("Set");
            if (method != null) {
                Type assetType = type.BaseType!.GetGenericArguments()[0]; // TAsset
                Type keyType = type.BaseType.GetGenericArguments()[1]; // TKey (enum)

                for (var i = 0; i < keys.Length; i++) {
                    Object assetObj = AssetDatabase.LoadAssetAtPath(assetPaths[i], assetType);
                    var keyEnumValue = Enum.Parse(keyType, keys[i]);
                    method.Invoke(instance, new[] { assetObj, keyEnumValue });
                }
            }


            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            AddressableAssetEntry entry = settings.FindAssetEntry(guid) ??
                                          settings.CreateOrMoveEntry(guid, settings.DefaultGroup);

            var mapperLabel = EditorPrefs.GetString(ADDRESSABLE_LABEL_KEY);
            entry.address = Path.GetFileNameWithoutExtension(assetPath);
            if (!settings.GetLabels().Contains(mapperLabel))
                settings.AddLabel(mapperLabel);

            entry.SetLabel(mapperLabel, true);

            EditorUtility.SetDirty(settings);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true);
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}