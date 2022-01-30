using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Vector3 boxSize = Vector3.one;
    public Vector3 boxPos;
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
    public bool IsInContact { get; set; }
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
        Collider[] objects = Physics.OverlapBox(transform.position + boxPos, boxSize);
        foreach (var obj in objects)
        {
            if (obj.CompareTag("Item"))
            {
                //Pickup
                IsHoldingItem = true;
                currentItem = obj.GetComponent<Item>();
                Debug.Log(currentItem);
            }
            if (obj.CompareTag("Patient"))
            {
                currentPatient = obj.GetComponent<Patient>();
                IsInContact = true;
            }
            if (obj.CompareTag("CPU"))
            {
                Debug.Log("Documentation Starts.");
                IsAtPc = true;
                obj.GetComponent<CPU>().BeginDocumentation();
                //Start Documentation.
            }
            if (obj.CompareTag("CoffeeMachine")) {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + boxPos, boxSize);
    }

}


