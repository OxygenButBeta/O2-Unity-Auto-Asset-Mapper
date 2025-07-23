using System;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

public static class AssetMapperContextMenu {
    [MenuItem("Assets/O2 Mapper/Create with Selected Items", true)]
    static bool ValidateMenu() {
        Object[] selected = Selection.objects;
        if (selected.Length == 0) return false;

        if (!selected[0]) return false;

        Type firstType = selected[0].GetType();
        return firstType != typeof(DefaultAsset) && selected.All(obj => obj.GetType() == firstType);
    }

    [MenuItem("Assets/O2 Mapper/Create with Selected Items")]
    static void OpenProcessorWindow() {
        Object[] selected = Selection.objects;
        Type type = selected[0].GetType();

        MapCreatorWindow.Open(selected, type);
    }
}