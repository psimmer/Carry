using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PopUp : MonoBehaviour , ISaveSystem
{
    [SerializeField] float duration;
    [SerializeField] Image radialBarImage;
    public Image RadialBarImage => radialBarImage;

    [SerializeField] Gradient gradient;
    [SerializeField] int timeOutDamagePatient;
    [SerializeField] float timeOutDamagePlayer;
    [SerializeField] float healingSpeed;
    public static event Action<float> e_OnPopUpTimeOut;
    public static event Action<Patient> e_RemovePatient;

    public TaskType popUpTaskType;
    public TaskType TaskType { get { return popUpTaskType; } set { popUpTaskType = value; } }


    private float timePassed;

    private bool isHealing;
    public bool IsHealing { get { return isHealing; } set { isHealing = value; } }


    private void Start()
    {
        timePassed = duration;
    }

    private void Update()
    {
        if (radialBarImage != null)
        {
            UpdateRadialBar();
            PopUpCondition();
            
        }
    }
    
    private void PopUpCondition()
    {
        if (radialBarImage.fillAmount <= 0)
        {
            GetComponentInParent<Patient>().Treatment(-timeOutDamagePatient);
            e_OnPopUpTimeOut?.Invoke(timeOutDamagePlayer);
            GetComponentInParent<Patient>().HasPopUp = false;
            Destroy(this.gameObject);
            if (GetComponentInParent<Patient>().CurrentHP <= 0)
            {
                e_RemovePatient?.Invoke(GetComponentInParent<Patient>());
            }
        }
        else if (radialBarImage.fillAmount >= 1)
        {
            GetComponentInParent<Patient>().HasPopUp = false;
        }
    }



    private void UpdateRadialBar()
    {
        if (isHealing)
            timePassed += Time.deltaTime * healingSpeed;
        else
            timePassed -= Time.deltaTime;

        radialBarImage.fillAmount = timePassed / duration;
        radialBarImage.color = gradient.Evaluate(radialBarImage.fillAmount);
    }

    public void SaveToStream(System.IO.FileStream fileStream)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(fileStream, timePassed);
        formatter.Serialize(fileStream, isHealing);

        //how do i save the popup
    }

    public void SaveData()
    {
        //throw new NotImplementedException();
    }

    public void LoadData()
    {
        //throw new NotImplementedException();
    }
}
