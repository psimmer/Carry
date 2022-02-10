using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatientData 
{
   public float[] position = new float[3];
   public int currentHP;
   public int currentIllnes;
   public bool isPopping;
   public bool hasTask;
   public bool isInBed;
   public bool hasPopUp;
   public bool isReleasing;

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
