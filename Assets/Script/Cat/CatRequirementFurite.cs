using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatRequirementFurite : MonoBehaviour
{
    public CatRequirement currentRequirement;
    public Image Furite_IMG;
    public TMP_Text furite_TMP;
    public GameObject complete_IMG;

    public int currentRequired;
    public int maxRequired;

    public void FuriteInit(CatRequirement catRequirement)
    {
        complete_IMG.SetActive(false);
        currentRequirement = catRequirement;
        maxRequired = currentRequirement.totalRequired;
        currentRequired = currentRequirement.totalRequired;
        furite_TMP.text = currentRequired.ToString();
        Furite_IMG.sprite = GetFuriteIcon(currentRequirement.requiredType);
        PlayGameManagement.Instance.allRequirements.Add(this);
    }

    // 改变UI显示
    public void DecreaseRequirement(int amount)
    {
        if (currentRequired <= 0)
            return;

        currentRequired = Mathf.Max(0, currentRequired - amount);

        // 只在需求变化时更新UI
        if (currentRequired <= 0)
        {
            complete_IMG.SetActive(true);
            furite_TMP.gameObject.SetActive(false);
            Invoke("OnRequirementCompleted", 0.3f);
        }
        else
        {
            furite_TMP.text = currentRequired.ToString();
        }
    }

    
    private void OnRequirementCompleted()
    {
        // 通知父对象检查是否所有需求都完成了
        GetComponentInParent<CatData>()?.CheckAllRequirementsCompleted();
        PlayGameManagement.Instance.allRequirements.Remove(this);
    }

    //获取图片
    public Sprite GetFuriteIcon(BlockPropType blockPropType)
    {
        Sprite _furite_IMG;
        switch (blockPropType)
        {
            case BlockPropType.Apple:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[0].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Avocado:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[1].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Banana:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[2].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Blueberry:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[3].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Grape:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[4].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.KiwiFruit:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[5].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Lemon:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[6].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Litchi:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[7].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Mango:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[8].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Peach:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[9].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Pear:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[10].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Pineapple:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[11].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Pitaya:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[12].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Strawberry:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[13].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Watermelon:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[14].fruits_IMG;
                return _furite_IMG;
            case BlockPropType.Coconut:
                _furite_IMG = PlayGameManagement.Instance.blockDataConfig[15].fruits_IMG;
                return _furite_IMG;
        }

        return null;
    }
}
