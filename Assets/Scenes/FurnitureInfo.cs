using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LoadAllConfigData;

public class FurnitureInfo : MonoBehaviour
{
    public SpriteRenderer spriteIcon;
    public FurnitureInfos furnitureInfosData;

    public void FurnitureInfoInit(FurnitureInfos furnitureInfos)
    {
        furnitureInfosData = furnitureInfos;
        spriteIcon.sprite = Resources.Load("Images/Hall_Brown" + furnitureInfosData.FurnitureName, typeof(Sprite)) as Sprite;
        spriteIcon.sortingOrder = furnitureInfosData.OrderLayer;
        transform.position = furnitureInfosData.FurniturePos;
    }
}
