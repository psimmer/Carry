using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Item Data")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int restoreHealth;
    public GameObject prefab;
    public GameObject UI_prefab;
    public TaskType task;
}
