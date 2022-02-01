using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SceneMan sceneManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] PatientSpawner patientSpawner;

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
    [SerializeField] private CoffeMachine coffeMachine;
    [SerializeField] private CPU computer;
    [SerializeField] private DayCycle dayCycle;
    [SerializeField] private Timer dayTime;
    #endregion

   
    void Update()
    {
        //player Stuff
        player.Interact();
        player.DropItem();

        Treatment(player.currentPatient);

        //Features
        dayCycle.dayCycle();
        dayTime.DoubledRealTime();
        DocumentationTask();
        DrinkingCoffee();
        uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);
    }

    private void DocumentationTask()
    {
        if (Input.GetKeyDown(KeyCode.Return) && player.IsAtPc)
        {
            bool IsInputCorrect = computer.EndDocumentation();
            if (IsInputCorrect)
                player.CurrentStressLvl -= 20;
            else
            {
                player.CurrentStressLvl += 20;
                isGameOver();
            }
            player.IsAtPc = false;
        }
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

        if (player.IsInContact)
        {
            //assign the patient from the hallway to the bed
            if (player.currentItem == null && patient.CurrentIllness == TaskType.AssignBed)
            {
                patientSpawner.MoveToBed(patient);
            }
            //damage to the patient, when you try to treat him without an item
            else if (player.currentItem == null)
            {
                Damage(patient);
                if (patient.CurrentHP <= 0)
                {
                    //GlobalData.instance.SetPatientDeadStatistics();
                    patientSpawner.PatientList.Remove(patient.gameObject);
                    Destroy(patient.gameObject);
                    //SpawnParticles(deathParticles, particlesDuration);
                }

            }
            //Success, right treatment
            else if (patient.CurrentIllness == player.currentItem.item.task)
            {

                patient.HasPopUp = false;
                patient.Treatment(+player.currentItem.item.restoreHealth);
                player.CurrentStressLvl -= player.currentItem.item.restoreHealth * stressReductionMultiplier;
                uiManager.UpdateStressLvlBar(player.CurrentStressLvl / player.MaxStressLvl);
                GlobalData.instance.ShiftTreatments++;

            }
            //Failure, wrong treatment
            else if (patient.CurrentIllness != player.currentItem.item.task)
            {
                Damage(patient);
               
            }

            if (itemSlot.CurrentItem != null)
            {
                Destroy(itemSlot.CurrentItem);
                player.currentItem = null;
                itemSlot.UI_Element = null;
            }

            player.IsInContact = false;
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
        isGameOver();
    }

    private void isGameOver()
    {
        if (player.CurrentStressLvl >= player.MaxStressLvl)
        {
            sceneManager.GameOver();
        }
    }


}
