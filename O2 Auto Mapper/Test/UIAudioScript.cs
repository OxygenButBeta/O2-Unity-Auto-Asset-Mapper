using UnityEngine;

public class UIAudioScript : MonoBehaviour {
    AudioSource audioSource;
    [SerializeField] VfxSoundAsset vfxSoundAsset;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio() {
        audioSource.clip = vfxSoundAsset;
        audioSource.Play();
    }
}