using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SceneMan sceneManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] PatientSpawner patientSpawner;
    [SerializeField] private Animator playerAnimator;

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

    #region Inventory Variables
    [SerializeField] private Inventory itemSlot;
    [SerializeField] private Transform itemslotPos;
    #endregion

    #region Features
    [SerializeField] private Camera mainCam;
    //[SerializeField] private CPU computer;
    [SerializeField] private DayCycle dayCycle;
    [SerializeField] private Timer dayTime;
    #endregion

    private void Awake()
    {
    }


    void Update()
    {
        //player Stuff
        player.Interact();
        player.DropItem();

        Treatment(player.currentPatient);

        //Features
        dayCycle.dayCycle();
        dayTime.DoubledRealTime();
        if (player.IsAtPc)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                player.IsAtPc = false;
                Camera.main.GetComponent<CamPosition>().lastPoint = Camera.main.GetComponent<CamPosition>().currentPoint;
                Camera.main.transform.position = Camera.main.GetComponent<CamPosition>().lastPoint.position;
                Camera.main.transform.rotation = Camera.main.GetComponent<CamPosition>().CameraRotation;
            }
        }
        DrinkingCoffee();
        uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);

        isGameOver();

        if (player.CurrentStressLvl <= 0)
            player.CurrentStressLvl = 0;
    }

    private void DrinkingCoffee()
    {
        if (player.IsDrinkingCoffee)
        {
            if (coffeeMachine.CoffeeCount > 0)
            {
                coffeeMachine.Drinking = true;
                uiManager.UpdateCoffeCounter(--coffeeMachine.CoffeeCount);
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

        if (player.IsInContact && patient.IsInBed) // the patient.isinbed fixed the issue with the null reference
        {
            // release the patient to leave the hospital
            if(player.currentItem == null && patient.CurrentIllness == TaskType.ReleasePatient && !patient.IsReleasing)
            {
                patient.transform.position = patient.LeaveHospital.transform.position;
                patient.transform.rotation = Quaternion.Euler(0, 0, 0);
                patient.PopUpCanvas.gameObject.SetActive(false);
                patient.HealthBarCanvas.gameObject.SetActive(false);
                patient.IsInBed = false;
                patient.IsReleasing = true;
                patientSpawner.PatientList.Remove(patient.gameObject);
            }
            //damage to the patient, when you try to treat him without an item
            else if (player.currentItem == null && !(patient.CurrentIllness == TaskType.ReleasePatient))
            {
                Damage(patient);
                if (patient.CurrentHP <= 0)
                {
                    patientSpawner.PatientList.Remove(patient.gameObject);
                    Destroy(patient.gameObject);
                    //SpawnParticles(deathParticles, particlesDuration);
                }
                player.IsInContact = false;
                if (patient.CurrentPopUp != null)
                    Destroy(patient.CurrentPopUp);
            }

            //Failure, wrong treatment
            else if (patient.CurrentIllness != player.currentItem.item.task)
            {
                Damage(patient);
                ResetItem();
                if (patient.CurrentPopUp != null)
                    Destroy(patient.CurrentPopUp);
                player.IsInContact = false;
            }


            //Success, right treatment
            else if (patient.CurrentIllness == player.currentItem.item.task)
            {
                playerAnimator.SetBool("isTreating", true);
                patient.GetComponentInChildren<PopUp>().IsHealing = true;
                player.GetComponent<PlayerMovement>().enabled = false;
                StartCoroutine(TreatmentProgress(patient));
            }

        }

        else if(player.IsInContact && !patient.IsInBed)
        {
            //assign the patient from the hallway to the bed
            if (player.currentItem == null && patient.CurrentIllness == TaskType.AssignBed)
            {
                patientSpawner.MoveToBed(patient);
                player.IsInContact = false;
            }
        }
    }

    private void Damage(Patient patient)
    {
        patient.HasPopUp = false;

        if (player.currentItem == null)
        {
            patient.Treatment(-player.NoItemDamage);
            player.CurrentStressLvl += player.NoItemDamage * stressMultiplier;
        }
        else
        {
            patient.Treatment(-player.currentItem.item.restoreHealth);
            player.CurrentStressLvl += player.currentItem.item.restoreHealth * stressMultiplier;
        }
        uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);
    }

    private void isGameOver()
    {
        if (player.CurrentStressLvl >= player.MaxStressLvl)
        {
            sceneManager.GameOver();
        }
    }

    IEnumerator TreatmentProgress(Patient patient)
    {
        while (player.IsInContact)
        {
            if (patient.CurrentPopUp != null)
            {
                if (player.currentItem != null)
                {

                    //Success
                    if (patient.GetComponentInChildren<PopUp>().RadialBarImage.fillAmount >= 1)
                    {
                        patient.GetComponentInChildren<PopUp>().IsHealing = false;

                        patient.Treatment(+player.currentItem.item.restoreHealth);
                        player.CurrentStressLvl -= player.currentItem.item.restoreHealth * stressReductionMultiplier;
                        uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);

                        playerAnimator.SetBool("isTreating", false);
                        ResetItem();
                        player.IsInContact = false;
                        player.GetComponent<PlayerMovement>().enabled = true;
                        Destroy(patient.GetComponentInChildren<PopUp>().gameObject);
                        StopCoroutine(TreatmentProgress(patient));
                    }
                    //Fail
                    else if (!Input.GetKey(KeyCode.Space))
                    {
                        playerAnimator.SetBool("isTreating", false);
                        Damage(patient);
                        ResetItem();
                        player.IsInContact = false;
                        player.GetComponent<PlayerMovement>().enabled = true;
                        Destroy(patient.GetComponentInChildren<PopUp>().gameObject);
                        StopCoroutine(TreatmentProgress(patient));
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }


    private void ResetItem()
    {
        if (itemSlot.CurrentItem != null)
        {
            Destroy(itemSlot.CurrentItem);
            player.currentItem = null;
            itemSlot.UI_Element = null;
        }
    }


}
