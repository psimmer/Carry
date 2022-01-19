using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<Patient> patients;
    //public event Action<Patient, Item> e_OnHeal;
    //public event Action<Patient, Item> e_OnDamage;
    public Func<Patient> getPatient;

    #region Patient Manager Variables
    [SerializeField] private List<SpawnPoint> freeSpawnPoints; // private List<Bed> allBeds;
    [SerializeField] private List<Patient> patientList;
    public int maxAmountOfPatients;
    [SerializeField] private GameObject testPatientPrefab;
    Transform list; // find better name
    #endregion


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
        list = GameObject.Find("Patients").transform;
    }
    void Update()
    {
        UpdatePatientList();
        UpdateRespawnPointsList();
        PatientSpawner();
        Treatment(getPatient());
    }

    //I think this will work fine ^^
    public void Treatment(Patient patient)
    {
        if(player.currentItem != null && touchPatient)
        {
            //if(patient.CurrentIllness == TaskType.ChangeBandage && player.currentItem.name == "Bandage")
            //{
            //    e_OnHeal?.Invoke(patient, player.currentItem);
            //}
            //else if (patient.CurrentIllness == TaskType.BringPills && player.currentItem.name == "Pill")
            //{
            //    e_OnHeal?.Invoke(patient, player.currentItem);
            //}
            //else
            //{
            //    e_OnDamage(patient, player.currentItem);
            //}

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
        spawnPoint.GetComponent<SpawnPoint>().IsFree = false;
    }
    private void PatientSpawner()
    {
        if(patientList.Count > maxAmountOfPatients)
        {
            return;
        }
        if(freeSpawnPoints.Count > 0)
        {
            //(0, freeSpawnPoints.Count - 1); // aks if we should take Random.Range or Random.Next
            int randomIndex = UnityEngine.Random.Range(0, freeSpawnPoints.Count - 1);
            Transform spawnPoint = freeSpawnPoints[randomIndex].transform;
            SpawnPatient(testPatientPrefab, spawnPoint);

        }
    }
    private void UpdateRespawnPointsList()
    {
        //Transform list = GameObject.Find("SpawnPoints").transform;   // is this method unperformant? Lukas will find out! :D
        for (int i=0; i<list.childCount; i++)
        {
            // if the spawn point is free, and is not already in the list
            if (list.transform.GetChild(i).GetComponent<SpawnPoint>().IsFree && 
                !freeSpawnPoints.Contains(list.transform.GetChild(i).GetComponent<SpawnPoint>()))
            {
                freeSpawnPoints.Add(list.transform.GetChild(i).GetComponent<SpawnPoint>());
            }
            if(list.transform.GetChild(i).GetComponent<SpawnPoint>().IsFree == false && 
                freeSpawnPoints.Contains(list.transform.GetChild(i).GetComponent<SpawnPoint>()))
            {
                freeSpawnPoints.Remove(list.transform.GetChild(i).GetComponent<SpawnPoint>());
            }
        }
    }
    
    private void UpdatePatientList()
    {
        //Transform list = GameObject.Find("Patients").transform;  // is this method unperformant? Lukas will find out! :D
        for (int i = 0; i < list.childCount; i++)
        {
            // if the patient is not already in the list
            if (!patientList.Contains(list.transform.GetChild(i).GetComponent<Patient>()))
                patientList.Add(list.transform.GetChild(i).GetComponent<Patient>());
        }
    }
    #endregion


}
