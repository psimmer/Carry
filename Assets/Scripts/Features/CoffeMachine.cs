using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CoffeMachine : MonoBehaviour, ISaveSystem
{

    [SerializeField] private int coffeeCount;
    [SerializeField] private float healCoffee;
    [SerializeField] Image coffeeFill;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Image coffeeCup;
    private bool drinking = false;
    [SerializeField] private float timer; // how long the effect lasts
    private float maxTimer;
    [SerializeField] private int extraSpeed; // gained speed through coffee

    private void Awake()
    {
        if (GlobalData.instance.IsSaveFileLoaded)
        {
            LoadData();
            //GlobalData.instance.IsSaveFileLoaded = false;
        }
    }

    private void Start()
    {
        maxTimer = timer;
    }
    private void Update()
    {
        if (drinking)
        {
            CoffeeIsActive(timer, extraSpeed);
        }
    }

    #region Getters and setters
    public int CoffeeCount
    {
        get { return coffeeCount; }
        set { coffeeCount = value; }
    }

    public Image CoffeeFill
    {
        get { return coffeeFill; }
        set { coffeeFill = value; }
    }
    public Image CoffeeCup
    {
        get { return coffeeCup; }
        set { coffeeCup = value; }
    }

    public bool Drinking
    {
        get { return drinking; }
        set { drinking = value; }
    }

    public float HealCoffee
    {
        get { return healCoffee; }
    }
    #endregion


    private void CoffeeIsActive(float totalTime, float gainedSpeed)
    {

        if (timer == maxTimer)
        {
            coffeeCup.gameObject.SetActive(true);
            playerMovement.PlayerMovementSpeed += gainedSpeed;
            coffeeFill.fillAmount = 1f;
        }

        timer -= Time.deltaTime;
        coffeeFill.fillAmount -= Time.deltaTime/maxTimer;
        //Debug.Log("FillAmount: "+ coffeeFill.fillAmount);

        if (totalTime <= 0)
        {
            coffeeFill.fillAmount = 0;
            coffeeCup.gameObject.SetActive(false);
            drinking = false;
            timer = maxTimer; // refill the timer once the coffee effect is over
            playerMovement.PlayerMovementSpeed -= gainedSpeed;
        }
    }

    

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataCoffeeMachine.carry";
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, coffeeCount);
        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveDataCoffeeMachine.carry";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Debug.Log("Save File loaded: " + path);

            coffeeCount = (int)formatter.Deserialize(stream);
            stream.Close();

        }
        else
        {
            Debug.Log("Save File not found" + path);
        }
    }
}
