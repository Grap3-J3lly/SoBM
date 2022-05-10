using System;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(0)]
public class AudioManager : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    public static AudioManager Instance;

    private static AudioSource audioSource;

    public Sound[] sounds;

    //------------------------------------------------------
    //                  GETTERS/SETTERS
    //------------------------------------------------------

    public AudioSource GetAudioSource() {
        return audioSource;
    }
    public void SetAudioSource(AudioSource newSource) {
        audioSource = newSource;
    }

    //------------------------------------------------------
    //                  STANDARD FUNCTIONS
    //------------------------------------------------------

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    //------------------------------------------------------
    //                  GENERAL FUNCTIONS
    //------------------------------------------------------

    public void Play(string name) {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        audioSource.clip = currentSound.clip;
        audioSource.volume = currentSound.volume;
        audioSource.pitch = currentSound.pitch;
        if(currentSound == null) {
            Debug.LogWarning("Audio not found");
            return;
        }
        audioSource.Play();
    }

}
