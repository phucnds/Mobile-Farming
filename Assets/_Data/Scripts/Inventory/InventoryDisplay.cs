using System;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private UICropContainer uICropContainer;
    [SerializeField] private Transform cropContainerParent;

    public void Configure(Inventory inventory)
    {
        InventoryItem[] inventoryItems = inventory.GetInventoryItems();

        for (int i = 0; i < inventoryItems.Length; i++)
        {
            UICropContainer cropContainer = Instantiate(uICropContainer, cropContainerParent);
            Sprite icon = DataManager.Instance.GetSprite(inventoryItems[i].cropType);
            cropContainer.Configure(icon, inventoryItems[i].amount);
        }
    }

    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] inventoryItems = inventory.GetInventoryItems();

        

        for (int i = 0; i < inventoryItems.Length; i++)
        {

            UICropContainer cropContainer;

            if (i < cropContainerParent.childCount)
            {
                cropContainer = cropContainerParent.GetChild(i).GetComponent<UICropContainer>();
                cropContainer.gameObject.SetActive(true);
            }
            else
            {
                cropContainer = Instantiate(uICropContainer, cropContainerParent);
               
            }

            Sprite icon = DataManager.Instance.GetSprite(inventoryItems[i].cropType);
            cropContainer.Configure(icon, inventoryItems[i].amount);
        }

        int remainingContainer = cropContainerParent.childCount - inventoryItems.Length;

        if (remainingContainer <= 0) return;

        for (int i = 0; i < remainingContainer; i++)
        {
            cropContainerParent.GetChild(inventoryItems.Length + i).gameObject.SetActive(false);
        }

    }
}
