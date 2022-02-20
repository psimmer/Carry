using UnityEngine;

public class Bed : MonoBehaviour
{
    [Header("Bed Information")]
    [SerializeField] Transform bedPos;
    public Transform BedPos => bedPos;

    [SerializeField] Transform whiteboardPos;
    public Transform WhiteboardPos => whiteboardPos;

    [SerializeField] GameObject lights;

    [SerializeField] private Transform popUpPosTransform;

    public Transform PopUpPosTransform => popUpPosTransform;
    private bool setHealthBarAndPopUpSpawnPos = true;
    public bool SetHealthBarAndPopUpSpawnPos { get { return setHealthBarAndPopUpSpawnPos; } set { setHealthBarAndPopUpSpawnPos = value; } }

    [Header("Patient Information")]
    [SerializeField] private bool isPatientInBed;
    public bool IsPatientInBed { get { return isPatientInBed; } set { isPatientInBed = value; } }

    [SerializeField] private Patient currentPatient;
    public Patient CurrentPatient { get { return currentPatient; } set { currentPatient = value; } }

    float timer;
    public float Timer { get { return timer; } set { timer = value; } }

    private void Start()
    {
        if(currentPatient == null)
        {
            isPatientInBed = false;
        }
        else if(currentPatient != null)
        {
            isPatientInBed = true;
        }
        lights.SetActive(false);
        Vector3 lookDir = Camera.main.transform.forward;
        popUpPosTransform.LookAt(popUpPosTransform.position + lookDir);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (currentPatient == null)
        {
            isPatientInBed = false;
            setHealthBarAndPopUpSpawnPos = true;
            if(lights)
                lights.SetActive(false);
        }

        // position healthbars in the whiteboard if there is a patient in bed
        if (currentPatient != null && setHealthBarAndPopUpSpawnPos) 
        {
            currentPatient.Healthbar.transform.position = new Vector3(
                WhiteboardPos.position.x,
                WhiteboardPos.position.y + whiteboardPos.localScale.y/4,
                WhiteboardPos.position.z - WhiteboardPos.localScale.z
                );
          
            currentPatient.Heartbeat.transform.position = new Vector3(
                WhiteboardPos.position.x,
                whiteboardPos.position.y - whiteboardPos.localScale.y / 5,
                whiteboardPos.position.z - whiteboardPos.localScale.z
                );

            Vector3 popUpPos = popUpPosTransform.position;
            Vector3 popUpRotation = PopUpPosTransform.eulerAngles;

            CurrentPatient.Canvas.transform.position = popUpPos;
            CurrentPatient.Canvas.transform.eulerAngles= popUpRotation;

            currentPatient.Heartbeat.SetActive(true);
            setHealthBarAndPopUpSpawnPos = false;
            if(lights)
                lights.SetActive(true);

           
        }

        if (timer >= 1 && currentPatient != null)
        {
            SoundManager.instance.PlayAudioClip(ESoundeffects.ECG, GetComponentInChildren<AudioSource>());
            timer = 0;
        }
    }
}
