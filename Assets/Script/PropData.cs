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

