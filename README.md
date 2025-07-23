# 🧩 O2 Unity Auto Asset Mapper

**O2 Unity Auto Asset Mapper** is a Unity tool that automates the process of mapping assets (like AudioClips, Sprites, Prefabs, etc.) to `enum` keys using `ScriptableObject` and `Addressables`. It generates all necessary code and assets so you can reference and load them in a clean, type-safe, and scalable way.

> 🔧 Access assets via enums, cache at runtime, load dynamically in editor — all generated for you.

---

## ✨ Features

- ✅ **Auto-generated enum keys** from selected assets
- ✅ **ScriptableObject-based asset mapping**
- ✅ **Enum-to-asset implicit conversion support**
- ✅ **Addressables label integration** (load by label)
- ✅ **Editor window UI** to create mappers easily
- ✅ **Runtime caching**, **Editor dynamic loading**
- ✅ Prevents common serialization issues via auto-generated boilerplate
- ✅ Easy to extend and integrate into existing Unity projects

---

## 🧠 How It Works

1. Select assets in the Project window.
2. Open the **Map Creator** from the context menu or custom editor.
3. Define the enum keys (automatically filled based on asset names).
4. Generate the mapper, asset wrapper, and key enum via a single button.
5. Use the generated struct (e.g. `MyAudioAsset`) directly in your code.

## Quick Start

### Create Mapper by Selecting a Type

1. Right-click in the **Project Explorer** window.
2. Navigate to:  
   `Assets > O2 Mapper > Create Mapper For a Type`
3. In the window that appears, search and select the Unity asset type you want to create a mapper for.
4. Click **Create Mapper** to open the mapper creation window.

### Create Mapper Directly from Selected Assets

1. Select one or multiple assets in the **Project Explorer** (e.g., Materials, GameObjects, Textures).
2. Right-click and choose:  
   `Assets > O2 Mapper > Create with Selected Items`
3. The mapper creation window opens automatically with the asset type detected.
4. Keys are assigned automatically based on the selected assets.

---

## Mapper Creation Process

- Assets are automatically assigned unique keys.
- Provide a name for the mapper (default is the selected asset type’s name).
- Click **Generate** to create the mapper script.
- After compilation, a ScriptableObject asset is generated and populated with the mapped assets.

---

## Screenshots

*(Add your screenshots here with captions)*

1. **Type Selection Window**  
2. **Mapper Settings and Generation**  
3. **Generated ScriptableObject Asset**

---

## Usage Notes

- Use the type selector to find the exact Unity asset type quickly.
- The mapper allows adding extra keys independent from assets for future extension.
- Supports generating mappers for any UnityEngine.Object derived asset.

---

## Audio Playback Example

```csharp
[SerializeField] VfxSoundAsset vfxSoundAsset;

public void PlayAudio() {
    // Play audio directly from the assigned asset
    audioSource.PlayOneShot(vfxSoundAsset);

    // Or get clip by index
    AudioClip clip = VfxSoundMapper.GetWithIndex(4);

    // Or get clip by enum key
    AudioClip clip2 = VfxSoundMapper.GetWithKey(VfxSoundAssetKey.CarHit

```
## 🛠️ Requirements

- Addressables package: `com.unity.addressables`
