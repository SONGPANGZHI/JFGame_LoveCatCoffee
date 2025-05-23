using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUpgrade : MonoBehaviour
{
    public GameObject gridPrefab;
    public Transform gridTrans;

    public GameObject furnitureObj;
    public GameObject dialogueBoxObj;
    public GameObject newTitleObj;

    public Button OkBTN;
    public Button saveBTN;
    public Button backBTN;

    public static List<FurnitureUseGrid> furnitureUseGridList = new List<FurnitureUseGrid>();

    public static List<FurnitureItem> allFurniture = new List<FurnitureItem>();
    private void Awake()
    {
        OkBTN.onClick.AddListener(OKClick);
        saveBTN.onClick.AddListener(SaveClick);
        backBTN.onClick.AddListener(BackClick);
    }

    //初始化
    public void FurnitureInit()
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.3F);
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
        if (!PlayerPrefs.HasKey(GameManager.SaveImageKey))
        {
            PlayerPrefs.SetString(GameManager.SaveImageKey, "SaveLoadImageKEY");
        }
        Invoke("Photograph", 0.5F);
    }

    public void Photograph()
    {
        BaseTools.Instance.CapturePhoto();
    }

    //返回
    public void BackClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        transform.DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            furnitureObj.SetActive(false);
            newTitleObj.SetActive(false);
            ClearGridTrans();
            GameManager.Instance.currentFurnitureData.Clear();
            this.gameObject.SetActive(false);
        });
        UIManagement.Instance.loadingPlane.gameObject.SetActive(true);
        UIManagement.Instance.CloseFurnitureUpgradePlane();
        UIManagement.Instance.loadingPlane.LoadUIScene();
    }

    //红点
    public void CloseRedPoint()
    {
        //关掉红点显示
        if (PlayerPrefs.HasKey(UIManagement.redPointKey))
            PlayerPrefs.DeleteKey(UIManagement.redPointKey);
    }

    //public void CloseFurniturePlane()
    //{ 

    //}

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
