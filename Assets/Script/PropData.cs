using System;
using UnityEngine;

//道具类
[Serializable]
public class PropData
{
    public PropType propType;
    public AwardvideoType awardvideoType;
    public Sprite propIcon;
    public string propDesc;
}

public enum BlockHierarchy
{ 
    TopBlock,
    MiddleBlock,
    BottomBlock,
}

//道具类型
public enum PropType
{
    Clear,          //清除
    Speed,          //移速
    Perspective     //透视
}

//通用界面类型
public enum CommonPlaneType
{
    Resurgence,     //复活界面
    Affirm,         //确认界面
    Prop            //道具界面 
}

//奖励发放
public enum AwardvideoType
{
    Clear,          //清除
    Speed,          //移速
    Perspective,    //透视
    Heart,          //复活
}

public enum BlockPropType
{ 
    None,               //
    Ball,               //球
    BlueBrush,          //蓝色刷子
    Brush,              //刷子
    CannedFish,         //鱼罐头
    Cat,                //猫
    CatTreas,           //猫条
    Cheese,             //奶酪
    Gamepad,            //手柄
    Gift,               //礼物
    HoneyPot,           //蜂蜜
    IceCream,           //甜筒
    Jelly,              //果冻
    Milk,               //牛奶
    Sanitizer,          //洗手液
    StrawberryMilk,     //草莓牛奶
    Sugar,              //糖
    Woolen              //毛线
}

public enum TypesCards
{ 
    None,
    KeyCard,            //关键牌
    SurplusCard,        //冗余牌
    MysteryCard,        //盲盒
}

public enum ConveyorLayer
{ 
    Top,
    Bottom,
}
