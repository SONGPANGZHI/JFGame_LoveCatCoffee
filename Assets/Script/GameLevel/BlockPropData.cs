using DG.Tweening;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BlockPropData : MonoBehaviour
{
    public BlockDataConfig dataConfig;

    public BlockPropType propType;
    public Image plate_Icon;
    public Image fruits_Icon;
    public GameObject mysteryBox;
    public Button click_BTN;

    public Transform blockParent;

    public bool midlleBlock;
    public bool _isMayster;
    private void Awake()
    {
        click_BTN.onClick.AddListener(BlockClick);
    }


    public void MiddleBlockInit(BlockDataConfig blockDataConfig,bool isMiddleBlock = false)
    {
        int index = Random.Range(0, 10);
        
        blockParent = this.transform.parent;
        midlleBlock = isMiddleBlock;
        dataConfig = blockDataConfig;
        propType = dataConfig.blockPropType;

        plate_Icon.sprite = dataConfig.plate_IMG;
        fruits_Icon.sprite = dataConfig.fruits_IMG;

        if (index <= PlayGameManagement.Instance.blockArea && !PlayGameManagement.Instance.perspective)
        {
            _isMayster = true;
            mysteryBox.SetActive(true);
            PlayGameManagement.Instance.currentMysteryBox.Add(this);
        }
        else
        {
            _isMayster = false;
            mysteryBox.SetActive(false);
        }
    }

    //初始化 传送带上方格
    public void ConveyorBlockInit(BlockDataConfig blockDataConfig)
    {
        int index = Random.Range(1, 10);
        blockParent = this.transform.parent;
        dataConfig = blockDataConfig;
        propType = dataConfig.blockPropType;

        plate_Icon.sprite = dataConfig.plate_IMG;
        fruits_Icon.sprite = dataConfig.fruits_IMG;

        if (index <= PlayGameManagement.Instance.blockArea && !PlayGameManagement.Instance.perspective)
        {
            _isMayster = true;
            mysteryBox.SetActive(true);
            PlayGameManagement.Instance.currentMysteryBox.Add(this);
        }
        else
        {
            _isMayster = false;
            mysteryBox.SetActive(false);
        }
    }


    //按钮不可以点击
    public void ButtonNotClickable()
    {
        click_BTN.interactable = false;
    }

    //关闭 盲盒
    public void CloseMysteryBox()
    {
        mysteryBox.SetActive(false);
    }

    //打开 盲盒
    public void OpenMaysteryBox()
    {
        mysteryBox.SetActive(true);
    }

    //按钮点击
    public void BlockClick()
    {
        MusicManagement.instance.ClickPlaySFX();

        if(_isMayster)
            mysteryBox.SetActive(false);

        click_BTN.interactable = false;
        fruits_Icon.transform.DOScale(1.2F,0.2F);
        plate_Icon.transform.DOScale(0, 0.2f);
        fruits_Icon.transform.DOMove(PlayGameManagement.Instance.blockAnimPos[PlayGameManagement.Instance.dropZoneData.Count].position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            fruits_Icon.transform.localScale = Vector3.one; // 动画完成后恢复原始大小
            PlayGameManagement.Instance.CreateMoveAnim(fruits_Icon, dataConfig, midlleBlock);
            PlayGameManagement.Instance.CreateDropZoneObject(dataConfig, midlleBlock);
        });

        UpdateButtonInteractability();

        if (midlleBlock)
        {
            PlayGameManagement.Instance.middleAllNum -= 1;
            if (_isMayster)
                PlayGameManagement.Instance.currentMysteryBox.Remove(this);
        }
        else
        { 
            if(_isMayster)
                PlayGameManagement.Instance.currentMysteryBox.Remove(this);
        }

    }

    

    //刷新按钮状态
    public void UpdateButtonInteractability()
    {
        if (blockParent.childCount - 2 < 0) return;

        blockParent.GetChild(blockParent.childCount-2).GetComponent<BlockPropData>().click_BTN.interactable = true;
    }



}


