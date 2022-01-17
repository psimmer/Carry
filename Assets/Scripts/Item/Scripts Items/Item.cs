using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO item;
    public void Heal(Patient patient)
    {
        patient.healthAmount += item.restoreHealth;
    }

    public void Damage(Patient patient)
    {
        patient.healthAmount -= item.restoreHealth;
    }
}
