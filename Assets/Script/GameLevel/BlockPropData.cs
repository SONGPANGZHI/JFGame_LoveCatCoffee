using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlockPropData : MonoBehaviour
{
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

        transform.DOScale(1.2f, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.localScale = Vector3.one; // 动画完成后恢复原始大小
            PlayGameManagement.Instance.CreateMoveAnim(dataConfig, midlleBlock, transform);
            //PlayGameManagement.Instance.CreateDropZoneObject(dataConfig, midlleBlock);
            //Destroy(gameObject);
            //CreateMoveAnim(dataConfig);
        });

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



}


