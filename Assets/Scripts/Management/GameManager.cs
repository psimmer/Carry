using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCam;
    [SerializeField] private SceneMan sceneManager;
    [SerializeField] private UIManager uiManager;
    //[SerializeField] private List<Patient> patients;
    [SerializeField] private List<GameObject> popUps;
    private Dictionary<int, GameObject> popUpList = new Dictionary<int, GameObject>();
    //private GameObject[] popUpList = new GameObject[6];

    #region Patient Manager Variables
    [SerializeField] private List<BedScript> bedList; // private List<Bed> allBeds;
    [SerializeField] private List<Patient> patientList;
    public int maxAmountOfPatients;
    private int newPatientID;
    [SerializeField] private GameObject patientPrefab;
    Transform patientContainer;
    Transform bedContainer;
    #endregion

    #region Inventory Variables
    [SerializeField] private Inventory itemSlot;
    [SerializeField] private Transform itemslotPos;
    #endregion

    #region DayCycle and DayTime references
    [SerializeField] private DayCycle dayCycle;
    [SerializeField] private Timer dayTime;
    #endregion


    #region Properties

    //public List<Patient> PatientList => patientList;
    //public Dictionary<int, GameObject> PopUpList => popUpList;
    //public List<GameObject> PopUps => popUps;

    #endregion

    private void Awake()
    {
        patientContainer = GameObject.Find("Patients").transform;
        GetAllBeds();
    }
    private void Start()
    {
        //SetHealthBarPos();
        AssignPatientIDs();
        newPatientID = patientContainer.childCount + 1;
    }
    void Update()
    {
        //Game will be paused
        uiManager.GamePaused();

        //DayCycle and Timer
        dayCycle.dayCycle();
        dayTime.DoubledRealTime();
        
        //Patient Spawning Stuff
        PatientSpawner();
        UpdatePatientList();

        Treatment(player.currentPatient);
        //SetHealthBarPos();

        //PopUp Stuff
        if (patientList != null)
        {
            uiManager.ManagePopUps(patientList, popUpList, popUps);
        }
        
    }
    private void LateUpdate()
    {
        uiManager.UpdateHealthBar(patientContainer);
    }

    /// <summary>
    /// Heals or damages the patient if it is the wrong item
    /// </summary>
    /// <param name="patient"></param>
    public void Treatment(Patient patient)
    {
        if (player.IsHealing)
        {
            if (player.currentItem == null)
            {
                Debug.Log("Damage");
                patient.HealthAmount -= 5;  //make a serializabel variable for balancing 
                //IsPatientDead(patient); 
            }
            else if (patient.CurrentIllness == player.currentItem.item.task)
            {
                //Success
                patient.HealthAmount += player.currentItem.item.restoreHealth;
                //TODO: update Healthbar, StressLvl, ParticleEffects, Soundeffect
                GlobalData.instance.TotalTreatments++;
                GlobalData.instance.ShiftTreatments++;

            }
            else
            {
                //Damage
                patient.HealthAmount -= player.currentItem.item.restoreHealth;
                //TODO: update Healthbar, Stresslvl, ParticleEffects, Soundeffects
                //TODO: patient is dead condition
            }

            if (itemSlot.CurrentItem != null)
            {
                Destroy(itemSlot.CurrentItem);
                player.currentItem = null;
                itemSlot.UI_Element = null;
            }
            player.IsHealing = false;
        }
    }

    #region Patient Spawn Manager
    private void SpawnPatient(GameObject patient, Transform spawnPoint)
    {
        GameObject newPatient = Instantiate(patient, spawnPoint);
        newPatient.GetComponent<Patient>().IsPopping = false;
        newPatient.GetComponent<Patient>().HasTask = false;
        newPatient.transform.parent = patientContainer;
        newPatient.GetComponent<Patient>().PatientID = newPatientID;
        newPatient.name = newPatientID.ToString();
        newPatient.transform.SetAsLastSibling();
        newPatientID++;
        newPatient.transform.position = spawnPoint.transform.position;
        spawnPoint.GetComponent<BedScript>().IsFree = false;
        //SetHealthBarPos(newPatient.GetComponent<Patient>());

    }
    private void PatientSpawner()
    {
        if (patientList.Count > maxAmountOfPatients)
        {
            return;
        }
        if (bedList.Count > 0)
        {
            //(0, freeSpawnPoints.Count - 1); // aks if we should take Random.Range or Random.Next
            int randomIndex = UnityEngine.Random.Range(0, bedList.Count - 1);
            Transform spawnPoint = bedList[randomIndex].transform;
            SpawnPatient(patientPrefab, spawnPoint);

        }
    }
    private void GetAllBeds()
    {
        UnityEngine.GameObject[] bedArray = GameObject.FindGameObjectsWithTag("Bed");
        for (int i = 0; i < bedArray.Length; i++)
        {
            bedList.Add(bedArray[i].GetComponent<BedScript>());
        }
    }

    private void UpdatePatientList()
    {
        if (patientContainer.childCount == patientList.Count)
            return;

        for (int i = 0; i < patientContainer.childCount; i++)
        {

            Patient patient = patientContainer.transform.GetChild(i).GetComponent<Patient>();

            // if the patient is not already in the list
            if (!patientList.Contains(patient))
                patientList.Add(patient);

        }

    }
    private void DestroyPatient(GameObject patient) // please use this for destroying patients!
    {
        int patientID = patient.GetComponent<Patient>().PatientID;
        foreach (Patient element in patientList)
        {
            if (element.GetComponent<Patient>().PatientID == patientID)
            {
                patientList.Remove(element);
            }
        }
        popUpList[patientID] = null;
        Destroy(patient);
    }


    private void AssignPatientIDs()
    {
        for (int i = 0; i < patientContainer.childCount; i++)
        {
            patientContainer.transform.GetChild(i).GetComponent<Patient>().PatientID = int.Parse(patientContainer.GetChild(i).name);
        }
    }
    #endregion

    /// <summary>
    /// Sets all statistics and destroys the patient
    /// </summary>
    /// <param name="patient"></param>
    /// <returns></returns>
    //private bool IsPatientDead(Patient patient)
    //{
    //    if(patient.HealthAmount <= 0)
    //    {
    //        GlobalData.instance.TotalPatientsLost++;
    //        GlobalData.instance.ShiftPatientsLost++;
    //        //TODO: set StressLvl, SoundEffects, Particles
    //        //DestroyPatient(patient.gameObject); --> DestroyPatient Method doesnt work
    //    }
    //    return false;
    //}


}
