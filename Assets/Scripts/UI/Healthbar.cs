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
    Transform cameraOverViewPoint;

    private void Start()
    {
        cameraOverViewPoint = GameObject.Find("CameraStartPosition").transform;

        healthbar = GetComponent<Slider>();


        if (GetComponentInParent<Patient>() != null && GetComponentInParent<Patient>().IsInBed)
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, 0); // reset rotation of the health bar so it fits the whiteboard
        }
        else
        {
            transform.parent.LookAt(transform.parent.position + cameraOverViewPoint.position);
            transform.parent.localRotation = Quaternion.Euler(transform.parent.eulerAngles.x, 0, 180);
            transform.parent.localPosition += new Vector3(0, 1.05f, 0);
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
            healthbar.value += 0.005f; 
        }
        if(healthbar.value > percent)
        {
            healthbar.value -= 0.005f;
        }
    }
}
