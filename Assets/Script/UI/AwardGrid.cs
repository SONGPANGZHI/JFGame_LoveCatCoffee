using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AwardGrid : MonoBehaviour
{
    public Image Icon;
    public TMP_Text text_Tmp;

    public void InitAwardGrid(string spriteKey)
    {
        Sprite itemSprite = ListExtensions.LoadFurnitureSprite(spriteKey);
        Icon.sprite = itemSprite;
        text_Tmp.text = "ÐÂ¼Ò¾ß";
    }
}
