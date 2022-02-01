using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class Healthbar : MonoBehaviour
{
    private Slider healthbar;
    


    private void Start()
    {
        healthbar = GetComponent<Slider>();
        Vector3 lookDir;
        if (GetComponentInParent<Patient>() != null && GetComponentInParent<Patient>().IsInBed)
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            lookDir = Camera.main.transform.forward;
            transform.parent.LookAt(transform.parent.position + lookDir);
        }

    }

    public void UpdateHealthbar(float percent)
    {
        if (healthbar == null)
            return;
        healthbar.value = percent;
    }



}
