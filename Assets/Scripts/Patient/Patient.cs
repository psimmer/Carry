using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Patient : MonoBehaviour
{
    #region Patient variables
    [Header("Patient Values")]
    [SerializeField] private int currentHP;
    public int CurrentHP { get { return currentHP; } set { currentHP = value; } }
    private int patientMaxHP = 100;
    //Patient Illnes
    [SerializeField] private TaskType currentIllness;
    public TaskType CurrentIllness { get { return currentIllness; } set { currentIllness = value; } }

    [SerializeField] bool isInBed = false;
    public bool IsInBed { get { return isInBed; } set { isInBed = value; } }
    [SerializeField] private GameObject currentBed;
    public GameObject CurrentBed { get { return currentBed; } set { currentBed = value; } }

    [Tooltip("When the patient is released, he walks out of the hospital. When he reaches this point, he will be destroyed")]
    [SerializeField] private Transform destroyPosition;
    private float destroyTimer = 0;

    [Tooltip("When the patient is released, he walks out of the hospital. This is the start point")]
    private Transform leaveHospital;
    public Transform LeaveHospital => leaveHospital;

    private int differentPatiensIndex;
    public int DifferentPatientsIndex { get { return differentPatiensIndex; } set { differentPatiensIndex = value; } }
    private float losingHpTimer = 0;
    #endregion
    #region PopUp variables
    [Header("PopUp")]
    [Tooltip("Range for the random time till a PopUp occurs (min)")]
    [SerializeField] float minTimeTillPopUp;
    [Tooltip("Range for the random time till a PopUp occurs (min)")]
    [SerializeField] float maxTimeTillPopUp;
    [Tooltip("Possible tasks that are available: Lvl 1 = 3; Lvl 2 = 5; Lvl 3 = 7; Lvl 4 = 7")]
    [SerializeField] int maxTaskIndex;
    public int MaxTaskIndex { get { return maxTaskIndex; } set { maxTaskIndex = value; } }
    [SerializeField] private Transform popUpCanvas;
    public Transform PopUpCanvas { get { return popUpCanvas; } set { popUpCanvas = value; } }

    [Tooltip("The damage that the patient takes when not assigned to a bed, every 5 seconds")]
    [SerializeField] int patientIdleDamage;
    [Tooltip("The damage that the patient takes when not assigned to a bed, every 5 seconds")]
    [SerializeField] int idleDeathStressDmg;

    [Tooltip("The patient died in the waiting area, this is for running the dead function just once")]
    bool patientDiedInSeat;
    public int PatientIdleDamage { get { return patientIdleDamage; } set { patientIdleDamage = value; } }
    public int IdleDeathStressDmg { get { return idleDeathStressDmg; } set { idleDeathStressDmg = value; } }

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
    public float TimeTillPopUp { get { return timetillPopUp; } set { timetillPopUp = value; } }

    bool hasPopUp;
    public bool HasPopUp { get { return hasPopUp; } set { hasPopUp = value; } }
    #endregion region
    #region Healthbar variables
    [Header("HealthBar")]
    [SerializeField] private Transform healthBarCanvas;
    public Transform HealthBarCanvas { get { return healthBarCanvas; } set { healthBarCanvas = value; } }

    [SerializeField] private Healthbar healthbar;
    public Healthbar Healthbar { get { return healthbar; } set { healthbar = value; } }

    [SerializeField] private GameObject heartbeat;
    public GameObject Heartbeat { get { return heartbeat; } set { heartbeat = value; } }

    [SerializeField] bool isLayingSinceStart;
    #endregion
    #region Particles variables
    [Header("Particles")]
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
    public static event Action<float> e_onPatientIdleDeath;
    public static event Action<Patient> e_deletePatientFromList;


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
        timetillPopUp = UnityEngine.Random.Range(minTimeTillPopUp, maxTimeTillPopUp);
        healthbar = GetComponentInChildren<Healthbar>();

        HasTask = false;
        IsPopping = false;

        if (isInBed)
        {
            CurrentIllness = (TaskType)UnityEngine.Random.Range(0, maxTaskIndex);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PatientDestroy"))
        {
            Destroy(this.gameObject);
        }
    }

    #region Spawning a PopUp
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
        if (CurrentIllness != TaskType.AssignBed)
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
    #endregion

    /// <summary>
    /// patient gets healed or damaged after the treatment
    /// </summary>
    /// <param name="health">Damage/Healing points</param>
    public void Treatment(int health)
    {
        if (CurrentIllness != TaskType.AssignBed)
        {

            currentHP += health;

            //Damage
            if (health < 0)
            {
                SoundManager.instance.PlayAudioClip(ESoundeffects.Damage, GetComponent<AudioSource>());
                CurrentIllness = (TaskType)UnityEngine.Random.Range(0, maxTaskIndex);
                SpawnParticles(damageParticles, particlesDuration);
            }
            //Heal
            else if (health > 0)
            {
                SoundManager.instance.PlayAudioClip(ESoundeffects.Heal, GetComponent<AudioSource>());
                CurrentIllness = (TaskType)UnityEngine.Random.Range(0, maxTaskIndex);
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
                e_deletePatientFromList?.Invoke(this);
                Destroy(this.gameObject, particlesDuration);
            }
            //setting statistics
            if (currentHP > 0 && currentHP < patientMaxHP && health > 0)
                GlobalData.instance.SetPatientTreatmentStatistics();

            healthbar.UpdateHealthbar(currentHP / (float)patientMaxHP);
        }
    }

    /// <summary>
    /// patient leaves the hospital
    /// </summary>
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


    /// <summary>
    /// Patients who wait for a bed get damaged over time
    /// </summary>
    private void TakeDamageByTime()
    {
        if (losingHpTimer >= 5)
        {
            losingHpTimer = 0;
            currentHP -= patientIdleDamage;
            SpawnParticles(sittingDamageParticles, particlesDuration);
        }

        if(currentHP <= 0 && !patientDiedInSeat)
        {
            patientDiedInSeat = true;
            e_onPatientIdleDeath?.Invoke(idleDeathStressDmg);
            SpawnParticles(DeathParticles, particlesDuration);
            e_deletePatientFromList?.Invoke(this);
            SoundManager.instance.PlayAudioClip(ESoundeffects.Death, GetComponent<AudioSource>());
            GlobalData.instance.SetPatientDeadStatistics();
            Destroy(this.gameObject, particlesDuration);
        }

        losingHpTimer += Time.deltaTime;
    }
}
