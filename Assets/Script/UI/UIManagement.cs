using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public static UIManagement Instance;

    public MainPlane mainPlane;

    public GamePlane gamePlane;
    [SerializeField]
    private SettingPlane settingPlane;
    [SerializeField]
    private CommonPlane commonPlane;

    public GameObject guidance;

    public GameOverPlane gameOverPlane;

    public FurnitureUpgrade furnitureUpgradePlane;
    public LoadingPlane loadingPlane;

    public string sceneName;
    public static string redPointKey = "RedPointKEY";

    public bool _isChallengBool;
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

    //关闭主界面
    public void CloseMainPlane()
    {
        mainPlane.gameObject.SetActive(false);
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

   
    //打开加载场景
    public void OpenLoadingPlane()
    {
        mainPlane.ClosePlane();
        mainPlane.gameObject.SetActive(false);
        loadingPlane.gameObject.SetActive(true);
        loadingPlane.LoadingPlaneInit();
    }

    //打开家具升级界面
    public void OpenFurnitureUpgradePlane()
    {
        mainPlane.ClosePlane();
        furnitureUpgradePlane.gameObject.SetActive(true);
        furnitureUpgradePlane.OpenPlaneInit();
        //furnitureUpgradePlane.FurnitureInit(); 
    }

    public void OpenFurnitureConfirmPlane()
    {
        furnitureUpgradePlane.ClosePlaneOpenConfirmPlane();
        furnitureUpgradePlane.OpenConfirmationPlane();
    }

    //打开家具换皮界面
    //public void OpenFurnitureSkinPlane()
    //{
    //    //furnitureUpgradePlane.FurnitureSkinInit();
    //}


    public void CloseFurnitureUpgradePlane()
    {
        furnitureUpgradePlane.ClosePlaneOpenConfirmPlane();
    }

    //打开新手引导
    public void OpenGuidancePlane()
    {
        guidance.SetActive(true);
    }

    //关闭新手引导
    public void CloseGuidancePlane()
    {
        guidance.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("测试已经使用复活道具---");
            PlayerPrefs.SetInt(GameManager.propUserKey,1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            int saveID = PlayerPrefs.GetInt(GameManager.CurrentGameLevelKey);
            PlayerPrefs.SetInt(GameManager.CurrentGameLevelKey, saveID + 1);
            Debug.LogError("CurrentGameLevelKey" + saveID);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            Debug.LogError("清除所有PlayerPrefs");
        }
    }
}
