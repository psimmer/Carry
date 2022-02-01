using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{

    [SerializeField] Transform bedPos;
    public Transform BedPos => bedPos;

    [SerializeField] Transform whiteboardPos;
    public Transform WhiteboardPos => whiteboardPos;

    [SerializeField] private bool isPatientInBed;
    public bool IsPatientInBed { get { return isPatientInBed; } set { isPatientInBed = value; } }

    [SerializeField] private Patient currentPatient;
    public Patient CurrentPatient { get { return currentPatient; } set { currentPatient = value; } }

    private bool positionHealthBarInWhiteboard = true;

    private void Start()
    {
        if(currentPatient == null)
        {
            isPatientInBed = false;
        }
        else if(currentPatient != null)
        {
            isPatientInBed = true;
        }
    }

    private void Update()
    {
        if(currentPatient == null)
            this.isPatientInBed = false;

        if(currentPatient != null && positionHealthBarInWhiteboard) // position healthbars in the whiteboard if there is a patient in bed
        {
            currentPatient.Healthbar.transform.position = new Vector3(
                WhiteboardPos.position.x,
                WhiteboardPos.position.y + whiteboardPos.localScale.y/4,
                WhiteboardPos.position.z - WhiteboardPos.localScale.z
                );
          
            currentPatient.Heartbeat.transform.position = new Vector3(
                WhiteboardPos.position.x,
                whiteboardPos.position.y - whiteboardPos.localScale.y / 5,
                whiteboardPos.position.z - whiteboardPos.localScale.z
                );

            currentPatient.Heartbeat.SetActive(true);
            positionHealthBarInWhiteboard = false;
        }
        
    }
}
