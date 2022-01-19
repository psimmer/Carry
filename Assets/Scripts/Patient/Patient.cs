using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I know it is the wrong script for this but i dont know in which script it should be
public enum TaskType
{
    ChangeBandage,
    BringPills,
    ChangeCatheter,
    TakeBloodSample,                //Maybe we do a own script "Tasks" and there are only the Tasks inside?
    AdministerTransfusion,
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

    // getters and setters
    public TaskType CurrentIllness
    {
        get { return currentIllness; }
    }
    public int HealthAmount
    {
        get;
        set;
    }

    void Start()
    {
        
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<Player>())
    //    {
    //       currentPatient = returnsHimself();
    //    }
    //}
    public Patient returnsHimself()
    {
        return this;
    }


}
