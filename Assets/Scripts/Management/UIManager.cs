using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button firstButton;
    [SerializeField] TMP_Text patientsHealed;
    [SerializeField] TMP_Text patientsLost;
    [SerializeField] TMP_Text treatments;

    [SerializeField] Slider stressLvlBar;
    [SerializeField] GameObject pauseElements;
    [SerializeField] GameObject optionsElements;
    GameObject mainMenuElements;


    public void Awake()
    {
        if (pauseElements != null)
        {
            pauseElements.SetActive(false);
        }

        if (optionsElements != null)
        {
            optionsElements.SetActive(false);
        }

        mainMenuElements = GameObject.Find("MainMenu");
        if (mainMenuElements != null)
        {
            mainMenuElements.SetActive(true);
        }

        if (firstButton != null)
        {
            firstButton.Select();
        }
    }

    private void Update()
    {
        //Game will be paused
        GamePaused();
    }

    #region Show Statistics after Complete Level/GameOver
    public void ShowStatsCompleteShift()
    {
        SoundManager.instance.PlayAudioClip(ESoundeffects.Winning, GetComponent<AudioSource>());
        patientsHealed.text = "Patients Healed: " + GlobalData.instance.ShiftPatientsHealed;
        patientsLost.text = "Patients Lost: " + GlobalData.instance.ShiftPatientsLost;
        treatments.text = "Treatments: " + GlobalData.instance.ShiftTreatments;
    }

    public void ShowStatsGameOver()
    {
        SoundManager.instance.PlayAudioClip(ESoundeffects.Losing, GetComponent<AudioSource>());
        patientsHealed.text = "Patients Healed: " + GlobalData.instance.TotalPatientsHealed;
        patientsLost.text = "Patients Lost: " + GlobalData.instance.TotalPatientsLost;
        treatments.text = "Treatments: " + GlobalData.instance.TotalTreatments;
    }
    #endregion

    #region Pausing the Game
    /// <summary>
    /// pauses the game and sets the Pause UI active
    /// </summary>
    public void GamePaused()
    {
        
        if (Input.GetKeyUp(KeyCode.Escape) && !optionsElements.activeSelf && pauseElements!= null)
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0f;
                pauseElements.SetActive(true);
                //TODO: dimm light
                //TODO: Pause AUdio
                //TODO: Pause Camera
            }
            else
            {
                pauseElements.SetActive(false);
                Time.timeScale = 1f;

                //TODO: dimm light
                //TODO: play audio
                //TODO: Play Camera

            }
        } else if(Input.GetKeyUp(KeyCode.Escape) && optionsElements.activeSelf)
        {
            if(pauseElements != null)
                pauseElements.SetActive(true);

            if (mainMenuElements != null)
                mainMenuElements.SetActive(true);

            optionsElements.SetActive(false);

        }
    }

    public void Continue()
    {
        pauseElements.SetActive(false);
        Time.timeScale = 1f;
        //TODO: dimm light
        //TODO: play audio
        //TODO: Play Camera
    }
    #endregion

    #region StressLvlBar and CoffeeCounter


    public void UpdateStressLvlBar(float percent)
    {
        GetComponent<AudioSource>().volume = percent;
        SoundManager.instance.PlayAudioClip(ESoundeffects.StressLevel, GetComponent<AudioSource>());
        stressLvlBar.value = percent;
    }
    #endregion
    
    #region Activate/Deactive Options
    //i think following two methods can be optimized. iam tired, i will look over it another time
    /// <summary>
    /// Sets the right UI elements active/inactive
    /// </summary>
    public void EnterOptions()
    {
        if (SceneManager.GetActiveScene().name == "Level 1" || SceneManager.GetActiveScene().name == "Level 2" ||
            SceneManager.GetActiveScene().name == "Level 3" || SceneManager.GetActiveScene().name == "Level 4")
        {
            pauseElements.SetActive(false);
            optionsElements.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuElements.SetActive(false);
            if (optionsElements != null)
                optionsElements.SetActive(true);
        }
    }

    /// <summary>
    /// Sets the right UI elements active/inactive
    /// </summary>
    public void LeaveOptions()
    {
        if (SceneManager.GetActiveScene().name == "Level 1" || SceneManager.GetActiveScene().name == "Level 2" ||
            SceneManager.GetActiveScene().name == "Level 3" || SceneManager.GetActiveScene().name == "Level 4")
        {
            pauseElements.SetActive(true);
            optionsElements.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuElements.SetActive(true);
            optionsElements.SetActive(false);
        }
    }
    #endregion


}