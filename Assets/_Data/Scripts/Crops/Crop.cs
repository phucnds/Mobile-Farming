using System;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] private Transform cropRenderer;

    public void ScaleUp()
    {
        cropRenderer.gameObject.LeanScale(Vector3.one, 1).setEase(LeanTweenType.easeOutBack);
    }
}
