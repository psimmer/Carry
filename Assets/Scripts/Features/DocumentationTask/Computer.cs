using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Computer : MonoBehaviour, ISaveSystem
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI inputfield;
    [SerializeField] private TextMeshProUGUI outputField;
    [SerializeField] Transform documentationCamPos;
    public Transform DocumentationCamPos { get { return documentationCamPos; } set { documentationCamPos = value; } }


    private void Update()
    {
        
    }

    public void BeginDocumentation()
    {
        Camera.main.transform.position = documentationCamPos.position;
    }

    //public bool EndDocumentation()
    //{

    //    outputField = outputField.GetComponentInChildren<TextMeshProUGUI>();
    //    Debug.Log(outputField.text);

    //    if(outputField.text == "Twelve hours of work completed.")
    //    {
    //        canvas.gameObject.SetActive(false);
    //        return true;
    //    }
    //    else
    //    {
    //        canvas.gameObject.SetActive(false);
    //        return false;
    //    }
    //}

    public void LoadData()
    {
        //throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        //throw new System.NotImplementedException();
    }
}
