using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    

    private void Awake()
    {
        
    }

    //public void DrinkCoffee(Player player)
    //{
    //    if (coffeeCount <= 0)
    //    {
    //        Debug.Log("No Coffee left");
    //        //show in UI that nothing is left
    //    }
    //    else
    //    {
    //        player.CurrentStressLvl -= healCoffee;  //multiply it by the stressReductionMultiplier?
    //        coffeeCount--;
    //        coffeeCounter.text = "Coffee: " + coffeeCount;
    //    }
    //}
}
