using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Computer : MonoBehaviour, ISaveSystem
{
    [SerializeField] private GameObject clipBoardCanvas;
    public GameObject ClipBoardCanvas { get { return clipBoardCanvas; } set { clipBoardCanvas = value; } }

    [SerializeField] private GameObject canvas;
    public GameObject Canvas { get { return canvas; } set { canvas = value; } }

    [SerializeField] private TMP_Text hintText;
    public TMP_Text HintText { get { return hintText; } set { hintText = value; } }

    [SerializeField] private TMP_InputField inputField;
    public TMP_InputField InputField { get { return inputField; } set { inputField = value; } }

    [SerializeField] Transform documentationCamPos;
    public Transform DocumentationCamPos { get { return documentationCamPos; } set { documentationCamPos = value; } }

    [SerializeField] GameObject DocumentationPopUp;

    private void Awake()
    {
        canvas.gameObject.SetActive(false);
        ClipBoardCanvas.gameObject.SetActive(false);
        if (GlobalData.instance.IsSaveFileLoaded)
            LoadData();
    }

    private void Update()
    {
        
    }

    public void BeginDocumentation()
    {
        Camera.main.transform.position = documentationCamPos.position;
        canvas.gameObject.SetActive(true);
        ClipBoardCanvas.gameObject.SetActive(true);
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataComputer.carry";
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        //float[] camPosition = new float[6];
        //formatter.Serialize(stream,documentationCamPos.position.x);
        //formatter.Serialize(stream,documentationCamPos.position.y);
        //formatter.Serialize(stream,documentationCamPos.position.z);
        //camPosition[3] = documentationCamPos.rotation.x;
        //camPosition[4] = documentationCamPos.position.x;
        //camPosition[5] = documentationCamPos.position.x;
        //ring test = inputField.text;
        formatter.Serialize(stream, inputField.text);

        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataComputer.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Debug.Log("Save File loaded: " + path);

            //float[] camPositino = new float[3];
            //Vector3 vector = new Vector3();
            
            //vector.x = (float)formatter.Deserialize(stream);
            //vector.y = (float)formatter.Deserialize(stream);
            //vector.z = (float)formatter.Deserialize(stream);
            Camera.main.transform.position = documentationCamPos.position;

            inputField.text = (string)formatter.Deserialize(stream);

            stream.Close();

        }
        else
        {
            Debug.Log("Save File not found" + path);
        }
    }

    
}
