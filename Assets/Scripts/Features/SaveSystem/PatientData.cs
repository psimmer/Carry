
[System.Serializable]
public class PatientData
{
    public float[] position = new float[3];
    public float[] rotation = new float[4];
    public int differentPatientsIndex;
    public int currentHP;
    public int currentIllnes;
    public bool isPopping;
    public bool hasTask;
    public bool isInBed;
    public bool hasPopUp;
    public bool isReleasing;
    public string currentBed;
    public int maxTaskIndex;

    public PatientData(Patient patient)
    {
        position[0] = patient.transform.position.x;
        position[1] = patient.transform.position.y;
        position[2] = patient.transform.position.z;
        rotation[0] = patient.transform.rotation.x;
        rotation[1] = patient.transform.rotation.y;
        rotation[2] = patient.transform.rotation.z;
        rotation[3] = patient.transform.rotation.w;
        differentPatientsIndex = patient.DifferentPatientsIndex;
        currentHP = patient.CurrentHP;
        currentIllnes = (int)patient.CurrentIllness;
        isPopping = patient.IsPopping;
        hasTask = patient.HasTask;
        isInBed = patient.IsInBed;
        hasPopUp = patient.HasPopUp;
        isReleasing = patient.IsReleasing;
        if(patient.CurrentBed != null)
            currentBed = patient.CurrentBed.name;
        maxTaskIndex = patient.MaxTaskIndex;
    }
}
