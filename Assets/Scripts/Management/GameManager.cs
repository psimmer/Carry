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
    //public List<Patient> PatientList => patientList;
    //public Dictionary<int, GameObject> PopUpList => popUpList;
    //public List<GameObject> PopUps => popUps;
    #region Multipliers
    [SerializeField] private float healCoffee;
    [Tooltip("This value multiplies the stress")]
    [Range(1, 4)]
    [SerializeField] private float stressMultiplier;
    [Tooltip("This value reduce the stress")]
    [Range(0, 1)]
    [SerializeField] private float stressReductionMultiplier;
    [SerializeField] private CoffeMachine coffeeMachine;
    #endregion

    #region Patient Manager Variables
    [SerializeField] private List<BedScript> bedList; // private List<Bed> allBeds;
    [SerializeField] private List<Patient> patientList;
    public int maxAmountOfPatients;
    private int newPatientID;
    [SerializeField] private GameObject patientPrefab;
    Transform patientContainer;
    Transform bedContainer;
    #endregion

    #region Particles

    [SerializeField] private GameObject healingParticles;
    //[SerializeField] private GameObject fullHealingParticles;
    [SerializeField] private GameObject damageParticles;
    //[SerializeField] private GameObject deathParticles;
    [SerializeField] private float particlesDuration;

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

        DrinkingCoffee();
        //PopUp Stuff
        if (patientList != null)
        {
            uiManager.ManagePopUps(patientList, popUpList, popUps);
        }

    }
    private void LateUpdate()
    {

        //uiManager.UpdateHealthBar(patientContainer);
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

    }    /// <summary>
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
                //Debug.Log("Damage");

                //patient.CurrentHP -= player.NoItemDamage;  //make a serializable variable for balancing 
                patient.Treatment(-player.NoItemDamage);

                player.CurrentStressLvl += player.NoItemDamage * stressMultiplier;
                SpawnParticles(damageParticles, patient, particlesDuration);


                //if(IsPatientDead(patient))
                //{
                //    //TODO: ParticleEffects Methods, Sound Effects
                //}
                isGameOver();
                //TODO: ParticleEffects Methods, Sound Effects

            }
            else if (patient.CurrentIllness == player.currentItem.item.task)    //doesnt work right
            {
                //Success

                //patient.CurrentHP += player.currentItem.item.restoreHealth;
                patient.Treatment(player.currentItem.item.restoreHealth);
                player.CurrentStressLvl -= player.currentItem.item.restoreHealth * stressReductionMultiplier;

                SpawnParticles(healingParticles, patient, particlesDuration);

                //if (IsPatientHealed(patient))
                //{
                //TODO: update Healthbar, ParticleEffects, Soundeffect
                //}
                //TODO: update Healthbar, ParticleEffects, Soundeffect
                //Debug.Log("currentStressLvl: " + player.CurrentStressLvl);
                GlobalData.instance.ShiftTreatments++;

            }
            else if (patient.CurrentIllness != player.currentItem.item.task)
            {
                //Damage

                //patient.CurrentHP -= player.currentItem.item.restoreHealth;
                patient.Treatment(-player.currentItem.item.restoreHealth);
                player.CurrentStressLvl += player.currentItem.item.restoreHealth * stressMultiplier;

                //Debug.Log("currentStressLvl: " + player.CurrentStressLvl);

                //if (IsPatientDead(patient)) ;
                //{
                //    //TODO: ParticleEffects Methods, Sound Effects
                //}
                ////TODO: Healthbar update, Sound Effects
                isGameOver();
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

        //bedList.AddRange(FindObjectsOfType<BedScript>());


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
    //    if (patient.CurrentHP <= 0)
    //    {
    //        GlobalData.instance.SetPatientDeadStatistics();
    //        Debug.Log("Patient is dead");
    //        DestroyPatient(patient.gameObject); //--> DestroyPatient Method doesnt work
    //        return true;
    //    }
    //    return false;
    //}


    //private bool IsPatientHealed(Patient patient)
    //{
    //    if(patient.CurrentHP >= patient.PatientMaxHP)
    //    {
    //        GlobalData.instance.SetPatientHealedStatistics();
    //        //Debug.Log("Patient is healed");
    //        //TODO: start release task
    //        return true;
    //    }
    //    return false;
    //}

    private void SpawnParticles(GameObject particles, Patient patient, float duration)
    {
        GameObject newParticles = Instantiate(particles, patient.transform);
        Destroy(newParticles, duration);
    }

    private void isGameOver()
    {
        if (player.CurrentStressLvl >= player.MaxStressLvl)
        {
            sceneManager.GameOver();
        }
    }
}
