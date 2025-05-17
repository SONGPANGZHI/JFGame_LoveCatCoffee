using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureInfo : MonoBehaviour,IPointerClickHandler
{
    public string furnitureName;
    public FurnitureItem FurnitureItem;
    public void Init(FurnitureItem _furnitureData)
    {
        FurnitureItem = _furnitureData;
        furnitureName = FurnitureItem.Id;
        Sprite itemSprite = ListExtensions.LoadFurnitureSprite(furnitureName);
        transform.GetComponent<SpriteRenderer>().sprite = itemSprite;
        transform.GetComponent<SpriteRenderer>().sortingOrder = FurnitureItem.OrderLayer;
        transform.position = FurnitureItem.DefaultPosition;
        PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider.autoTiling = true;

    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        FurnitureManagement.instance.currentClickFurniture = this;
        UIManagement.Instance.OpenFurnitureSkinPlane();
    }
}
