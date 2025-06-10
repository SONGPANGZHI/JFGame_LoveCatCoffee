using System.Collections.Generic;
using UnityEngine;

public class CatData : MonoBehaviour
{
    public GameObject furitePrefab;
    public Transform furiteTrans;
    public GameObject catAnim;
    public List<CatRequirement> catRequirements;


    //猫咪初始化UI
    public void CatDataInit()
    {
        RandomCatNeed();
        for (int i = 0; i < catRequirements.Count; i++)
        {
            GameObject GO = Instantiate(furitePrefab, furiteTrans);
            GO.GetComponent<CatRequirementFurite>().FuriteInit(catRequirements[i]);
        }
       
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
        // 可以播放动画、增加分数等
        Debug.Log("所有需求已完成!");
        PlayGameManagement.Instance.cats.Remove(this);
    }
}
