using UnityEngine;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    float timer;
    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        if (timer >= 0.2f)
        {
            SoundManager.instance.PlayAudioClip(ESoundeffects.Button, GetComponent<AudioSource>());
            timer = 0;
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        if (timer >= 0.2f)
        {
            SoundManager.instance.PlayAudioClip(ESoundeffects.Button, GetComponent<AudioSource>());
            timer = 0;
        }

    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        if (timer >= 0.2f)
        {
            SoundManager.instance.PlayAudioClip(ESoundeffects.Button, GetComponent<AudioSource>());
            timer = 0;
        }
    }
}