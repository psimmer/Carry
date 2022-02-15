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
    bool oneTimeBool = true;
    private void Awake()
    {
        canvas.gameObject.SetActive(false);
        ClipBoardCanvas.gameObject.SetActive(false);

    }

    private void Update()
    {
        timer += Time.deltaTime;
        //if((int)timer == 10 && oneTimeBool)
        //{
        //    oneTimeBool = false;
        //    SpawnPopUpDocumentation();
        //}
    }


    public void BeginDocumentation()
    {
        Camera.main.transform.position = documentationCamPos.position;
        canvas.gameObject.SetActive(true);
        ClipBoardCanvas.gameObject.SetActive(true);
    }
    public void SpawnPopUpDocumentation()
    {
        GameObject docPopUp = Instantiate(DocumentationPopUp, popUpCanvas);
        docPopUp.transform.position = popUpPos.position;
        docPopUp.transform.LookAt(Camera.main.transform);

    }

}
