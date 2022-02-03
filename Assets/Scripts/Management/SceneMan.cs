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
        SceneManager.LoadScene("Level 1");
    }

    public void SaveAndQuit()
    {
        //searches all scripts after the interface and executes the method (so everything gets saved)
        //foreach(var saveMethod in FindObjectsOfType<MonoBehaviour>().OfType<ISaveSystem>())
        //{
        //    saveMethod.SaveData();
        //}
        //SceneManager.LoadScene("MainMenu");
    }

    public void LoadSaveFile()
    {
        //searches all scripts after the interface and executes the method (so everything gets saved)
        //foreach (var loadMethod in FindObjectsOfType<MonoBehaviour>().OfType<ISaveSystem>())
        //{
        //    loadMethod.LoadData();
        //}
        //Load saved scene
    }

    public void ContinueNextLvl()
    {
        GlobalData.instance.ResetShiftStatistics();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        GlobalData.instance.ResetTotalStatistics();
        //GlobalData.instance.isSaveFileLoaded = false;
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
