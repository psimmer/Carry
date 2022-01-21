using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I know it is the wrong script for this but i dont know in which script it should be
public enum TaskType
{
    Bandage,
    Pills,
    Catheter,
    BloodSample,                //Maybe we do a own script "Tasks" and there are only the Tasks inside?
    Transfusion,
    WashThePatient,
    RelocateAPatient,
    AnswerTheTelephone,
    Documentation,
    TalkToThePatient
}

public class Patient : MonoBehaviour
{
    [SerializeField] private int healthAmount;
    [SerializeField] private TaskType currentIllness;
    private int patientID;
    private bool isPopping;

    // getters and setters
    public bool IsPopping
    {
        get { return isPopping; }
        set { isPopping = value; }
    }

    public TaskType CurrentIllness
    {
        get { return currentIllness; }
    }
    public int HealthAmount
    {
        get { return healthAmount; }
        set { healthAmount = value; }
    }

    void Start()
    {
        IsPopping = false;
    }

}
