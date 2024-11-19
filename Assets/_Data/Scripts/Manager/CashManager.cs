using System;
using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    private int coins;

    public static CashManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadData();
        UpdateDisplayCoin();
    }

    public void UseCoins(int amount)
    {
        AddCoins(-amount);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveData();
        UpdateDisplayCoin();
    }

    private void UpdateDisplayCoin()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("CoinAmount");

        foreach (GameObject go in gameObjects)
        {
            go.GetComponent<TextMeshProUGUI>().text = coins.ToString();
        }
    }

    [NaughtyAttributes.Button]
    private void Add500()
    {
        AddCoins(500);
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins");
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);
    }

    public int GetCoin()
    {
        return coins;
    }

}
