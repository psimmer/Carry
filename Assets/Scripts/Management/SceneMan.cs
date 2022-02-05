using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMan : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "GameOver")
            uiManager.ShowStatsGameOver();
        else if (SceneManager.GetActiveScene().name == "LevelComplete")
            uiManager.ShowStatsCompleteShift();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        GlobalData.instance.ResetTotalStatistics();
        GlobalData.instance.ResetShiftStatistics();
        GlobalData.instance.CurrentLevel = 1;
        SceneManager.LoadScene("Level 1");
    }

    public void SaveAndQuit()
    {
        //searches all scripts after the interface and executes the method (so everything gets saved)
        foreach(var saveMethod in FindObjectsOfType<MonoBehaviour>().OfType<ISaveSystem>())
        {
            saveMethod.SaveData();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSaveFile()
    {
        //searches all scripts after the interface and executes the method (so everything gets saved)
        GlobalData.instance.IsSaveFileLoaded = true;
        GlobalData.instance.LoadData();
        SceneManager.LoadScene("Level " + GlobalData.instance.CurrentLevel);
    }

    public void ContinueNextLvl()
    {
        GlobalData.instance.CurrentLevel++;
        GlobalData.instance.ResetShiftStatistics();
        SceneManager.LoadScene("Level " + GlobalData.instance.CurrentLevel);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        GlobalData.instance.ResetShiftStatistics();     //just for safety
        GlobalData.instance.ResetTotalStatistics();     //just for safety
        GlobalData.instance.IsSaveFileLoaded = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void StartTutorial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tutorial");
    }

    public void GameOver()
    {

        SceneManager.LoadScene("GameOver");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

 
}
