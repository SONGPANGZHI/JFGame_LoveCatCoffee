using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureInfo : MonoBehaviour,IPointerClickHandler
{
    public FurnitureInfos furnitureInfosData;
    public List<string> furnitureSkin;          //家具皮肤名
    public string furnitureName;

    public void FurnitureInfoInit(FurnitureInfos furnitureInfos)
    {
        if (furnitureInfos == null)
            return;

        furnitureInfosData = furnitureInfos;
        furnitureName = furnitureInfosData.FurnitureName;
        furnitureSkin = furnitureInfosData.FurnitureSkinName;
        Sprite itemSprite = ListExtensions.LoadFurnitureSprite(furnitureInfosData.FurnitureName);
        transform.GetComponent<SpriteRenderer>().sprite = itemSprite;
        transform.GetComponent<SpriteRenderer>().sortingOrder = furnitureInfosData.OrderLayer;
        transform.position = furnitureInfosData.FurniturePos;
        PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider.autoTiling = true;
    }

    public void ChangeSpriteInit(string spriteKey)
    {
        furnitureName = spriteKey;
        Sprite itemSprite = ListExtensions.LoadFurnitureSprite(spriteKey);
        transform.GetComponent<SpriteRenderer>().sprite = itemSprite;
        PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider.autoTiling = true;
    }

    

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.LogError("点击---");
        FurnitureManagement.instance.currentClickFurniture = this;
        UIManagement.Instance.OpenFurnitureSkinPlane();
    }
}
