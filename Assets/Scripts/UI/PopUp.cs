using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public TaskType popUpTaskType;
    
    public TaskType TaskType
    {
        get { return popUpTaskType; }
        set { popUpTaskType = value; }
    }


}
