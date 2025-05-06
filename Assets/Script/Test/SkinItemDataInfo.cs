using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinItemDataInfo : MonoBehaviour
{
    public string defaultItemName;
    [HideInInspector]
    public string currentItemName;
    public int itemLayerID;
    public Vector3 itemPos;
    public bool isUnlock = false;
    public bool loucengUnlock = false;  //楼层解锁状态
    public SkinsItem skinItem;
    private float initPos;
    public Material defaultMat;
    public Material windEffectMat;


    public void InitData(SkinsItem _skinItem)
    {
        skinItem = _skinItem;
        defaultItemName = _skinItem.skinName;


        //0.更新itemdata
        //currentItemName = PlayerPrefs.GetString("furniture_" + skinItem.skinName, skinItem.skinName);
        //Debug.LogError(currentItemName);
        //skinItem.canClick = _skinItem.canClick;
        if (skinItem == null) skinItem = _skinItem;

        //1.加载图片资源
        Sprite itemSprite = Resources.Load(skinItem.imagePath + "/" + defaultItemName, typeof(Sprite)) as Sprite;
        Debug.LogError(skinItem.imagePath + "/" + defaultItemName);
        SetFurniture(itemSprite);
        //if (skinItem.skinName.Contains("beijingban"))
        //    this.GetComponent<SpriteRenderer>().sortingOrder = 8;

        //2.设置位置 
        itemPos = skinItem.localPostion;
        Debug.Log(currentItemName + "   :itemPos:  " + itemPos.x + "     " + itemPos.y);
        transform.localPosition = itemPos;

        //3.设置层级
        itemLayerID = skinItem.orderLayer;
        this.GetComponent<SpriteRenderer>().sortingOrder = skinItem.orderLayer;

        //4.添加PolygonCollider2D
        //this.gameObject.AddComponent<PolygonCollider2D>().autoTiling = true;
        //5.解锁状态

        //楼层解锁状态：
        //item解锁状态：

        //if (skinItem.skinName.Contains("kuangjia"))
        //{
        //    this.gameObject.AddComponent<PolygonCollider2D>().autoTiling = true;
        //    skinItem.orderLayer = 5;
        //    this.GetComponent<SpriteRenderer>().sortingOrder = 5;
        //}
        //this.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void SetFurniture(Sprite _sprite)
    {
        if (transform.GetComponent<SpriteRenderer>().sprite == null)
        {
            transform.GetComponent<SpriteRenderer>().sprite = _sprite;
            //PlayerPrefs.SetString("furniture_" + skinItem.skinName, _sprite.name);
            //PlayerPrefs.Save();

            //重新设置位置信息
            //SkinsItem newItem = MarketMgr.Instance.GetFurnitureByName(_sprite.name);

            //Debug.LogError(newItem.localPostion);
            //if (newItem == null)
            //{
            //    Debug.LogError("Get Furniture By Name error !");
            //    return;
            //}

            //transform.localPosition = newItem.localPostion;
            //this.transform.parent.GetComponent<CreateCurArea>().test();
        }
    }

    public void RefreshData()
    {
        isUnlock = transform.gameObject.name != transform.GetComponent<SpriteRenderer>().sprite.name;
        currentItemName = PlayerPrefs.GetString("furniture_" + skinItem.skinName, skinItem.skinName);

        ////楼层解锁状态：

    }

}
