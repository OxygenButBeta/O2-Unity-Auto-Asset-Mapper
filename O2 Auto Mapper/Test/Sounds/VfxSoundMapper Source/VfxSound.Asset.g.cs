
    // O2 UNITY AUTO ASSET MAPPER
    // ==============================
    // Auto Generated Asset Wrapper Struct
    //
    // This struct wraps the VfxSound enum and provides
    // easy access to the corresponding UnityEngine.AudioClip asset.
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
    public struct VfxSoundAsset {
        public VfxSoundAssetKey Key;
        UnityEngine.AudioClip cached;

        public UnityEngine.AudioClip Value {
            get {
        #if UNITY_EDITOR
                return GetAssetDynamic();
        #else
                return GetAssetAndCache();
        #endif
            }
        }

        public static implicit operator UnityEngine.AudioClip(VfxSoundAsset asset) {
            return asset.Value; 
        }

        UnityEngine.AudioClip GetAssetDynamic() {
            return VfxSoundMapper.Instance.GetByKey(Key);
        }

        UnityEngine.AudioClip GetAssetAndCache() {
            if (!cached)
                cached = VfxSoundMapper.Instance.GetByKey(Key);

            return cached;
        }
    }
    