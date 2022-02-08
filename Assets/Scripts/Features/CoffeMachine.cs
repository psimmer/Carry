using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CoffeMachine : MonoBehaviour, ISaveSystem
{

    [SerializeField] private int coffeeCount;
    [SerializeField] GameObject coffeeUI;
    [SerializeField] Image coffeeFill;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Image coffeeCup;
    [SerializeField] TMP_Text coffeeCounter;
    private bool drinking = false;
    private bool refillCup = false;
    [SerializeField] private float timer; // how long the effect lasts
    private float maxTimer;
    [SerializeField] private int extraSpeed; // gained speed through coffee

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
    public bool RefillCup
    {
        get { return refillCup; }
        set { refillCup = value; }
    }

    #endregion

    private void Awake()
    {
        if (GlobalData.instance.IsSaveFileLoaded)
            LoadData();
    }

    private void Start()
    {
        maxTimer = timer;
    }
    private void Update()
    {
        if (drinking)
        {
            CoffeeIsActive(maxTimer, extraSpeed);
        }
    }

    private void CoffeeIsActive(float totalTime, float gainedSpeed)
    {

        if (timer == totalTime)
        {
            coffeeUI.SetActive(true);
            playerMovement.PlayerMovementSpeed += gainedSpeed;
        }

        if(refillCup)
        {
            coffeeFill.fillAmount = 1f;
            timer = totalTime;
            refillCup = false;
        }

        timer -= Time.deltaTime;
        coffeeFill.fillAmount -= Time.deltaTime/totalTime;
        //Debug.Log("FillAmount: "+ coffeeFill.fillAmount);

        if (timer <= 0)
        {
            coffeeFill.fillAmount = 0;
            coffeeUI.SetActive(false);
            drinking = false;
            timer = totalTime; // refill the timer once the coffee effect is over
            playerMovement.PlayerMovementSpeed -= gainedSpeed;
        }
    }
    public void UpdateCoffeCounter(int coffeeCount)
    {
        coffeeCounter.text = "Coffee: " + coffeeCount;
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataCoffeeMachine.carry";
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, coffeeCount);
        formatter.Serialize(stream, timer);
        formatter.Serialize(stream, drinking);

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
            timer = (float)formatter.Deserialize(stream);
            drinking = (bool)formatter.Deserialize(stream);

            stream.Close();
            UpdateCoffeCounter(coffeeCount);

        }
        else
        {
            Debug.Log("Save File not found" + path);
        }
    }

   
}
