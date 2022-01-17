using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,           // None because we have tasks without item needed
    Bandage,
    Pill,
    Catheter,
    Transfusion,
    BloodSample,
    Sponge,
}


public class Item : MonoBehaviour
{
    public int restoreHealth;
    public ItemType itemType;
}
