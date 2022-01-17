using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item Data")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int restoreHealth;
    public GameObject prefab;

}
