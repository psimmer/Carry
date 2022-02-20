using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;


public class CoffeMachine : MonoBehaviour, ISaveSystem
{

    [SerializeField] GameObject coffeeUI;
    [SerializeField] Image coffeeFill;
    public Image CoffeeFill{get { return coffeeFill; }set { coffeeFill = value; }}
    [SerializeField] NewPlayerMovement playerMovement;
    [SerializeField] Image coffeeCup;
    [Tooltip("How long the effect lasts")]
    [SerializeField] private float timer; 
    [Tooltip("Gained speed through coffee")]
    [SerializeField] private int extraSpeed; 
    [Tooltip("Cooldown in seconds after the coffee effect")]
    [SerializeField] private float cooldownDuration;
    public Image CoffeeCup { get { return coffeeCup; } set { coffeeCup = value; } }
    private float maxTimer;

    private bool drinking = false;
    public bool Drinking{get { return drinking; }set { drinking = value; }}
    private bool refillCup = false;

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
        if (drinking)
            CoffeeIsActive(maxTimer, extraSpeed);
        
        if(!drinking && isOnCooldown)
            CoffeeCooldown(cooldownDuration);
    }
    /// <summary>
    /// During the coffee effect, the player is faster
    /// </summary>
    /// <param name="totalTime">How long the coffee effect lasts</param>
    /// <param name="gainedSpeed">Amount of speed that the player will gain</param>
    private void CoffeeIsActive(float totalTime, float gainedSpeed)
    {
        //player gains speed
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

        //coffee effect is over, everything is set too normal
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

    /// <summary>
    /// disables the coffee maker duringt the cooldown
    /// </summary>
    /// <param name="coffeeColdown"></param>
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

    #region Save/Load Methods
    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SaveDataCoffeeMachine.carry";
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
    #endregion
}
