using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardGrid : MonoBehaviour
{
    public Image Icon;

    public void InitAwardGrid(string imagePath, string spriteKey)
    {
        Sprite itemSprite = Resources.Load(imagePath + "/" + spriteKey, typeof(Sprite)) as Sprite;
        Icon.sprite = itemSprite;
    }
}
