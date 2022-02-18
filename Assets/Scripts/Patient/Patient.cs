using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Patient : MonoBehaviour
{

    [Header("Patient Values")]
    [SerializeField] private int currentHP;
    public int CurrentHP { get { return currentHP; } set { currentHP = value; } }
    private int patientMaxHP = 100;
    //range for the random HP that the patient spawns with
    [SerializeField] private int minCurrentHp;
    [SerializeField] private int maxCurrentHp;
    //Patient Illnes
    [SerializeField] private TaskType currentIllness;
    public TaskType CurrentIllness { get { return currentIllness; } set { currentIllness = value; } }
    //Is to check if the patient is in a bed or is the bed free
    [SerializeField] bool isInBed = false;
    public bool IsInBed { get { return isInBed; } set { isInBed = value; } }
    [SerializeField] private GameObject currentBed;
    public GameObject CurrentBed { get { return currentBed; } set { currentBed = value; } }

    [SerializeField] private Transform destroyPosition;
    private float destroyTimer = 0;

    private Transform leaveHospital;
    public Transform LeaveHospital => leaveHospital;

    private int differentPatiensIndex;
    public int DifferentPatientsIndex { get { return differentPatiensIndex; } set { differentPatiensIndex = value; } }
    private float losingHpTimer = 0;

    [Header("PopUp")]
    [SerializeField] private Transform popUpCanvas;
    public Transform PopUpCanvas { get { return popUpCanvas; } set { popUpCanvas = value; } }

    [SerializeField] private List<GameObject> popUpList;

    [SerializeField] private bool isPopping;
    public bool IsPopping { get { return isPopping; } set { isPopping = value; } }

    [SerializeField] private bool hasTask;
    public bool HasTask { get { return hasTask; } set { hasTask = value; } }

    private bool isReleasing;
    public bool IsReleasing { get { return isReleasing; } set { isReleasing = value; } }

    GameObject currentPopUp;
    public GameObject CurrentPopUp { get { return currentPopUp; } set { currentPopUp = value; } }
    float popUpTimer;
    float timetillPopUp;
    bool hasPopUp;
    public bool HasPopUp { get { return hasPopUp; } set { hasPopUp = value; } }

    [Header("HealthBar")]
    [SerializeField] private Transform healthBarCanvas;
    public Transform HealthBarCanvas { get { return healthBarCanvas; } set { healthBarCanvas = value; } }

    [SerializeField] private Healthbar healthbar;
    public Healthbar Healthbar { get { return healthbar; } set { healthbar = value; } }

    [SerializeField] private GameObject heartbeat;
    public GameObject Heartbeat { get { return heartbeat; } set { heartbeat = value; } }

    [SerializeField] bool isLayingSinceStart;
    
    
    [Header("Particels")]
    [SerializeField] private GameObject spawningParticles;
    public GameObject SpawningParticles => spawningParticles;
    [SerializeField] private GameObject healingParticles;
    public Transform Canvas    {get { return popUpCanvas; }set { popUpCanvas = value; }}
    public GameObject HealingParticles => healingParticles;
    [SerializeField] private GameObject healParticles;
    [SerializeField] private GameObject healingRayParticles;
    [SerializeField] private GameObject damageParticles;
    public GameObject DamageParticles { get { return damageParticles; } }
    [SerializeField] private GameObject sittingDamageParticles;
    [SerializeField] private GameObject deathParticles;
    public GameObject DeathParticles => deathParticles;
    [SerializeField] private float particlesDuration;
    public float ParticlesDuration => particlesDuration;
    private GameObject currentParticles;
    public GameObject CurrentParticles { get { return currentParticles; } set { currentParticles = value; } }


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
        timetillPopUp = Random.Range(7, 30);       //this gets serialized;
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
        if (!hasPopUp)
            popUpTimer += Time.deltaTime;

        PopUpTimer(CurrentIllness, popUpCanvas);

        ReleasingPatient();

        if (!isInBed)
        {
            TakeDamageByTime();
        }

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
            }
            //Heal
            else if (health > 0)
            {
                SoundManager.instance.PlayAudioClip(ESoundeffects.Heal, GetComponent<AudioSource>());
                CurrentIllness = (TaskType)Random.Range(0, 6);
                SpawnParticles(healParticles, particlesDuration);
            }

            // patient full recovered
            if (currentHP >= patientMaxHP)
            {
                SoundManager.instance.PlayAudioClip(ESoundeffects.Heal, GetComponent<AudioSource>());
                currentHP = patientMaxHP;
                GlobalData.instance.SetPatientHealedStatistics();
                currentIllness = TaskType.ReleasePatient;
            }
            // patient dead
            else if (currentHP <= 0)
            {
                SpawnParticles(DeathParticles, particlesDuration);
                SoundManager.instance.PlayAudioClip(ESoundeffects.Death, GetComponent<AudioSource>());
                GlobalData.instance.SetPatientDeadStatistics();
                Destroy(this.gameObject, particlesDuration);
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
            }
        }
    }

    public void SpawnParticles(GameObject particles, float duration)
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

    private void TakeDamageByTime()
    {
        if (losingHpTimer >= 5)
        {
            losingHpTimer = 0;
            currentHP--;
            SpawnParticles(sittingDamageParticles, particlesDuration);
        }

        losingHpTimer += Time.deltaTime;
    }
}
