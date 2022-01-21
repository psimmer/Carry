using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera camera;
    public Item currentItem { get; set; }
    public Patient currentPatient { get; set; }
    public bool IsHealing { get; set; }
    public bool IsHoldingItem { get; set; }

    private void Awake()
    {
        IsHoldingItem = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.F) && currentItem != null)
        {
            DropItem();
        }
    }

    /// <summary>
    /// Interacting with Objects/ with Items and Patients
    /// </summary>
    public void Interact()
    {
        Collider[] obj = Physics.OverlapBox(transform.position + new Vector3(0f, 1f, 0f), Vector3.one);
        foreach (var item in obj)
        {
            if (item.CompareTag("Item"))
            {
                //Pickup
                IsHoldingItem = true;
                currentItem = item.GetComponent<Item>();
                Debug.Log(currentItem);
            }
            if (item.CompareTag("Patient"))
            {
                currentPatient = item.GetComponent<Patient>();
                IsHealing = true;
                Debug.Log(currentPatient.name);
               }
            //Debug.Log(item.name);
        }
    }

    public void DropItem()
    {
            //Drop Item
            currentItem = null;
            Debug.Log("Item thrown away");
    }

    #region Stuff for Cam

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CamColliders>() != null)
        {
            Transform lastPos = other.GetComponent<CamColliders>().FirstPosition;
            Transform newPos = other.GetComponent<CamColliders>().NewPosition;
            if (camera.GetComponent<CamPosition>().currentPoint != newPos)
            {
                camera.GetComponent<CamPosition>().currentPoint = newPos;
            }
            else if(camera.GetComponent<CamPosition>().currentPoint == newPos)
            {
                camera.GetComponent<CamPosition>().currentPoint = lastPos;
            }
        }
        
    }




    #endregion
}
