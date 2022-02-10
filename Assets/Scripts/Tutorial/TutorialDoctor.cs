using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoctor : MonoBehaviour
{
    [SerializeField] Canvas docCanvas;


    private void Awake()
    {
        docCanvas.GetComponent<Transform>().GetChild(0).transform.LookAt(Camera.main.transform, Vector3.forward);
    }



}
