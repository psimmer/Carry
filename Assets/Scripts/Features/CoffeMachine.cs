using UnityEngine;
using UnityEngine.UI;

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
        coffeeFill.fillAmount -= Time.deltaTime/timer;
        Debug.Log("FillAmount: "+ coffeeFill.fillAmount);

        if (totalTime <= 0)
        {
            coffeeCup.gameObject.SetActive(false);
            drinking = false;
            timer = maxTimer; // refill the timer once the coffee effect is over
            playerMovement.PlayerMovementSpeed -= gainedSpeed;
        }
    }

    public void LoadData()
    {
        throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
}
