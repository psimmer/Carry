using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
    [SerializeField] Transform popUpCanvas;
    [SerializeField] GameObject DocumentationPopUp;
    [SerializeField] Transform popUpPos;
    [SerializeField] Timer gameTime;

    bool oneTimeBool = true;
    GameObject currentPopUp;
    public GameObject CurrentPopUp { get { return currentPopUp; } set { currentPopUp = value; } }
    bool canDoComputerThing = true;
    public bool CanDoComputerThing => canDoComputerThing;
    private void Awake()
    {
        canvas.gameObject.SetActive(false);
        ClipBoardCanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (GlobalData.instance.IsSaveFileLoaded)
            LoadData();
    }

    private void Update()
    {
        //timer += Time.deltaTime;
        if (gameTime.TimeInHours >= 16 && oneTimeBool)    //timer is hardcoded, because in every level the task shall start after 5 minutes
        {
            oneTimeBool = false;
            SpawnPopUpDocumentation();
        }
    }


    public void BeginDocumentation()
    {
        Camera.main.transform.position = documentationCamPos.position;
        canvas.gameObject.SetActive(true);
        ClipBoardCanvas.gameObject.SetActive(true);
        canDoComputerThing = false;
    }
    public void SpawnPopUpDocumentation()
    {
        currentPopUp = Instantiate(DocumentationPopUp, popUpCanvas);
        currentPopUp.transform.position = popUpPos.position;
        Vector3 lookDir = Camera.main.transform.forward;
        currentPopUp.transform.LookAt(currentPopUp.transform.position + lookDir);

    }


    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataComputer.carry";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, oneTimeBool);

        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataComputer.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            oneTimeBool = (bool)formatter.Deserialize(stream);
            stream.Close();

        }
    }
}
