using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ISaveSystem
{
    [SerializeField] private Camera camera;
    [SerializeField] private Vector3 boxSize = Vector3.one;
    public Vector3 boxPos;
    //Player values
    [SerializeField] private float currentStressLvl;
    public float CurrentStressLvl { get { return currentStressLvl; } set { currentStressLvl = value; } }
    [SerializeField] private float maxStressLvl;
    public float MaxStressLvl { get { return maxStressLvl; } }
    [SerializeField] private int noItemDamage;
    [SerializeField] private Animator animator;
    Coroutine reduceStressIfOutside;

    private bool isAtPC;
    public bool IsAtPc { get { return isAtPC; } set { isAtPC = value; } }
    public int NoItemDamage { get { return noItemDamage; } }
    public Item currentItem { get; set; }
    public Patient currentPatient { get; set; }
    public bool IsInContact { get; set; }
    public bool IsDrinkingCoffee { get; set; }
    public bool IsHoldingItem { get; set; }



    private void Awake()
    {
        IsHoldingItem = false;
        PopUp.e_OnPopUpTimeOut += TimeOutDamage;
        Timer.e_OnLevelCompleteSaveStressLvl += SaveStressLevel;

    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 2" || SceneManager.GetActiveScene().name == "Level 3" ||
            SceneManager.GetActiveScene().name == "Level 4")
        {
            CurrentStressLvl = GlobalData.instance.CurrentStressLvl;
        }
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
                    InteractWithLaptop(obj.GetComponent<Computer>());
                }
                if (obj.CompareTag("CoffeeMachine"))
                {
                    animator.Play("Picking Up");
                    IsDrinkingCoffee = true;
                }
            }
        }
    }

    private void InteractWithLaptop(Computer obj)
    {
        animator.SetBool("isWalking", false);
        camera.GetComponent<CamPosition>().MovePoint.CameraOnPc = true;
        IsAtPc = true;
        if (obj.GetComponent<Computer>().CurrentPopUp != null)
        {
            Destroy(obj.GetComponent<Computer>().CurrentPopUp);
        }
        obj.GetComponent<Computer>().BeginDocumentation();
        if (camera.GetComponent<CamPosition>().MovePoint.IsCameraFixed)
        {
            camera.GetComponent<CamPosition>().MovePoint.IsCameraFixed = false;
        }
        GetComponent<NewPlayerMovement>().enabled = false;
        camera.GetComponent<CamPosition>().lastPoint = obj.GetComponent<Computer>().DocumentationCamPos;
        camera.transform.rotation = obj.GetComponent<Computer>().DocumentationCamPos.rotation;
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

    private void SaveStressLevel()
    {
        GlobalData.instance.CurrentStressLvl = currentStressLvl;
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataPlayer.carry";
        FileStream stream = new FileStream(path, FileMode.Create);

        //Serialize player data
        formatter.Serialize(stream, transform.position.x);
        formatter.Serialize(stream, transform.position.y);
        formatter.Serialize(stream, transform.position.z);
        formatter.Serialize(stream, currentStressLvl);
        formatter.Serialize(stream, IsInContact);
        formatter.Serialize(stream, IsDrinkingCoffee);
        formatter.Serialize(stream, IsHoldingItem);
        formatter.Serialize(stream, isAtPC);

        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataPlayer.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //Set Player position
            float[] position = new float[3];
            position[0] = (float)formatter.Deserialize(stream);
            position[1] = (float)formatter.Deserialize(stream);
            position[2] = (float)formatter.Deserialize(stream);
            Vector3 vector = new Vector3(position[0], position[1], position[2]);
            transform.position = vector;

            //setting the rest of the player data
            currentStressLvl = (float)formatter.Deserialize(stream);
            IsInContact = (bool)formatter.Deserialize(stream);
            IsDrinkingCoffee = (bool)formatter.Deserialize(stream);
            IsHoldingItem = (bool)formatter.Deserialize(stream);
            isAtPC = (bool)formatter.Deserialize(stream);

            stream.Close();
        }
    }
}


