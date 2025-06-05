using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//新手引导
public class GuidancePlane : MonoBehaviour
{
    public static GuidancePlane Instance;
    public Image mask_IMG;
    public GameObject finger_OBJ;
    public GameObject dialogue_OBJ;
    public TMP_Text guidance_TMP;
    public GameObject Special_TMP;
    public Sprite mysterySprite;
    private GuidanceConfig guidanceData;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        mask_IMG.GetComponent<Button>().onClick.AddListener(ClickEvent);
    }


    //新手引导初始化
    public void GuidanceInit(int ID,Vector3 guidancePos,int size = 1)
    {
        guidanceData = GameManager.Instance.guidanceData[ID];
        mask_IMG.sprite = guidanceData.maskSprite;
        mask_IMG.transform.localScale = new Vector3(size, size, size);
        mask_IMG.SetNativeSize();
        finger_OBJ.transform.DOMove(guidancePos,0.3f); 
        mask_IMG.transform.DOMove(guidancePos, 0.3f).OnComplete(() => 
        {
            mask_IMG.GetComponent<Button>().interactable = true;
        });
        
        if (guidanceData._isDialogue)
        {
            dialogue_OBJ.SetActive(true);
            DialogueInit(ID);
        }
        else
            dialogue_OBJ.SetActive(false);

    }

    //新手引导对话框初始化 
    public void DialogueInit(int ID)
    {
        if (ID == 1)
        {
            Special_TMP.SetActive(true);
        }
        else
        {
            Special_TMP.SetActive(false);
        }

        guidance_TMP.text = ListExtensions.LoadSprite(guidanceData.dialogueStr);
    }

    //点击事件
    public void ClickEvent()
    {
        switch (guidanceData.ID)
        {
            case 0:
                UIManagement.Instance.mainPlane.StartPlayClick();
                Debug.LogError("当前ID;" + guidanceData.ID);
                break;
            case 2:
                UIManagement.Instance.gameOverPlane.NextLevelClick();
                Debug.LogError("解锁下一关");
                break;
            case 3:
                UIManagement.Instance.gameOverPlane.BackMain();
                Debug.LogError("返回主界面");
                break;
            case 4:
                UIManagement.Instance.OpenFurnitureUpgradePlane();
                Debug.LogError("点击装扮");
                break;
            case 5:
                FurnitureManagement.useFurnitureItemGrid.UseClick();
                Debug.LogError("家具使用");
                break;
        }

        PlayerPrefs.SetString(guidanceData.saveKey, "Save" + guidanceData.saveKey);
        finger_OBJ.transform.DOMove(new Vector3(540,2880), 0.1f);
        mask_IMG.transform.DOMove(new Vector3(540, 2880), 0.1f);
        UIManagement.Instance.CloseGuidancePlane();
    }



    //判断是否打开新手引导
    public bool JudgeWhetherOpenGuide(int IDKey)
    {
        if (!PlayerPrefs.HasKey(GameManager.Instance.guidanceData[IDKey].saveKey))
            return true;

        return false;
    }
}
