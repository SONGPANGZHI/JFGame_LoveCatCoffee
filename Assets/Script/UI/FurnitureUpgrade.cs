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


    private void Awake()
    {
        back_BTN.onClick.AddListener(ClosePlane);
    }

    //界面初始化
    public void OpenPlaneInit()
    {

    }

    //生成大厅外家具
    public void CreateOutsideStore()
    { 
    
    }

    //生成大厅家具
    public void CreateHall()
    { 
    
    }

    //生成猫屋家具
    public void CreateCatHouse()
    { 
    
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

    //关闭界面
    public void ClosePlane()
    { 
    
    }


    public GameObject gridPrefab;
    public Transform gridTrans;

    public GameObject furnitureObj;
    public GameObject dialogueBoxObj;
    public GameObject newTitleObj;

    public Button OkBTN;
    public Button saveBTN;
    public Button backBTN;

    public Transform top_Obj;
    public Transform bottom_Obj;

    public static List<FurnitureUseGrid> furnitureUseGridList = new List<FurnitureUseGrid>();

    public static List<FurnitureItem> allFurniture = new List<FurnitureItem>();
    //private void Awake()
    //{
    //    OkBTN.onClick.AddListener(OKClick);
    //    saveBTN.onClick.AddListener(SaveClick);
    //    backBTN.onClick.AddListener(BackClick);
    //}

    //初始化
    public void FurnitureInit()
    {
        top_Obj.DOLocalMoveY(800, 0.3f);
        bottom_Obj.DOMoveY(250, 0.3f);
        furnitureUseGridList.Clear();
        allFurniture.Clear();
        if (!PlayerPrefs.HasKey(FurnitureManagement.dialogueNoveicKey) /*&& GameManager.Instance.CurrentData.collectionFurnitureName.Count == 0*/)
        {
            dialogueBoxObj.SetActive(true);
        }

        if (GameManager.Instance.CurrentData.collectionFurnitureName.Count > 0)
        {
            furnitureObj.SetActive(true);
            newTitleObj.SetActive(true);
            for (int i = 0; i < GameManager.Instance.CurrentData.collectionFurnitureName.Count; i++)
            {
                GameObject GO = Instantiate(gridPrefab, gridTrans);
                GO.GetComponent<FurnitureUseGrid>().FurnitureGridInit(FurnitureManagement.instance.GetFurnitureItem(
                GameManager.Instance.CurrentData.collectionFurnitureName[i]));
            }

        }

        CloseRedPoint();
    }

    //点击家具 查看不同皮肤
    public void FurnitureSkinInit()
    {
        newTitleObj.SetActive(false);
        ClearGridTrans();
        allFurniture.Clear();
        furnitureUseGridList.Clear();
        furnitureObj.SetActive(true);
        GetAllSkinsForBase(FurnitureManagement.instance.currentClickFurniture.FurnitureItem.DaseFurnitureId);
        for (int i = 0; i < allFurniture.Count; i++)
        {
            GameObject GO = Instantiate(gridPrefab, gridTrans);
            GO.GetComponent<FurnitureUseGrid>().currentState = GetSkinState(allFurniture[i], FurnitureManagement.instance.currentClickFurniture.FurnitureItem);
            GO.GetComponent<FurnitureUseGrid>().FurnitureSkinInit(allFurniture[i]);
            furnitureUseGridList.Add(GO.GetComponent<FurnitureUseGrid>());
        }
    }

    //判断状态
    public FurnitureSkinState GetSkinState(FurnitureItem skin, FurnitureItem currentItem)
    {
        if (skin.Id == currentItem.Id)
            return FurnitureSkinState.Current;
        if (skin.IsUnlocked)
            return FurnitureSkinState.Unlocked;
        return FurnitureSkinState.Locked;
    }

    public static void SetGridState()
    {
        for (int i = 0; i < furnitureUseGridList.Count; i++)
        {
            if (furnitureUseGridList[i].furnitureItem.IsDefault)
            {
                furnitureUseGridList[i].use_Tmp.gameObject.SetActive(true);
                furnitureUseGridList[i].useBTN.gameObject.SetActive(false);
            }
            else
            {
                furnitureUseGridList[i].use_Tmp.gameObject.SetActive(false);
                furnitureUseGridList[i].useBTN.gameObject.SetActive(true);
            }
        }
    }

    //点击OK按钮
    public void OKClick()
    {
        dialogueBoxObj.SetActive(false);
        //销毁箱子
        FurnitureManagement.instance.NoviceLevel();
        PlayerPrefs.SetString(FurnitureManagement.dialogueNoveicKey, "dialogueNoveic");
        GameManager.Instance.SaveData();
    }

    //保存
    public void SaveClick()
    {
        furnitureObj.SetActive(false);
        newTitleObj.SetActive(false);
        BaseTools.Instance.RetureCameraDefualtPosition();
        FurnitureManagement.instance.SaveFurnitureDefualtMaterial();
        ClearGridTrans();

    }

    //返回
    public void BackClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        top_Obj.DOLocalMoveY(1200, 0.3f);
        bottom_Obj.DOMoveY(-300, 0.3f).OnComplete(() =>
        {
            furnitureObj.SetActive(false);
            newTitleObj.SetActive(false);
            ClearGridTrans();
            GameManager.Instance.currentFurnitureData.Clear();
            this.gameObject.SetActive(false);
            UIManagement.Instance.OpenMainPlane();
        });


    }

    //红点
    public void CloseRedPoint()
    {
        //关掉红点显示
        if (PlayerPrefs.HasKey(UIManagement.redPointKey))
            PlayerPrefs.DeleteKey(UIManagement.redPointKey);
    }



    public void ClearGridTrans()
    {
        for (int i = 0; i < gridTrans.childCount; i++)
        {
            Destroy(gridTrans.GetChild(i).gameObject);
        }
    }

    public List<FurnitureItem> GetAllSkinsForBase(string baseFurnitureId)
    {
        foreach (var item in GameManager.Instance.CurrentData.AllFurniture)
        {
            if (item.DaseFurnitureId == baseFurnitureId)
            {
                allFurniture.Add(item);
            }
        }

        return allFurniture;
    }

}
