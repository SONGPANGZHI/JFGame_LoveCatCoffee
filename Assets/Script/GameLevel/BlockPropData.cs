using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPropData : MonoBehaviour
{
    #region 新版玩法三消

    public BlockDataConfig dataConfig;

    public BlockPropType propType;
    public Image icon;

    private void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(BlockClick);
    }


    public void BlockInit(BlockDataConfig blockDataConfig)
    {
        dataConfig = blockDataConfig;
        propType = dataConfig.blockPropType;
        icon.sprite = dataConfig.DorpZoneSprite;
    }

    //按钮点击
    public void BlockClick()
    {
        //MusicManagement.instance.ClickPlaySFX();
        PlayGameManagement.Instance.CreateDropZoneObject(dataConfig);
        Destroy(gameObject);
    }

    #endregion


    #region 旧版玩法三消

    //public BlockPropDataClass blockPropData;

    //public int ID;
    //public BlockPropType propType;
    //public Image icon;
    //public bool active;
    //public BlockHierarchy hierarchy;
    //public List<GameObject> unlockblock;
    //public Color unlockColor;

    //public static Vector2 clickPos;
    //public static Action JudgeThirdRowUnlockActon;
    //public static Action JudgeScendRowUnlockActon;

    //private void Awake()
    //{
    //    transform.GetComponent<Button>().onClick.AddListener(BlockClick);
    //}

    //public void BlockInit(BlockPropDataClass Data)
    //{
    //    blockPropData = Data;
    //    ID = blockPropData.ID;
    //    active = blockPropData.active;
    //    propType = blockPropData.config.blockPropType;
    //    icon.sprite = blockPropData.config.Icon;
    //    icon.SetNativeSize();
    //    gameObject.SetActive(active);
    //    Invoke("ShowUnlock", 0.2F);
    //}

    ////按钮点击
    //public void BlockClick()
    //{
    //    MusicManagement.instance.ClickPlaySFX();
    //    gameObject.SetActive(false);
    //    active = false;
    //    UpdateData();
    //    GameLevelManagement.Instance.CreateDropZoneObject(this);
    //    JudgeBlockClick();
    //}

    ////延迟一秒 显示状态
    //public void ShowUnlock()
    //{
    //    if (CheckUnlock())
    //    {
    //        transform.GetComponent<Image>().color = Color.white;
    //        transform.GetComponent<Button>().interactable = true;
    //    }
    //    else
    //    {
    //        transform.GetComponent<Image>().color = unlockColor;
    //        transform.GetComponent<Button>().interactable = false;
    //    }
    //}


    //public bool CheckUnlock()
    //{
    //    if (unlockblock.Count == 0)
    //    {
    //        return true;
    //    }
    //    else if (unlockblock.Count == 1)
    //    {
    //        if (!unlockblock[0].activeSelf)
    //            return true;
    //    }
    //    else
    //    {
    //        for (int i = 0; i < unlockblock.Count; i++)
    //        {
    //            if (!unlockblock[0].activeSelf && !unlockblock[1].activeSelf)
    //                return true;
    //        }
    //    }

    //    return false;
    //}

    ////刷新数据
    //public void UpdateData()
    //{
    //    GameLevelManagement.Instance.ModifyBlockByIndex(ID, false);
    //}

    ////判断方块是否解锁
    //public void JudgeBlockClick()
    //{
    //    switch (hierarchy)
    //    {
    //        case BlockHierarchy.BottomBlock:
    //            JudgeScendRowUnlockActon?.Invoke();
    //            break;
    //        case BlockHierarchy.MiddleBlock:
    //            JudgeThirdRowUnlockActon?.Invoke();
    //            break;
    //    }
    //}

    ////添加数据
    //public void ButtonClickable()
    //{
    //    transform.GetComponent<Button>().interactable = true;
    //    transform.GetComponent<Image>().color = Color.white;
    //}

    #endregion

}


