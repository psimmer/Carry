using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I know it is the wrong script for this but i dont know in which script it should be
public enum TaskType
{
    RelocateAPatient,
    Bandage,
    Pills,
    Catheter,
    BloodSample,                //Maybe we do a own script "Tasks" and there are only the Tasks inside?
    Transfusion,
    WashThePatient,
    AnswerTheTelephone,
    Documentation,
    TalkToThePatient
}

public class Patient : MonoBehaviour
{
    [SerializeField] private int currentHP; //changed it from healthAmount to currentHP
    [SerializeField] private int patientMaxHP;

    //range for the random HP that the patient spawns with
    [SerializeField] private int minCurrentHp;
    [SerializeField] private int maxCurrentHp;
    

    [SerializeField] private TaskType currentIllness;
    [SerializeField] private int patientID;
    [SerializeField] private bool isPopping;
    [SerializeField] private bool hasTask;
    [SerializeField] private int minTimer;
    [SerializeField] private int maxTimer;
    //[SerializeField] private GameObject healthbarPrefab;
    [SerializeField] private Healthbar healthbar;
    TaskType RandomTask;
    [SerializeField] bool isInBed = false;

    //public GameObject InstantiatedHealthbar { get; set; }

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

    public int PatientMaxHP
    {
        get { return patientMaxHP; }
    }
    
    public Healthbar Healthbar
    {
        get { return healthbar; }
        set { healthbar = value; }
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
        set { 
            if(isInBed == false)
                currentIllness = TaskType.RelocateAPatient;
            else
            currentIllness = (TaskType) Random.Range(1, 3); 
        }
    }

    #endregion

    void Start()
    {
        //InstantiatedHealthbar = null;
        //healthbar = healthbarPrefab.GetComponent<Healthbar>();
        healthbar = GetComponentInChildren<Healthbar>();
        HasTask = false;
        minTimer = Random.Range(10, 15);
        maxTimer = Random.Range(20, 30);
        IsPopping = false;
        CurrentIllness = RandomTask;
        //Treatment(currentHP);
    }

    private void LateUpdate()
    {
        if(healthbar)
            healthbar.UpdateHealthbar(currentHP / (float)patientMaxHP);

    }

    public void Treatment(int health)
    {
        currentHP += health;
        // patient full recovered

        if(health < 0)
        {
            SpawnParticles(damageParticles, particlesDuration);
        }
        else if (health > 0)
        {
            SpawnParticles(healingParticles, particlesDuration);
        }

        if(currentHP >= patientMaxHP)
        {
            currentHP = patientMaxHP;
            GlobalData.instance.SetPatientHealedStatistics();
            //SpawnParticles(fullHealingParticles, particlesDuration);
        }
        // patient dead
        else if (currentHP <= 0)
        {
            //currentHP = 0;
            GlobalData.instance.SetPatientDeadStatistics();
            //SpawnParticles(deathParticles, particlesDuration);
            Destroy(this.gameObject);
        }
        
        healthbar.UpdateHealthbar(currentHP / (float)patientMaxHP);
        

    }

    private void SpawnParticles(GameObject particles, float duration)
    {
        GameObject newParticles = Instantiate(particles, this.transform);
        Destroy(newParticles, duration);
    }
}
