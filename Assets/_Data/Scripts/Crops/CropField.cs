using System;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    [SerializeField] private CropData cropData;
    [SerializeField] private Transform tilesParent;

    private List<CropTile> cropTiles = new List<CropTile>();

    private FieldTileState state;
    private int tilesSown;
    private int tilesWatered;

    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;

    private void Start()
    {
        state = FieldTileState.Empty;
        StoreTiles();
    }

    private void StoreTiles()
    {
        for (int i = 0; i < tilesParent.childCount; i++)
        {
            cropTiles.Add(tilesParent.GetChild(i).GetComponent<CropTile>());
        }
    }

    public void SeedCollisionCallback(Vector3[] seedPositions)
    {
        for (int i = 0; i < seedPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosetCropTile(seedPositions[i]);

            if (closestCropTile == null) continue;

            if (!closestCropTile.IsEmpty()) continue;

            Sow(closestCropTile);
        }

    }

    public void WaterCollisionCallback(Vector3[] waterPositions)
    {
        for (int i = 0; i < waterPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosetCropTile(waterPositions[i]);

            if (closestCropTile == null) continue;

            if (!closestCropTile.IsSown()) continue;

            Watering(closestCropTile);
        }
    }

    private void Sow(CropTile cropTile)
    {
        cropTile.Sow(cropData);
        tilesSown++;

        if (tilesSown == cropTiles.Count)
        {
            FieldFullySown();
        }

    }

    private void Watering(CropTile cropTile)
    {
        cropTile.Watering(cropData);
        tilesWatered++;

        if (tilesWatered == cropTiles.Count)
        {
            FieldFullyWatered();
        }

    }

    private void FieldFullySown()
    {
        state = FieldTileState.Sown;
        onFullySown?.Invoke(this);
    }

    private void FieldFullyWatered()
    {
        state = FieldTileState.Watered;
        onFullyWatered?.Invoke(this);
    }

    private CropTile GetClosetCropTile(Vector3 pos)
    {

        float minDistance = 5000;
        int closestCropTileIndex = -1;

        for (int i = 0; i < cropTiles.Count; i++)
        {
            CropTile cropTile = cropTiles[i];
            float distanceTileSeed = Vector3.Distance(cropTile.transform.position, pos);

            if (distanceTileSeed < minDistance)
            {
                minDistance = distanceTileSeed;
                closestCropTileIndex = i;
            }
        }

        if (closestCropTileIndex < 0) return null;

        return cropTiles[closestCropTileIndex];
    }

    public bool IsEmpty()
    {
        return state == FieldTileState.Empty;
    }

    public bool IsWatered()
    {
        return state == FieldTileState.Watered;
    }

    public bool IsSown()
    {
        return state == FieldTileState.Sown;
    }
}
