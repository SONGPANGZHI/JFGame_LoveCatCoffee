using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureInfo : MonoBehaviour,IPointerClickHandler
{
    public string furnitureName;
    public FurnitureItem FurnitureItem;
    public Material shaderOutlineMat;

    private Material defualtMat;
    public void Init(FurnitureItem _furnitureData)
    {
        FurnitureItem = _furnitureData;
        furnitureName = FurnitureItem.Id;
        Sprite itemSprite = ListExtensions.LoadFurnitureSprite(furnitureName);
        transform.GetComponent<SpriteRenderer>().sprite = itemSprite;
        transform.GetComponent<SpriteRenderer>().sortingOrder = FurnitureItem.OrderLayer;
        defualtMat = transform.GetComponent<SpriteRenderer>().material;
        transform.position = FurnitureItem.DefaultPosition;
        if (!_furnitureData.NoUpgradeFurniture)
        {
            PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            polygonCollider.autoTiling = true;
        }

    }

    //点击家具
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        UseOutlineMaterial();
        FurnitureManagement.instance.currentClickFurniture = this;
        FurnitureManagement.instance.JudgeCurrentClickFurniture();
        BaseTools.Instance.SetCameraPosition(this.transform.position, FurnitureItem.CameraSize);
        //UIManagement.Instance.OpenFurnitureSkinPlane();
    }

    //使用默认材质
    public void UseDefualtMaterial()
    {
        transform.GetComponent<SpriteRenderer>().material = defualtMat;
    }

    //使用描边材质
    public void UseOutlineMaterial()
    {
        transform.GetComponent<SpriteRenderer>().material = shaderOutlineMat;
    }
}
