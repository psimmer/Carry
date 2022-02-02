using System.Collections;
using System.Collections.Generic;
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

public class Patient : MonoBehaviour
{
    [SerializeField] private int currentHP;
    [SerializeField] private int patientMaxHP;
    [SerializeField] private List<GameObject> popUpList;
    [SerializeField] private Transform canvas;

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
    TaskType RandomTask;
    [SerializeField] bool isInBed = false;

    //PopUpStuff
    GameObject currentPopUp;
    public GameObject CurrentPopUp { get { return currentPopUp; } set { currentPopUp = value; } }
    float popUpTimer;
    float timetillPopUp;
    bool hasPopUp;
    public bool HasPopUp { get { return hasPopUp; } set { hasPopUp = value; } }

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
        get { return canvas; }
        set { canvas = value; }
    }

    #endregion


    void Start()
    {
        tag = "Patient";
        hasPopUp = false;
        timetillPopUp = Random.Range(5, 10);       //this gets serialized;
        healthbar = GetComponentInChildren<Healthbar>();
        
        //CurrentIllness = (TaskType)Random.Range(0, 6);
        
        HasTask = false;
        IsPopping = false;

    }

    private void Update()
    {
        popUpTimer += Time.deltaTime;
        PopUpTimer(CurrentIllness, canvas);
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
            Destroy(currentPopUp);
            //Damage
            if (health < 0)
            {
                CurrentIllness = (TaskType)Random.Range(0, 6);
                SpawnParticles(damageParticles, particlesDuration);
                Debug.Log($"health kleiner als 0 {health}");
            }
            //Heal
            else if (health > 0)
            {

                CurrentIllness = (TaskType)Random.Range(0, 6);
                SpawnParticles(healingParticles, particlesDuration);
                Debug.Log($"health gr��er als 0 {health}");
            }

            // patient full recovered
            if (currentHP >= patientMaxHP)
            {
                currentHP = patientMaxHP;
                GlobalData.instance.SetPatientHealedStatistics();
                Debug.Log($"patient vollgeheilt {health}");
                currentIllness = TaskType.ReleasePatient;
                //SpawnParticles(fullHealingParticles, particlesDuration);
            }
            // patient dead
            else if (currentHP <= 0)
            {
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
                        currentPopUp.transform.position = new Vector3(canvas.position.x, canvas.position.y + 1f, canvas.position.z);
                        //Vector3 lookDir = Camera.main.transform.forward;
                        //currentPopUp.transform.parent.LookAt(currentPopUp.transform.parent.position + lookDir);
                    }
                }
            }
        }
        yield return new WaitForEndOfFrame();
    }


}
