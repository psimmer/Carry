using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Item currentItem;
    public Patient current;
    private void Awake()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Interact
            Collider[] obj = Physics.OverlapBox(transform.position + new Vector3(0f, 1f, 0.5f), Vector3.one * 0.5f);
            foreach (var item in obj)
            {
                if (item.CompareTag("Item"))
                {
                    //Pickup
                    if (currentItem == null)
                    {
                        currentItem = item.GetComponent<Item>();
                    }
                }
                if (item.CompareTag("Patients"))
                {

                }
                Debug.Log(item.name);
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && currentItem != null)
        {
            //Drop Item
        }
    }


    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<Item>() && Input.GetKey(KeyCode.Space))
    //    {
    //        currentItem = other.gameObject.GetComponent<Item>();
    //        Debug.Log("Du Hast eingesammelt: " + currentItem.name);
    //    }

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<Patient>() != null)
    //    {
    //        current = other.GetComponent<Patient>();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<Patient>() != null)
    //    {
    //        current = null;
    //    }
    //}

    //public void HealPatientWithItem(Patient patient, Item item)
    //{
    //    item.Heal(patient);
    //}
    //public void DamagePatientWithItem(Patient patient, Item item)
    //{
    //    item.Damage(patient);
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, 1f, 0.5f), Vector3.one);
    }

}
