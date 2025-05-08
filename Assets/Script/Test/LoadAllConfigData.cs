using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAllConfigData : MonoBehaviour
{

    private void Awake()
    {
        if(GameManager.Instance.gameLevelDic.Count == 0)
            StartLoadConfigAsset();
    }

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
            GameManager.Instance.gameLevelInfos.Add(gameLevelInfo);
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
}
