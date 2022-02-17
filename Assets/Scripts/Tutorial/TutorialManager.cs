using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] TMP_Text doctorTextField;
    [SerializeField] GameObject patientTutorialPrefab;
    [SerializeField] Transform patientSpawnPoint;
    [SerializeField] Transform patientBedPos;
    [SerializeField] GameObject popUpPrefab;
    [SerializeField] Transform popUpPos;
    [SerializeField] List<Texts> tutorialTexts;
    GameObject currentPatient;
    GameObject currentPopUp;
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
    #endregion



    void Awake()
    {
        doctorTextField.text = $"{tutorialTexts[textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
        TutorialLoop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
        }
        MovePatientInBed();
        player.Interact();

        if (textDirectionIndex == 7 && !isSpawned)
        {
            isSpawned = true;
            currentPatient = Instantiate(patientTutorialPrefab);
            currentPatient.transform.position = patientSpawnPoint.position;
            currentPatient.transform.eulerAngles = new Vector3(0, 90, 0);
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
                currentPopUp = Instantiate(popUpPrefab, popUpPos);
                currentPopUp.transform.position = popUpPos.position;
                Vector3 lookDir = Camera.main.transform.forward;
                currentPopUp.transform.LookAt(currentPopUp.transform.position + lookDir);
                currentPopUp.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
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
                        //while (player.IsInContact)
                        //{
                        //    for (int i = 0; i < ParticleLoops.Length; i++)
                        //    {
                        //        ParticleLoops[i].loop = false;
                        //    }
                        //    player.GetComponent<NewPlayerMovement>().enabled = false;
                        //}
                        //StartCoroutine(TreatmentProgress(currentPatient.GetComponent<Patient>()));
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
                    doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                }
            }

            //}
            //if()
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
                        //player.CurrentStressLvl -= player.currentItem.item.restoreHealth * stressReductionMultiplier;

                        player.GetComponent<Animator>().SetBool("isTreating", false);
                        Destroy(patient.CurrentParticles, patient.ParticlesDuration);
                        player.currentItem = null;
                        player.IsInContact = false;
                        player.GetComponent<NewPlayerMovement>().enabled = true;
                        Destroy(patient.GetComponentInChildren<PopUp>().gameObject);
                        StopCoroutine(TreatmentProgress(patient));
                    }
                    //Fail
                    else if (!Input.GetKey(KeyCode.Space))
                    {
                        
                        player.GetComponent<Animator>().SetBool("isTreating", false);
                        Destroy(patient.CurrentParticles);
                        currentPatient.GetComponent<Patient>().SpawnParticles(currentPatient.GetComponent<Patient>().DamageParticles, 3);
                        Damage(patient);
                        player.currentItem = null;
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