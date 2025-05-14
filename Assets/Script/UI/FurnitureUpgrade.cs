using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUpgrade : MonoBehaviour
{
    public GameObject gridPrefab;
    public Transform gridTrans;

    public GameObject furnitureObj;
    public GameObject dialogueBoxObj;

    public Button OkBTN;
    public Button saveBTN;
    public Button backBTN;

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
        if (!PlayerPrefs.HasKey(FurnitureManagement.dialogueNoveicKey) && GameManager.Instance.CurrentData.collectionFurnitureName.Count == 0)
        {
            dialogueBoxObj.SetActive(true);
        }

        if (GameManager.Instance.CurrentData.collectionFurnitureName.Count > 0)
        {
            furnitureObj.SetActive(true);

            for (int i = 0; i < GameManager.Instance.CurrentData.collectionFurnitureName.Count; i++)
            {
                GameObject GO = Instantiate(gridPrefab, gridTrans);
                GO.GetComponent<FurnitureUseGrid>().FurnitureGridInit(GameManager.Instance.CurrentData.collectionFurnitureName[i]);
            }

        }

        CloseRedPoint();
    }

    //点击OK按钮
    public void OKClick()
    {
        dialogueBoxObj.SetActive(false);
        //销毁箱子
        FurnitureManagement.instance.NoviceLevel();
        PlayerPrefs.SetString(FurnitureManagement.dialogueNoveicKey, "dialogueNoveicKey");
    }

    //保存
    public void SaveClick()
    { 
    
    }

    //返回
    public void BackClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        transform.DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            ClearGridTrans();
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

    public void CloseFurniturePlane()
    { 
    
    }

    public void ClearGridTrans()
    {
        for (int i = 0; i < gridTrans.childCount; i++)
        {
            Destroy(gridTrans.GetChild(i).gameObject);
        }
    }
}
