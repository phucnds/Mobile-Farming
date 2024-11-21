using System;
using System.IO;
using System.Text;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Inventory inventory = new Inventory();
    private InventoryDisplay inventoryDisplay;

    private string dataPath = Application.dataPath + "/_Data/StorageData/inventory.txt";
    // private string dataPath = Application.persistentDataPath + "/_Data/StorageData/inventory.txt";


    private void Start()
    {
        CropTile.onCropHarvested += CropHarvestedCallback;
        AppleTree.onAppHarvested += CropHarvestedCallback;

        LoadInventory();
        ConfigureInventoryDisplay();
    }

    private void OnDestroy()
    {
        CropTile.onCropHarvested -= CropHarvestedCallback;
        AppleTree.onAppHarvested -= CropHarvestedCallback;
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

    public void LoadNew()
    {
        inventory = new Inventory();
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(dataPath, data);
    }

    // private void LoadInventory1()
    // {
    //     string data = "";

    //     if (File.Exists(dataPath))
    //     {
    //         data = File.ReadAllText(dataPath);
    //         inventory = JsonUtility.FromJson<Inventory>(data);
    //         if (inventory == null) inventory = new Inventory();
    //     }
    //     else
    //     {
    //         File.Create(dataPath);
    //         inventory = new Inventory();
    //     }
    // }

    private void LoadInventory()
    {
        string data = "";

        if (!File.Exists(DataPath.InventoryData))
        {
            FileStream fs = new FileStream(DataPath.InventoryData, FileMode.Create);
            inventory = new Inventory();

            string dataString = JsonUtility.ToJson(inventory, true);
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataString);
            fs.Write(dataBytes);
            fs.Close();

        }
        else
        {
            data = File.ReadAllText(DataPath.InventoryData);
            inventory = JsonUtility.FromJson<Inventory>(data);
            if (inventory == null) inventory = new Inventory();
        }
    }

    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(DataPath.InventoryData, data);
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
