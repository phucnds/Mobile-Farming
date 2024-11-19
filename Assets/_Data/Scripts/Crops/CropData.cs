using UnityEngine;

[CreateAssetMenu(fileName = "CropData", menuName = "ScriptableObject/CropData", order = 0)]
public class CropData : ScriptableObject
{
    public Crop cropPrefab;
    public CropType cropType;
    public Sprite Sprite;
    public int Price;
}