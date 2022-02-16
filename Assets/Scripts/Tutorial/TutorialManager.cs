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
    [SerializeField] List<Texts> tutorialTexts;
    GameObject currentPatient;
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

        if(textDirectionIndex == 7 && !isSpawned)
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
        if((int)tutorialTimer == 5)
        {
            if(textDirectionIndex <= 2)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        if(tutorialTexts[textDirectionIndex].NumberOfExecution == 3)
        {
            CheckListMovement();
            if(IsWPressed && IsAPressed && IsSPressed && IsDPressed)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                IsDPressed = false;
                IsWPressed = false;
            }
        }
        else if(tutorialTexts[textDirectionIndex].NumberOfExecution == 4)
        {
            CheckForInteractionInput();
            if (IsSpacePressed)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                IsSpacePressed = false;
            }
        }
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 5)
        {
            if(player.currentItem != null && !playerGrabItem)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                playerGrabItem = true;
                tutorialTimer = 0;
            }
        }
        else if(tutorialTexts[textDirectionIndex].NumberOfExecution == 6)
        {
            if(tutorialTimer >= 3)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 7)
        {
            if(tutorialTimer >= 4)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                tutorialTimer = 0;
            }
        }
        else if (tutorialTexts[textDirectionIndex].NumberOfExecution == 8)
        {
            if (currentPatient.GetComponent<Patient>().IsInBed && !isLayingInBed)
            {
                doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
                isLayingInBed = true;
            }
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