using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform itemSlotPos;
    private GameObject currentItem;
    public GameObject CurrentItem{get { return currentItem; }set { currentItem = value; }}

    private GameObject Ui_element;
    public GameObject UI_Element{get { return Ui_element; }set { Ui_element = value; }}



    private void Update()
    {
        InstantiateItem();
    }

    /// <summary>
    /// Instantiate the UI Prefab of the item you grabed
    /// </summary>
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
                Ui_element = player.currentItem.item.UI_prefab;
            }
        }
        //Throw the item away
        if (Input.GetKeyDown(KeyCode.F) && currentItem != null)
        {
            Destroy(currentItem);
            Ui_element = null;
            player.IsHoldingItem = false;
        }

    }


}
