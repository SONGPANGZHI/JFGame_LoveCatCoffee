using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUpgrade : MonoBehaviour
{
    public List<Sprite> title_Sprit;        //0 默认图 ，1 点击图

    public Button outsideStore_BTN;
    public Button hall_BTN;
    public Button catHouse_BTN;
    public Button back_BTN;

    public Transform outsideStoreTrans;
    public Transform hallTrans;
    public Transform catHouseTrans;

    public GameObject furnitureTypePrefab;

    public List<Transform> HallFurnitureTran = new List<Transform>();
    public List<Transform> CatHouseFurnitureTran = new List<Transform>();

    private void Awake()
    {
        back_BTN.onClick.AddListener(ClosePlane);
    }



    //界面初始化
    public void OpenPlaneInit()
    {
        CreateHall();
        CloseRedPoint();
    }

    //生成大厅外家具
    public void CreateOutsideStore()
    { 
    
    }

    //生成大厅家具
    public void CreateHall()
    {
        for (int i = 0; i < FurnitureManagement.instance.firstFloorFurniture.Count; i++)
        {
            CreateByCategory(FurnitureManagement.instance.firstFloorFurniture[i], HallFurnitureTran);
        }
    }

    //生成猫屋家具
    public void CreateCatHouse()
    {
        
    }

    //按分类生成
    public void CreateByCategory(FurnitureItem Item, List<Transform> Trans)
    {
        switch (Item.FurnitureType)
        {
            case FurnitureType.Floor:
                Trans[0].GetComponent<FurnitureTypeGrid>().GridInit(Item);
                break;
            case FurnitureType.Window:
                Trans[1].GetComponent<FurnitureTypeGrid>().GridInit(Item);
                break;
            case FurnitureType.Wall:
                Trans[2].GetComponent<FurnitureTypeGrid>().GridInit(Item);
                break;
            case FurnitureType.Furniture:
                Trans[3].GetComponent<FurnitureTypeGrid>().GridInit(Item);
                break;
            case FurnitureType.Decorate:
                Trans[4].GetComponent<FurnitureTypeGrid>().GridInit(Item);
                break;
            case FurnitureType.FreenPlants:
                Trans[5].GetComponent<FurnitureTypeGrid>().GridInit(Item);
                break;
        }
    }


    

    //关闭界面
    public void ClosePlane()
    {
        MusicManagement.instance.ClickPlaySFX();
        gameObject.SetActive(false);
        ClearTrans(HallFurnitureTran);
        UIManagement.Instance.OpenMainPlane();
    }

    //红点
    public void CloseRedPoint()
    {
        //关掉红点显示
        if (PlayerPrefs.HasKey(UIManagement.redPointKey))
            PlayerPrefs.DeleteKey(UIManagement.redPointKey);
    }

    //清空
    public void ClearTrans(List<Transform> Trans)
    {
        for (int i = 0; i < Trans.Count; i++)
        {
            for (int j = 0; j < HallFurnitureTran[i].GetComponent<FurnitureTypeGrid>().trans.childCount; j++)
            {
                Destroy(HallFurnitureTran[i].GetComponent<FurnitureTypeGrid>().trans.GetChild(j).gameObject);
            }
        }
    }

    //改变按钮状态 按钮点击 外部调用
    public void ChangeBTNState(string btnName)
    {
        outsideStore_BTN.GetComponent<Image>().sprite = title_Sprit[0];
        hall_BTN.GetComponent<Image>().sprite = title_Sprit[0];
        catHouse_BTN.GetComponent<Image>().sprite = title_Sprit[0];
        outsideStoreTrans.gameObject.SetActive(false);
        hallTrans.gameObject.SetActive(false);
        catHouseTrans.gameObject.SetActive(false);
        switch (btnName)
        {
            case "outsideStore_BTN":
                outsideStore_BTN.GetComponent<Image>().sprite = title_Sprit[1];
                outsideStoreTrans.gameObject.SetActive(true);
                break;
            case "hall_BTN":
                hall_BTN.GetComponent<Image>().sprite = title_Sprit[1];
                hallTrans.gameObject.SetActive(true);
                break;
            case "catHouse_BTN":
                catHouse_BTN.GetComponent<Image>().sprite = title_Sprit[1];
                catHouseTrans.gameObject.SetActive(true);
                break;
        }
    }

    #region 弃用
    //public GameObject gridPrefab;
    //public Transform gridTrans;

    //public Button OkBTN;
    //public Button saveBTN;
    //public Button backBTN;

    //public Transform top_Obj;
    //public Transform bottom_Obj;

    //public static List<FurnitureItem> allFurniture = new List<FurnitureItem>();


    ////点击家具 查看不同皮肤
    //public void FurnitureSkinInit()
    //{
    //    ClearGridTrans();
    //    allFurniture.Clear();
    //    furnitureUseGridList.Clear();
    //    furnitureObj.SetActive(true);
    //    GetAllSkinsForBase(FurnitureManagement.instance.currentClickFurniture.FurnitureItem.DaseFurnitureId);
    //    for (int i = 0; i < allFurniture.Count; i++)
    //    {
    //        GameObject GO = Instantiate(gridPrefab, gridTrans);
    //        GO.GetComponent<FurnitureUseGrid>().currentState = GetSkinState(allFurniture[i], FurnitureManagement.instance.currentClickFurniture.FurnitureItem);
    //        GO.GetComponent<FurnitureUseGrid>().FurnitureSkinInit(allFurniture[i]);
    //        furnitureUseGridList.Add(GO.GetComponent<FurnitureUseGrid>());
    //    }
    //}

    ////判断状态
    //public FurnitureSkinState GetSkinState(FurnitureItem skin, FurnitureItem currentItem)
    //{
    //    if (skin.Id == currentItem.Id)
    //        return FurnitureSkinState.Current;
    //    if (skin.IsUnlocked)
    //        return FurnitureSkinState.Unlocked;
    //    return FurnitureSkinState.Locked;
    //}

    //public void ClearGridTrans()
    //{
    //    for (int i = 0; i < gridTrans.childCount; i++)
    //    {
    //        Destroy(gridTrans.GetChild(i).gameObject);
    //    }
    //}

    //public List<FurnitureItem> GetAllSkinsForBase(string baseFurnitureId)
    //{
    //    foreach (var item in GameManager.Instance.CurrentData.AllFurniture)
    //    {
    //        if (item.DaseFurnitureId == baseFurnitureId)
    //        {
    //            allFurniture.Add(item);
    //        }
    //    }

    //    return allFurniture;
    //}
    #endregion



}
