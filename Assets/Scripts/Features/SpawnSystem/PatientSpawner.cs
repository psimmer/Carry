using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSpawner : MonoBehaviour
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
            if (randomSpawn.GetComponent<SpawnPoint>().IsFree)
            {
                GameObject newPatient = Instantiate(differentPatients[Random.Range(0, differentPatients.Count)], randomSpawn);
                patientList.Add(newPatient);
                newPatient.GetComponent<Patient>().CurrentIllness = TaskType.AssignBed;
                randomSpawn.GetComponent<SpawnPoint>().IsFree = false;
                randomTime = Random.Range(minRandomTime, maxRandomTime);
            }
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
                Vector3 lookDir = Camera.main.transform.forward;
                patient.Healthbar.transform.parent.LookAt(patient.Healthbar.transform.parent.position + lookDir);
                bedList[i].IsPatientInBed = true;
                bedList[i].CurrentPatient = patient;
                patient.CurrentIllness = (TaskType)Random.Range(0, 2);
                return;
            }
        }
    }




}
