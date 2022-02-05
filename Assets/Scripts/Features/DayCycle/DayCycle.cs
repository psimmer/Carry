using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class DayCycle : MonoBehaviour, ISaveSystem
{
    [SerializeField] private float interpolationValue;
    private float interpolation;
    private Quaternion startPos;
    private Quaternion endPos;
    public Action dayCycle;


    private void Start()
    {
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
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);

        float[] position = new float[3];
        position[0] = this.transform.rotation.x;
        position[1] = this.transform.rotation.y;
        position[2] = this.transform.rotation.z;

        formatter.Serialize(stream, position);
        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataDayCycle.carry.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            float[] position = new float[3];
            position = (float[])formatter.Deserialize(stream);
            stream.Close();

            Quaternion loadedQuaternion = new Quaternion();
            loadedQuaternion.x = position[0];
            loadedQuaternion.y = position[1];
            loadedQuaternion.z = position[2];
            this.transform.rotation = loadedQuaternion;

        }
        else
        {
            Debug.Log("Save File not found" + path);
        }
    }
}

