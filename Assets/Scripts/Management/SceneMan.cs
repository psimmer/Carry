using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMan : MonoBehaviour
{

    public void StartGame()
    {
        Time.timeScale = 1f;
        GlobalData.instance.ResetStatistics();
        SceneManager.LoadScene("Level 1");
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        GlobalData.instance.ResetStatistics();
        //GlobalData.instance.isSaveFileLoaded = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void StartTutorial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tutorial");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

 
}
