using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FurnitureUseGrid : MonoBehaviour
{
    public GameObject selectBox;
    public Image furnitureIcon;
    public Button useBTN;
    public TMP_Text use_Tmp;
    public TMP_Text unlock_Tmp;
    public GameObject unlock_IMG;

    private string spriteKey;

    private void Awake()
    {
        useBTN.onClick.AddListener(UseClick);
    }

    //格子初始化 未使用
    public void FurnitureGridInit(string spriteName)
    {
        spriteKey = spriteName;
        unlock_IMG.SetActive(false);
        furnitureIcon.gameObject.SetActive(true);
        furnitureIcon.sprite = ListExtensions.LoadFurnitureSprite(spriteName);
        useBTN.gameObject.SetActive(true);
        use_Tmp.gameObject.SetActive(false);
    }

    //生成其他皮肤
    public void FurnitureSkinInit(string spriteName)
    {
        spriteKey = spriteName;
        unlock_IMG.SetActive(false);
        if (spriteName == "None")
        {
            unlock_IMG.SetActive(true);
            furnitureIcon.gameObject.SetActive(false);
        }
        else
        {
            furnitureIcon.sprite = ListExtensions.LoadFurnitureSprite(spriteName);
            furnitureIcon.color = Color.white;
        }

        //本地使用家具
        GridState();
    }

    //格子状态
    public void GridState()
    {
        if (DetermineFurnitureUnlocked(spriteKey))
        {
            if (FurnitureManagement.instance.currentClickFurniture.furnitureName == spriteKey)
            {
                use_Tmp.gameObject.SetActive(true);
            }

        }
        else if (GetNewSkin(spriteKey))
        {
            useBTN.gameObject.SetActive(true);
        }
        else
        {
            furnitureIcon.color = Color.grey;
            unlock_Tmp.gameObject.SetActive(true);
        }
    }

    //点击使用
    public void UseClick()
    {
        FurnitureManagement.instance.CreateFurniture(spriteKey);

        //添加到本地保存家具列表
        FurnitureReward furnitureReward = new FurnitureReward(spriteKey);
        GameManager.Instance.CurrentData.collectionFurnitureName.Remove(spriteKey);
        GameManager.Instance.CurrentData.usedFurniture.Add(furnitureReward);
        GameManager.Instance.SaveData();

        //状态改变
        useBTN.gameObject.SetActive(false);
        use_Tmp.gameObject.SetActive(true);
    }

    //默认使用
    public void DefaultUseBox()
    {
        selectBox.SetActive(true);
        use_Tmp.gameObject.SetActive(true);
    }


    //选择框打开
    public void OpenSelectBox()
    {
       
    }

    //选择框关闭
    public void CloceSelectBox()
    {
       
    }

    //判断家具是否解锁
    public bool DetermineFurnitureUnlocked(string spriteName)
    {
        for (int i = 0; i < GameManager.Instance.CurrentData.usedFurniture.Count; i++)
        {
            if (GameManager.Instance.CurrentData.usedFurniture[i].name == spriteName)
                return true;
        }

        return false;
    }

    //判断家具是否解锁
    public bool GetNewSkin(string spriteName)
    {
        for (int i = 0; i < GameManager.Instance.CurrentData.newSkinFurniture.Count; i++)
        {
            if (GameManager.Instance.GetSkinString(GameManager.Instance.CurrentData.newSkinFurniture[i]) == spriteName)
                return true;
        }

        return false;
    }
}
