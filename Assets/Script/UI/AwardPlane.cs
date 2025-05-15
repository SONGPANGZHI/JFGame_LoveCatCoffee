using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class AwardPlane : MonoBehaviour
{
    [SerializeField]
    public GameObject awardGrid;
    [SerializeField] 
    public Transform awardTran;
    [SerializeField]
    private Button ok_BTN;

    public GameObject unlock_TMP;

    private void Awake()
    {
        ok_BTN.onClick.AddListener(OKClick);
    }

    //界面初始化
    public void AwardPlaneInit()
    {
        if (GameManager.Instance.currentGameLevel.LevelID == 1)
        {
            unlock_TMP.SetActive(true);
        }
        else
        {
            unlock_TMP.SetActive(false);

            for (int i = 0; i < PlayGameManagement.Instance.furnitureName.Count; i++)
            {
                string _furnitureName = PlayGameManagement.Instance.furnitureName[i];
                GameObject GO = Instantiate(awardGrid, awardTran);
                GO.GetComponent<AwardGrid>().InitAwardGrid(_furnitureName);

                if (GameManager.Instance.currentGameLevel.LevelID >= 17)
                {
                    GameManager.Instance.CurrentData.levelAwardFureiture.Remove(GameManager.Instance.GetFurnitureReward(_furnitureName));

                    if (GameManager.Instance.GetDefaultSkin(_furnitureName))
                    {
                        GameManager.Instance.CurrentData.newSkinFurniture.Add(_furnitureName);
                    }
                    else
                    {
                        GameManager.Instance.CurrentData.collectionFurnitureName.Add(_furnitureName);
                    }
                }
                else
                {
                    //把获得的家具名字添加到本地保存
                    GameManager.Instance.CurrentData.collectionFurnitureName.Add(_furnitureName);
                }

            }
            GameManager.Instance.SaveData();
        }

        //保存红点
        PlayerPrefs.SetString(UIManagement.redPointKey,"RedPiont");
        transform.GetChild(0).DOScale(new Vector3(1,1,1),0.7f);
        GameManager.Instance.SavaGameLevel();
        GameManager.Instance.GetGameLevelData();
        Debug.LogError("解锁下一关");
    }

    //三个阶段回调
    public void OKClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        transform.GetChild(0).DOScale(new Vector3(0,0,0), 0.7f).OnComplete(() =>
        {
            ClearTrans();
            this.gameObject.SetActive(false);

        });
    }

    public void ClearTrans()
    {
        for (int i = 0; i < awardTran.childCount; i++)
        {
            Destroy(awardTran.GetChild(i).gameObject);
        }
    }

    
}
