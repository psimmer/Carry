using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour, ISaveSystem
{
    [SerializeField] private Camera camera;
    [SerializeField] private Vector3 boxSize = Vector3.one;
    public Vector3 boxPos;
    //Player values
    [SerializeField] private float currentStressLvl;
    [SerializeField] private float maxStressLvl;
    [SerializeField] private int noItemDamage;
    [SerializeField] private Animator animator;
    Coroutine reduceStressIfOutside;

    private bool isAtPC;
    public bool IsAtPc { get { return isAtPC; } set { isAtPC = value; } }


    #region Properties
    public int NoItemDamage
    {
        get { return noItemDamage; }
    }
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
        //IsAtPc = false;
        PopUp.e_OnPopUpTimeOut += TimeOutDamage;
        if (GlobalData.instance.IsSaveFileLoaded)
            LoadData();

    }

    private void TimeOutDamage(float damage)
    {
        CurrentStressLvl += damage;
    }

    /// <summary>
    /// Interacting with Objects/ with Items and Patients
    /// </summary>
    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider[] objects = Physics.OverlapBox(transform.position + boxPos, boxSize);
            foreach (var obj in objects)
            {
                if (obj.CompareTag("Item"))
                {
                    SoundManager.instance.PlayAudioClip(ESoundeffects.PickUpItem, GetComponent<AudioSource>());
                    //Pickup
                    animator.Play("Picking Up");
                    IsHoldingItem = true;
                    currentItem = obj.GetComponent<Item>();
                }
                if (obj.GetComponent<SpawnPoint>())
                {
                    obj.GetComponent<SpawnPoint>().IsFree = true;
                }
                if (obj.CompareTag("Patient"))
                {
                    currentPatient = obj.GetComponent<Patient>();
                    IsInContact = true;
                }
                if (obj.GetComponent<Computer>())
                {
                    animator.SetBool("isWalking", false);
                    camera.GetComponent<CamPosition>().MovePoint.CameraOnPc = true;
                    IsAtPc = true;
                    obj.GetComponent<Computer>().BeginDocumentation();
                    if (camera.GetComponent<CamPosition>().MovePoint.IsCameraFixed)
                    {
                        camera.GetComponent<CamPosition>().MovePoint.IsCameraFixed = false;
                    }
                    GetComponent<PlayerMovement>().enabled = false;
                    camera.GetComponent<CamPosition>().lastPoint = obj.GetComponent<Computer>().DocumentationCamPos;
                    camera.transform.rotation = obj.GetComponent<Computer>().DocumentationCamPos.rotation;
                }
                if (obj.CompareTag("CoffeeMachine"))
                {
                    animator.Play("Picking Up");
                    IsDrinkingCoffee = true;
                }
            }
        }
    }

    public void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentItem != null)
        {
            currentItem = null;
            Debug.Log("Item thrown away");
        }
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
        if (other.CompareTag("Inside"))
        {
            if (reduceStressIfOutside != null)
                StopCoroutine(reduceStressIfOutside);
        }

        if (other.GetComponent<CamColliders>() != null)
        {
            Transform newPos = other.GetComponent<CamColliders>().NewPosition;
            if (camera.GetComponent<CamPosition>().currentPoint != newPos)
            {
                camera.GetComponent<CamPosition>().currentPoint = newPos;
                camera.GetComponent<CamPosition>().lastPoint = newPos;
            }
        }

       

    }

    #endregion

    IEnumerator ReduceStressLevel()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            CurrentStressLvl--;
        }
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataPlayer.carry";
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, transform.position.x);
        formatter.Serialize(stream, transform.position.y);
        formatter.Serialize(stream, transform.position.z);
        formatter.Serialize(stream, IsInContact);
        formatter.Serialize(stream, IsDrinkingCoffee);
        formatter.Serialize(stream, IsHoldingItem);
        formatter.Serialize(stream, isAtPC);
        formatter.Serialize(stream, currentStressLvl);

        //how do i save the currentItem
        //formatter.Serialize(stream, currentItem.item.itemName);
        //formatter.Serialize(stream, currentItem.item.restoreHealth);
        //formatter.Serialize(stream, currentItem.item.task);
        //formatter.Serialize(stream, currentItem.item.prefab);
        //formatter.Serialize(stream, currentItem.item.UI_prefab);

        //how do i save the currentPatient
        //currentPatient.SaveToStream(stream);

        stream.Close();
    }

    public void LoadData()
    {
        //throw new NotImplementedException();
    }
}


