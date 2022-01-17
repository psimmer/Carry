using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Item currentItem;

    private void Awake()
    {

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && Input.GetKey(KeyCode.Space))
        {
            currentItem = other.gameObject.GetComponent<Item>();
            Debug.Log("Hallo " + currentItem.name);
        }

    }

    public void HealPatientWithItem(Patient patient, Item item)
    {
        item.Heal(patient);
    }
    public void DamagePatientWithItem(Patient patient, Item item)
    {
        item.Damage(patient);
    }



}
