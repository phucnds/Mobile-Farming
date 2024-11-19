using System;
using System.IO;
using System.Text;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Transform world;

    private string dataPath = Application.dataPath + "/_Data/StorageData/WorldData.txt";

    private WorldData worldData;
    private bool shouldSave;

    private void Awake()
    {
        Chunk.onUnlocked += ChunkUnlockedCallback;
        Chunk.onPriceChange += ChunkPriceChangeCallback;
    }

    private void Start()
    {
        LoadWorldData();
        Initialize();

        InvokeRepeating(nameof(TrySaveGame), 1, 1);
    }

    private void OnDestroy()
    {
        Chunk.onUnlocked -= ChunkUnlockedCallback;
        Chunk.onPriceChange -= ChunkPriceChangeCallback;
    }

    private void Initialize()
    {
        for (int i = 0; i < world.childCount; i++)
        {
            world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);
        }
    }

    private void ChunkUnlockedCallback()
    {
        SaveWorldData();
    }

    private void ChunkPriceChangeCallback()
    {
        shouldSave = true;
    }

    private void TrySaveGame()
    {
        if (shouldSave)
        {
            SaveWorldData();
            shouldSave = false;
        }
    }

    private void LoadWorldData()
    {
        string data = "";

        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);
            worldData = new WorldData();

            for (int i = 0; i < world.childCount; i++)
            {
                int chunkIntialPrice = world.GetChild(i).GetComponent<Chunk>().GetIntialPrice();
                worldData.chunkPrices.Add(chunkIntialPrice);
            }

            string worldDataString = JsonUtility.ToJson(worldData, true);
            byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);
            fs.Write(worldDataBytes);
            fs.Close();
        }
        else
        {
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);

            if (worldData.chunkPrices.Count < world.childCount)
            {
                UpdateData();
            }
        }

    }

    private void UpdateData()
    {
        int missingData = world.childCount - worldData.chunkPrices.Count;

        for (int i = 0; i < missingData; i++)
        {
            int index = world.childCount - missingData + i;
            int chunkPrices = world.GetChild(index).GetComponent<Chunk>().GetIntialPrice();
            worldData.chunkPrices.Add(chunkPrices);
        }
    }

    private void SaveWorldData()
    {
        if (worldData.chunkPrices.Count != world.childCount)
        {
            worldData = new WorldData();
        }

        for (int i = 0; i < world.childCount; i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();
            if (worldData.chunkPrices.Count > i)
            {
                worldData.chunkPrices[i] = chunkCurrentPrice;
            }
            else
            {
                worldData.chunkPrices.Add(chunkCurrentPrice);
            }

        }

        string data = JsonUtility.ToJson(worldData, true);
        File.WriteAllText(dataPath, data);
    }
}
