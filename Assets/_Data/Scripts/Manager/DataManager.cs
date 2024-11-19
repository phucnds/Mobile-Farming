using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private CropData[] data;

    public static DataManager Instance;

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
    }

    public Sprite GetSprite(CropType type)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].cropType == type)
            {
                return data[i].Sprite;
            }
        }
        return null;
    }

    public int GetCropPrice(CropType type)
    {

        int price = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].cropType == type)
            {
                price = data[i].Price;
            }
        }

        return price;
    }
}
