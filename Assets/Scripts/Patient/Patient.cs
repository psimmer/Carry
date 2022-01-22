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
    [SerializeField] private int patientID;
    [SerializeField] private bool isPopping;
    [SerializeField] private bool hasTask;
    [SerializeField] private int minTimer;
    [SerializeField] private int maxTimer;

    // getters and setters
    public bool IsPopping
    {
        get { return isPopping; }
        set { isPopping = value; }
    }

    public bool HasTask
    {
        get { return hasTask; }
        set { hasTask = value; }
    }

    public int PatientID
    {
        get { return patientID; }
        set { patientID = value; }
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
        HasTask = false;
        minTimer = Random.Range(3, 6);
        maxTimer = Random.Range(7, 12);
        IsPopping = false;
    }

    public IEnumerator PopUpTimer()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTimer, maxTimer));
        IsPopping = true;
        Debug.Log($"patient {patientID} finished waiting and is popping");
        StopCoroutine("PopUpTimer");
        //foreach(GameObject task in popUps)
        //{
        //    if(task.GetComponent<PopUp>().TaskType == patient.CurrentIllness)
        //    {
        //        GameObject currentPopUp = Instantiate(task.GetComponent<PopUp>().Prefab, patient.transform);
        //        currentPopUp.transform.SetParent(GameObject.Find("UIManager").transform, false);
        //        currentPopUp.transform.SetAsFirstSibling();
        //        popUpList.Add(patient.PatientID, currentPopUp);
        //        //Debug.Log(currentPopUp);
        //        //patient.IsPopping = false;
        //        //break;
        //    }
        //}
        //GameObject currentPopUp = Instantiate();
    }


}
