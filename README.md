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

### Example
<div align="center">
  <img width="382" height="582" alt="image" src="https://github.com/user-attachments/assets/a6102125-c0d1-4439-8d26-0974937286dc" />
  <img width="382" height="582" alt="image" src="https://github.com/user-attachments/assets/60ad43a0-8ebe-4dbc-b2eb-520f2dca9ed7" />
</div>

## Audio Playback Example
```csharp

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
