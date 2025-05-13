using System;
using System.Collections.Generic;
using UnityEngine;
using static LoadAllConfigData;

public class FurnitureManagement : MonoBehaviour
{
    public static FurnitureManagement instance;
    public List<GameObject> defaultFurniture;

    public GameObject furnitureGrid;
    public Transform furnitureTrans;

    public FurnitureUseGrid currentGrid;        //选中格子
    public FurnitureUseGrid selectBoxGrid;      //之前选择的格子


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
        
        for (int i = 0; i < GameManager.Instance.unlockFurniture.Count; i++)
        {
            GameObject GO = Instantiate(furnitureGrid, furnitureTrans);
            GO.GetComponent<FurnitureInfo>().FurnitureInfoInit(GameManager.Instance.unlockFurniture[i]);
        }
    }

    //生成建筑 
    public void CreateFurniture(string spriteKey)
    {

        GameObject GO = Instantiate(furnitureGrid, furnitureTrans);
        GO.GetComponent<FurnitureInfo>().FurnitureInfoInit(GetFurnitureKey(spriteKey));
    }

    //返回类型
    public FurnitureInfos GetFurnitureKey(string spriteKey)
    {
        if (GameManager.Instance.FurniturePosDic.ContainsKey(spriteKey))
            return GameManager.Instance.FurniturePosDic[spriteKey];

        return null;
    }

    //判断是否是默认家具
    public bool GetDefaultFurniture()
    {

        return false;
    }
}

