using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureData : MonoBehaviour
{
    public SpriteRenderer furnitureIcon;

    //≥ı ºªØ
    public void InitFurniture(FurnitureItemData furnitureItemData)
    {
        furnitureIcon.sprite = Resources.Load("images/" + furnitureItemData.skinName, typeof(Sprite)) as Sprite;
        //ga.transform.position = new Vector3(keyValuePairs[i].width, keyValuePairs[i].height);
        //GameObject imageObject = new GameObject(item.Key);
        //imageObject.transform.SetParent(spriteTrans);
        //SpriteRenderer renderer = imageObject.AddComponent<SpriteRenderer>();
        //Sprite sprite = Sprite.Create(ga, new Rect(0, 0, FurnitureList[item.Key].width, FurnitureList[item.Key].height), new Vector2(0.5f, 0.5f));
        //renderer.sprite = sprite;
        //imageObject.transform.localPosition = FurnitureList[item.Key].localPostion;
    }
}
