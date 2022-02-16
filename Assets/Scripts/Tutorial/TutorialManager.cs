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
        doctorTextField.text = $"{tutorialTexts[textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
        TutorialLoop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            doctorTextField.text = $"{tutorialTexts[++textDirectionIndex].Text} \n {tutorialTexts[textDirectionIndex].Text2} \n {tutorialTexts[textDirectionIndex].Text3}";
        }

        //tutorialTimer += Time.deltaTime;
        //TutorialLoop();
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

    [SerializeField] string text2;
    public string Text2 => text2;

    [SerializeField] string text3;
    public string Text3 => text3;

    [SerializeField] int numberOfExecution;
    public int NumberOfExecution => numberOfExecution;
}