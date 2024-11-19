using System;
using System.IO;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Inventory inventory = new Inventory();
    private InventoryDisplay inventoryDisplay;

    private string dataPath = Application.dataPath + "/_Data/StorageData/inventory.txt";

    private void Start()
    {
        CropTile.onCropHarvested += CropHarvestedCallback;
        LoadInventory();
        ConfigureInventoryDisplay();
    }

    private void OnDestroy()
    {
        CropTile.onCropHarvested -= CropHarvestedCallback;
    }

    private void ConfigureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.Configure(inventory);
    }

    private void CropHarvestedCallback(CropType type)
    {
        inventory.CropHarvestedCallback(type);
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    private void LoadInventory()
    {
        string data = "";

        if (File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);
            if (inventory == null) inventory = new Inventory();
        }
        else
        {
            File.Create(dataPath);
            inventory = new Inventory();
        }
    }

    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(dataPath, data);
    }

    [Button]
    private void DebugInventory() => inventory.DebugInventory();

    [Button]
    public void ClearInventory()
    {
        inventory.Clear();
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}
