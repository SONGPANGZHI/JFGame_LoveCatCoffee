using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameLevelConfig", menuName = "GameLevel")]
public class GameLevelConfig : ScriptableObject
{
    public int LevelID;                                 //�ؿ�ID
    public int Target;                                  //èè��������
    public int KeyCardNum;
    public int Amount;                                  //������
    public int TypeID;                                  //�����Ƶ���������
    public float ConveyorSpeed;                         //���ʹ��ٶ�
    public float ClearTime;                             //ͨ��ʱ��
    public int ClearStep;                               //ͨ�ز���
    public List<ProbabilityCardsAppeare> CardType;      //�ؼ��� �ڸ������
}

[Serializable]
public class ProbabilityCardsAppeare
{
    public TypesCards CardType;
    public float BottomLayer;
    public float InterLayer;
    public float TopLayer;
}