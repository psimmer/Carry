using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PatientSpawner : MonoBehaviour, ISaveSystem
{
    [SerializeField] int minRandomTime;
    [SerializeField] int maxRandomTime;
    [SerializeField] List<GameObject> differentPatients;
    [SerializeField] List<GameObject> patientList;
    public List<GameObject> PatientList { get { return patientList; } set { patientList = value; } }

    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] List<Bed> bedList;
    public List<Bed> BedList { get { return bedList; } set { bedList = value; } }


    //timer Stuff
    float spawnTimer;
    float randomTime;

    private void Start()
    {
        PopUp.e_RemovePatient += RemovePatientFromList;
        patientList.AddRange(GameObject.FindGameObjectsWithTag("Patient"));
        randomTime = Random.Range(minRandomTime, maxRandomTime);
    }


    private void Update()
    {
        spawnTimer += Time.deltaTime;
        SpawnPatient();
    }

    public void SpawnPatient()
    {
        if (patientList.Count < bedList.Count && spawnTimer >= randomTime)
        {
            spawnTimer = 0;
            Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
            //if (randomSpawn.GetComponent<SpawnPoint>().IsFree)
            //{
                GameObject newPatient = Instantiate(differentPatients[Random.Range(0, differentPatients.Count)], randomSpawn);
                patientList.Add(newPatient);
                newPatient.GetComponent<Patient>().CurrentIllness = TaskType.AssignBed;
                //randomSpawn.GetComponent<SpawnPoint>().IsFree = false;
                randomTime = Random.Range(minRandomTime, maxRandomTime);
            //}
        }
    }

    public void MoveToBed(Patient patient)
    {
        for (int i = 0; i < bedList.Count; i++)
        {
            if (!bedList[i].IsPatientInBed && bedList[i].CurrentPatient == null)
            {
                patient.transform.position = bedList[i].BedPos.position;
                patient.transform.rotation = bedList[i].BedPos.rotation;
                //patient.transform.rotation = Quaternion.Euler(0, 0, 0)
                patient.Healthbar.transform.parent.rotation = Quaternion.Euler(0,0,0); // these 2 lines position the healthbar on the whiteboard when you move the patient
                bedList[i].IsPatientInBed = true;
                bedList[i].CurrentPatient = patient;
                patient.CurrentIllness = (TaskType)Random.Range(0, 6);
                patient.IsInBed = true;
                patient.GetComponent<Animator>().SetBool("isLaying", true);
                return;
            }
        }
    }

    public void RemovePatientFromList(Patient patient)
    {
        patientList.Remove(patient.gameObject);
    }


    public void SaveData()
    {
        //BinaryFormatter formatter = new BinaryFormatter();

        //string path = Application.persistentDataPath + "/player.carry";
        //Debug.Log("Save File location: " + path);
        //FileStream stream = new FileStream(path, FileMode.Create);
        ////PlayerData data = new PlayerData(player, timeLeft);

        ////save each patient from the list and the count!
        //formatter.Serialize(stream, patientList.Count);
        //foreach (var patient in patientList)
        //{
        //    patient.GetComponent<Patient>().SaveToStream(stream);
        //}

        //formatter.Serialize(stream, bedList);
        //stream.Close();
    }

    public void LoadData()
    {
        //throw new System.NotImplementedException();
    }
}
