using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log("HELLLOO");
        Time.timeScale = 1f;
        SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.gameObject.GetComponent<AudioSource>());
        GlobalData.instance.ResetTotalStatistics();
        GlobalData.instance.ResetShiftStatistics();
        GlobalData.instance.CurrentLevel = 1;
        SceneManager.LoadScene("Level 1");
    }

    public void SaveAndQuit()
    {
        //searches all scripts after the interface and executes the method (so everything gets saved)
        foreach (var saveMethod in FindObjectsOfType<MonoBehaviour>().OfType<ISaveSystem>())
        {
            saveMethod.SaveData();
        }
        SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.StressLevelBar.gameObject.GetComponent<AudioSource>());
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSaveFile()
    {
        string path = Application.persistentDataPath + "/SaveDataTimer.carry";
        if (!File.Exists(path))
        {
            StartGame();
            return;
        }
        Time.timeScale = 1f;
        SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.gameObject.GetComponent<AudioSource>());
        GlobalData.instance.IsSaveFileLoaded = true;
        GlobalData.instance.LoadData();
        SceneManager.LoadScene("Level " + GlobalData.instance.CurrentLevel);
    }

    public void ContinueNextLvl()
    {
        SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.gameObject.GetComponent<AudioSource>());
        GlobalData.instance.CurrentLevel++;
        GlobalData.instance.ResetShiftStatistics();
        GlobalData.instance.IsSaveFileLoaded = false;
        SceneManager.LoadScene("Level " + GlobalData.instance.CurrentLevel);
    }

    public void TryAgain()
    {
        SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.gameObject.GetComponent<AudioSource>());
        GlobalData.instance.ResetShiftStatistics();
        GlobalData.instance.IsSaveFileLoaded = false;
        SceneManager.LoadScene("Level " + GlobalData.instance.CurrentLevel);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.StressLevelBar.gameObject.GetComponent<AudioSource>());
        GlobalData.instance.ResetShiftStatistics();     //just for safety
        GlobalData.instance.ResetTotalStatistics();     //just for safety
        GlobalData.instance.IsSaveFileLoaded = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitToMainMenuFromCredits()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartTutorial()
    {
        Time.timeScale = 1f;
        SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.gameObject.GetComponent<AudioSource>());
        SceneManager.LoadScene("Tutorial");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        if (SceneManager.GetActiveScene().name == "Level 1" || SceneManager.GetActiveScene().name == "Level 2" ||
            SceneManager.GetActiveScene().name == "Level 3" || SceneManager.GetActiveScene().name == "Level 4")
        {
            SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.StressLevelBar.gameObject.GetComponent<AudioSource>());
        }
        else
        {
            SoundManager.instance.PlayAudioClip(ESoundeffects.Button, uiManager.gameObject.GetComponent<AudioSource>());
        }
        Application.Quit();
    }


}
