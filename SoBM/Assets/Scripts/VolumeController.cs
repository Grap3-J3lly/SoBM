using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    public enum MixerType {
        GameVolume,
        MusicVolume,
        SoundVolume
    }

    [SerializeField] private MixerType mixerType;
    public AudioMixer mixer;

    public void SetVolume(float sliderValue) {
        sliderValue = Mathf.Log10(sliderValue) * 20;
        switch(this.mixerType) {
            case MixerType.GameVolume:
                mixer.SetFloat("GameVolume", sliderValue);
                break;
            case MixerType.MusicVolume:
                mixer.SetFloat("MusicVolume", sliderValue);
                break;
            case MixerType.SoundVolume:
                mixer.SetFloat("SoundVolume", sliderValue);
                break;
        }
    }

}
