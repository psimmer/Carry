using UnityEngine;

public class CoffeMachine : MonoBehaviour
{

    [SerializeField] private int coffeeCount;
    [SerializeField] private float healCoffee;


    public int CoffeeCount
    {
        get { return coffeeCount; }
        set { coffeeCount = value; }
    }

    public float HealCoffee
    {
        get { return healCoffee; }
    }

}
