using System;
using System.IO;
using System.Text;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private enum ChunkShape
    {
        None,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft,
        Top,
        Right,
        Bottom,
        Left,
        Four
    }

    [SerializeField] private Transform world;
    [SerializeField] private int gridSize;
    [SerializeField] private int gridScale;
    [SerializeField] private Mesh[] chunkShapes;

    private string dataPath = Application.dataPath + "/_Data/StorageData/WorldData.txt";

    private WorldData worldData;
    private bool shouldSave;
    private Chunk[,] grid;

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

        IntializeGrid();
        UpdateChunkWall();
        UpdateGridRenderers();
    }

    private void IntializeGrid()
    {
        grid = new Chunk[gridSize, gridSize];

        for (int i = 0; i < world.childCount; i++)
        {
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();

            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x / gridScale, (int)chunk.transform.position.z / gridScale);
            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);

            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
        }
    }

    private void ChunkUnlockedCallback()
    {
        SaveWorldData();
        UpdateChunkWall();
        UpdateGridRenderers();
    }

    private void UpdateChunkWall()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];
                if (chunk == null) continue;

                Chunk fChunk = IsValidGridPosition(x, y + 1) ? grid[x, y + 1] : null;
                Chunk rChunk = IsValidGridPosition(x + 1, y) ? grid[x + 1, y] : null;
                Chunk bChunk = IsValidGridPosition(x, y - 1) ? grid[x, y - 1] : null;
                Chunk lChunk = IsValidGridPosition(x - 1, y) ? grid[x - 1, y] : null;

                int configuration = 0;
                if (fChunk != null && fChunk.IsUnlocked()) configuration = configuration + 1;
                if (rChunk != null && rChunk.IsUnlocked()) configuration = configuration + 2;
                if (bChunk != null && bChunk.IsUnlocked()) configuration = configuration + 4;
                if (lChunk != null && lChunk.IsUnlocked()) configuration = configuration + 8;

                chunk.UpdateWalls(configuration);
                SetChunkRenderer(chunk, configuration);
            }
        }
    }

    private void SetChunkRenderer(Chunk chunk, int configuration)
    {
        chunk.SetRenderer(chunkShapes[9]);

        switch (configuration)
        {
            case 0: chunk.SetRenderer(chunkShapes[(int)ChunkShape.Four]); break;
            case 1: chunk.SetRenderer(chunkShapes[(int)ChunkShape.Bottom]); break;
            case 2: chunk.SetRenderer(chunkShapes[(int)ChunkShape.Left]); break;
            case 3: chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomLeft]); break;
            case 4: chunk.SetRenderer(chunkShapes[(int)ChunkShape.Top]); break;
            case 5: chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 6: chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopLeft]); break;
            case 7: chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 8: chunk.SetRenderer(chunkShapes[(int)ChunkShape.Right]); break;
            case 9: chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomRight]); break;
            case 10: chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 11: chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 12: chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopRight]); break;
            case 13: chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 14: chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;
            case 15: chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]); break;

        }
    }

    private void UpdateGridRenderers()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];
                if (chunk == null) continue;
                if (chunk.IsUnlocked()) continue;

                Chunk fChunk = IsValidGridPosition(x, y + 1) ? grid[x, y + 1] : null;
                Chunk rChunk = IsValidGridPosition(x + 1, y) ? grid[x + 1, y] : null;
                Chunk bChunk = IsValidGridPosition(x, y - 1) ? grid[x, y - 1] : null;
                Chunk lChunk = IsValidGridPosition(x - 1, y) ? grid[x - 1, y] : null;

                if (fChunk != null && fChunk.IsUnlocked()) chunk.DisplayLockedElements();
                else if (rChunk != null && rChunk.IsUnlocked()) chunk.DisplayLockedElements();
                else if (bChunk != null && bChunk.IsUnlocked()) chunk.DisplayLockedElements();
                else if (lChunk != null && lChunk.IsUnlocked()) chunk.DisplayLockedElements();


            }
        }
    }

    private bool IsValidGridPosition(int x, int y)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize) return false;
        return true;
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
