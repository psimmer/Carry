using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<SoundFile> soundeffects;

    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null && instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);   
    }

    /// <summary>
    /// PlaysTheAudioClip
    /// </summary>
    /// <param name="soundType">The Enum Type of the Sound</param>
    /// <param name="audioSource">On this AudioSource the clip gets played</param>
    public void PlayAudioClip(ESoundeffects soundType, AudioSource audioSource)
    {
        for (int i = 0; i < soundeffects.Count; i++)
        {
            if(soundeffects[i].TypeOfSound == soundType &&  IsSoundPlayable(soundeffects[i], soundeffects[i].Offset))
            {
                audioSource.volume = soundeffects[i].SoundVolume;
                audioSource.PlayOneShot(soundeffects[i].ClipSound);
                return;
            }
        }
    }
    /// <summary>
    /// Checks if the Sound can get played
    /// </summary>
    /// <param name="sound">The SoundFile from the list soundeffects</param>
    /// <param name="offset">The time till the sound plays again</param>
    /// <returns></returns>
    bool IsSoundPlayable(SoundFile sound, float offset)
    {
        if(sound.SoundTimer - offset <= Time.time)
        {
            SetTimer(sound);
            return true;
        }
        else if (sound.IsStackable)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 
    /// <summary>
    /// Adding the time while the sound plays
    /// </summary>
    /// <param name="sound"></param>
    void SetTimer(SoundFile sound)
    {
        sound.SoundTimer = Time.time + sound.ClipSound.length;
    }
}

/// <summary>
/// EveryThing what a SoundFile have
/// </summary>
[System.Serializable]
public class SoundFile
{
    [SerializeField] string name;
    [SerializeField] ESoundeffects typeOfSound;
    public ESoundeffects TypeOfSound => typeOfSound;

    [SerializeField] AudioClip clipSound;
    public AudioClip ClipSound => clipSound;

    [SerializeField] float offset;
    public float Offset { get { return offset; } }

    [SerializeField] float soundTimer;
    public float SoundTimer { get { return soundTimer; } set { soundTimer = value; } }

    [SerializeField] float soundVolume;
    public float SoundVolume { get { return soundVolume; } }

    [SerializeField] bool isStackable;
    public bool IsStackable { get { return isStackable; } }

}

