using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Timer : MonoBehaviour , ISaveSystem
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private int startTimeHours;
    public int TimeInHours => startTimeHours;


    [Tooltip("This is the hour when the Game ends + 59 minutes")]
    [SerializeField] private int endTimeHours;
    public int EndTimeHours => endTimeHours;

    private string dayOrNight;
    private float realTime;

    private void Awake()
    {
        if (GlobalData.instance.IsSaveFileLoaded)
        {
            LoadData();
        }
    }
    private void Update()
    {
        
        //This would be the condition to end the scene with a success
        if (startTimeHours == endTimeHours && (int)realTime == 59)
        {
            SceneManager.LoadScene("LevelComplete");    //should be in the GameManager
        }

    }

    /// <summary>
    /// Throws out the realtime * 2
    /// </summary>
    public void DoubledRealTime()
    {
        realTime += Time.deltaTime * 2;

        if ((int)realTime <= 9 && startTimeHours <= 9)
            timeText.text = "0" + startTimeHours.ToString() + ":" + "0" + (int)realTime;

        if ((int)realTime <= 9 && startTimeHours > 9)
            timeText.text = startTimeHours.ToString() + ":" + "0" + (int)realTime;

        if ((int)realTime >= 10 && startTimeHours <= 9)
            timeText.text = "0" + startTimeHours.ToString() + ":" + (int)realTime;

        if ((int)realTime > 9 && startTimeHours > 9)
            timeText.text = startTimeHours.ToString() + ":" + (int)realTime;

        if ((int)realTime == 60)
        {
            startTimeHours++;
            realTime = 0;
            timeText.text = "0" + startTimeHours.ToString() + ":" + (int)realTime;
        }
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataTimer.carry";
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, startTimeHours);
        formatter.Serialize(stream, realTime);

        stream.Close();

    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataTimer.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Debug.Log("Save File loaded: " + path);

            startTimeHours = (int)formatter.Deserialize(stream);
            realTime = (float)formatter.Deserialize(stream);

            stream.Close();

        }
        else
        {
            Debug.Log("Save File not found" + path);
        }
    }
}
