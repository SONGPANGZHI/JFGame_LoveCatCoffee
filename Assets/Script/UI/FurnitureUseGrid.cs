using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUseGrid : MonoBehaviour
{
    public GameObject selectBox;
    public Image furnitureIcon;
    public Button useBTN;
    public TMP_Text use_Tmp;

    private string spriteKey;
    private void Awake()
    {
        useBTN.onClick.AddListener(UseClick);
    }

    //格子初始化 未使用
    public void FurnitureGridInit(string spriteName)
    {
        spriteKey = spriteName;
        furnitureIcon.sprite = ListExtensions.LoadFurnitureSprite(spriteName);
        useBTN.gameObject.SetActive(true);
        use_Tmp.gameObject.SetActive(false);

        
    }

    //已使用
    public void FurnitureUseGridInit(string spriteName)
    {
        furnitureIcon.sprite = ListExtensions.LoadFurnitureSprite(spriteName);
        selectBox.SetActive(false);
        useBTN.gameObject.SetActive(false);
        use_Tmp.gameObject.SetActive(true);
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

    //选择框打开
    public void OpenSelectBox()
    {
        
    }

    //选择框关闭
    public void CloceSelectBox()
    {
       
    }
   

}
