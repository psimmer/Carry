using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SceneMan sceneManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private List<Patient> patients;
    public Func<Patient> getPatient;

    #region Patient Manager Variables
    [SerializeField] private List<BedScript> bedList; // private List<Bed> allBeds;
    [SerializeField] private List<Patient> patientList;
    public int maxAmountOfPatients;
    [SerializeField] private GameObject testPatientPrefab;
    Transform patientContainer;
    Transform bedContainer;
    #endregion

    //UI 
    [SerializeField] private Inventory itemSlot;
    [SerializeField] private Transform itemslotPos;


    //Just for now can be deleted later
    private bool touchPatient = false; // this bool stays false forever in moment

    private void Awake()
    {
        getPatient = patients[0].returnsHimself;     //we need to fix this index atomatically to the correct patient

        //e_OnHeal += player.HealPatientWithItem;
        //e_OnDamage += player.DamagePatientWithItem;
    }
    private void Start()
    {
        GetAllBeds();
        patientContainer = GameObject.Find("Patients").transform;
    }
    void Update()
    {
        //Game will be paused
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            uiManager.GamePaused();
        }

        UpdatePatientList();
        PatientSpawner();
        Treatment(getPatient());
        //zoomCam.MoveCamera();
    }

    //I think this will work fine ^^
    public void Treatment(Patient patient)
    {
        if(player.currentItem != null && touchPatient)
        {

            if(patient.CurrentIllness == player.currentItem.item.task)
            {
                //Success
                patient.HealthAmount += player.currentItem.item.restoreHealth;
            }
            else
            {
                //Damage
                patient.HealthAmount -= player.currentItem.item.restoreHealth;
            }

        }
    }
    #region Patient Spawn Manager
    private void SpawnPatient(GameObject patient, Transform spawnPoint)
    {
        GameObject newPatient = Instantiate(patient, spawnPoint);
        newPatient.transform.parent = GameObject.Find("Patients").transform;  // unperformant?
        newPatient.transform.SetAsLastSibling();
        int patientNumber = patientList.Count+1;  
        newPatient.name = patientNumber.ToString();
        newPatient.transform.position = spawnPoint.transform.position;
        spawnPoint.GetComponent<BedScript>().IsFree = false;
    }
    private void PatientSpawner()
    {
        if(patientList.Count > maxAmountOfPatients)
        {
            return;
        }
        if(bedList.Count > 0)
        {
            //(0, freeSpawnPoints.Count - 1); // aks if we should take Random.Range or Random.Next
            int randomIndex = UnityEngine.Random.Range(0, bedList.Count - 1);
            Transform spawnPoint = bedList[randomIndex].transform;
            SpawnPatient(testPatientPrefab, spawnPoint);

        }
    }
    private void GetAllBeds()
    {
        GameObject[] bedArray = GameObject.FindGameObjectsWithTag("Bed");
        for(int i=0; i<bedArray.Length; i++)
        {
            bedList.Add(bedArray[i].GetComponent<BedScript>());
        }
    }

    private void UpdatePatientList()
    {
        //Transform list = GameObject.Find("Patients").transform;  // is this method unperformant? Lukas will find out! :D
        for (int i = 0; i < patientContainer.childCount; i++)
        {
            Patient patient = patientContainer.transform.GetChild(i).GetComponent<Patient>();
            // if the patient is not already in the list
            if (!patientList.Contains(patient))
                patientList.Add(patient);
        }
    }
    #endregion


}
