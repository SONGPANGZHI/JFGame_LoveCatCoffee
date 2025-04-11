using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public static UIManagement Instance;

    [SerializeField]
    private MainPlane mainPlane;

    public GamePlane gamePlane;
    [SerializeField]
    private SettingPlane settingPlane;
    [SerializeField]
    private CommonPlane commonPlane;
    [SerializeField]
    private GameOverPlane gameOverPlane;
    [SerializeField]
    private AwardPlane awardPlane;

    [SerializeField]
    private GameObject mainGame;

    public LoadingPlane loadingPlane;

    private GameObject mainGameOBJ;
    public string sceneName;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OpenMainPlane();
    }

    //打开主界面
    public void OpenMainPlane()
    {
        mainPlane.gameObject.SetActive(true);
        mainPlane.InitPlane();
        GameManager.Instance.pauseGame = true;
        //CloseGame();
    }

    //打开游戏界面
    public void OpenGamePlane()
    {
        mainPlane.ClosePlane();
        mainPlane.gameObject.SetActive(false);
        gamePlane.gameObject.SetActive(true);
        //OpenGame();
        gamePlane.GamePlaneInit();
    }

    public void CloseGamePlane()
    {
        gamePlane.CloseGamePlane();
        gamePlane.gameObject.SetActive(false);
    }

    //打开设置界面 
    public void OpenSettingPlane(bool _isGame = false)
    {
        GameManager.Instance.pauseGame = false;
        settingPlane.gameObject.SetActive(true);
        settingPlane.InitSetPlane(_isGame);
    }

    //打开游戏结束界面
    public void OpenGameOverPlane(bool isGameOver = false)
    {
        GameManager.Instance.pauseGame = false;
        gameOverPlane.gameObject.SetActive(true);
        gameOverPlane.GameOverPlaneInit(isGameOver);
    }

    //打开通用界面
    public void OpenCommonPlane(CommonPlaneType _planeType,PropData propData = null) 
    {
        commonPlane.gameObject.SetActive(true);
        switch (_planeType)
        {
            case CommonPlaneType.Resurgence:
                commonPlane.ResurgenceInitPlane();
                break;
            case CommonPlaneType.Affirm:
                commonPlane.AffirmInitPlane();
                break;
            case CommonPlaneType.Prop:
                commonPlane.PropInitPlane(propData);
                break;
        }
    }

    //打开奖励界面
    public void OpenAwardPlane()
    {
        awardPlane.gameObject.SetActive(true);
        awardPlane.AwardPlaneInit();
    }

    //打开加载场景
    public void OpenLoadingPlane()
    {
        mainPlane.ClosePlane();
        mainPlane.gameObject.SetActive(false);
        loadingPlane.gameObject.SetActive(true);
        loadingPlane.LoadingPlaneInit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("测试已经使用复活道具---");
            PlayerPrefs.SetInt(SettingPlane.propUserKey,1);
        }
    }
}
