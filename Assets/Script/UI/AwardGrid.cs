using UnityEngine;
using UnityEngine.UI;

public class AwardGrid : MonoBehaviour
{
    public Image Icon;

    public void InitAwardGrid(string spriteKey)
    {
        Sprite itemSprite = ListExtensions.LoadFurnitureSprite(spriteKey);
        Icon.sprite = itemSprite;
    }
}
