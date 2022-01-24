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
                patient.HealthAmount -= 5;
            }
            else if (patient.CurrentIllness == player.currentItem.item.task)
            {
                //Success
                patient.HealthAmount += player.currentItem.item.restoreHealth;
                GlobalData.instance.TotalTreatments++;

            }
            else
            {
                //Damage
                patient.HealthAmount -= player.currentItem.item.restoreHealth;
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
            SpawnPatient(testPatientPrefab, spawnPoint);

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

    #region PopUp Spawn Manager


    //private void ManagePopUps()
    //{
    //    //foreach (GameObject value in popUpList.Values)
    //    //{
    //    //    Debug.Log($"{value}");
    //    //}

    //    foreach (Patient patient in patientList)
    //    {
    //        if (patient != null)
    //        {
    //            int patientID = patient.PatientID;

    //            if (!patient.HasTask && !patient.IsPopping)
    //            {
    //                //Debug.Log($"patient {patient.PatientID}has no task");
    //                StartCoroutine("PopUpTimer", patient);
    //            }

    //            if (patient.IsPopping && !popUpList.ContainsKey(patientID))
    //            {
    //                //Debug.Log($"patient {patient.PatientID} is popping");
    //                foreach (GameObject task in popUps)
    //                {
    //                    if (task.GetComponent<PopUp>().TaskType == patient.CurrentIllness)
    //                    {
    //                        if (popUpList.ContainsKey(patientID))
    //                            continue;

    //                        GameObject currentPopUp = Instantiate(task.GetComponent<PopUp>().Prefab, patient.transform);
    //                        currentPopUp.transform.SetParent(GameObject.Find("UIManager").transform, false);
    //                        currentPopUp.transform.SetAsFirstSibling();
    //                        popUpList.Add(patient.PatientID, currentPopUp);
    //                        patient.HasTask = true;
    //                        patient.IsPopping = false;
    //                    }
    //                }
    //            }

    //            if (popUpList.ContainsKey(patientID))
    //            {
    //                GameObject popUp;
    //                bool success = false;
    //                success = popUpList.TryGetValue(patientID, out popUp);
    //                if (!success)
    //                    return;
    //                popUp.transform.position = mainCam.WorldToScreenPoint(new Vector3(patient.transform.position.x,
    //                patient.transform.position.y + 2, patient.transform.position.z));
    //            }
    //        }
    //    }
    //}
    //public IEnumerator PopUpTimer(Patient patient)
    //{
    //    int randomTime = UnityEngine.Random.Range(10, 15);
    //    int maxRandomTime = UnityEngine.Random.Range(20, 25);
    //    yield return new WaitForSeconds(UnityEngine.Random.Range(randomTime, maxRandomTime));  // Lukas likes this random timer method: I tooked it out to test smth Random.Range(minTimer, maxTimer)
    //    patient.IsPopping = true;
    //    //Debug.Log($"patient {patient.patientID} finished waiting and is popping");
    //    StopCoroutine("PopUpTimer");
    //    //private void RemovePopUpFromList(int patientID)
    //    //{
    //    //    GameObject removeIfExists;
    //    //    popUpList.TryGetValue(patientID, out removeIfExists);
    //    //    if(removeIfExists != null)
    //    //        popUpList.Remove(patientID);
    //    //}




    //}
    #endregion

    #region Health Bar Manager

    //public void UpdateHealthBar()
    //{
    //    for(int i = 0; i < patientContainer.childCount; i++)
    //    {
    //        Patient patient = patientContainer.GetChild(i).GetComponent<Patient>();
    //        GameObject instantiatedHealthbar = patient.InstantiatedHealthbar;
    //        if (patient != null)
    //        {
    //             Vector3 patientPos = patient.transform.position;

    //             if (patient.InstantiatedHealthbar != null)
    //             {
    //                 instantiatedHealthbar.transform.position = mainCam.WorldToScreenPoint(new Vector3(patientPos.x,
    //                     patientPos.y + 0.5f, patientPos.z));
    //             }
    //            else
    //            {
    //                Debug.Log(patient.PatientID);
    //                patient.InstantiatedHealthbar = Instantiate(patient.Prefab, uiManager.transform);
    //            }
    //        }
    //    }
    //}

    //public void LerpHealthBar()
    //{



    //}


    #endregion
}
