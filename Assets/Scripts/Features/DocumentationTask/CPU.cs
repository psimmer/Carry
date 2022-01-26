using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CPU : MonoBehaviour
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

        if(inputfield.text != outputField.text)
        {
            canvas.gameObject.SetActive(false);
            return false;
        }
        else
        {
            canvas.gameObject.SetActive(false);
            return true;
        }
    }



}
