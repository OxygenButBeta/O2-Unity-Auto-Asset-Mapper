public static partial class MapGenerator {
    const string AssetBody = @"
    // O2 UNITY AUTO ASSET MAPPER
    // ==============================
    // Auto Generated Asset Wrapper Struct
    //
    // This struct wraps the {0} enum and provides
    // easy access to the corresponding {1} asset.
    //
    // Features:
    // - Implicit conversion to AudioClip for convenience.
    // - Caches the loaded asset for runtime efficiency.
    // - Uses dynamic loading in the Editor for live updates.
    //
    // WARNING:
    // - This code is auto-generated; do NOT modify manually.
    //
    // ==============================

    [System.Serializable]
    public struct {0}Asset {{
        public {0}AssetKey Key;
        {1} cached;

        public {1} Value {{
            get {{
        #if UNITY_EDITOR
                return GetAssetDynamic();
        #else
                return GetAssetAndCache();
        #endif
            }}
        }}

        public static implicit operator {1}({0}Asset asset) {{
            return asset.Value; 
        }}

        {1} GetAssetDynamic() {{
            return {0}Mapper.Instance.GetByKey(Key);
        }}

        {1} GetAssetAndCache() {{
            if (!cached)
                cached = {0}Mapper.Instance.GetByKey(Key);

            return cached;
        }}
    }}
    ";

    static string GetAssetSource(string mapName, string typeFullName) {
        return string.Format(AssetBody, mapName, typeFullName);
    }
}