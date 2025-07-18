using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapCreatorWindow : EditorWindow {
    Object[] selectedAssets;
    Type assetType;
    string mapName = "MyMap";
    readonly List<string> keys = new();
    readonly List<string> extraKeys = new();
    Vector2 scrollPos;

    public static void Open(Object[] selected, Type type) {
        MapCreatorWindow window = GetWindow<MapCreatorWindow>("Asset Processor");
        window.selectedAssets = selected;
        window.assetType = type;
        window.Show();
    }

    void OnEnable() => UpdateKeysList();

    void UpdateKeysList() {
        if (selectedAssets == null)
            return;

        while (keys.Count < selectedAssets.Length) {
            keys.Add(selectedAssets[keys.Count].name.Replace(" ", "_"));
        }

        while (keys.Count > selectedAssets.Length) {
            keys.RemoveAt(keys.Count - 1);
        }
    }

    void OnGUI() {
        EditorGUILayout.LabelField("Selected Asset Type:", assetType.Name, EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Count:", selectedAssets.Length.ToString());
        EditorGUILayout.Space();

        mapName = EditorGUILayout.TextField("Mapper Type Name:", mapName);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Keys for each asset:");

        UpdateKeysList();
        EditorGUILayout.Space();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));
        for (var i = 0; i < selectedAssets.Length; i++) {
            EditorGUILayout.Space();
            Object asset = selectedAssets[i];
            EditorGUILayout.ObjectField(asset.name, asset, assetType, false);
            keys[i] = EditorGUILayout.TextField("Key :", keys[i]);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Extra Keys (independent of assets):");
        for (var i = 0; i < extraKeys.Count; i++) {
            extraKeys[i] = EditorGUILayout.TextField("Extra Key:", extraKeys[i]);
        }

        if (GUILayout.Button("Add Extra Key"))
            extraKeys.Add("NewExtraKey");


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Selected Items:");

        if (!GUILayout.Button("Generate Mapper"))
            return;

        var allKeys = keys.Concat(extraKeys).ToArray();
        var assetPaths = selectedAssets.Select(AssetDatabase.GetAssetPath).ToArray();

        if (!MapValidator.ValidateMapInputs(mapName, allKeys, out var error)) {
            EditorUtility.DisplayDialog("Invalid Input", error, "OK");
            return;
        }

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