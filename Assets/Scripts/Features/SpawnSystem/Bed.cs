using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{

    [SerializeField] Transform bedPos;
    public Transform BedPos => bedPos;
    [SerializeField] private bool isPatientInBed;
    public bool IsPatientInBed { get { return isPatientInBed; } set { isPatientInBed = value; } }

    [SerializeField] private Patient currentPatient;
    public Patient CurrentPatient { get { return currentPatient; } set { currentPatient = value; } }

    private void Start()
    {
        if(currentPatient == null)
        {
            isPatientInBed = false;
        }
        else if(currentPatient != null)
        {
            isPatientInBed = true;
        }
    }


}
