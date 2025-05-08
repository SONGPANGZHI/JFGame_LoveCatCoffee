using System;
using System.Reflection;
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


    public void MiddleBlockInit(BlockDataConfigNew blockDataConfig,bool isMiddleBlock = false)
    {
        int index = UnityEngine.Random.Range(0, 10);
        
        blockParent = this.transform.parent;
        midlleBlock = isMiddleBlock;
        dataConfig = blockDataConfig;
        propType = dataConfig.blockPropType;
        icon.sprite = dataConfig.Icon;
        if (index <= PlayGameManagement.Instance.blockArea && !PlayGameManagement.Instance.perspective)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            PlayGameManagement.Instance.currentMysteryBox.Add(this);
        }
        else
            transform.GetChild(0).gameObject.SetActive(false);
    }

    //初始化 传送带上方格
    public void ConveyorBlockInit(BlockDataConfigNew blockDataConfig)
    {
        int index = UnityEngine.Random.Range(1, 10);
        blockParent = this.transform.parent;
        dataConfig = blockDataConfig;
        propType = dataConfig.blockPropType;
        icon.sprite = dataConfig.Icon;
        if (index <= PlayGameManagement.Instance.conveyorArea && !PlayGameManagement.Instance.perspective)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            PlayGameManagement.Instance.currentMysteryBox.Add(this);
        }
        else
            transform.GetChild(0).gameObject.SetActive(false);
    }


    //按钮不可以点击
    public void ButtonNotClickable()
    {
        transform.GetComponent<Button>().interactable = false;
    }

    //关闭 盲盒
    public void CloseMysteryBox()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    //打开 盲盒
    public void OpenMaysteryBox()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    //按钮点击
    public void BlockClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        PlayGameManagement.Instance.CreateDropZoneObject(dataConfig, midlleBlock);
        Destroy(gameObject);
        UpdateButtonInteractability();
        if (midlleBlock)
            PlayGameManagement.Instance.middleAllNum -= 1;

    }

    //刷新按钮状态
    public void UpdateButtonInteractability()
    {
        if (blockParent.childCount - 2 < 0) return;

        blockParent.GetChild(blockParent.childCount-2).GetComponent<Button>().interactable = true;
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


