using System.IO;
using UnityEditor;

public static partial class MapGenerator {
    public static void CreateMap(string mapName, string assetTypeName, string[] keys,
        string[] assetPaths, string path) {
        var sourceDir = path + $"/{mapName}Mapper Source/";
        if (!Directory.Exists(sourceDir))
            Directory.CreateDirectory(sourceDir);

        var addressableLabel = "Mapper_" + mapName;

        var key = GetMapperKeySource(mapName, keys);
        File.WriteAllText(sourceDir + mapName + ".Keys.g.cs", key);

        var assetSource = GetAssetSource(mapName, assetTypeName);
        File.WriteAllText(sourceDir + mapName + ".Asset.g.cs", assetSource);


        var mapperSource = GetMapperSource(mapName, assetTypeName, addressableLabel);
        File.WriteAllText(sourceDir + mapName + ".g.cs", mapperSource);

        EditorPrefs.SetString(MapGeneratorPostCompileHandler.MAP_KEY, mapName + "Mapper");
        EditorPrefs.SetString(MapGeneratorPostCompileHandler.PENDING_KEYS, string.Join(",", keys));
        EditorPrefs.SetString(MapGeneratorPostCompileHandler.PENDING_ASSET_PATHS, string.Join(",", assetPaths));
        EditorPrefs.SetString(MapGeneratorPostCompileHandler.PATH_KEY, path);
        EditorPrefs.SetString(MapGeneratorPostCompileHandler.ADDRESSABLE_LABEL_KEY, addressableLabel);
        AssetDatabase.Refresh();
    }
}