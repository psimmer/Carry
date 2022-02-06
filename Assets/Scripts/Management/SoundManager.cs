using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum ESoundeffects
{
    Damage,
    Heal,
    PickUpItem,
    ECG,
    FootSteps,
    Death,
    Losing,
    NewPatientArrived,
    PopUp,
    StressLevel,
    Winning,
    Button
}

public class SoundManager : MonoBehaviour
{
    public List<SoundFile> soundeffects;

    public static SoundManager instance;

    float timer;

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
    }

    public void PlayAudioClip(ESoundeffects soundType, AudioSource audioSource)
    {
        for (int i = 0; i < soundeffects.Count; i++)
        {
            if(soundeffects[i].TypeOfSound == soundType &&  IsSoundPlayable(soundeffects[i]))
            {
                audioSource.PlayOneShot(soundeffects[i].ClipSound);
                return;
            }
        }
    }

    bool IsSoundPlayable(SoundFile sound)
    {
        if(sound.SoundTimer <= Time.time)
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

    void SetTimer(SoundFile sound)
    {
        sound.SoundTimer = Time.time + sound.ClipSound.length;
    }
}

[System.Serializable]
public class SoundFile
{
    [SerializeField] ESoundeffects typeOfSound;
    public ESoundeffects TypeOfSound => typeOfSound;

    [SerializeField] AudioClip clipSound;
    public AudioClip ClipSound => clipSound;

    [SerializeField] float soundTimer;
    public float SoundTimer { get { return soundTimer; } set { soundTimer = value; } }

    [SerializeField] bool isStackable;
    public bool IsStackable { get { return isStackable; } }
}

