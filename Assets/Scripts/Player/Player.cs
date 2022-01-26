using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera camera;

    //Player values
    [SerializeField] private float currentStressLvl;
    [SerializeField] private float maxStressLvl;
    [SerializeField] private int noItemDamage;
    [SerializeField] private float healCoffee;
    [Tooltip("This value multiplies the stress")] [Range(1, 4)]
    [SerializeField] private float stressMultiplier;
    [Tooltip("This value reduce the stress")] [Range(0, 1)]
    [SerializeField] private float stressReductionMultiplier;
    private bool isAtPC;


    #region Properties
    public int NoItemDamage
    {
        get { return noItemDamage; }
    }
    public bool IsAtPc { get { return isAtPC; } set { isAtPC = value; } }
    public float CurrentStressLvl
    {
        get { return currentStressLvl; }
        set { currentStressLvl = value; }
    }

    public float MaxStressLvl
    {
        get { return maxStressLvl; }
    }

    public float StressMultiplier
    {
        get { return stressMultiplier; }
    }

    public float StressReductionMultiplier
    {
        get { return stressReductionMultiplier; }
    }

    #endregion

    public Item currentItem { get; set; }
    public Patient currentPatient { get; set; }
    public bool IsHealing { get; set; }
    public bool IsHoldingItem { get; set; }

    private void Awake()
    {
        IsHoldingItem = false;
        IsAtPc = false;
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
            }
            if (item.CompareTag("CPU"))
            {
                Debug.Log("Documentation Starts.");
                IsAtPc = true;
                //Start Documentation.
            }
            if (item.CompareTag("CoffeeMachine")) {
                CurrentStressLvl -= healCoffee;     //should it be multiplied by stressReductionMultiplier?
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
