using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I know it is the wrong script for this but i dont know in which script it should be
public enum TaskType
{
    ChangeBandage,
    BringPills,
    ChangeCatheter,
    TakeBloodSample,
    AdministerTransfusion,
    WashThePatient,
    RelocateAPatient,
    AnswerTheTelephone,
    Documentation,
    TalkToThePatient
}

public class Patient : MonoBehaviour
{
    private string patientID;
    public int healthAmount;
    public TaskType currentIllness;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Patient returnsHimself()
    {
        return this;
    }


}
