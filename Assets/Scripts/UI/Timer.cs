using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Timer : MonoBehaviour , ISaveSystem
{
    [SerializeField] private TextMeshProUGUI timeText;
    public TextMeshProUGUI TimeText { get { return timeText; } set { timeText = value; } }
    [SerializeField] private int startTimeHours;
    public int TimeInHours { get { return startTimeHours; } set { startTimeHours = value; } }


    [Tooltip("This is the hour when the Game ends + 59 minutes")]
    [SerializeField] private int endTimeHours;
    public int EndTimeHours => endTimeHours;
    private float realTime;
    public float RealTime { get { return realTime; } set { realTime = value; } }
    public static event Action e_OnLevelCompleteSaveStressLvl;

    private void Start()
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
            e_OnLevelCompleteSaveStressLvl?.Invoke();
            SceneManager.LoadScene("LevelComplete");    
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

    #region Save/Load methods
    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataTimer.carry";
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

            startTimeHours = (int)formatter.Deserialize(stream);
            realTime = (float)formatter.Deserialize(stream);

            stream.Close();

        }
    }
    #endregion
}
