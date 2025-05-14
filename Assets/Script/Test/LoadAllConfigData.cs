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
            LoadFurnitureRewardJson();
            LoadFurniturePosJson();
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
    public void LoadFurnitureRewardJson()
    {
        string localUrl = "Json/FurnitureReward";
        StartLoadFurnitureReward(Resources.Load<TextAsset>(localUrl).text);
    }


    public void StartLoadFurnitureReward(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<List<object>>(_data);

        for (int i = 0; i < rootData.Count; i++)
        {
            FurnitureReward furnitureInfo = JsonConvert.DeserializeObject<FurnitureReward>(rootData[i].ToString());
            GameManager.Instance.furnitureRewardDic.Add(furnitureInfo.ID, furnitureInfo);
        }

        Debug.Log("家具奖励池加载成功---");
    }

   

   

    #endregion

    #region 读取位置奖励池
    //读取奖励池
    public void LoadFurniturePosJson()
    {
        string localUrl = "Json/FurniturePosData";
        StartLoadFurniturePos(Resources.Load<TextAsset>(localUrl).text);
    }

    public void StartLoadFurniturePos(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<List<object>>(_data);

        for (int i = 0; i < rootData.Count; i++)
        {
            FurnitureInfos furnitureInfo = JsonConvert.DeserializeObject<FurnitureInfos>(rootData[i].ToString());
            GameManager.Instance.FurniturePosDic.Add(furnitureInfo.FurnitureName, furnitureInfo);
        }

        Debug.Log("家具位置加载成功---");
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
    public int cat;
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

[Serializable]
public class FurnitureInfos
{
    public string FurnitureName;
    public Vector2 FurniturePos;
    public int OrderLayer;
    public List<string> FurnitureSkinName;
}

//奖励池子
[Serializable]
public class FurnitureReward
{
    public int ID;
    public AwardType AwardType;
    public string name;
    public FurnitureFloor FurnitureFloor;

    public FurnitureReward(string _name)
    {
        name = _name;
    }
}

[Serializable]
public class GameSaveData
{
    public List<FurnitureReward> usedFurniture = new List<FurnitureReward>();           //使用的家具
    public List<string> collectionFurnitureName = new List<string>();                   //领取家具 但未使用

}
