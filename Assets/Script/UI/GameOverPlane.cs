using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPlane : MonoBehaviour
{
    [SerializeField]
    private GameObject defeated_UI;
    [SerializeField]
    private GameObject victory_UI;
    [SerializeField]
    private GameObject menu_BTN;

    [SerializeField]
    public GameObject awardGrid;
    [SerializeField]
    public Transform awardTran;


    [SerializeField]    
    private Button resChallenge_BTN;                //重玩按钮
    [SerializeField]
    private Button back_BTN;                        //返回按钮
    [SerializeField]
    private Button nextLevel_BTN;                   //返回按钮

    private float progress;

    private bool planeState;


    private void Awake()
    {
        resChallenge_BTN.onClick.AddListener(RecChanllengeClick);
        back_BTN.onClick.AddListener(BackMain);
        nextLevel_BTN.onClick.AddListener(NextLevelClick);
    }

    //界面初始化  _isVictory = true 打开胜利界面  false 失败界面
    public void GameOverPlaneInit(bool _isVictory = false)
    {
        planeState = _isVictory;
        gameObject.SetActive(true);

        JudgingPlaneState();

    }

    //判断界面状态
    public void JudgingPlaneState()
    {
        if (planeState)
        {
            //胜利
            //按钮状态
            nextLevel_BTN.gameObject.SetActive(true);
            resChallenge_BTN.gameObject.SetActive(false);

            //界面显示动画
            defeated_UI.SetActive(false);
            victory_UI.SetActive(true);
            victory_UI.transform.DOScale(new Vector3(1,1,1),0.3f);
            menu_BTN.transform.DOLocalMoveY(-400,0.3F);
            InitAward();
            //奖励
        }
        else
        {
            //失败
            resChallenge_BTN.gameObject.SetActive(true);
            nextLevel_BTN.gameObject.SetActive(false);


            victory_UI.SetActive(false);
            defeated_UI.SetActive(true);
            defeated_UI.transform.DOScale(Vector3.one, 0.3f);
            menu_BTN.transform.DOLocalMoveY(-400, 0.3F);
        }

        PlayerPrefs.DeleteKey("ADResurgenceKey");
    }

    //初始化奖励
    public void InitAward()
    {
        if (GameManager.Instance.currentGameLevel.LevelID == 1)
        {
            GameObject GO = Instantiate(awardGrid, awardTran);
        }
        else
        {
            for (int i = 0; i < PlayGameManagement.Instance.furnitureName.Count; i++)
            {
                string _furnitureName = PlayGameManagement.Instance.furnitureName[i];
                GameObject GO = Instantiate(awardGrid, awardTran);
                GO.GetComponent<AwardGrid>().InitAwardGrid(_furnitureName);

                if (GameManager.Instance.currentGameLevel.LevelID >= 17)
                {
                    GameManager.Instance.CurrentData.AwardFurniturePool.Remove(_furnitureName);
                }

                //把获得的家具名字添加到本地保存
                GameManager.Instance.CurrentData.collectionFurnitureName.Add(_furnitureName);
            }

            GameManager.Instance.SaveData();
        }

        //保存红点
        PlayerPrefs.SetString(UIManagement.redPointKey, "RedPiont");
        GameManager.Instance.SavaGameLevel();
        GameManager.Instance.GetGameLevelData();
        Debug.Log("解锁下一关");
    }


    //重新挑战
    public void RecChanllengeClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        menu_BTN.transform.DOMoveY(-1200, 0.3F);
        defeated_UI.transform.DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            ClearTrans();
            //加载界面
            UIManagement.Instance.CloseGamePlane();
            this.gameObject.SetActive(false);
            UIManagement.Instance.OpenLoadingPlane();
            
        });
    }

    //返回主界面
    public void BackMain()
    {
        MusicManagement.instance.ClickPlaySFX();
        menu_BTN.transform.DOMoveY(-1200, 0.3F);
        if (planeState)
            ClosePlane(victory_UI);
        else
            ClosePlane(defeated_UI);

        UIManagement.Instance.loadingPlane.gameObject.SetActive(true);
        UIManagement.Instance.CloseGamePlane();
        UIManagement.Instance.loadingPlane.LoadUIScene();
    }

    //
    public void NextLevelClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        menu_BTN.transform.DOMoveY(-1200, 0.3F);
        victory_UI.transform.DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            ClearTrans();
            //加载界面
            if (GameManager.Instance.currentGameLevel.LevelID >= 30)
                GameManager.Instance.GetGameLevelData_TEMP(30);
            else
                GameManager.Instance.GetGameLevelData();
            UIManagement.Instance.CloseGamePlane();
            this.gameObject.SetActive(false);
            UIManagement.Instance.OpenLoadingPlane();
            
        });
    }

    public void ClosePlane(GameObject plane)
    {
        plane.transform.DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() =>
        {
            ClearTrans();
            this.gameObject.SetActive(false);
        });
    }

    //清空奖励
    public void ClearTrans()
    {
        for (int i = 0; i < awardTran.childCount; i++)
        {
            Destroy(awardTran.GetChild(i).gameObject);
        }
    }
}
