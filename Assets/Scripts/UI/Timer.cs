using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour , ISaveSystem
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private int startTimeHours;


    [Tooltip("This is the hour when the Game ends + 59 minutes")]
    [SerializeField] private int endTimeHours;
    private string dayOrNight;
    private float realTime;

    private void Awake()
    {
        //timeText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        
        //This would be the condition to end the scene with a success
        if (startTimeHours == endTimeHours && (int)realTime == 59)
        {
            SceneManager.LoadScene("LevelComplete");    //should be in the GameManager
            Debug.Log("SceneManager....");
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
        //throw new System.NotImplementedException();
    }

    public void LoadData()
    {
        //throw new System.NotImplementedException();
    }
}
