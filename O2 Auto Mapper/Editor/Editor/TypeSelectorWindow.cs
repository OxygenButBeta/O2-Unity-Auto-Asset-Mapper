using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TypeSelectorWindow : EditorWindow {
    List<Type> allTypes;
    List<Type> filteredTypes;
    string[] filteredNames;
    string searchTerm = "";
    int selectedIndex = -1;
    Vector2 scrollPos;

    [MenuItem("Assets/O2 Mapper/Create Mapper For a Type")]
    public static void ShowWindow() {
        TypeSelectorWindow window = GetWindow<TypeSelectorWindow>("Select Type");
        window.minSize = new Vector2(500, 400);
        window.PopulateTypes();
        window.Show();
    }

    void PopulateTypes() {
        allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(UnityEngine.Object).IsAssignableFrom(t) &&
                !t.IsAbstract &&
                !t.IsInterface &&
                !t.IsGenericType &&
                t.IsPublic &&
                !typeof(Editor).IsAssignableFrom(t) &&
                !typeof(EditorWindow).IsAssignableFrom(t)
            )
            .OrderBy(t => t.FullName)
            .ToList();

        ApplySearchFilter();
    }

    void ApplySearchFilter() {
        if (string.IsNullOrWhiteSpace(searchTerm))
            filteredTypes = allTypes;
        else
            filteredTypes = allTypes
                .Where(t => t.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

        filteredNames = filteredTypes.Select(t => t.FullName).ToArray();

        if (filteredTypes.Count > 0)
            selectedIndex = 0;
        else
            selectedIndex = -1;
    }

    void OnGUI() {
        if (filteredTypes is null) {
            Close();
            ShowWindow();
            return;
        }

        GUILayout.Space(15);
        EditorGUILayout.LabelField("🔧 O2 Unity Auto Mapper", EditorStyles.largeLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("🧠 Select A Unity Object Type for Mapping", EditorStyles.largeLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.HelpBox(
            "Select the type of asset you want to generate a mapper for. You can filter by typing below.",
            MessageType.Info
        );

        GUILayout.Space(10);
        EditorGUILayout.LabelField("🔍 Search", EditorStyles.boldLabel);
        var newSearchTerm = EditorGUILayout.TextField(searchTerm);

        if (newSearchTerm != searchTerm) {
            searchTerm = newSearchTerm;
            ApplySearchFilter();
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("📚 Matching Types", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
        GUIStyle style = new(EditorStyles.miniButton) {
            normal = {
                textColor = new Color(0.8f, 0.8f, 0.8f)
            }
        };
        for (var i = 0; i < filteredNames.Length; i++) {
            var isSelected = i == selectedIndex;

            Color bgColor = isSelected ? new Color(0.35f, 1f, 0.19f, 0.6f) : Color.white;
            GUI.backgroundColor = bgColor;
            var fullName = filteredNames[i];
            var shortName = fullName.Contains('.') ? fullName[(fullName.LastIndexOf('.') + 1)..] : fullName;
            if (GUILayout.Button(shortName, style)) {
                selectedIndex = i;
            }

            GUI.backgroundColor = Color.white;
        }

        EditorGUILayout.EndScrollView();
        DrawSelectedTypeBox();

        GUILayout.Space(20);
        EditorGUILayout.LabelField("✅ Ready to Proceed", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUI.enabled = selectedIndex >= 0 && selectedIndex < filteredTypes!.Count;
        if (GUILayout.Button("🚀 Create Mapper", GUILayout.Width(220), GUILayout.Height(42))) {
            Type selectedType = filteredTypes![selectedIndex];
            MapCreatorWindow.Open(Enumerable.Empty<UnityEngine.Object>(), selectedType);
            Close();
        }

        GUI.enabled = true;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
    }

    void DrawSelectedTypeBox() {
        if (selectedIndex < 0 || selectedIndex >= filteredTypes.Count)
            return;

        Type selectedType = filteredTypes[selectedIndex];
        GUILayout.Space(8);

        Color defaultColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.8f, 0.9f, 1f, 0.3f);

        EditorGUILayout.BeginVertical("box");
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.IconContent("FilterSelectedOnly"), GUILayout.Width(20),
                GUILayout.Height(20));
            GUILayout.Label("Selected Type", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Label(selectedType.FullName, EditorStyles.wordWrappedLabel);
        }
        EditorGUILayout.EndVertical();

        GUI.backgroundColor = defaultColor;
    }
}