using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Item currentItem;
    public Patient currentPatient;
    private void Awake()
    {

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


    public void Interact()
    {
        Collider[] obj = Physics.OverlapBox(transform.position + new Vector3(0f, 1f, 0f), Vector3.one);
        foreach (var item in obj)
        {
            if (item.CompareTag("Item"))
            {
                //Pickup
                currentItem = item.GetComponent<Item>();
                Debug.Log(currentItem);
            }
            if (item.CompareTag("Patient"))
            {
                currentPatient = item.GetComponent<Patient>();
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
    

}
