using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class GlobalData : MonoBehaviour, ISaveSystem
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

    private int currentLevel = 1;

    private bool isSaveFileLoaded;

    #region Properties

    public bool IsSaveFileLoaded
    {
        get { return isSaveFileLoaded; }
        set { isSaveFileLoaded = value; }
    }
    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }
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
        shiftPatientsHealed++;
        TotalPatientsHealed++;
    }

    public void SetPatientTreatmentStatistics()
    {
        shiftTreatments++;
        TotalTreatments++;
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataGlobalData.carry";
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, GlobalData.instance.currentLevel);
        formatter.Serialize(stream, GlobalData.instance.shiftPatientsHealed);
        formatter.Serialize(stream, GlobalData.instance.shiftPatientsLost);
        formatter.Serialize(stream, GlobalData.instance.shiftTreatments);
        formatter.Serialize(stream, GlobalData.instance.totalPatientsHealed);
        formatter.Serialize(stream, GlobalData.instance.totalPatientsLost);
        formatter.Serialize(stream, GlobalData.instance.totalTreatments);
        stream.Close();        
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataGlobalData.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Debug.Log("Save File loaded: " + path);
         
            GlobalData.instance.currentLevel = (int)formatter.Deserialize(stream);
            GlobalData.instance.shiftPatientsHealed = (int)formatter.Deserialize(stream);
            GlobalData.instance.shiftPatientsLost = (int)formatter.Deserialize(stream);
            GlobalData.instance.shiftTreatments = (int)formatter.Deserialize(stream);
            GlobalData.instance.totalPatientsHealed = (int)formatter.Deserialize(stream);
            GlobalData.instance.totalPatientsLost = (int)formatter.Deserialize(stream);
            GlobalData.instance.totalTreatments = (int)formatter.Deserialize(stream);

            stream.Close();
         
        }
        else
        {
            Debug.Log("Save File not found" + path);
        }
    }
}
