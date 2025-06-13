using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatData : MonoBehaviour
{
    public int priority = 0; // 数值越小优先级越高
    public GameObject furitePrefab;
    public Transform furiteTrans;
    public Image catHead;
    public Image catArm;
    public Image catHand;
    public List<CatRequirement> catRequirements;


    //猫咪初始化UI
    public void CatDataInit(int id)
    {
        priority = id;
        RandomCatNeed();
        RandomCatSkin();
        for (int i = 0; i < catRequirements.Count; i++)
        {
            GameObject GO = Instantiate(furitePrefab, furiteTrans);
            GO.GetComponent<CatRequirementFurite>().FuriteInit(catRequirements[i]);
        }
       
    }

    //随机猫猫皮肤
    public void RandomCatSkin()
    { 
        int skinIndex = Random.Range(0, PlayGameManagement.Instance.catSkinList.Count);
        UpdateCatSkin(PlayGameManagement.Instance.catSkinList[skinIndex]);
    }

    //更新皮肤
    public void UpdateCatSkin(CatSkin catSkin)
    {
        catHead.sprite = catSkin.catHeat;
        catArm.sprite = catSkin.catArm;
        catHand.sprite = catSkin.catHand;
    }

    //随机猫咪需求
    public void RandomCatNeed()
    { 
        int Requirementid = Random.Range(1, 3);
        RandomRequirement(Requirementid);
    }

    //随机需求
    public void RandomRequirement(int times)
    {
        for (int i = 0; i < times; i++)
        {
            int Num = Random.Range(1, 3);
            int typeID = Random.Range(0, GameManager.Instance.currentGameLevel.BlockType);
            int totalRequired = Num * 3;
            BlockPropType blockPropType;
            blockPropType = PlayGameManagement.Instance.blockTypes[typeID].blockPropType;

            CatRequirement newNeed = new CatRequirement(blockPropType, totalRequired, 0);
            catRequirements.Add(newNeed);
        }
    }

    public bool AllRequirementsCompleted()
    {
        foreach (Transform child in furiteTrans)
        {
            CatRequirementFurite requirement = child.GetComponent<CatRequirementFurite>();
            if (requirement != null && requirement.currentRequired > 0)
            {
                return false;
            }
        }
        return true;
    }

    // 检查所有需求是否完成
    public void CheckAllRequirementsCompleted()
    {
        bool allCompleted = true;

        foreach (Transform child in furiteTrans)
        {
            CatRequirementFurite requirement = child.GetComponent<CatRequirementFurite>();
            if (requirement != null && requirement.currentRequired > 0)
            {
                allCompleted = false;
                break;
            }
        }

        if (allCompleted)
        {
            // 所有需求完成，触发猫猫满意行为
            OnAllRequirementsMet();
        }
    }

    private void OnAllRequirementsMet()
    {
        Debug.Log("所有需求已完成!");
        PlayGameManagement.Instance.requirementNum += 1;
        PlayGameManagement.Instance.GetLevelTitle();
        PlayGameManagement.Instance.cats.Remove(this);
        Destroy(gameObject);

        if (PlayGameManagement.Instance.JuageCreateCat() && GameManager.Instance.pauseGame)
        {
            //游戏胜利
             if(PlayGameManagement.Instance.allRequirements.Count == 1)
                UIManagement.Instance.OpenGameOverPlane(true);
        }
        else
        {
            //生成新的猫猫
            PlayGameManagement.Instance.GenerateNewCatRequirements();
        }
       
    }
}
