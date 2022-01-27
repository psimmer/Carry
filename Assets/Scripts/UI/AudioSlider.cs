using UnityEngine;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }
}