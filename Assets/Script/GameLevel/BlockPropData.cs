using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPropData : MonoBehaviour
{
    #region 新版玩法三消

    public BlockDataConfigNew dataConfig;

    public BlockPropTypeNew propType;
    public Image icon;
    public Transform blockParent;

    public bool midlleBlock;
    private void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(BlockClick);
    }


    public void BlockInit(BlockDataConfigNew blockDataConfig,bool isMiddleBlock = false)
    {
        blockParent = this.transform.parent;
        midlleBlock = isMiddleBlock;
        dataConfig = blockDataConfig;
        propType = dataConfig.blockPropType;
        icon.sprite = dataConfig.Icon;
    }

    //按钮不可以点击
    public void ButtonNotClickable()
    {
        transform.GetComponent<Button>().interactable = false;
    }

    //按钮点击
    public void BlockClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        PlayGameManagement.Instance.CreateDropZoneObject(dataConfig);
        Destroy(gameObject);
        UpdateButtonInteractability();
        if (midlleBlock)
            PlayGameManagement.Instance.middleAllNum -= 1;

        Vectory();
    }

    //刷新按钮状态
    public void UpdateButtonInteractability()
    {
        if (blockParent.childCount - 2 < 0) return;

        blockParent.GetChild(blockParent.childCount-2).GetComponent<Button>().interactable = true;
    }

    public void Vectory()
    {
        if (PlayGameManagement.Instance.middleAllNum <= 0)
        {
            //游戏结束
            UIManagement.Instance.OpenGameOverPlane(true);
            GameManager.Instance.SavaGameLevel();
        }
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


