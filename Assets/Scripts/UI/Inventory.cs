using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public void UpdateInventory(GameObject currentItem, Transform itemSlot)
    {
        Instantiate(currentItem, itemSlot);
    }



}
