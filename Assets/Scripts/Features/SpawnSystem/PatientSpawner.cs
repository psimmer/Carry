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
        if (GlobalData.instance.IsSaveFileLoaded)
        {
            Patient[] pArray = FindObjectsOfType<Patient>();
            for (int i = 0; i < pArray.Length; i++)
            {
                Destroy(pArray[i].gameObject);
            }
            LoadData();
        }
    }


    private void Update()
    {
        if (patientList.Count < bedList.Count)
        {
            spawnTimer += Time.deltaTime;
        }
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
                SoundManager.instance.PlayAudioClip(ESoundeffects.NewPatientArrived, GetComponent<AudioSource>());
                int differentPatientsIndex = Random.Range(0, differentPatients.Count);
                GameObject newPatient = Instantiate(differentPatients[differentPatientsIndex], randomSpawn);
                patientList.Add(newPatient);
                newPatient.GetComponent<Patient>().DifferentPatientsIndex = differentPatientsIndex;
                newPatient.GetComponent<Patient>().CurrentIllness = TaskType.AssignBed;
                randomSpawn.GetComponent<SpawnPoint>().IsFree = false;
                randomTime = Random.Range(minRandomTime, maxRandomTime);
                GameObject newParticles = Instantiate(newPatient.GetComponent<Patient>().SpawningParticles, randomSpawn.position + Vector3.up, Quaternion.identity);
                Destroy(newParticles, newPatient.GetComponent<Patient>().ParticlesDuration);
                Debug.Log(randomSpawn);
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
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataPatientSpawner.carry";
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        //serialize patientlist
        formatter.Serialize(stream, patientList.Count);
        foreach (var p in patientList)
        {
            PatientData data = new PatientData(p.GetComponent<Patient>());
            formatter.Serialize(stream, data);
        }

        //serialize bedlist
        //formatter.Serialize(stream, bedList.Count);
        //foreach (var b in bedList)
        //{
        //    BedData data = new BedData(b);
        //    formatter.Serialize(stream, data);
        //}

        //serialize spawnpoints
        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataPatientSpawner.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Debug.Log("Save File loaded: " + path);

            //deseriialize patientlist
            int patientListCount = (int)formatter.Deserialize(stream);
            List<GameObject> loadedPatientList = new List<GameObject>();
            for (int i = 0; i < patientListCount; i++)
            {
                PatientData data = (PatientData)formatter.Deserialize(stream);
                GameObject go = Instantiate(differentPatients[data.patientID]);
                Patient p = go.GetComponent<Patient>();

                //setting patient position
                Vector3 vector = new Vector3(data.position[0], data.position[1], data.position[2]);
                go.transform.position = vector;
                Quaternion quat = new Quaternion(data.rotation[0], data.rotation[1], data.rotation[2], data.rotation[3]);
                go.transform.rotation = quat;

                p.DifferentPatientsIndex = data.patientID;
                p.CurrentHP = data.currentHP;
                p.CurrentIllness = (TaskType)data.currentIllnes;
                p.IsPopping = data.isPopping;
                p.HasTask = data.hasTask;
                p.IsInBed = data.isInBed;
                p.HasPopUp = data.hasPopUp;
                p.IsReleasing = data.isReleasing;

                if(p.IsInBed)
                    p.GetComponent<Animator>().SetBool("isLaying", true);

                loadedPatientList.Add(go);

            }
            patientList = loadedPatientList;

            //deserialize Bedlist
            //int bedcount = (int)formatter.Deserialize(stream);
            //List<Bed> loadedBedList = new List<Bed>();
            //for (int i = 0; i < bedcount; i++)
            //{
            //    BedData data = (BedData)formatter.Deserialize(stream);
            //}

            //deserialize spawnpoints
            stream.Close();

        }
    }
}
