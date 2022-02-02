using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform itemSlotPos;
    private GameObject currentItem;
    private GameObject Ui_element;

    public GameObject CurrentItem
    {
        get { return currentItem; }
        set { currentItem = value; }
    }
    public GameObject UI_Element
    {
        get { return Ui_element; }
        set { Ui_element = value; }
    }


    private void Update()
    {
        InstantiateItem();
    }

    public void InstantiateItem()
    {
        if (player.currentItem != null && player.IsHoldingItem)
        {
            if (Ui_element != player.currentItem.item.UI_prefab)
            {
                if (Ui_element != null)
                    Destroy(currentItem);

                player.IsHoldingItem = false;
                currentItem = Instantiate(player.currentItem.item.UI_prefab, itemSlotPos);
                Debug.Log("item größe");
                Ui_element = player.currentItem.item.UI_prefab;
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && currentItem != null)
        {
            Destroy(currentItem);
            Ui_element = null;
            player.IsHoldingItem = false;
        }

    }


}
