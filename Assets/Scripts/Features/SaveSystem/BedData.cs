[System.Serializable]
public class BedData
{
    public bool isPatientInBed;
    public bool setHealthBarAndPopUpSpawnPos;
    public float timer;

    public BedData(Bed bed)
    {
        isPatientInBed = bed.IsPatientInBed;
        setHealthBarAndPopUpSpawnPos = bed.SetHealthBarAndPopUpSpawnPos;
        timer = bed.Timer;
    }
}
