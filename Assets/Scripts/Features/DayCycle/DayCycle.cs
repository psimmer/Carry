using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class DayCycle : MonoBehaviour, ISaveSystem
{
    [SerializeField] private float interpolationValue;
    private float interpolation;
    private Quaternion startPos;
    private Quaternion endPos;
    public Action dayCycle;

    private void Start()
    {
        if (GlobalData.instance.IsSaveFileLoaded)  
        {
            LoadData();
        }
        startPos = Quaternion.Euler(10, -50, 0);
        endPos = Quaternion.Euler(180, -50, 0);
        dayCycle = LerpTheSun;
    }

    public void LerpTheSun()
    {
        interpolation += Time.deltaTime * (interpolationValue / 1000); 
        transform.rotation = Quaternion.Lerp(startPos, endPos, interpolation);
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveDataDayCycle.carry";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, interpolation);
        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataDayCycle.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Debug.Log("Save File loaded: " + path);

            interpolation = (float)formatter.Deserialize(stream);

            stream.Close();

        }
    }
}

