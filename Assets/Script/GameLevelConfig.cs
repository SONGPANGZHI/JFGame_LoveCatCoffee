using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameLevelConfig", menuName = "GameLevel")]
public class GameLevelConfig : ScriptableObject
{
    public int LevelID;                                 //关卡ID
    public int Target;                                  //猫猫需求总数
    public int KeyCardNum;
    public int Amount;                                  //总牌数
    public int TypeID;                                  //生成牌的种类数量
    public float ConveyorSpeed;                         //传送带速度
    public float ClearTime;                             //通关时长
    public int ClearStep;                               //通关步数
    public List<ProbabilityCardsAppeare> CardType;      //关键牌 在各层比例
}

[Serializable]
public class ProbabilityCardsAppeare
{
    public TypesCards CardType;
    public float BottomLayer;
    public float InterLayer;
    public float TopLayer;
}