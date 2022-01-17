using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : Item
{
    [SerializeField] private int p_restoreHealth;
    private void Awake()
    {
        restoreHealth = p_restoreHealth;
        itemType = ItemType.Pill;
    }
}
