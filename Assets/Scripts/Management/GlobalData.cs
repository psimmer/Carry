using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance;

    //not sure if needed for the Save/Load System
    //private float currentStresslvl;
    //private int currentItem;
    //private Vector3 currentPlayerPosition;
    //private bool isSaveFileLoaded = false;
    //private float timeLeft;

    //Overall statistics
    private int totalTreatments;
    private int totalPatientsHealed;
    private int totalPatientsLost;

    //Shift statistics
    private int shiftTreatments;
    private int shiftPatientsHealed;
    private int shiftPatientsLost;

    [SerializeField] private List<AudioClip> allMusic;
    [SerializeField] private List<AudioClip> allSoundEffects;

    #region Getter/Setter
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
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);  //take GlobalData to the next scene
    }

    public void ResetStatistics()
    {
        GlobalData.instance.TotalTreatments = 0;
        GlobalData.instance.TotalPatientsLost = 0;
        GlobalData.instance.TotalPatientsHealed = 0;
    }
}
