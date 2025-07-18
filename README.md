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
  <img width="389" height="348" src="https://github.com/user-attachments/assets/2e143fce-e5f3-4915-99a3-7068f318c001" alt="Mapper Editor Window" style="margin-right: 10px;" />
  <img width="221" height="159" src="https://github.com/user-attachments/assets/2987fe8a-4a5c-4aa0-bc05-13c8d94b9b39" alt="Generated Files" style="margin-right: 10px;" />
  <img width="372" height="95" src="https://github.com/user-attachments/assets/1335e2de-41da-4905-8d40-487ca3c07830" alt="Usage Example" />
</div>

```csharp
[SerializeField]
MySoundAsset sound; // Contains Enum keys only.

void Play() {
    audioSource.PlayOneShot(sound); // Implicit conversion to AudioClip
}
```
## 🛠️ Requirements

- Addressables package: `com.unity.addressables`
