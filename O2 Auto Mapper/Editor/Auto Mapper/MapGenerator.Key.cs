
using System;

public static partial class MapGenerator {
    const string KeyBody = @"
    // O2 UNITY AUTO ASSET MAPPER
    // ==============================
    // ! Auto Generated Mapper Key Enum
    //    - Remove existing members (will break serialized data).
    //    - Reorder existing members (index-based mapping may corrupt).
    // ------------------------------
    // Generated 1.0 {3}
    public enum {0}AssetKey {2} {{
        {1}
     }}
    ";

    static string GetMapperKeySource(string mapName, string[] keys) {
        var keyValues = string.Join(",", keys);
        return string.Format(KeyBody, mapName, keyValues, GetEnumUnderlyingType(keys), DateTime.Now);
    }

    static string GetEnumUnderlyingType(string[] keys) {
        if (keys == null || keys.Length == 0)
            return "";

        return keys.Length <= 256 ? ": byte" : ": int";
    }
}
