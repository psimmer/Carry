using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class Healthbar : MonoBehaviour
{
    private Slider healthbar;
    [SerializeField] Image heartbeatImage;
    [SerializeField] float heartbeatLerpSpeed;
    float heartBeatTValue = 0;

    private void Start()
    {
        healthbar = GetComponent<Slider>();
        Vector3 lookDir;


        if (GetComponentInParent<Patient>() != null && GetComponentInParent<Patient>().IsInBed)
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, 0); // reset rotation of the health bar so it fits the whiteboard
        }
        else
        {
            lookDir = Camera.main.transform.forward;
            transform.parent.LookAt(transform.parent.position + lookDir);
        }

    }
    private void Update()
    {
        if(heartbeatImage != null && heartbeatImage.IsActive())
        {
            heartBeatTValue += heartbeatLerpSpeed * Time.deltaTime;
            heartbeatImage.fillAmount = Mathf.Lerp(0, 1, heartBeatTValue);
            if(heartbeatImage.fillAmount == 1)
            {
                heartBeatTValue = 0;
                heartbeatImage.fillAmount = 0;
            }
        }
    }

    public void UpdateHealthbar(float percent)
    {
        if (healthbar == null)
            return;
        if (healthbar.value < percent)
        {
            healthbar.value += 0.005f; // this could have errors but it works for now, if anyone finds an error, it's here and it would be about the increment/decrement of the healthbar values (it should be calculated instead)
        }
        if(healthbar.value > percent)
        {
            healthbar.value -= 0.005f;
        }
    }
}
