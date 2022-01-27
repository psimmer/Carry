using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public TaskType popUpTaskType;
    //float popUpTimer;
    //float timetillPopUp;
    //GameObject currentPopUp;
    //public GameObject CurrentPopUp { get { return currentPopUp; } set { currentPopUp = value; } }

    //bool hasPopUp;
    //public bool HasPopUp { get { return hasPopUp; } set { hasPopUp = value; } }
    public TaskType TaskType
    {
        get { return popUpTaskType; }
        set { popUpTaskType = value; }
    }


}
