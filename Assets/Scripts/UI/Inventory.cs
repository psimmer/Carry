using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform itemSlotPos;
    private GameObject Ui_element;
    private void Update()
    {
        if(player.currentItem != null && player.IsHoldingItem )
        {
            if(Ui_element != player.currentItem.item.UI_prefab)
            {
                player.IsHoldingItem = false;
                Instantiate(player.currentItem.item.UI_prefab, itemSlotPos);
                Ui_element = player.currentItem.item.UI_prefab;
            }
            else 
            { 
                // if he already have the item he will do this! Correct Nothing :D
            }
        }



    }





}
