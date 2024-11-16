using System;
using System.Collections;
using UnityEngine;

public enum FieldTileState { Empty, Sown, Watered }

public class CropTile : MonoBehaviour
{
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;

    private FieldTileState state;
    private Crop crop;

    private void Start()
    {
        state = FieldTileState.Empty;
    }

    public void Sow(CropData cropData)
    {
        state = FieldTileState.Sown;

        crop = Instantiate(cropData.cropPrefab, cropParent);
    }

    public void Watering(CropData cropData)
    {
        state = FieldTileState.Watered;
        // tileRenderer.material.color = Color.white * .3f;
        crop.ScaleUp();

        // StartCoroutine(ColorTileCoroutine());

        tileRenderer.gameObject.LeanColor(Color.white * .3f, 1).setEase(LeanTweenType.easeOutBack);
    }

    /*
    private IEnumerator ColorTileCoroutine()
    {
        float duration = 1;
        float timer = 0;

        while (timer < duration)
        {
            float t = timer / duration;

            Color lerpedColor = Color.Lerp(Color.white, Color.white * .3f, t);
            tileRenderer.material.color = lerpedColor;
            timer += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
    */
    public bool IsEmpty()
    {
        return state == FieldTileState.Empty;
    }

    public bool IsSown()
    {
        return state == FieldTileState.Sown;
    }

    public bool IsWatered()
    {
        return state == FieldTileState.Watered;
    }
}
