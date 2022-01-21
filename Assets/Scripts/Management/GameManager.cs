using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<Patient> patients;

    #region Patient Manager Variables
    [SerializeField] private List<BedScript> bedList; // private List<Bed> allBeds;
    [SerializeField] private List<Patient> patientList;
    public int maxAmountOfPatients;
    [SerializeField] private GameObject testPatientPrefab;
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


    private void Awake()
    {
    }
    private void Start()
    {
        GetAllBeds();
        patientContainer = GameObject.Find("Patients").transform;
    }
    void Update()
    {
        
        dayCycle.dayCycle();
        dayTime.DoubledRealTime();
        UpdatePatientList();
        PatientSpawner();
        Treatment(player.currentPatient);
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
                patient.HealthAmount -= 5;
            }
            else if(patient.CurrentIllness == player.currentItem.item.task)
            {
                //Success
                patient.HealthAmount += player.currentItem.item.restoreHealth;
                
            }
            else
            {
                //Damage
                patient.HealthAmount -= player.currentItem.item.restoreHealth;
            }

            if(itemSlot.CurrentItem != null)
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
