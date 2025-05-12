using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static LoadAllConfigData;

public class FurnitureManagement : MonoBehaviour
{
    public List<GameObject> defaultFurniture;

    public GameObject furnitureGrid;
    public Transform furnitureTrans;

    private void Start()
    {
        InitFurniture();
    }

    //新手关卡（第一关通关）
    public void NoviceLevel()
    {
        for (int i = 0; i < defaultFurniture.Count; i++)
        {
            Destroy(defaultFurniture[i].gameObject);
        }
    }

    //初始化建筑
    public void InitFurniture()
    {
        for (int i = 0; i < GameManager.Instance.unlockFurniture.Count; i++)
        {
            GameObject GO = Instantiate(furnitureGrid, furnitureTrans);
            GO.GetComponent<FurnitureInfo>().FurnitureInfoInit(GameManager.Instance.unlockFurniture[i]);
        }
    }
}

