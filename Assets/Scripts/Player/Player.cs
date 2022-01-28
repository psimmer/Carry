using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera camera;

    //Player values
    [SerializeField] private float currentStressLvl;
    [SerializeField] private float maxStressLvl;
    [SerializeField] private int noItemDamage;
    Coroutine reduceStressIfOutside;

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
    public Item currentItem { get; set; }
    public Patient currentPatient { get; set; }
    public bool IsHealing { get; set; }
    public bool IsDrinkingCoffee { get; set; }
    public bool IsHoldingItem { get; set; }
    #endregion


    private void Awake()
    { 
        IsHoldingItem = false;
        IsAtPc = false;
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
                item.GetComponent<CPU>().BeginDocumentation();
                //Start Documentation.
            }
            if (item.CompareTag("CoffeeMachine")) {
                IsDrinkingCoffee = true;
            }
        }
    }

    public void DropItem()
    {
            currentItem = null;
            Debug.Log("Item thrown away");
    }

    #region Stuff for Cam

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Inside"))
        {
            reduceStressIfOutside = StartCoroutine(ReduceStressLevel());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Inside"))
        {
            if(reduceStressIfOutside != null)
                StopCoroutine(reduceStressIfOutside);
        }

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

    IEnumerator ReduceStressLevel()
    {
        while(true)
        {
            yield return new WaitForSeconds(3);
            currentStressLvl--;
        }
    }


}


