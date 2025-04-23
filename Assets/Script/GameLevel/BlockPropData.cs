using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPropData : MonoBehaviour
{
    public BlockPropDataClass blockPropData;

    public int ID;
    public BlockPropType propType;
    public Image icon;
    public bool active;
    public BlockHierarchy hierarchy;
    public List<GameObject> unlockblock;
    public Color unlockColor;

    public static Vector2 clickPos;
    public static Action JudgeThirdRowUnlockActon;
    public static Action JudgeScendRowUnlockActon;

    private void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(BlockClick);
    }

    public void BlockInit(BlockPropDataClass Data)
    {
        blockPropData = Data;
        ID = blockPropData.ID;
        active = blockPropData.active;
        propType = blockPropData.config.blockPropType;
        icon.sprite = blockPropData.config.Icon;
        icon.SetNativeSize();
        gameObject.SetActive(active);
        Invoke("ShowUnlock", 0.2F);
    }

    //按钮点击
    public void BlockClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        gameObject.SetActive(false);
        active = false;
        UpdateData();
        GameLevelManagement.Instance.CreateDropZoneObject(this);
        JudgeBlockClick();
    }

    //延迟一秒 显示状态
    public void ShowUnlock()
    {
        if (CheckUnlock())
        {
            transform.GetComponent<Image>().color = Color.white;
            transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            transform.GetComponent<Image>().color = unlockColor;
            transform.GetComponent<Button>().interactable = false;
        }
    }


    public bool CheckUnlock()
    {
        if (unlockblock.Count == 0)
        {
            return true;
        }
        else if (unlockblock.Count == 1)
        {
            if (!unlockblock[0].activeSelf)
                return true;
        }
        else
        {
            for (int i = 0; i < unlockblock.Count; i++)
            {
                if (!unlockblock[0].activeSelf && !unlockblock[1].activeSelf)
                    return true;
            }
        }

        return false;
    }

    //刷新数据
    public void UpdateData()
    {
        GameLevelManagement.Instance.ModifyBlockByIndex(ID,false);
    }

    //判断方块是否解锁
    public void JudgeBlockClick()
    {
        switch (hierarchy)
        {
            case BlockHierarchy.BottomBlock:
                JudgeScendRowUnlockActon?.Invoke();
                break;
            case BlockHierarchy.MiddleBlock:
                JudgeThirdRowUnlockActon?.Invoke();
                break;
        }
    }

    //添加数据
    public void ButtonClickable()
    {
        transform.GetComponent<Button>().interactable = true;
        transform.GetComponent<Image>().color = Color.white;
    }


    //public BlockPropType blockPropType;
    //public BlockHierarchy blockHierarchy;
    //public ConveyorLayer conveyorLayer;
    //public Sprite dorpZoneSprite;
    //public GameObject prefab;
    //public List<GameObject> unlockblock;      //0 左边 1 右边
    //public Color unlockColor;
    //public int ID;

    //public static Action JudgeThirdRowUnlockActon;
    //public static Action JudgeScendRowUnlockActon;

    //public bool _isActive = true;

    //public int hierarchyID;
    //public int listID;
    //private void Awake()
    //{
    //    prefab.GetComponent<Button>().onClick.AddListener(BlockClick);
    //}

    ////初始化
    //public void InitBlock(int _hierarchyID,int _listID,bool active,int _ID)
    //{
    //    hierarchyID = _hierarchyID;
    //    listID = _listID;
    //    if(conveyorLayer == ConveyorLayer.Top)
    //        ID = _hierarchyID * 6 + _listID + _ID;
    //    else
    //        ID = _hierarchyID * 6 + _listID + 54 + _ID;
    //    gameObject.SetActive(active);
    //    if (CheckUnlock())
    //    {
    //        transform.GetComponent<Image>().color = Color.white;
    //        transform.GetComponent<Button>().interactable = true;
    //    }

    //    if (blockHierarchy == BlockHierarchy.BottomBlock)
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

    ////点击方块
    //public void BlockClick()
    //{
    //    gameObject.SetActive(false);
    //    //_isActive = false;
    //    //UpdateData();
    //    GameLevelManagement.Instance.CreateDropZoneObject(this);
    //    JudgeBlockClick();
    //}

    ////判断方块是否解锁
    //public void JudgeBlockClick()
    //{

    //    switch (blockHierarchy)
    //    {
    //        case BlockHierarchy.BottomBlock:
    //            JudgeScendRowUnlockActon?.Invoke();
    //            //BlockGeneration.instance.CheckMiddleData();
    //            break;
    //        case BlockHierarchy.MiddleBlock:
    //            JudgeThirdRowUnlockActon?.Invoke();
    //            //BlockGeneration.instance.CheckTopData();
    //            break;
    //    }
    //}

    ////添加数据
    //public void ButtonClickable()
    //{
    //    transform.GetComponent<Button>().interactable = true;
    //    transform.GetComponent<Image>().color = Color.white;
    //}

    ////按钮不可点击
    //public void BtnInteractableState()
    //{
    //    transform.GetComponent<Button>().interactable = false;
    //}

    ////检查 是否解锁
    //public bool CheckUnlock()
    //{
    //    foreach (var item in unlockblock)
    //    {
    //        if (item.activeSelf)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //public void UpdateData()
    //{
    //    switch (conveyorLayer)
    //    {
    //        case ConveyorLayer.Top:
    //            GetLayerDicData();
    //            break;
    //        case ConveyorLayer.Bottom:
    //            break;
    //    }
    //}

    //public void GetLayerDicData()
    //{
    //    switch (blockHierarchy)
    //    {
    //        case BlockHierarchy.TopBlock:
    //            //GameLevelManagement.Instance.ModifyBlockByIndex(hierarchyID,listID,false);
    //            break;
    //        case BlockHierarchy.MiddleBlock:
    //            break;
    //        case BlockHierarchy.BottomBlock:
    //            GameLevelManagement.Instance.ModifyBlockByIndex(hierarchyID, listID, false);
    //            break;
    //    }
    //}
}


