using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LoadAllConfigData;

public class FurnitureInfo : MonoBehaviour
{
    public FurnitureInfos furnitureInfosData;

    public void FurnitureInfoInit(FurnitureInfos furnitureInfos)
    {
        furnitureInfosData = furnitureInfos;
        Sprite itemSprite = ListExtensions.LoadFurnitureSprite(furnitureInfosData.FurnitureName);
        transform.GetComponent<SpriteRenderer>().sprite = itemSprite;
        transform.GetComponent<SpriteRenderer>().sortingOrder = furnitureInfosData.OrderLayer;
        transform.position = furnitureInfosData.FurniturePos;
    }
}
