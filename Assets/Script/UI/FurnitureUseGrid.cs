using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUseGrid : MonoBehaviour
{
    public GameObject selectBox;
    public Image furnitureIcon;
    public Button useBTN;
    public TMP_Text use_Tmp;
    public TMP_Text unlock_Tmp;
    public GameObject unlock_IMG;
    public FurnitureSkinState currentState;

    public FurnitureItem furnitureItem;

    private void Awake()
    {
        useBTN.onClick.AddListener(UseClick);
    }

    //格子初始化 未使用
    public void FurnitureGridInit(FurnitureItem furniture)
    {
        furnitureItem = furniture;
        furnitureIcon.sprite = ListExtensions.LoadFurnitureSprite(furnitureItem.Id);
        useBTN.gameObject.SetActive(true);
    }

    //生成其他皮肤
    public void FurnitureSkinInit(FurnitureItem furniture)
    {
        furnitureItem = furniture;
        furnitureIcon.sprite = ListExtensions.LoadFurnitureSprite(furnitureItem.Id);
        SetFurnitureState();
    }

    //根据状态确定显示按钮
    public void SetFurnitureState()
    {
        switch (currentState)
        {
            case FurnitureSkinState.Current:
                use_Tmp.gameObject.SetActive(true);
                break;
            case FurnitureSkinState.Unlocked:
                useBTN.gameObject.SetActive(true);
                break;
            case FurnitureSkinState.Locked:
                unlock_Tmp.gameObject.SetActive(true);
                break;
        }
    }

    //点击使用
    public void UseClick()
    {
        MusicManagement.instance.PlayDropZoneSFX();

        for (int i = 0; i < FurnitureUpgrade.allFurniture.Count; i++)
        {
            FurnitureManagement.instance.GetFurnitureNameDestory(FurnitureUpgrade.allFurniture[i].Id);
            FurnitureManagement.instance.ChangeFurnitureItemDefault(FurnitureUpgrade.allFurniture[i].Id, false);
        }

        //点击生成新的家具
        FurnitureManagement.instance.CreateFurniture(furnitureItem.Id);
        FurnitureManagement.instance.ChangeFurnitureItemDefault(furnitureItem.Id, true);
        //判断是否解锁
        FurnitureManagement.instance.ChangeFurnitureItemUnlock(furnitureItem.Id);

        //添加到本地保存家具列表
        GameManager.Instance.CurrentData.collectionFurnitureName.Remove(furnitureItem.Id);
        GameManager.Instance.SaveData();

        //状态改变

        useBTN.gameObject.SetActive(false);
        use_Tmp.gameObject.SetActive(true);

        FurnitureUpgrade.SetGridState();
    }

   

}
