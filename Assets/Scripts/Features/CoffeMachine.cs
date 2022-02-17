using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CoffeMachine : MonoBehaviour, ISaveSystem
{


    [SerializeField] GameObject coffeeUI;
    [SerializeField] Image coffeeFill;
    public Image CoffeeFill{get { return coffeeFill; }set { coffeeFill = value; }}
    [SerializeField] NewPlayerMovement playerMovement;
    [SerializeField] Image coffeeCup;
    public Image CoffeeCup{get { return coffeeCup; }set { coffeeCup = value; }}
    [SerializeField] private float timer; // how long the effect lasts
    private float maxTimer;
    [SerializeField] private int extraSpeed; // gained speed through coffee  
    private bool drinking = false;
    public bool Drinking{get { return drinking; }set { drinking = value; }}
    private bool refillCup = false;
    [SerializeField] private float cooldownDuration; 
    private float cooldownTimer = 0; 
    private bool isOnCooldown = false; 
    public bool IsOnCooldown{get { return isOnCooldown; }set { isOnCooldown = value; } }
    public bool RefillCup{get { return refillCup; }set { refillCup = value; }}

    private void Start()
    {
        if (GlobalData.instance.IsSaveFileLoaded)
            LoadData();

        maxTimer = timer;
    }
    private void Update()
    {
        Debug.Log(cooldownTimer);
        if (drinking)
            CoffeeIsActive(maxTimer, extraSpeed);
        
        if(!drinking && isOnCooldown)
            CoffeeCooldown(cooldownDuration);
    }

    private void CoffeeIsActive(float totalTime, float gainedSpeed)
    {
        if (timer == totalTime)
        {
            Color tempColor = coffeeCup.color;
            tempColor.a = 1;
            coffeeCup.color = tempColor;
            playerMovement.PlayerSpeed += gainedSpeed;

            playerMovement.PlayerAnimator.speed = 1.5f;
        }

        if (refillCup)
        {
            coffeeFill.fillAmount = 1f;
            timer = totalTime;
            refillCup = false;
        }

        timer -= Time.deltaTime;
        coffeeFill.fillAmount -= Time.deltaTime/totalTime;

        if (timer <= 0)
        {
            coffeeFill.fillAmount = 0;
            drinking = false;
            Color tempColor = coffeeCup.color;
            tempColor.a = .2f;
            coffeeCup.color = tempColor;
            isOnCooldown = true;
            timer = totalTime; // refill the timer once the coffee effect is over
            playerMovement.PlayerSpeed -= gainedSpeed;
            playerMovement.PlayerAnimator.speed = 1;

        }
    }

    private void CoffeeCooldown(float coffeeColdown)
    {
        if (cooldownTimer < coffeeColdown)
        {
            Color tempColor = coffeeCup.color;
            tempColor.a += 1f/coffeeColdown * Time.deltaTime;
            coffeeFill.color = tempColor;
            coffeeFill.fillAmount += 1f/coffeeColdown * Time.deltaTime;
            coffeeCup.color = tempColor;
            cooldownTimer += Time.deltaTime;
        }
        else
        {
            Color tempColor = coffeeCup.color;
            tempColor.a = 1;
            coffeeCup.color = tempColor;
            coffeeFill.color = tempColor;
            cooldownTimer = 0;
            isOnCooldown = false;
        }
    }


    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataCoffeeMachine.carry";
        Debug.Log("Save File location: " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, timer);
        formatter.Serialize(stream, drinking);
        formatter.Serialize(stream, cooldownDuration);
        formatter.Serialize(stream, cooldownTimer);
        formatter.Serialize(stream, isOnCooldown);
        formatter.Serialize(stream, coffeeFill.fillAmount);
        formatter.Serialize(stream, coffeeCup.color.a);

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

            timer = (float)formatter.Deserialize(stream);
            drinking = (bool)formatter.Deserialize(stream);
            cooldownDuration = (float)formatter.Deserialize(stream);
            cooldownTimer = (float)formatter.Deserialize(stream);
            isOnCooldown = (bool)formatter.Deserialize(stream);
            coffeeFill.fillAmount = (float)formatter.Deserialize(stream);

            //setting the transparency
            Color tempColor = coffeeCup.color;
            tempColor.a = (float)formatter.Deserialize(stream);
            coffeeCup.color = tempColor;
            coffeeFill.color = tempColor;

            stream.Close();

        }
    }

   
}
