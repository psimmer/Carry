using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void LoadData()
    {
        //throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        //throw new System.NotImplementedException();
    }
}
