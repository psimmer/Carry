using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance;

    //not sure if needed for the Save/Load System
    //private float currentStresslvl;           Lukas would say lets make the currentstresslevel in the playerscript static so we dont need to save and in start lvl 1 reset 
    //private int currentItem;                  totally!!
    //private Vector3 currentPlayerPosition;    totally!!
    //private bool isSaveFileLoaded = false;    
    //private float timeLeft;                   totally!!

    //Overall statistics
    private int totalTreatments;
    private int totalPatientsHealed;
    private int totalPatientsLost;

    //Shift statistics
    private int shiftTreatments;
    private int shiftPatientsHealed;
    private int shiftPatientsLost;

    #region Properties
    public int TotalTreatments
    {
        get { return totalTreatments; }
        set { totalTreatments = value; }
    }

    public int TotalPatientsHealed
    {
        get { return totalPatientsHealed; }
        set { totalPatientsHealed = value; }
    }

    public int TotalPatientsLost
    {
        get { return totalPatientsLost; }
        set { totalPatientsLost = value; }
    }

    public int ShiftTreatments
    {
        get { return shiftTreatments; }
        set { shiftTreatments = value; }
    }

    public int ShiftPatientsHealed
    {
        get { return shiftPatientsHealed; }
        set { shiftPatientsHealed = value; }
    }

    public int ShiftPatientsLost
    {
        get { return shiftPatientsLost; }
        set { shiftPatientsLost = value; }
    }
    #endregion

    private void Awake()
    {
        if (instance == null)       //Singleton
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);  //take GlobalData to the next scene
    }

    public void ResetTotalStatistics()
    {
        totalTreatments = 0;
        totalPatientsLost = 0;
        totalPatientsHealed = 0;
    }

    public void ResetShiftStatistics()
    {
        shiftPatientsHealed = 0;
        shiftPatientsLost = 0;
        shiftTreatments = 0;
    }

    public void SetPatientDeadStatistics()
    {
        totalPatientsLost++;
        shiftPatientsLost++;
    }

    public void SetPatientHealedStatistics()
    {
        totalTreatments++;
        shiftTreatments++;
    }
}
