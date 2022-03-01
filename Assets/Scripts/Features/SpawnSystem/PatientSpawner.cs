using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class PatientSpawner : MonoBehaviour, ISaveSystem
{
    #region Variables
    [Header("Patient Spawning time")]
    [Tooltip("Time window for spawning patients (minimum in seconds")]
    [SerializeField] int minRandomTime;
    public int MinRandomTime { get { return minRandomTime; } set { minRandomTime = value; } }
    [Tooltip("Time window for spawning patients (maximum in seconds")]
    [SerializeField] int maxRandomTime;
    public int MaxRandomTime { get { return MaxRandomTime; } set { maxRandomTime = value; } }
    [Tooltip("Possible tasks that are available: Lvl 1 = 3; Lvl 2 = 5; Lvl 3 = 7; Lvl 4 = 7")]
    [SerializeField] int maxTaskIndex;


    [Header("Patient idle values")]
    [Tooltip("Damage taken every 5 seconds when sitting in the waiting area")]
    [SerializeField] int patientIdleDamage;
    [Tooltip("Stress level raise after a sitting patient dies")]
    [SerializeField] int idleDeathStressDmg;

    [Header ("Random health range for patients")]

    [Tooltip("range for the random HP that the patient spawns with (minimum")]
    [SerializeField] private int minCurrentHp;
    [Tooltip("range for the random HP that the patient spawns with (minimum")]
    [SerializeField] private int maxCurrentHp;

    [SerializeField] Transform cameraOverViewPoint;
    [SerializeField] List<GameObject> differentPatients;
    [SerializeField] List<GameObject> patientList;
    public List<GameObject> PatientList { get { return patientList; } set { patientList = value; } }

    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] List<Bed> bedList;
    public List<Bed> BedList { get { return bedList; } set { bedList = value; } }

    //timer 
    float spawnTimer;
    float randomTime;
    private void Awake()
    {
        cameraOverViewPoint = GameObject.Find("CameraStartPosition").transform;
    }
    #endregion
    private void Start()
    {
        Patient.e_deletePatientFromList += RemovePatientFromList;
        PopUp.e_RemovePatient += RemovePatientFromList;
        patientList.AddRange(GameObject.FindGameObjectsWithTag("Patient"));
        randomTime = Random.Range(minRandomTime, maxRandomTime);

        if (GlobalData.instance.IsSaveFileLoaded)
        {
            //deleting the patient list before loading the Save File
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

    /// <summary>
    /// As soon as a bed is free, a new patient will spawn after a random time
    /// </summary>
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
                newPatient.GetComponent<Patient>().HealthBarCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
                newPatient.GetComponent<Patient>().MaxTaskIndex = maxTaskIndex;
                newPatient.GetComponent<Patient>().DifferentPatientsIndex = differentPatientsIndex;
                newPatient.GetComponent<Patient>().CurrentIllness = TaskType.AssignBed;
                randomSpawn.GetComponent<SpawnPoint>().IsFree = false;
                randomTime = Random.Range(minRandomTime, maxRandomTime);
                GameObject newParticles = Instantiate(newPatient.GetComponent<Patient>().SpawningParticles, randomSpawn.position + Vector3.up, Quaternion.identity);
                Destroy(newParticles, newPatient.GetComponent<Patient>().ParticlesDuration);
                newPatient.GetComponent<Patient>().CurrentHP = Random.Range(minCurrentHp, maxCurrentHp);
                newPatient.GetComponent<Patient>().PatientIdleDamage = patientIdleDamage;
                newPatient.GetComponent<Patient>().IdleDeathStressDmg = idleDeathStressDmg;

            }
        }
    }

    /// <summary>
    /// Moves the Patient from the Hallway to the bed and sets the important variables
    /// </summary>
    /// <param name="patient">The patient that the player interacts</param>
    public void MoveToBed(Patient patient)
    {
        for (int i = 0; i < bedList.Count; i++)
        {
            if (!bedList[i].IsPatientInBed && bedList[i].CurrentPatient == null)
            {
                patient.transform.position = bedList[i].BedPos.position;
                patient.transform.rotation = bedList[i].BedPos.rotation;
                patient.Healthbar.transform.parent.rotation = Quaternion.Euler(0, 0, 0); // these 2 lines position the healthbar on the whiteboard when you move the patient
                bedList[i].IsPatientInBed = true;
                bedList[i].CurrentPatient = patient;
                patient.CurrentIllness = (TaskType)Random.Range(0, maxTaskIndex);
                patient.IsInBed = true;
                patient.CurrentBed = bedList[i].gameObject;
                patient.GetComponent<Animator>().SetBool("isLaying", true);
                return;
            }
        }
    }

    /// <summary>
    /// Only used after loading the game. Moves the loaded patient to the correct Bed and sets all the important values
    /// </summary>
    /// <param name="patient">Patient that has to be moved</param>
    /// <param name="bedname">Same Bed, which the patient was after saving the game</param>
    public void MoveToBed(Patient patient, string bedname)
    {
        for (int i = 0; i < bedList.Count; i++)
        {
            if (bedList[i].name == bedname)
            {
                patient.transform.position = bedList[i].BedPos.position;
                patient.transform.rotation = bedList[i].BedPos.rotation;
                patient.Healthbar.transform.parent.rotation = Quaternion.Euler(0, 0, 0); // these 2 lines position the healthbar on the whiteboard when you move the patient
                bedList[i].IsPatientInBed = true;
                bedList[i].CurrentPatient = patient;
                patient.CurrentIllness = (TaskType)Random.Range(0, maxTaskIndex);
                patient.IsInBed = true;
                patient.CurrentBed = bedList[i].gameObject;
                patient.GetComponent<Animator>().SetBool("isLaying", true);
            }
        }
    }

    public void RemovePatientFromList(Patient patient)
    {
        patientList.Remove(patient.gameObject);
    }

    #region Save/Load Methods
    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataPatientSpawner.carry";
        FileStream stream = new FileStream(path, FileMode.Create);

        //serialize patientlist
        formatter.Serialize(stream, patientList.Count);
        foreach (var p in patientList)
        {
            PatientData data = new PatientData(p.GetComponent<Patient>());
            formatter.Serialize(stream, data);
        }

        //serialize bedlist
        //foreach (var b in bedList)
        //{
        //    BedData data = new BedData(b);
        //    formatter.Serialize(stream, data);
        //}

        //serialize spawnpoints
        formatter.Serialize(stream, spawnPoints.Count);
        foreach (var spawnpoint in spawnPoints)
        {
            formatter.Serialize(stream, spawnpoint.GetComponent<SpawnPoint>().IsFree);
        }
        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataPatientSpawner.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //deseriialize patientlist
            int patientListCount = (int)formatter.Deserialize(stream);
            List<GameObject> loadedPatientList = new List<GameObject>();
            for (int i = 0; i < patientListCount; i++)
            {
                PatientData data = (PatientData)formatter.Deserialize(stream);
                GameObject go = Instantiate(differentPatients[data.differentPatientsIndex]);
                Patient p = go.GetComponent<Patient>();

                

                p.DifferentPatientsIndex = data.differentPatientsIndex;
                p.CurrentHP = data.currentHP;
                p.CurrentIllness = (TaskType)data.currentIllnes;
                p.IsPopping = data.isPopping;
                p.HasTask = data.hasTask;
                p.IsInBed = data.isInBed;
                p.HasPopUp = data.hasPopUp;
                p.IsReleasing = data.isReleasing;
                p.MaxTaskIndex = data.maxTaskIndex;
                foreach (Bed b in bedList)
                {
                    if (b.name == data.currentBed)
                    {
                        MoveToBed(p, data.currentBed);
                    }
                    else
                    {
                        //setting patient position
                        Vector3 vector = new Vector3(data.position[0], data.position[1], data.position[2]);
                        go.transform.position = vector;
                        Quaternion quat = new Quaternion(data.rotation[0], data.rotation[1], data.rotation[2], data.rotation[3]);
                        go.transform.rotation = quat;
                    }

                }

                loadedPatientList.Add(go);

            }
            patientList = loadedPatientList;

            //deserialize Bedlist
            //List<Bed> loadedBedList = new List<Bed>();
            //for (int i = 0; i < bedList.Count; i++)
            //{
            //    BedData data = (BedData)formatter.Deserialize(stream);
            //    bedList[i].IsPatientInBed = data.isPatientInBed;
            //    bedList[i].SetHealthBarAndPopUpSpawnPos = data.setHealthBarAndPopUpSpawnPos;
            //    bedList[i].Timer = data.timer;
            //}

            //deserialize spawnpoints
            int spawnPointsCount = (int)formatter.Deserialize(stream);
            for (int i = 0; i < spawnPointsCount; i++)
            {
                spawnPoints[i].GetComponent<SpawnPoint>().IsFree = (bool)formatter.Deserialize(stream);
            }
            stream.Close();

        }
    }
    #endregion
}
