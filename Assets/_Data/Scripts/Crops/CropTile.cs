using System;
using UnityEngine;

public enum FieldTileState { Empty, Sown, Watered }

public class CropTile : MonoBehaviour
{
    [SerializeField] private Transform cropParent;

    private FieldTileState state;

    private void Start()
    {
        state = FieldTileState.Empty;
    }

    public void Sow(CropData cropData)
    {
        state = FieldTileState.Sown;

        Crop crop = Instantiate(cropData.cropPrefab, cropParent);
        Debug.Log("dasd");
    }

    public bool IsEmpty()
    {
        return state == FieldTileState.Empty;
    }
}
