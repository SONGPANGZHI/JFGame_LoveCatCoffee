using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LoadAllConfigData : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.Instance.gameLevelDic.Count == 0)
        {
            StartLoadConfigAsset();
            LoadFurnitureItemJson();
            LoadAwardFurniturePoolJson();
        }
    }

    #region 关卡数据加载

    public void StartLoadConfigAsset()
    {
        string localUrl = "Json/GameLevelData";
        ParsingContent(Resources.Load<TextAsset>(localUrl).text);
    }

    void ParsingContent(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<List<object>>(_data);

        for (int i = 0; i < rootData.Count; i++)
        {
            GameLevelInfo gameLevelInfo = JsonConvert.DeserializeObject<GameLevelInfo>(rootData[i].ToString());
            GameManager.Instance.gameLevelDic.Add(gameLevelInfo.LevelID, gameLevelInfo);
        }
        Debug.Log("关卡数据加载成功---");

    }

    #endregion

    #region 家具奖励池

    //读取奖励池
    public void LoadFurnitureItemJson()
    {
        string localUrl = "Json/FurnitureData";
        StartLoadFurnitureItam(Resources.Load<TextAsset>(localUrl).text);
    }


    public void StartLoadFurnitureItam(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<List<object>>(_data);

        for (int i = 0; i < rootData.Count; i++)
        {
            FurnitureItem furnitureInfo = JsonConvert.DeserializeObject<FurnitureItem>(rootData[i].ToString());
            GameManager.AllFurnitureData.Add(furnitureInfo);
        }

        Debug.Log("家具奖励池加载成功---");
    }

   

   

    #endregion

    #region 读取位置奖励池
    //读取奖励池
    public void LoadAwardFurniturePoolJson()
    {
        string localUrl = "Json/FurnitureReward";
        StartLoadAwardFurniturePool(Resources.Load<TextAsset>(localUrl).text);
    }

    public void StartLoadAwardFurniturePool(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<List<object>>(_data);

        for (int i = 0; i < rootData.Count; i++)
        {
            FurnitureReward furnitureInfo = JsonConvert.DeserializeObject<FurnitureReward>(rootData[i].ToString());
            GameManager.Instance.awardFurniturePool.Add(furnitureInfo.name);
        }

        Debug.Log("奖励池加载成功---");
    }

    

    #endregion

}

[Serializable]
public class GameLevelInfo
{
    public int LevelID;
    public int BlockType;
    public MysteryBoxArea MysteryBox;
    public MiddleBlockNum BlockNum;
    public float conveyorSpeed;
    public int PositionsNum;
    public bool cat;
    public int CatAppearTime;
    public CatSectionProbability CatSection;
    public List<string> FurnitureName;

}

[Serializable]
public class MysteryBoxArea
{
    public float ConveyorArea;
    public float BlockArea;
}

[Serializable]
public class MiddleBlockNum
{
    public int min;
    public int max;
}

[Serializable]
public class CatSectionProbability
{
    public float min;
    public float max;
}

//奖励池子
[Serializable]
public class FurnitureReward
{
    public int ID;
    public AwardType AwardType;
    public string name;
    public FurnitureFloor FurnitureFloor;
    public bool DefultUse;

}

//本地保存
[Serializable]
public class GameSaveData
{
    public List<FurnitureItem> AllFurniture = new List<FurnitureItem>();           //使用的家具
    //public List<FurnitureReward> usedFurniture = new List<FurnitureReward>();           //使用的家具
    public List<string> collectionFurnitureName = new List<string>();                   //领取家具 但未使用
    public List<string> AwardFurniturePool = new List<string>();                          //家具新皮肤

}

[Serializable]
public class FurnitureItem
{
    public string Id;                   // 家具唯一ID
    public bool IsUnlocked;             // 是否已解锁
    public bool IsDefault;              // 是否是默认皮肤
    public Vector2 DefaultPosition;     // 默认位置
    public string DaseFurnitureId;      // 基础家具ID（用于皮肤分组）
    public int OrderLayer;
    public bool CameraSize;             //改变摄像机大小
    public bool NoUpgradeFurniture;     //不需要升级
}

[Serializable]
public class PlacedFurniture
{
    public string instanceId;
    public string furnitureId;
    public Vector3 position;
    public Vector3 rotation;
}