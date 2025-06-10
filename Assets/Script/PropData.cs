using System;
using System.Collections.Generic;
using System.Linq;
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
[Serializable]
public class CatRequirement
{
    public BlockPropType requiredType; // 需求方块类型
    public int totalRequired;     // 总需求数量
    public int currentCount;      // 当前已收集数量

    public bool IsSatisfied => currentCount <= 0;

    public CatRequirement(BlockPropType _blockPropType,int total,int current)
    {
        requiredType =  _blockPropType;
        totalRequired = total;
        currentCount = current;
    }
}

[Serializable]
public class Cat
{
    public string name;
    public Sprite icon;
    public List<CatRequirement> requirements = new List<CatRequirement>();

    public bool IsFullySatisfied => requirements.All(r => r.IsSatisfied);

    // 添加新需求
    //public void AddRequirement(BlockPropType type, int count)
    //{
    //    requirements.Add(new CatRequirement()
    //    {
    //        requiredType = type,
    //        totalRequired = count,
    //        currentCount = 0
    //    });
    //}
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

//关卡类型
public enum LevelType
{
    Countdown,
    CatNeedNum,
    TimeAndCat
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

public enum FurnitureType
{ 
    None,
    Window,             //窗户
    Hall,               //大厅
    Floor,              //地板
    Furniture,          //家具
    FreenPlants,        //绿植
    Wall,               //墙
    Decorate,           //装饰
}


