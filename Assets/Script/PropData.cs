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

[Serializable]
public class BlockPropDataClass
{
    public int ID;
    public bool active;
    public BlockDataConfig config;

    public BlockPropDataClass(int _id,bool _active, BlockDataConfig _config)
    { 
        ID= _id;
        active= _active;    
        config= _config;
    }
}

public enum BlockHierarchy
{ 
    None,
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

//新版玩法类型
public enum BlockPropType
{
    None,               //
    Apple,              //苹果
    Avocado,            //牛油果
    Banana,             //香蕉
    Blueberry,          //蓝莓
    Coconut,            //椰子
    Grape,              //葡萄
    KiwiFruit,          //猕猴桃
    Lemon,              //柠檬
    Litchi,             //荔枝
    Mango,              //芒果
    Peach,              //桃子
    Pear,               //梨
    Pineapple,          //菠萝
    Pitaya,             //火龙果
    Strawberry,         //草莓
    Watermelon,         //西瓜
}

public enum TypesCards
{ 
    None,
    KeyCard,            //关键牌
    SurplusCard,        //冗余牌
    MysteryCard,        //盲盒
}

//家具皮肤
public enum FurnitureType
{ 
    None,
    YellowHouse,
    PinkHouse,
}

public enum FurnitureFloor
{
    None,
    FirstFloor,
    SecondFloor,
    ThirdFloor,
}
public enum AwardType
{
    None,
    Furniture,
    Cat,
    Block,
}
public enum FurnitureSkinState 
{  
    Current, 
    Default, 
    Unlocked, 
    Locked 
}
