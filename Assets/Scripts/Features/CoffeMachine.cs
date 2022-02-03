using UnityEngine;

public class CoffeMachine : MonoBehaviour , ISaveSystem
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

    public void LoadData()
    {
        throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
}
