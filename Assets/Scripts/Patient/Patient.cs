using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// I know it is the wrong script for this but i dont know in which script it should be
public enum TaskType
{
    Bandages,
    Pills,
    Syringe,
    Transfusion,
    Defibrillator,
    Sponge,
    RelocateAPatient,
    ReleasePatient,
    AnswerTheTelephone,
    Documentation,
    AssignBed
}

public class Patient : MonoBehaviour , ISaveSystem
{
    [SerializeField] private int currentHP;
    [SerializeField] private int patientMaxHP;
    [SerializeField] private List<GameObject> popUpList;
    [SerializeField] private Transform popUpCanvas;
    //[SerializeField] private PatientSpawner spawner; // this is ugly but its the only way if we use this script to release patients (patient shouldnt know about the patientspawner)
    public Transform PopUpCanvas { get { return popUpCanvas; } set { popUpCanvas = value; } }

    [SerializeField] private Transform healthBarCanvas;
    public Transform HealthBarCanvas { get { return healthBarCanvas; } set { healthBarCanvas = value; } }

    //range for the random HP that the patient spawns with
    [SerializeField] private int minCurrentHp;
    [SerializeField] private int maxCurrentHp;


    [SerializeField] private TaskType currentIllness;
    [SerializeField] private int patientID;
    [SerializeField] private bool isPopping;
    [SerializeField] private bool hasTask;

    //[SerializeField] private GameObject healthbarPrefab;
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private GameObject heartbeat;

    [SerializeField] bool isInBed = false;
    [SerializeField] private Transform destroyPosition;
    private Transform leaveHospital;
    public Transform LeaveHospital => leaveHospital;

    private bool isReleasing;
    private float destroyTimer = 0;
    public bool IsReleasing { get { return isReleasing; } set { isReleasing = value; } }

    //PopUpStuff
    GameObject currentPopUp;
    public GameObject CurrentPopUp { get { return currentPopUp; } set { currentPopUp = value; } }
    float popUpTimer;
    float timetillPopUp;
    bool hasPopUp;
    public bool HasPopUp { get { return hasPopUp; } set { hasPopUp = value; } }

    [SerializeField] bool isLayingSinceStart;

    #region Particles

    [SerializeField] private GameObject healingParticles;
    //[SerializeField] private GameObject fullHealingParticles;
    [SerializeField] private GameObject damageParticles;
    //[SerializeField] private GameObject deathParticles;
    [SerializeField] private float particlesDuration;

    #endregion

    #region Properties
    //public GameObject Prefab => healthbarPrefab;

    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
    public bool IsInBed
    {
        get { return isInBed; }
        set { isInBed = value; }
    }
    public int PatientMaxHP
    {
        get { return patientMaxHP; }
    }

    public Healthbar Healthbar
    {
        get { return healthbar; }
        set { healthbar = value; }
    }

    public GameObject Heartbeat
    {
        get { return heartbeat; }
        set { heartbeat = value; }
    }

    public bool IsPopping
    {
        get { return isPopping; }
        set { isPopping = value; }
    }

    public bool HasTask
    {
        get { return hasTask; }
        set { hasTask = value; }
    }

    public int PatientID
    {
        get { return patientID; }
        set { patientID = value; }
    }

    public TaskType CurrentIllness
    {
        get { return currentIllness; }
        set
        {
            currentIllness = value;
            //if (isInBed == false)
            //    currentIllness = TaskType.RelocateAPatient;
            //else
            //    currentIllness = (TaskType)Random.Range(1, 3);
        }
    }

    public Transform Canvas
    {
        get { return popUpCanvas; }
        set { popUpCanvas = value; }
    }

    #endregion

    private void Awake()
    {
        if (isLayingSinceStart)
        {
            GetComponent<Animator>().SetBool("isLaying", true);
        }
        leaveHospital = GameObject.Find("LeaveHospitalPoint").transform;
        destroyPosition = GameObject.Find("DestoyPos").transform;
    }
    void Start()
    {
        tag = "Patient";
        hasPopUp = false;
        timetillPopUp = Random.Range(10, 15);       //this gets serialized;
        healthbar = GetComponentInChildren<Healthbar>();
        
        HasTask = false;
        IsPopping = false;

        if (isInBed)
        {
            CurrentIllness = (TaskType)Random.Range(0, 6);
        }

    }

    private void Update()
    {
        if(!hasPopUp)
            popUpTimer += Time.deltaTime;

        PopUpTimer(CurrentIllness, popUpCanvas);

        ReleasingPatient();
    }

    private void LateUpdate()
    {
        if (healthbar)
            healthbar.UpdateHealthbar(currentHP / (float)patientMaxHP);

    }

    public void Treatment(int health)
    {
        if (CurrentIllness != TaskType.AssignBed)
        {

            currentHP += health;
            //Damage
            if (health < 0)
            {
                SoundManager.instance.PlayAudioClip(ESoundeffects.Damage, GetComponent<AudioSource>());
                CurrentIllness = (TaskType)Random.Range(0, 6);
                SpawnParticles(damageParticles, particlesDuration);
                //Destroy(currentPopUp);

                Debug.Log($"health kleiner als 0 {health}");
            }
            //Heal
            else if (health > 0)
            {
                SoundManager.instance.PlayAudioClip(ESoundeffects.Heal, GetComponent<AudioSource>());
                CurrentIllness = (TaskType)Random.Range(0, 6);
                SpawnParticles(healingParticles, particlesDuration);
                Debug.Log($"health größer als 0 {health}");
            }

            // patient full recovered
            if (currentHP >= patientMaxHP)
            {
                SoundManager.instance.PlayAudioClip(ESoundeffects.Heal, GetComponent<AudioSource>());
                currentHP = patientMaxHP;
                GlobalData.instance.SetPatientHealedStatistics();
                Debug.Log($"patient vollgeheilt {health}");
                currentIllness = TaskType.ReleasePatient;
                //SpawnParticles(fullHealingParticles, particlesDuration);
            }
            // patient dead
            else if (currentHP <= 0)
            {
                SoundManager.instance.PlayAudioClip(ESoundeffects.Death, GetComponent<AudioSource>());
                GlobalData.instance.SetPatientDeadStatistics();
                Debug.Log($"patient tot {health}");
                Destroy(this.gameObject);
                //SpawnParticles(deathParticles, particlesDuration);
            }
            if (currentHP > 0 && currentHP < patientMaxHP && health > 0)
                GlobalData.instance.SetPatientTreatmentStatistics();
            
            healthbar.UpdateHealthbar(currentHP / (float)patientMaxHP);
        }
    }

    public void ReleasingPatient()
    {
        if (IsReleasing)
        {
            destroyTimer += Time.deltaTime;
            float interpolation = 0.2f * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, destroyPosition.position, interpolation);
            if (destroyTimer >= 6) // the value is the time which will take the patient to be destroyed after being released
            {
                Destroy(gameObject);
                destroyTimer = 0;
                //spawner.RemovePatientFromList(this); // this is ugly! (check patient spawner reference in this script for more details)
            }
            
        }
    }



    private void SpawnParticles(GameObject particles, float duration)
    {
        GameObject newParticles = Instantiate(particles, this.transform);
        Destroy(newParticles, duration);
    }

    public void PopUpTimer(TaskType illness, Transform canvas)
    {
        if (popUpTimer >= timetillPopUp)
        {
            popUpTimer = 0;
            StartCoroutine(PopUpSpawn(illness, canvas));
        }
    }

    IEnumerator PopUpSpawn(TaskType illness, Transform canvas)
    {
        if (CurrentIllness != TaskType.RelocateAPatient)
        {
            foreach (GameObject popUp in popUpList)
            {
                if (illness == popUp.GetComponent<PopUp>().TaskType)
                {
                    if (!hasPopUp)
                    {
                        hasPopUp = true;
                        currentPopUp = Instantiate(popUp.gameObject, canvas);
                        SoundManager.instance.PlayAudioClip(ESoundeffects.PopUp, GetComponent<AudioSource>());
                        //currentPopUp.transform.position = new Vector3(canvas.position.x, canvas.position.y + 1f, canvas.position.z);

                    }
                }
            }
        }
        yield return new WaitForEndOfFrame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PatientDestroy"))
        {
            Destroy(this.gameObject);
        }
    }


    public void SaveToStream(System.IO.FileStream fileStream)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(fileStream, currentHP);
    }

    public void SaveData()
    {
        //throw new System.NotImplementedException();
    }

    public void LoadData()
    {
        //throw new System.NotImplementedException();
    }
}
