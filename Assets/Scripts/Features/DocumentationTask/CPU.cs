using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CPU : MonoBehaviour, ISaveSystem
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI inputfield;
    [SerializeField] private TextMeshProUGUI outputField;

    public void BeginDocumentation()
    {
        canvas.gameObject.SetActive(true);
    }

    public bool EndDocumentation()
    {
        
        outputField = outputField.GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(outputField.text);

        if(outputField.text == "Twelve hours of work completed.")
        {
            canvas.gameObject.SetActive(false);
            return true;
        }
        else
        {
            canvas.gameObject.SetActive(false);
            return false;
        }
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
