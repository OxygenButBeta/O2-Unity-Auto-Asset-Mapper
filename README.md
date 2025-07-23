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

## Usage

### Create Mapper by Selecting a Type

1. Right-click in the **Project Explorer** window.
2. Navigate to:  
   `Assets > O2 Mapper > Create Mapper For a Type`
3. A window will open displaying all Unity asset types.  
   Use the search box to filter and select your desired type.

   <div align="center">
   <img src="https://github.com/user-attachments/assets/7d59298c-8c41-437f-a4df-598debdee9b7" alt="Type Selection Window" width="300" />
   </div>

4. After selection, proceed to the mapper creation window where you assign keys and configure the mapper.

   <div align="center">
   <img src="https://github.com/user-attachments/assets/a6102125-c0d1-4439-8d26-0974937286dc" alt="Mapper Settings and Generation" width="300" />
   </div>

5. Click **Generate** to create the mapper script.  
   Once compilation finishes, a ScriptableObject asset is generated and populated automatically.

   <div align="center">
   <img src="https://github.com/user-attachments/assets/60ad43a0-8ebe-4dbc-b2eb-520f2dca9ed7" alt="Generated ScriptableObject Asset" width="300" />
   </div>

---

### Create Mapper Directly from Selected Assets

1. Select one or multiple assets (e.g., Material, GameObject) in **Project Explorer**.
2. Right-click and choose:  
   `Assets > O2 Mapper > Create with Selected Items`
3. The mapper creation window opens immediately for the detected asset type, with automatic keys assigned.

---

This workflow makes it quick and easy to generate type-safe mappers for any asset type in your project.

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
