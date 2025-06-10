using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatNeedBlockData : MonoBehaviour
{
    public int ID;
    public Image furiteIcon;
    public TMP_Text needNum_TMP;

    //初始化
    public void CatNeedBlockInit(BlockDataConfig blockData, int _ID = 0)
    {
        ID = _ID;
        furiteIcon.sprite = blockData.fruits_IMG;
    }

    //改变猫猫需求文本
    public void ChangeCatNeedTMP()
    { 
    
    }
}
