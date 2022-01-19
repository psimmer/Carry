using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private Text AmAndPm;
    [SerializeField] private int startTimeHours;
    [SerializeField] private int endTimeHours;
    private string PM = "pm";
    private string AM = "am";
    private string dayOrNight;
    private float realTime;

    private void Awake()
    {
        timeText = GetComponentInChildren<Text>();
        dayOrNight = AM;
    }
    private void Update()
    {
        realTime += Time.deltaTime * 2;

        AmAndPm.text = dayOrNight;

        if (startTimeHours == 12)
        {
            dayOrNight = PM;
        }
        if(startTimeHours == 13)
        {
            startTimeHours = 1;
        }

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
}
