using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class Healthbar : MonoBehaviour
{
    private Slider healthbar;
    [SerializeField] Image heartbeatImage;
    [SerializeField] float lerpSpeed;
    float tValue = 0;

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
            tValue += lerpSpeed * Time.deltaTime;
            heartbeatImage.fillAmount = Mathf.Lerp(0, 1, tValue);
            if(heartbeatImage.fillAmount == 1)
            {
                tValue = 0;
                heartbeatImage.fillAmount = 0;
            }
        }
    }

    public void UpdateHealthbar(float percent)
    {
        if (healthbar == null)
            return;
        healthbar.value = percent;
    }
}
