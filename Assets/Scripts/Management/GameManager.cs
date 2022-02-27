using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Important managing references
    [Header("Important managing references")]
    [SerializeField] Player player;
    [SerializeField] SceneMan sceneManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] PatientSpawner patientSpawner;
    [SerializeField] private Animator playerAnimator;
    [Tooltip("This is needed in level 4 only!")]
    [SerializeField] AudioSource level4Music;
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
    [SerializeField] GameObject items;
    [SerializeField] List<Outline> outlineList;
    #endregion

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 4")
            StartCoroutine(DecreaseStressReductionMultiplier());
        Player.e_OnDocumentationStart += SetItemOutlines;
    }
    void Update()
    {
        //for testing in the build
        Godmode();


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

    /// <summary>
    /// with 'F2' you can skip to the next level and 'F1' go back one level. for testing in the build
    /// </summary>
    private void Godmode()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (SceneManager.GetActiveScene().buildIndex == 4)
                return;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + -1);
            Destroy(GameObject.Find("DontDestroyOnLoad"));
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (SceneManager.GetActiveScene().buildIndex == 7)
                return;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Destroy(GameObject.Find("DontDestroyOnLoad"));
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.CurrentStressLvl = 0f;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            dayTime.TimeInHours++;
        }
    }

    #region Treating a Patient
    /// <summary>
    /// Success: patient gets healed, stress level of the player will decrease. Fail: patient loses health, stresslevel
    /// of the player rises
    /// </summary>
    /// <param name="patient">treated patient</param>
    public void Treatment(Patient patient)
    {

        if (player.IsInContact && patient != null && patient.IsInBed && !patient.IsReleasing) // the patient.isinbed fixed the issue with the null reference
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
                if (patient.CurrentPopUp == null)
                {
                    Damage(patient);
                    return;
                }
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
            else if (Input.GetKeyDown(KeyCode.Space) && patient.CurrentIllness == player.currentItem.item.task && patient.CurrentPopUp != null)
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

            else if(Input.GetKeyDown(KeyCode.Space) && patient.CurrentIllness == player.currentItem.item.task && patient.CurrentPopUp == null)
            {
                Damage(patient);
                ResetItem();
                if (patient.CurrentPopUp != null)
                    Destroy(patient.CurrentPopUp);
                player.IsInContact = false;
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
                Camera.main.GetComponent<CamPosition>().MovePoint.IsCameraFixed = true;
                Camera.main.GetComponent<CamPosition>().lastPoint = Camera.main.GetComponent<CamPosition>().CameraOverview;
                Camera.main.GetComponent<CamPosition>().MovePoint.CameraOnPc = false;
                Camera.main.transform.position = Camera.main.GetComponent<CamPosition>().CameraOverview.position;
                Camera.main.transform.rotation = Camera.main.GetComponent<CamPosition>().CameraOverview.rotation;


                // Alejandro's suggestion: we accept values that are not capitalized or without the dot at the end, the lines marked with "//flexible" are responsible for this
                
                //Debug.Log(computer.InputField.text.ToLower()); //flexible check
                //Debug.Log(computer.HintText.text.ToLower());  //flexible check

                //if (computer.HintText.text == computer.InputField.text) old condition (not flexible)
                if (computer.HintText.text.ToLower().Contains(computer.InputField.text.ToLower()) && computer.InputField.text.Length >= computer.HintText.text.Length - 1) //flexible
                {
                    //Success
                    SoundManager.instance.PlayAudioClip(ESoundeffects.ComputerSuccess, computer.gameObject.GetComponent<AudioSource>());
                    player.GetComponent<Player>().CurrentStressLvl -= documentationReward;
                    SetItemOutlines(true);
                }
                else
                {
                    //Failed
                    SoundManager.instance.PlayAudioClip(ESoundeffects.ComputerFail, computer.gameObject.GetComponent<AudioSource>());
                    player.GetComponent<Player>().CurrentStressLvl += documentationReward;
                    SetItemOutlines(true);

                }

            }
        }
    }
    #endregion

    private void SetItemOutlines(bool enableOutline)
    {
        if (computer.OneTimeBool)
            return;


        for (int i = 0; i < outlineList.Count; i++)
        {
            if (outlineList[i] != null)
                outlineList[i].gameObject.GetComponent<Outline>().enabled = enableOutline;
        }
    }
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

    #region Level 4 only stuff
    IEnumerator DecreaseStressReductionMultiplier()
    {
        while(true)
        {
            stressReductionMultiplier -= 1f / 360f;
            if(dayTime.TimeInHours == 6)
                level4Music.pitch = 0.94f;
            if (dayTime.TimeInHours == 7)
                level4Music.pitch = 0.95f;
            if (dayTime.TimeInHours == 8)
                level4Music.pitch = 0.96f;
            if (dayTime.TimeInHours == 9)
                level4Music.pitch = 0.97f;
            if (dayTime.TimeInHours == 10)
                level4Music.pitch = 0.98f;
            if (dayTime.TimeInHours == 11)
                level4Music.pitch = 0.99f;
            if (dayTime.TimeInHours == 12)
                level4Music.pitch = 1f;
            if (dayTime.TimeInHours == 13)
                level4Music.pitch = 1.01f;
            if (dayTime.TimeInHours == 14)
                level4Music.pitch = 1.02f;
            if (dayTime.TimeInHours == 15)
                level4Music.pitch = 1.03f;
            if (dayTime.TimeInHours == 16)
                level4Music.pitch = 1.04f;
            if (dayTime.TimeInHours == 17)
                level4Music.pitch = 1.05f;
            yield return new WaitForSeconds(1);
        }
    }

    #endregion
}
