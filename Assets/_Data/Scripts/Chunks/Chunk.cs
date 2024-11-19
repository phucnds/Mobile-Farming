using System;
using TMPro;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GameObject unlockedElement;
    [SerializeField] private GameObject lockedElement;
    [SerializeField] private TextMeshPro priceText;

    [SerializeField] private int intialPrice;

    public static Action onUnlocked;
    public static Action onPriceChange;

    private int currentPrice;
    private bool unlocked;

    private void Start()
    {
        currentPrice = intialPrice;
        priceText.text = currentPrice.ToString();
    }

    public void TryUnlock()
    {
        if (CashManager.Instance.GetCoin() <= 0) return;


        currentPrice--;
        CashManager.Instance.UseCoins(1);

        onPriceChange?.Invoke();

        priceText.text = currentPrice.ToString();
        if (currentPrice <= 0)
        {
            Unlock();
        }
    }



    private void Unlock(bool triggerAction = true)
    {
        unlockedElement.SetActive(true);
        lockedElement.SetActive(false);
        unlocked = false;

        if (!triggerAction) return;
        onUnlocked?.Invoke();
    }

    private bool IsUnlocked()
    {
        return unlocked;
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }

    public int GetIntialPrice()
    {
        return intialPrice;
    }

    public void Initialize(int price)
    {
        currentPrice = price;
        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
        {
            Unlock(false);
        }
    }
}
