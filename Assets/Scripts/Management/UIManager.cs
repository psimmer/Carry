using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button firstButton;

    GameObject pauseElements;
    GameObject optionsElements;
    GameObject mainMenuElements;


    public void Awake()
    {
        pauseElements = GameObject.Find("PauseMenu");
        if (pauseElements != null)
        {
            pauseElements.SetActive(false);
        }

        optionsElements = GameObject.Find("OptionsMenu");
        if (optionsElements != null)
        {
            optionsElements.SetActive(false);
        }

        mainMenuElements = GameObject.Find("MainMenu");
        if (mainMenuElements != null)
        {
            mainMenuElements.SetActive(true);
        }

        if (firstButton != null)
        {
            firstButton.Select();
        }
    }

    /// <summary>
    /// pauses the game and sets the Pause UI active
    /// </summary>
    public void GamePaused()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0f;
                pauseElements.SetActive(true);
                //TODO: dimm light
                //TODO: Pause AUdio
                //TODO: Pause Camera
            }
            else
            {
                pauseElements.SetActive(false);
                Time.timeScale = 1f;

                //TODO: dimm light
                //TODO: play audio
                //TODO: Play Camera

            }
        }
    }

    //i think following two methods can be optimized. iam tired, i will look over it another time
    public void EnterOptions()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            pauseElements.SetActive(false);
            optionsElements.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuElements.SetActive(false);
            if (optionsElements != null)
                optionsElements.SetActive(true);
        }
    }

    public void LeaveOptions()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            pauseElements.SetActive(true);
            optionsElements.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuElements.SetActive(true);
            optionsElements.SetActive(false);
        }
    }

    public void UpdateHealthBar(Transform patientContainer)
    {
        for (int i = 0; i < patientContainer.childCount; i++)
        {
            Patient patient = patientContainer.GetChild(i).GetComponent<Patient>();
            GameObject instantiatedHealthbar = patient.InstantiatedHealthbar;
            if (patient != null)
            {
                Vector3 patientPos = patient.transform.position;

                if (patient.InstantiatedHealthbar != null)
                {
                    instantiatedHealthbar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(patientPos.x,
                        patientPos.y + 0.5f, patientPos.z));
                }
                else
                {
                    patient.InstantiatedHealthbar = Instantiate(patient.Prefab, transform);
                }
            }
        }
    }


    public void ManagePopUps(List<Patient> patientList, Dictionary<int, GameObject> popUpList, List<GameObject> popUps)
    {
        //foreach (GameObject value in popUpList.Values)
        //{
        //    Debug.Log($"{value}");
        //}

        foreach (Patient patient in patientList)
        {
            if (patient != null)    //do you need this if in a foreach? is it even possible, that a patient is null?
            {
                int patientID = patient.PatientID;

                if (!patient.HasTask && !patient.IsPopping)
                {
                    //Debug.Log($"patient {patient.PatientID}has no task");
                    StartCoroutine("PopUpTimer", patient);
                }

                if (patient.IsPopping && !popUpList.ContainsKey(patientID))
                {
                    //Debug.Log($"patient {patient.PatientID} is popping");
                    foreach (GameObject task in popUps)
                    {
                        if (task.GetComponent<PopUp>().TaskType == patient.CurrentIllness)
                        {
                            if (popUpList.ContainsKey(patientID))
                                continue;

                            GameObject currentPopUp = Instantiate(task.GetComponent<PopUp>().Prefab, patient.transform);
                            currentPopUp.transform.SetParent(GameObject.Find("UIManager").transform, false);
                            currentPopUp.transform.SetAsFirstSibling();
                            popUpList.Add(patient.PatientID, currentPopUp);
                            patient.HasTask = true;
                            patient.IsPopping = false;
                        }
                    }
                }

                if (popUpList.ContainsKey(patientID))
                {
                    GameObject popUp;
                    bool success = false;
                    success = popUpList.TryGetValue(patientID, out popUp);
                    if (!success)
                        return;
                    popUp.transform.position = Camera.main.WorldToScreenPoint(new Vector3(patient.transform.position.x,
                        patient.transform.position.y + 2, patient.transform.position.z));
                }
            }
        }
    }
    public IEnumerator PopUpTimer(Patient patient)
    {
        int randomTime = UnityEngine.Random.Range(10, 15);
        int maxRandomTime = UnityEngine.Random.Range(20, 25);
        yield return new WaitForSeconds(UnityEngine.Random.Range(randomTime, maxRandomTime));  // Lukas likes this random timer method: I tooked it out to test smth Random.Range(minTimer, maxTimer)
        patient.IsPopping = true;
        //Debug.Log($"patient {patient.patientID} finished waiting and is popping");
        StopCoroutine("PopUpTimer");
        //private void RemovePopUpFromList(int patientID)
        //{
        //    GameObject removeIfExists;
        //    popUpList.TryGetValue(patientID, out removeIfExists);
        //    if(removeIfExists != null)
        //        popUpList.Remove(patientID);
        //}






    }
}