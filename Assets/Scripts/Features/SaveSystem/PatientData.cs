using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatientData 
{
    float[] position = new float[3];
    int currentHP;
    int currentIllnes;
    bool isPopping;
    bool hasTask;
    bool isInBed;
    bool hasPopUp;
    bool isReleasing;

    public PatientData(Patient patient)
    {
        position[0] = patient.transform.position.x;
        position[1] = patient.transform.position.y;
        position[2] = patient.transform.position.z;
        currentHP = patient.CurrentHP;
        currentIllnes = (int)patient.CurrentIllness;
        isPopping = patient.IsPopping;
        hasTask = patient.HasTask;
        isInBed = patient.IsInBed;
        hasPopUp = patient.HasPopUp;
        isReleasing = patient.IsReleasing;
    }
}
