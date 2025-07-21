using System;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

public static class AssetMapperContextMenu {
    [MenuItem("Assets/Create Mapper", true)]
    static bool ValidateMenu() {
        Object[] selected = Selection.objects;
        if (selected.Length == 0) return false;

        if (!selected[0]) return false;

        Type firstType = selected[0].GetType();
        return selected.All(obj => obj.GetType() == firstType);
    }

    [MenuItem("Assets/Create Mapper")]
    static void OpenProcessorWindow() {
        Object[] selected = Selection.objects;
        Type type = selected[0].GetType();

        MapCreatorWindow.Open(selected, type);
    }
}