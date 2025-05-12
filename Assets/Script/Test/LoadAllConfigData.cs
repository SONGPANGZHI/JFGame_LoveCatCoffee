using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.XR;
using static LoadAllConfigData;

public class LoadAllConfigData : MonoBehaviour
{
    private string furnitureReward_url;
    public SavaFurnitureData furnitureRewardInfo;
    private void Awake()
    {
        furnitureReward_url = Application.persistentDataPath + "/furnitureRewardInfo. txt";
        LoadLocal();

        if (GameManager.Instance.gameLevelDic.Count == 0)
            StartLoadConfigAsset();


        LoadFurnitureRewardJson();
        LoadFurniturePosJson();
    }
    private void Start()
    {
        //加载保存数据
        LoadSavaFurnitureData();
    }

    #region 本地数据读取

    public void LoadLocal()
    {
        if (File.Exists(furnitureReward_url))
        {
            //读取文件的所有字节数组，转换成字符串json，然后还原成player对象bytel] bytes = File.ReadAllBytes(playerInfo url);
            byte[] bytes = File.ReadAllBytes(furnitureReward_url);
            string json = System.Text.Encoding.UTF8.GetString(bytes);
            furnitureRewardInfo = JsonUtility.FromJson<SavaFurnitureData>(json);
        }
        else
        {
            //没有这个文件，就先创建
            furnitureRewardInfo = new SavaFurnitureData();
            SaveFurnitureReward();//测试打印下这个路径Debug.LogError(playerInfo url);
        }

    }

    public void SaveFurnitureReward()
    {
        string json = JsonUtility.ToJson(furnitureRewardInfo);
        File.WriteAllBytes(furnitureReward_url, System.Text.Encoding.UTF8.GetBytes(json));
    }

    #endregion

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

    public void LoadSavaFurnitureData()
    {
        GameManager.Instance.furnitureRewards = furnitureRewardInfo.furnitureReward;
    }

    //奖励池子
    [Serializable]
    public class FurnitureReward
    {
        public int ID;
        public AwardType AwardType;
        public string name;
        public FurnitureFloor FurnitureFloor;
    }

    #endregion

    public class SavaFurnitureData
    {
        public List<FurnitureReward> furnitureReward;
    }



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

    [Serializable]
    public class FurnitureInfos
    {
        public string FurnitureName;
        public Vector2 FurniturePos;
        public int OrderLayer;
    }
}
