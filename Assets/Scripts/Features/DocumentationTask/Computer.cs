using UnityEngine;
using TMPro;

public class Computer : MonoBehaviour
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
    float timer;
    public float Timer { get { return timer; } }
    bool oneTimeBool = true;
    GameObject currentPopUp;
    public GameObject CurrentPopUp { get { return currentPopUp; } set { currentPopUp = value; } }
    private void Awake()
    {
        canvas.gameObject.SetActive(false);
        ClipBoardCanvas.gameObject.SetActive(false);

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if ((int)timer > 10 && oneTimeBool)    //timer is hardcoded, because in every level the task shall start after 5 minutes
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
    }
    public void SpawnPopUpDocumentation()
    {
        currentPopUp = Instantiate(DocumentationPopUp, popUpCanvas);
        currentPopUp.transform.position = popUpPos.position;
        Vector3 lookDir = Camera.main.transform.forward;
        currentPopUp.transform.LookAt(currentPopUp.transform.position + lookDir);

    }

}
