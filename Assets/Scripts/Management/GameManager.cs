using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Important managing references
    [Header("Important managing references")]
    [SerializeField] Player player;
    [SerializeField] SceneMan sceneManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] PatientSpawner patientSpawner;
    [SerializeField] private Animator playerAnimator;
    #endregion

    #region Multipliers
    [Header("Multipliers")]
    [Tooltip("Amount of stress the player gains when drinking coffee")]
    [SerializeField] private float coffeeDamage;
    [Tooltip("This value multiplies the stress")]
    [Range(1, 4)]
    [SerializeField] private float stressMultiplier;
    [Tooltip("This value reduce the stress")]
    [Range(0, 1)]
    [SerializeField] private float stressReductionMultiplier;
    #endregion

    #region Inventory Variables
    [Header("Inventory")]
    [SerializeField] private Inventory itemSlot;
    [SerializeField] private Transform itemslotPos;
    [SerializeField] private Item relocateAPatient;
    Item lastItem;
    bool lastItemIsSaved;
    #endregion

    #region Features
    [Header("Features")]
    [SerializeField] private CoffeMachine coffeeMachine;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Computer computer;
    [SerializeField] private DayCycle dayCycle;
    [SerializeField] private Timer dayTime;
    [SerializeField] float documentationReward;
    #endregion


    void Update()
    {
        //player Methods
        player.Interact();
        player.DropItem();
        Treatment(player.currentPatient);

        //Features
        dayCycle.dayCycle();
        ComputerTaskSuccessCondition();

        dayTime.DoubledRealTime();

        DrinkingCoffee();
        uiManager.transform.GetComponent<StressLvl>().FillOfStress.fillAmount = player.CurrentStressLvl / player.MaxStressLvl;

        if (player.CurrentStressLvl > 75)
        {
            SoundManager.instance.PlayAudioClip(ESoundeffects.StressLevel, uiManager.GetComponent<AudioSource>());
        }
        isGameOver();

        if (player.CurrentStressLvl <= 0)
            player.CurrentStressLvl = 0;
    }

    #region Treating a Patient
    /// <summary>
    /// Success: patient gets healed, stress level of the player will decrease. Fail: patient loses health, stresslevel
    /// of the player rises
    /// </summary>
    /// <param name="patient">treated patient</param>
    public void Treatment(Patient patient)
    {

        if (player.IsInContact && patient != null && patient.IsInBed) // the patient.isinbed fixed the issue with the null reference
        {
            if (patient.CurrentIllness == TaskType.RelocateAPatient && patient.CurrentPopUp != null)
            {
                if (!lastItemIsSaved)
                {
                    lastItem = player.currentItem;
                    player.currentItem = relocateAPatient;
                    lastItemIsSaved = true;
                }
            }
            // release the patient to leave the hospital
            if (patient.CurrentIllness == TaskType.ReleasePatient && !patient.IsReleasing)
            {
                patient.transform.position = patient.LeaveHospital.transform.position;
                patient.transform.rotation = patient.LeaveHospital.transform.rotation;
                patient.PopUpCanvas.gameObject.SetActive(false);
                patient.HealthBarCanvas.gameObject.SetActive(false);
                patient.IsInBed = false;
                patient.IsReleasing = true;
                patientSpawner.PatientList.Remove(patient.gameObject);
                patient.GetComponent<Animator>().SetBool("isWalking", true);
            }

            //damage to the patient, when you try to treat him without an item
            else if (player.currentItem == null && !(patient.CurrentIllness == TaskType.ReleasePatient))
            {
                Damage(patient);
                if (patient.CurrentHP <= 0)
                {
                    patientSpawner.PatientList.Remove(patient.gameObject);
                    Destroy(patient.gameObject, patient.ParticlesDuration);
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
            else if (Input.GetKeyDown(KeyCode.Space) && patient.CurrentIllness == player.currentItem.item.task)
            {
                playerAnimator.SetBool("isTreating", true);
                patient.CurrentParticles = Instantiate(patient.HealingParticles, patient.transform);
                ParticleSystem[] ParticleLoops = patient.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < ParticleLoops.Length; i++)
                {
                    ParticleLoops[i].loop = false;
                }

                patient.GetComponentInChildren<PopUp>().IsHealing = true;
                player.GetComponent<NewPlayerMovement>().enabled = false;
                StartCoroutine(TreatmentProgress(patient));
            }

        }
        //assign the patient from the hallway to the bed
        else if (player.IsInContact && patient != null && !patient.IsInBed)
        {
            if (patient.CurrentIllness == TaskType.AssignBed)
            {
                patientSpawner.MoveToBed(patient);
                player.IsInContact = false;
            }
        }
    }

    /// <summary>
    /// Co-Routine, which executes the treatment
    /// </summary>
    /// <param name="patient">treated patient</param>
    /// <returns></returns>
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

                        playerAnimator.SetBool("isTreating", false);
                        Destroy(patient.CurrentParticles, patient.ParticlesDuration);
                        ResetItem();
                        player.IsInContact = false;
                        player.GetComponent<NewPlayerMovement>().enabled = true;
                        Destroy(patient.GetComponentInChildren<PopUp>().gameObject);
                        StopCoroutine(TreatmentProgress(patient));
                    }

                    //Fail
                    else if (!Input.GetKey(KeyCode.Space))
                    {
                        playerAnimator.SetBool("isTreating", false);
                        Destroy(patient.CurrentParticles);
                        Damage(patient);
                        if (patient.CurrentIllness != TaskType.RelocateAPatient)
                        {
                            ResetItem();
                        }
                        player.IsInContact = false;
                        player.GetComponent<NewPlayerMovement>().enabled = true;
                        Destroy(patient.GetComponentInChildren<PopUp>().gameObject);
                        StopCoroutine(TreatmentProgress(patient));
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// damage that is dealed, when the treatment went wrong
    /// </summary>
    /// <param name="patient">treated patient</param>
    private void Damage(Patient patient)
    {
        patient.HasPopUp = false;

        //damage, if you try to treat the patient without an item
        if (player.currentItem == null)
        {
            patient.Treatment(-player.NoItemDamage);
            player.CurrentStressLvl += player.NoItemDamage * stressMultiplier;
        }

        //damage, if you treat the patient with the wrong item
        else
        {
            patient.Treatment(-player.currentItem.item.restoreHealth);
            player.CurrentStressLvl += player.currentItem.item.restoreHealth * stressMultiplier;
        }
    }
    #endregion

    #region Coffee and Computer Feature
    /// <summary>
    /// Coffee Feature is activated: stresslevel and player speed are raised
    /// </summary>
    private void DrinkingCoffee()
    {
        if (player.IsDrinkingCoffee && !coffeeMachine.Drinking && !coffeeMachine.IsOnCooldown)
        {
            Debug.Log("asd");
            if (coffeeMachine.Drinking == false) // raises stresslevel after activating the coffee
                player.CurrentStressLvl += coffeeDamage;

            coffeeMachine.Drinking = true;
            coffeeMachine.RefillCup = true;
        }
        player.IsDrinkingCoffee = false;

    }

    /// <summary>
    /// Player has to fullfill the documentation task in the last hour
    /// </summary>
    private void ComputerTaskSuccessCondition()
    {
        if (player.IsAtPc)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                player.IsAtPc = false;
                player.GetComponent<NewPlayerMovement>().enabled = true;
                computer.Canvas.gameObject.SetActive(false);
                computer.ClipBoardCanvas.gameObject.SetActive(false);
                Camera.main.GetComponent<CamPosition>().lastPoint = Camera.main.GetComponent<CamPosition>().currentPoint;
                Camera.main.GetComponent<CamPosition>().MovePoint.CameraOnPc = false;
                Camera.main.transform.position = Camera.main.GetComponent<CamPosition>().lastPoint.position;
                Camera.main.transform.rotation = Camera.main.GetComponent<CamPosition>().CameraRotation;

                if (computer.HintText.text == computer.InputField.text)
                {
                    //Success
                    SoundManager.instance.PlayAudioClip(ESoundeffects.ComputerSuccess, computer.gameObject.GetComponent<AudioSource>());
                    player.GetComponent<Player>().CurrentStressLvl -= documentationReward;
                    Debug.Log("Success DocumentationTask");
                }
                else
                {
                    //Failed
                    SoundManager.instance.PlayAudioClip(ESoundeffects.ComputerFail, computer.gameObject.GetComponent<AudioSource>());
                    player.GetComponent<Player>().CurrentStressLvl += documentationReward;
                    Debug.Log("Failed DocumentationTask");
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// When the max stresslevel is reached, the player loses the game
    /// </summary>
    private void isGameOver()
    {
        if (player.CurrentStressLvl >= player.MaxStressLvl)
        {
            sceneManager.GameOver();
        }
    }

    /// <summary>
    /// resets the item in the inventory after using or droppping
    /// </summary>
    private void ResetItem()
    {
        if (itemSlot.CurrentItem != null && player.currentItem != relocateAPatient)
        {
            Destroy(itemSlot.CurrentItem);
            player.currentItem = null;
            itemSlot.UI_Element = null;
        }
        lastItemIsSaved = false;
        player.currentItem = lastItem;
    }


}
