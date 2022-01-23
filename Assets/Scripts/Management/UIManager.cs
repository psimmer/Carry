using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button firstButton;

    GameObject pauseElements;
    GameObject optionsElements;
    GameObject mainMenuElements;


    public void Awake()
    {
        pauseElements = GameObject.Find("PauseMenu");
        if (pauseElements != null)
        {
            pauseElements.SetActive(false);
        }

        optionsElements = GameObject.Find("OptionsMenu");
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

    /// <summary>
    /// pauses the game and sets the Pause UI active
    /// </summary>
    public void GamePaused()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
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
        }
    }

    //i think following two methods can be optimized. iam tired, i will look over it another time
    public void EnterOptions()
    {
        if(SceneManager.GetActiveScene().name == "Level 1")
        {
            pauseElements.SetActive(false);
            optionsElements.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuElements.SetActive(false);
            if(optionsElements != null)
                optionsElements.SetActive(true);
        }
    }

    public void LeaveOptions()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
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
}
