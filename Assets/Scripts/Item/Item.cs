using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item Data")]
public class Item : ScriptableObject
{
    public string itemName;
    public int restoreHealth;
    public GameObject prefab;
    
    public void Heal(Patient patient)
    {
        patient.healthAmount += restoreHealth;
    }

    public void Damage(Patient patient)
    {
        patient.healthAmount -= restoreHealth;
    }


}
