using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameLevelInfo
{
    public int LevelID;
    public int BlockType;
    public MysteryBoxArea MysteryBox;
    public MiddleBlockNum BlockNum;
    public float conveyorSpeed;
    public int PositionsNum;
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
    public FurnitureFloor FurnitureFloor;
    public FurnitureType FurnitureType;
    public bool IsUnlocked;             // 是否已解锁
    public bool IsDefault;              // 是否是默认皮肤
    public bool IsNewSkin;              // 新皮
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