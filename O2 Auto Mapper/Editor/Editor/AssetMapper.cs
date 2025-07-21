using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapCreatorWindow : EditorWindow {
    List<Object> selectedAssets;
    Type assetType;
    string mapName;
    readonly List<string> keys = new();
    readonly List<string> extraKeys = new();
    Vector2 scrollPos;
    Vector2 extraScrollPos;
    Vector2 mainScroll;
    Object assetToAdd;

    public static void Open(Object[] selected, Type type) {
        MapCreatorWindow window = GetWindow<MapCreatorWindow>("O2 Unity Auto Mapper");
        window.selectedAssets = selected.ToList();
        window.assetType = type;
        window.mapName = type.Name;
        window.Show();
    }

    void OnEnable() => UpdateKeysList();

    void UpdateKeysList() {
        if (selectedAssets == null)
            return;

        while (keys.Count < selectedAssets.Count) {
            var rawName = selectedAssets[keys.Count].name;
            var camelCaseName = Regex.Replace(rawName, @"[_\s]+(.)", match => match.Groups[1].Value.ToUpper());
            keys.Add(camelCaseName);
        }

        while (keys.Count > selectedAssets.Count) {
            keys.RemoveAt(keys.Count - 1);
        }
    }

    void OnGUI() {
        if (assetType == null) {
            Close();
            return;
        }

        mainScroll = EditorGUILayout.BeginScrollView(mainScroll);
        GUILayout.Space(15);
        EditorGUILayout.LabelField("🔧 O2 Unity Auto Mapper", EditorStyles.largeLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.HelpBox(
            "Create automatic mapper scripts for selected assets. Assign unique keys for each asset and add extra keys if needed. Then generate the mapper class.",
            MessageType.Info
        );

        GUILayout.Space(10);

        EditorGUILayout.LabelField("📂 Selected Asset Type:", assetType.Name, EditorStyles.boldLabel);

        GUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("?", GUILayout.Width(20), GUILayout.Height(EditorGUIUtility.singleLineHeight))) {
            EditorUtility.DisplayDialog("Mapper Type Name",
                "The Mapper Type Name is the name of the generated mapper script.\nExample: AudioClipMapper, SpriteMapper, etc.",
                "OK");
        }

        EditorGUILayout.LabelField("📦 Mapper Type Name:", GUILayout.Width(150));
        mapName = EditorGUILayout.TextField(mapName, GUILayout.ExpandWidth(true));
        GUILayout.Label("Mapper", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("🗂 Keys for Each Asset", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField($"🔢 Count: {selectedAssets.Count}", EditorStyles.boldLabel, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        UpdateKeysList();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(220));
        for (int i = 0; i < selectedAssets.Count; i++) {
            GUILayout.Space(6);

            bool removed = false;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.ObjectField($"Asset ({assetType.Name})", selectedAssets[i], assetType, false);
            keys[i] = EditorGUILayout.TextField($"{mapName.Replace(" ", "")}Asset (Key)", keys[i]);
            EditorGUILayout.EndVertical();

            GUILayout.Space(8);
            GUI.backgroundColor = new Color(1f, 0.4f, 0.4f); // light red
            if (GUILayout.Button(new GUIContent("❌", "Remove this asset from the mapper"), GUILayout.Width(30),
                    GUILayout.Height(38))) {
                selectedAssets.RemoveAt(i);
                keys.RemoveAt(i);
                removed = true;
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (removed)
                break;
        }

        EditorGUILayout.BeginHorizontal();

        assetToAdd = EditorGUILayout.ObjectField("Add Asset:", assetToAdd, assetType, false);

        if (assetToAdd != null && GUILayout.Button("Add", GUILayout.Width(60))) {
            if (!selectedAssets.Contains(assetToAdd)) {
                selectedAssets.Add(assetToAdd);
                keys.Add(assetToAdd.name.Replace(" ", ""));
                assetToAdd = null;
            }
            else {
                EditorUtility.DisplayDialog("Duplicate Asset", "This asset is already added.", "OK");
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        GUILayout.Space(20);

        EditorGUILayout.LabelField("➕ Extra Keys (Independent of Assets)", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "Extra keys will be added to the generated enum, even though they are not assigned to any asset. " +
            "These are useful for placeholders, future extensions, or optional keys.\n" +
            "They are not required to be used immediately, and can remain unassigned.",
            MessageType.Info
        );

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginVertical("box");
        extraScrollPos = EditorGUILayout.BeginScrollView(extraScrollPos, GUILayout.Height(75));
        for (var i = 0; i < extraKeys.Count; i++) {
            extraKeys[i] = EditorGUILayout.TextField($"Extra Key {i + 1}:", extraKeys[i]);
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("➕ Add Extra Key", GUILayout.Height(30))) {
            extraKeys.Add("NewExtraKey");
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndVertical();

        GUILayout.Space(25);

        EditorGUILayout.LabelField("✅ Ready to Generate", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        var isValid = true;
        if (GUILayout.Button("🚀 Generate Mapper", GUILayout.Width(220), GUILayout.Height(42))) {
            var allKeys = keys.Concat(extraKeys).ToArray();
            var assetPaths = selectedAssets.Select(AssetDatabase.GetAssetPath).ToArray();

            if (!MapValidator.ValidateMapInputs(mapName, allKeys, out var error)) {
                EditorUtility.DisplayDialog("Invalid Input", error, "OK");
                isValid = false;
            }

            if (isValid) {
                MapGenerator.CreateMap(
                    mapName,
                    assetType.FullName,
                    allKeys,
                    assetPaths,
                    Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedAssets[0]))
                );

                Close();
            }
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        EditorGUILayout.EndScrollView();
    }
}