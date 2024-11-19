using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void CropHarvestedCallback(CropType cropType)
    {
        bool cropFound = false;

        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];

            if (item.cropType == cropType)
            {
                item.amount++;
                cropFound = true;
                break;
            }
        }

        if (cropFound) return;

        items.Add(new InventoryItem(cropType, 1));
    }

    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }

    public void DebugInventory()
    {
        foreach (InventoryItem item in items)
        {
            Debug.Log("We have " + item.cropType + ": " + item.amount);
        }
    }

    public void Clear()
    {
        items.Clear();
    }
}
