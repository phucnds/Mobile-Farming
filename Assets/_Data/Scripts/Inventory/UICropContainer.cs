using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICropContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtAmount;

    public void Configure(Sprite sprite, int amount)
    {
        icon.sprite = sprite;
        txtAmount.text = amount.ToString();
    }

    public void UpdateDisplay(int amount)
    {
        txtAmount.text = amount.ToString();
    }
}
