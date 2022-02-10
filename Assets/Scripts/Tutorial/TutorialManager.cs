using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TMP_Text doctorTextField;
    [SerializeField] List<Texts> tutorialTexts;

    float tutorialTimer;
    int textDirectionIndex = 0;


    void Awake()
    {
        doctorTextField.text = tutorialTexts[0].Text;
        TutorialLoop();
    }

    private void Update()
    {
        tutorialTimer += Time.deltaTime;
        TutorialLoop();
    }
    

    private void TutorialLoop()
    {
        if(tutorialTimer >= 5 && tutorialTexts[textDirectionIndex].NumberOfExecution == textDirectionIndex) doctorTextField.text = tutorialTexts[++textDirectionIndex].Text; tutorialTimer = 0f;
    }


}

[System.Serializable]
public class Texts
{
    [SerializeField] string text;
    public string Text => text;

    [SerializeField] int numberOfExecution;
    public int NumberOfExecution => numberOfExecution;
}