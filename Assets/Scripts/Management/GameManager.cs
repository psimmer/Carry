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
    [SerializeField] private CPU computer;
    //[SerializeField] private List<Patient> patients;
    //[SerializeField] private List<GameObject> popUps;
    private Dictionary<int, GameObject> popUpList = new Dictionary<int, GameObject>();

    //private GameObject[] popUpList = new GameObject[6];
    //public List<Patient> PatientList => patientList;
    //public Dictionary<int, GameObject> PopUpList => popUpList;
    //public List<GameObject> PopUps => popUps;
    #region Multipliers
    [SerializeField] private float healCoffee;
    [Tooltip("This value multiplies the stress")] [Range(1, 4)]
    [SerializeField] private float stressMultiplier;
    [Tooltip("This value reduce the stress")] [Range(0, 1)]
    [SerializeField] private float stressReductionMultiplier;
    [SerializeField] private CoffeMachine coffeeMachine;
    #endregion

    #region Patient Manager Variables
    [SerializeField] private List<BedScript> freeBedList; // private List<Bed> allBeds;
    [SerializeField] private List<Patient> patientList;
    public int maxAmountOfPatients;
    float SpawnDelay;
    TaskType RandomTask;
    bool delayIsOver;
    private int newPatientID;
    [SerializeField] private GameObject[] patientPrefabList;
    Transform patientContainer;
    [SerializeField] private List<GameObject> SpawnPointList;

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
        patientContainer = GameObject.Find("Patients").transform;
        GetAllSpawnPoints();
        GetFreeBeds();
    }
    private void Start()
    {
        //patientSpawner is now here, it works through a coroutine
        PatientSpawner();
        //SetHealthBarPos();
        AssignPatientIDs();
        newPatientID = patientContainer.childCount + 1;
    }
    void Update()
    {
        //player Stuff
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Interact();
        }
        if (Input.GetKeyDown(KeyCode.F) && player.currentItem != null)
        {
            player.DropItem();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            bool IsInputCorrect = computer.EndDocumentation();
            if (IsInputCorrect)
                player.CurrentStressLvl -= 20;
            else
            {
                player.CurrentStressLvl += 20;
                isGameOver();
            }
        }

        //Game will be paused
        uiManager.GamePaused();

        //DayCycle and Timer
        dayCycle.dayCycle();
        dayTime.DoubledRealTime();

        //Patient Spawning Stuff
        UpdatePatientList();

        Treatment(player.currentPatient);

        DrinkingCoffee();
        //PopUp Stuff
        //if (patientList != null)
        //{
        //    uiManager.ManagePopUps(patientList, popUpList, popUps);
        //}

    }
    private void LateUpdate()
    {
        uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);
    }

    private void DrinkingCoffee()
    {
        if (player.IsDrinkingCoffee)
        {
            if (coffeeMachine.CoffeeCount <= 0)
            {
                Debug.Log("No Coffee left");
                //show in UI that nothing is left
            }
            else
            {
                player.CurrentStressLvl -= coffeeMachine.HealCoffee;  //multiply it by the stressReductionMultiplier?
                uiManager.updateCoffeCounter(--coffeeMachine.CoffeeCount);
            }
        }
        player.IsDrinkingCoffee = false;

    }    
    /// <summary>
    /// Heals or damages the patient if it is the wrong item
    /// </summary>
    /// <param name="patient"></param>
    public void Treatment(Patient patient)
    {

        if (player.IsHealing)
        {
            //if player wants to treat the patient without an item. maybe we have to overthink for itemless tasks
            if (player.currentItem == null)
            {
                patient.HasPopUp = false;
                patient.Treatment(-player.NoItemDamage);
                player.CurrentStressLvl += player.NoItemDamage * stressMultiplier;
                uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);
                
                isGameOver();


            }
            else if (patient.CurrentIllness == player.currentItem.item.task)   
            {
                //Success
                patient.HasPopUp = false;
                patient.Treatment(+player.currentItem.item.restoreHealth);
                player.CurrentStressLvl -= player.currentItem.item.restoreHealth * stressReductionMultiplier;
                uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);
                GlobalData.instance.ShiftTreatments++;

            }
            else if (patient.CurrentIllness != player.currentItem.item.task)
            {
                //Damage
                patient.HasPopUp = false;
                patient.Treatment(-player.currentItem.item.restoreHealth);
                player.CurrentStressLvl += player.currentItem.item.restoreHealth * stressMultiplier;
                uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);
                isGameOver();
            }

            if (itemSlot.CurrentItem != null)
            {
                Destroy(itemSlot.CurrentItem);
                player.currentItem = null;
                itemSlot.UI_Element = null;
            }

            if (patient.CurrentIllness == TaskType.RelocateAPatient)
            {
                MovePatientToBed(patient.gameObject);
                return;
            }

            player.IsHealing = false;
        }
    }


    #region Patient Spawn Manager
    private void MovePatientToBed(GameObject patient)
    {
        Patient patientScript = patient.GetComponent<Patient>();
        if (freeBedList.Count > 0)
        {
            if(patientScript.CurrentIllness == TaskType.RelocateAPatient)
            {
                
                int randomBed = UnityEngine.Random.Range(0, freeBedList.Count);
                Transform patientBed = freeBedList[randomBed].transform;
                //patient.transform.Rotate(new Vector3(-90, -90, 0));
                //rotation and position must still be fixed, but it's working more or less
                patient.transform.position = patientBed.position;
                patientScript.IsInBed = true;
                patientScript.CurrentIllness = RandomTask;
                freeBedList.RemoveAt(randomBed);
                
            }
        }
        else
            Debug.LogWarning("No free beds!");
    }
    private void SpawnPatientInSpawnPoint(GameObject patient, Transform spawnPoint)
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
        spawnPoint.GetComponent<SpawnPoint>().IsFree = false;
        newPatient.transform.Rotate(new Vector3 (0, 180));
        //SetHealthBarPos(newPatient.GetComponent<Patient>());

    }
    private void PatientSpawner()
    {
            IEnumerator coroutine = WaitAndSpawn();
            StartCoroutine(coroutine);
    }


        //if (freeBedList.Count > 0)
        //{
        //    //(0, freeSpawnPoints.Count - 1); // aks if we should take Random.Range or Random.Next
        //    int randomIndex = UnityEngine.Random.Range(0, freeBedList.Count - 1);
        //    Transform spawnPoint = freeBedList[randomIndex].transform;
        //    SpawnPatientInBed(patientPrefabList[UnityEngine.Random.Range(0, patientPrefabList.Length)], spawnPoint);

        //}
    

    IEnumerator WaitAndSpawn()
    {
        while(true)
        {
            SpawnDelay = (float)UnityEngine.Random.Range(4, 10);
            yield return new WaitForSeconds(SpawnDelay);
            if(patientContainer.childCount < maxAmountOfPatients)
            {
                int randomIndex = UnityEngine.Random.Range(0, SpawnPointList.Count);
                Transform spawnPoint = SpawnPointList[randomIndex].transform;
                if(spawnPoint.GetComponent<SpawnPoint>().IsFree)
                {
                    SpawnPatientInSpawnPoint(patientPrefabList[UnityEngine.Random.Range(0, patientPrefabList.Length)], spawnPoint);
                    spawnPoint.GetComponent<SpawnPoint>().IsFree = false;
                }
            }
        }
    }

    //@alejandro i changed the code: commented area is yours, the other code (line) is the more efficent one (tip from Andi). cut it out and paste it in the Awake() Method
    // perfect! I deleted the bed container, thanks!

    private void GetFreeBeds()
    {
        UnityEngine.GameObject[] bedArray = GameObject.FindGameObjectsWithTag("Bed");
        for (int i = 0; i < bedArray.Length; i++)
        {
            if(bedArray[i].GetComponent<BedScript>().IsFree)
            {
                freeBedList.Add(bedArray[i].GetComponent<BedScript>());
            }
        }


    }
    private void GetAllSpawnPoints()
    {
        SpawnPointList.AddRange(GameObject.FindGameObjectsWithTag("SpawnPoint"));
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
        //foreach (Patient element in patientList)
        for (int i = 0; i < patientList.Count; i++) //@alejandro: i changed the foreach to a for loop. you are not allowed to edit things in a foreach loop! there was an exception!
        {
            if (patientList[i].GetComponent<Patient>().PatientID == patientID)
            {
                patientList.RemoveAt(i);
            }
        }
        popUpList[patientID] = null;
        //TODO: destroy healthbar and PopUp UI
        Destroy(patient);
    }


    private void AssignPatientIDs()
    {
        for (int i = 0; i < patientContainer.childCount; i++)
        {
            patientContainer.transform.GetChild(i).GetComponent<Patient>().CurrentIllness = RandomTask;
            patientContainer.transform.GetChild(i).GetComponent<Patient>().PatientID = int.Parse(patientContainer.GetChild(i).name);
        }
    }
    #endregion


    private void isGameOver()
    {
        if (player.CurrentStressLvl >= player.MaxStressLvl)
        {
            sceneManager.GameOver();
        }
    }


}
