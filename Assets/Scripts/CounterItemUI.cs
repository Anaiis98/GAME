using UnityEngine;
using UnityEngine.UI;

public class CounterItemUI : MonoBehaviour
{
    public Image itemImage;
    public Text itemCountText;

    private int itemCount = 0;
    private const int maxCount = 6;

    public void Initialize(Sprite sprite, string itemName)
    {
        itemImage.sprite = sprite;
        UpdateText(itemName);
    }

    public void IncrementCount()
    {
        if (itemCount < maxCount)
        {
            itemCount++;
            UpdateText(itemCountText.text.Split(':')[0]);
        }
        else
        {
            Debug.LogWarning("Maximum item count reached for " + itemCountText.text.Split(':')[0]);
        }
    }

    private void UpdateText(string itemName)
    {
        itemCountText.text = itemName + ": " + itemCount.ToString();
    }
}