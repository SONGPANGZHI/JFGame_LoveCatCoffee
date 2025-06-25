using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureItemGrid : MonoBehaviour
{
    public GameObject redPonit;
    public Image icon;
    public Button use_BTN;
    public TMP_Text use_TMP;
    public GameObject unlock_OBJ;
    public Color garyColor;
    public FurnitureSkinState currentState;

    public FurnitureItem furnitureItem;

    private void Awake()
    {
        use_BTN.onClick.AddListener(UseClick);
    }

    //初始化
    public void ItemInit(FurnitureItem Gird)
    {
        furnitureItem = Gird;
        icon.sprite = ListExtensions.LoadFurnitureIconSprite(furnitureItem.Id);
        SetFurnitureState();
    }

    public void SetFurnitureState()
    {
        if (furnitureItem.IsDefault)
        {
            //使用中
            icon.color = Color.white;
            use_TMP.gameObject.SetActive(true);
        }
        else if (furnitureItem.IsNewSkin)
        {
            icon.color = Color.white;
            redPonit.SetActive(true);
            use_BTN.gameObject.SetActive(true);
        }
        else if (!furnitureItem.IsUnlocked)
        {
            //没有解锁
            unlock_OBJ.SetActive(true);
            icon.color = garyColor;
        }
        else if (furnitureItem.IsUnlocked)
        {
            //解锁
            use_BTN.gameObject.SetActive(true);
            icon.color = Color.white;
        }


    }

    public void ShowUseBtn()
    {
        use_BTN.gameObject.SetActive(true);
        use_TMP.gameObject.SetActive(false);
    }

    //点击使用
    public void UseClick()
    {
        FurnitureManagement.instance.ChangeFurnitureItemGetNewSkine(furnitureItem.Id,false);

        //测试
        FurnitureManagement.instance.GetFurnitureNameDestory(FurnitureManagement.CheckFirstFurniture(furnitureItem.Id));
        FurnitureManagement.instance.ChangeFurnitureItemDefault(FurnitureManagement.CheckFirstFurniture(furnitureItem.Id), false);
        FurnitureManagement.ChangeGridState(FurnitureManagement.CheckFirstFurniture(furnitureItem.Id));

        //if (GameManager.Instance.currentGameLevel.LevelID >= 17)
        //{
        //    //奖励第二套皮肤
        //    FurnitureManagement.instance.GetFurnitureNameDestory(FurnitureManagement.CheckFirstFurniture(furnitureItem.Id));
        //    FurnitureManagement.instance.ChangeFurnitureItemDefault(FurnitureManagement.CheckFirstFurniture(furnitureItem.Id), false);
        //    FurnitureManagement.ChangeGridState(FurnitureManagement.CheckFirstFurniture(furnitureItem.Id));
        //}
        //else
        //    FurnitureManagement._openHallTitle = false;

        //点击生成新的家具
        FurnitureManagement.instance.CreateFurniture(furnitureItem.Id);
        FurnitureManagement.instance.ChangeFurnitureItemDefault(furnitureItem.Id, true);

        //判断是否解锁
        FurnitureManagement.instance.ChangeFurnitureItemUnlock(furnitureItem.Id);

        //添加到本地保存家具列表
        GameManager.Instance.CurrentData.collectionFurnitureName.Remove(furnitureItem.Id);
        GameManager.Instance.SaveData();

        

        //状态改变
        use_BTN.gameObject.SetActive(false);
        use_TMP.gameObject.SetActive(true);
        redPonit.SetActive(false);

        UIManagement.Instance.OpenFurnitureConfirmPlane();

        
    }

}
