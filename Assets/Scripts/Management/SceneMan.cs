using System.Collections;
using System.Collections.Generic;
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
        //SaveSystem.SavePlayer(FindObjectOfType<playerScript>(), GlobalData.instance.timeLeft);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSaveFile()
    {
        //PlayerData data = SaveSystem.LoadPlayer();
        //if (data != null)
        //{
        //    GlobalData.instance.isSaveFileLoaded = true;

        //    GlobalData.instance.currentStresslvl = data.currentStressLvl;
        //    GlobalData.instance.currentItem = data.currentItem;

        //    Vector3 position;
        //    position.x = data.position[0];
        //    position.y = data.position[1];
        //    position.z = data.position[2];

        //    GlobalData.instance.currentPlayerPosition = position;
        //    SceneManager.LoadScene("Main Scene");
        //}
        //else
        //{
        //    SceneManager.LoadScene("Main Scene");
        //}
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
