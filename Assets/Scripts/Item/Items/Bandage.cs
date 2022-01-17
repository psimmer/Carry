using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandage : Item
{
    [SerializeField] private int b_restoreHealth;

    private void Awake()
    {
        restoreHealth = b_restoreHealth;
        itemType = ItemType.Bandage;
    }
}
