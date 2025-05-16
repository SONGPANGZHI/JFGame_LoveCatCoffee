using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureManagement : MonoBehaviour
{
    public static FurnitureManagement instance;
    public List<GameObject> defaultFurniture;

    public GameObject furnitureGrid;
    public Transform furnitureTrans;

    public FurnitureUseGrid currentGrid;        //选中格子
    public FurnitureUseGrid selectBoxGrid;      //之前选择的格子

    public FurnitureInfo currentClickFurniture;

    private int defaultFurnitureID;

    public static string dialogueNoveicKey = "DialogueNoveicKEY";       //对话新手引导
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        InitFurniture();
    }

    //新手关卡（第一关通关）
    public void NoviceLevel()
    {
        for (int i = 0; i < 2; i++)
        {
            Destroy(defaultFurniture[i].gameObject);
        }


    }

    //初始化建筑
    public void InitFurniture()
    {
        if (PlayerPrefs.HasKey(dialogueNoveicKey))
        {
            NoviceLevel();
        }
        
        for (int i = 0; i < GameManager.Instance.CurrentData.usedFurniture.Count; i++)
        {
            if (GetDefaultFurniture(GameManager.Instance.CurrentData.usedFurniture[i].name))
            {
                defaultFurniture[defaultFurnitureID].GetComponent<FurnitureInfo>().ChangeSpriteInit(GameManager.Instance.CurrentData.usedFurniture[i].name);
            }
            else
            {
                //根据DefultUse; 判断使用皮肤加载，根据名字转换默认皮肤的位置信息 

                GameObject GO = Instantiate(furnitureGrid, furnitureTrans);
                GO.GetComponent<FurnitureInfo>().FurnitureInfoInit(GetFurnitureKey(GameManager.Instance.CurrentData.usedFurniture[i].name));
                GO.name = GameManager.Instance.CurrentData.usedFurniture[i].name;
            }
          
        }
    }

    //生成建筑 
    public void CreateFurniture(string spriteKey)
    {
        if (GetDefaultFurniture(spriteKey))
        {
            defaultFurniture[defaultFurnitureID].GetComponent<FurnitureInfo>().ChangeSpriteInit(spriteKey);
        }
        else
        {
            GameObject GO = Instantiate(furnitureGrid, furnitureTrans);
            GO.GetComponent<FurnitureInfo>().FurnitureInfoInit(GetFurnitureKey(spriteKey));
            GO.name = spriteKey;
        }
        
    }

    //返回类型
    public FurnitureInfos GetFurnitureKey(string spriteKey)
    {
        if (GameManager.Instance.FurniturePosDic.ContainsKey(spriteKey))
            return GameManager.Instance.FurniturePosDic[spriteKey];

        return null;
    }

    //判断是否是默认家具
    public bool GetDefaultFurniture(string spriteKey)
    {
        for (int i = 2; i < defaultFurniture.Count; i++)
        {
            if (defaultFurniture[i].name == spriteKey)
            {
                defaultFurnitureID = i;
                return true;
            }
        }
        return false;
    }

    //新皮肤
    public FurnitureInfos NewSkinFurnitureInfos(string spriteKey)
    {
        GameManager.Instance.GetSkinString(spriteKey);


        return null;

    }

}

