using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour , ISaveSystem
{
    [SerializeField] float duration;
    [SerializeField] Image radialBarImage;
    public Image RadialBarImage => radialBarImage;

    [SerializeField] Gradient gradient;
    [SerializeField] int timeOutDamagePatient;
    [SerializeField] float timeOutDamagePlayer;
    private float timePassed;
    public TaskType popUpTaskType;
    public static event Action<float> e_OnPopUpTimeOut;
    private bool isHealing;

    public bool IsHealing { get { return isHealing; } set { isHealing = value; } }
    public TaskType TaskType { get { return popUpTaskType; } set { popUpTaskType = value; } }

    private void Start()
    {
        timePassed = duration;
    }

    private void Update()
    {
        if (radialBarImage != null)
        {
            UpdateRadialBar();
            if (radialBarImage.fillAmount <= 0)
            {
                GetComponentInParent<Patient>().Treatment(-timeOutDamagePatient);
                e_OnPopUpTimeOut?.Invoke(timeOutDamagePlayer);
                Destroy(this.gameObject);
            }
            else if (radialBarImage.fillAmount >= 1)
            {
                GetComponentInParent<Patient>().HasPopUp = false;
                Destroy(this.gameObject);
            }
        }
    }

    private void UpdateRadialBar()
    {
        if (isHealing)
            timePassed += Time.deltaTime;
        else
            timePassed -= Time.deltaTime;

        radialBarImage.fillAmount = timePassed / duration;
        radialBarImage.color = gradient.Evaluate(radialBarImage.fillAmount);
    }

    public void SaveData()
    {
        throw new NotImplementedException();
    }

    public void LoadData()
    {
        throw new NotImplementedException();
    }
}
