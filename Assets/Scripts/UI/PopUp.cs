using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    public TaskType popUpTaskType;

    public GameObject Prefab => popUpPrefab;

    public TaskType TaskType
    {
        get { return popUpTaskType; }
        set { popUpTaskType = value; }
    }

    void Start()
    {
        
    }


}
