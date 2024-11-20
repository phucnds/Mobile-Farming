using System;
using UnityEngine;

public class PlayerBuyerInteractor : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
        {
            SellCrops();
        }
    }

    private void SellCrops()
    {
        Inventory inventory = inventoryManager.GetInventory();
        InventoryItem[] items = inventory.GetInventoryItems();

        int coinEarned = 0;

        for (int i = 0; i < items.Length; i++)
        {
            int itemPrice = DataManager.Instance.GetCropPrice(items[i].cropType);
            coinEarned += itemPrice * items[i].amount;
        }

        // CashManager.Instance.AddCoins(coinEarned);
        TransactionEffectManager.Instance.PlayCoinParticlesystem(coinEarned);

        inventoryManager.ClearInventory();
    }
}
