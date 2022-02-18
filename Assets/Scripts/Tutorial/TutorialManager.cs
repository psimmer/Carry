using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Timer gameTime;
    [SerializeField] GameObject doctor;
    [SerializeField] Transform doctorDocPos;
    [SerializeField] Transform doctorLevelStartPos;
    [SerializeField] GameObject documentationBubbleCanvas;
    [SerializeField] TMP_Text doctorTextField;
    [SerializeField] Transform cameraPositionOverview;
    [SerializeField] GameObject patientTutorialPrefab;
    [SerializeField] Transform patientSpawnPoint;
    [SerializeField] Transform patientBedPos;
    [SerializeField] GameObject popUpPrefab;
    [SerializeField] GameObject releasePopUpPrefab;
    [SerializeField] Transform popUpPos;
    [SerializeField] GameObject laptop;
    [SerializeField] List<Texts> tutorialTexts;
    GameObject currentPatient;
    GameObject currentPopUp;
    float realTime;
    float tutorialTimer;
    int textDirectionIndex = 0;
    bool isSpawned = false;

    #region TutorialCheckList

    bool IsWPressed = false;
    bool IsAPressed = false;
    bool IsSPressed = false;
    bool IsDPressed = false;
    bool IsSpacePressed = false;
    bool playerGrabItem = false;
    bool isLayingInBed = false;
    bool isPopUpSpawned = false;
    bool playerdroppedItem = false;
    bool releaseBool = true;
    bool timerBool = false;

    #endregion



    void Awake()
    {
        doctorTextField.text = $"{tutorialTexts[textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
        documentationBubbleCanvas.gameObject.SetActive(false);
        TutorialLoop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
        }
        MovePatientInBed();
        player.Interact();
        if (!timerBool)
        {
            gameTime.DoubledRealTime();
        }
        else
        {
            RealTime();
        }
        if (textDirectionIndex == 7 && !isSpawned)
        {
            isSpawned = true;
            currentPatient = Instantiate(patientTutorialPrefab);
            currentPatient.transform.position = patientSpawnPoint.position;
            currentPatient.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        if(gameTime.TimeInHours == 18)
        {
            SceneManager.LoadScene(0);
        }
        //doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
        tutorialTimer += Time.deltaTime;
        TutorialLoop();
    }

    private void MovePatientInBed()
    {
        if (player.IsInContact)
        {
            player.currentPatient.transform.position = patientBedPos.position;
            player.currentPatient.transform.rotation = patientBedPos.rotation;
            player.currentPatient.GetComponent<Animator>().SetBool("isLaying", true);
            player.currentPatient.IsInBed = true;
        }
    }

    private void CheckListMovement()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            IsWPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            IsAPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            IsSPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            IsDPressed = true;
        }
    }

    private void CheckForInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsSpacePressed = true;
        }
    }


    private void TutorialLoop()
    {
        if ((int)tutorialTimer == 5)
        {
            if (textDirectionIndex <= 2)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        //ToMove: WASD
        if (tutorialTexts[textDirectionIndex].NumberOfExecution == 3)
        {
            CheckListMovement();
            if (IsWPressed && IsAPressed && IsSPressed && IsDPressed)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                IsDPressed = false;
                IsWPressed = false;
            }
        }
        //ToInteract: Space
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 4)
        {
            CheckForInteractionInput();
            if (IsSpacePressed)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                IsSpacePressed = false;
            }
        }
        //Go and grab a item!
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 5)
        {
            if (player.currentItem != null && !playerGrabItem)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                playerGrabItem = true;
                tutorialTimer = 0;
            }
        }
        //Drop Item
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 6)
        {
            if (Input.GetKeyDown(KeyCode.F) && !playerdroppedItem)
            {
                playerdroppedItem = true;
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
            }
        }
        //Great
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 7)
        {
            if (tutorialTimer >= 3)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        //Patient came in
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 8)
        {
            if (tutorialTimer >= 4)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        //Interact with patient to lay him in bed
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 9)
        {
            if (currentPatient.GetComponent<Patient>().IsInBed && !isLayingInBed)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                isLayingInBed = true;
                tutorialTimer = 0;
            }
        }
        //Great
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 10 && !isPopUpSpawned)
        {
            if ((int)tutorialTimer == 2)
            {
                currentPopUp = Instantiate(popUpPrefab, currentPatient.GetComponent<Patient>().PopUpCanvas);
                currentPatient.GetComponent<Patient>().PopUpCanvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                currentPopUp.transform.position = popUpPos.position;
                Vector3 lookDir = Camera.main.transform.forward;
                currentPopUp.transform.LookAt(currentPopUp.transform.position + lookDir);
                isPopUpSpawned = true;
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        // Patient need your help
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 11)
        {
            if ((int)tutorialTimer >= 3)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                isPopUpSpawned = true;
                tutorialTimer = 0;
            }
        }
        //grab the correct item
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 12)
        {
            if (player.currentItem != null)
            {
                if (Input.GetKeyDown(KeyCode.Space) && player.currentItem.item.task == TaskType.Bandages)
                {
                    doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                    player.IsInContact = false;
                }
            }
        }
        //Interact with the patient to heal him and hold space
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 13)
        {
            if (player.IsInContact)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        currentPopUp.GetComponent<PopUp>().IsHealing = true;
                        player.GetComponent<Animator>().SetBool("isTreating", true);
                        currentPatient.GetComponent<Patient>().CurrentParticles = Instantiate(currentPatient.GetComponent<Patient>().HealingParticles, currentPatient.GetComponent<Patient>().transform);
                        ParticleSystem[] ParticleLoops = currentPatient.GetComponent<Patient>().GetComponentsInChildren<ParticleSystem>();
                    }
                }
                else if (!Input.GetKey(KeyCode.Space))
                {
                    Destroy(currentPatient.GetComponent<Patient>().CurrentParticles);

                    player.IsInContact = false;
                    currentPopUp.GetComponent<PopUp>().IsHealing = false;
                    player.GetComponent<Animator>().SetBool("isTreating", false);
                    player.GetComponent<NewPlayerMovement>().enabled = true;
                    Destroy(currentPopUp);
                    Damage(currentPatient.GetComponent<Patient>());
                }
            }
            if (currentPopUp != null)
            {
                if (currentPopUp.GetComponent<PopUp>().RadialBarImage.fillAmount >= 1)
                {
                    Destroy(currentPopUp);
                    Destroy(currentPatient.GetComponent<Patient>().CurrentParticles);
                    currentPopUp = null;
                    player.GetComponent<Animator>().SetBool("isTreating", false);
                    doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                    currentPatient.GetComponent<Patient>().CurrentHP = 100;
                    tutorialTimer = 0;
                }
            }
        }
        //great you did it
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 14)
        {
            if (tutorialTimer >= 3)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                isPopUpSpawned = false;
                tutorialTimer = 0;
            }
        }
        //release patient
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 15)
        {
            if (currentPatient != null)
            {
                if (currentPatient.GetComponent<Patient>().CurrentHP >= 100 && !isPopUpSpawned)
                {
                    isPopUpSpawned = true;
                    currentPopUp = Instantiate(releasePopUpPrefab, currentPatient.GetComponent<Patient>().PopUpCanvas);
                    currentPopUp.transform.position = popUpPos.position;
                    Vector3 lookDir = Camera.main.transform.forward;
                    currentPopUp.transform.LookAt(currentPopUp.transform.position + lookDir);
                    player.IsInContact = false;
                }
                if (Input.GetKeyDown(KeyCode.Space) && player.IsInContact && releaseBool)
                {
                    Destroy(currentPopUp);
                    currentPopUp = null;
                    player.IsInContact = false;
                    currentPatient.GetComponent<Patient>().HealthBarCanvas.gameObject.SetActive(false);
                    currentPatient.GetComponent<Patient>().PopUpCanvas.gameObject.SetActive(false);
                    currentPatient.GetComponent<Animator>().SetBool("isWalking", true);
                    currentPatient.transform.position = currentPatient.GetComponent<Patient>().LeaveHospital.position;
                    currentPatient.GetComponent<Patient>().IsReleasing = true;
                    currentPatient = null;
                    releaseBool = false;
                    gameTime.TimeInHours = 17;
                    gameTime.RealTime = 0;
                    timerBool = true;
                    doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                }
            }
        }
        //Lets go to computer
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 16)
        {
            if (player.IsAtPc)
            {
                doctor.transform.position = doctorDocPos.position;
                doctor.transform.rotation = doctorDocPos.rotation;
                player.GetComponent<NewPlayerMovement>().enabled = false;
                documentationBubbleCanvas.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if(laptop.GetComponent<Computer>().ClipBoardCanvas.GetComponentInChildren<TMP_Text>().text != laptop.GetComponent<Computer>().Canvas.GetComponentInChildren<TMP_InputField>().text)
                    {
                        player.CurrentStressLvl += 50;
                    }
                    Camera.main.transform.position = cameraPositionOverview.position;
                    Camera.main.transform.rotation = cameraPositionOverview.rotation;
                    doctor.transform.position = doctorLevelStartPos.position;
                    doctor.transform.rotation = doctorLevelStartPos.rotation;
                    player.GetComponent<NewPlayerMovement>().enabled = true;
                    documentationBubbleCanvas.gameObject.SetActive(false);
                    laptop.GetComponent<Computer>().ClipBoardCanvas.SetActive(false);
                    laptop.GetComponent<Computer>().Canvas.SetActive(false);
                    tutorialTimer = 0;
                    doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                }
            }
        }
        //Litlle tip
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 17)
        {
            if(tutorialTimer >= 4)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        //You can zoom
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 18)
        {
            if(tutorialTimer >= 5)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        //Good Luck
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 19)
        {
            if(tutorialTimer >= 3)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void Damage(Patient patient)
    {
        patient.HasPopUp = false;

        if (player.currentItem == null)
        {
            patient.Treatment(-player.NoItemDamage);
            player.CurrentStressLvl += player.NoItemDamage * 1;
        }
        else
        {
            patient.Treatment(-player.currentItem.item.restoreHealth);
            player.CurrentStressLvl += player.currentItem.item.restoreHealth * 1;
        }
    }

    public void RealTime()
    {
        realTime += Time.deltaTime;

        if ((int)realTime <= 9 && gameTime.TimeInHours <= 9)
            gameTime.TimeText.text = "0" + gameTime.TimeInHours.ToString() + ":" + "0" + (int)realTime;

        if ((int)realTime <= 9 && gameTime.TimeInHours > 9)
            gameTime.TimeText.text = gameTime.TimeInHours.ToString() + ":" + "0" + (int)realTime;

        if ((int)realTime >= 10 && gameTime.TimeInHours <= 9)
            gameTime.TimeText.text = "0" + gameTime.TimeInHours.ToString() + ":" + (int)realTime;

        if ((int)realTime > 9 && gameTime.TimeInHours > 9)
            gameTime.TimeText.text = gameTime.TimeInHours.ToString() + ":" + (int)realTime;

        if ((int)realTime == 60)
        {
            gameTime.TimeInHours++;
            realTime = 0;
            gameTime.TimeText.text = "0" + gameTime.TimeInHours.ToString() + ":" + (int)realTime;
        }
    }
}

[System.Serializable]
public class Texts
{
    [SerializeField] string text;
    public string Text => text;

    [SerializeField] string text2;
    public string Text2 => text2;

    [SerializeField] string text3;
    public string Text3 => text3;

    [SerializeField] int numberOfExecution;
    public int NumberOfExecution => numberOfExecution;
}