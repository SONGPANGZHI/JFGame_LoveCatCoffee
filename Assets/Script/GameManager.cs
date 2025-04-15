using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("游戏暂停")]
    public bool pauseGame = true;

    [Header("当前猫咪猫咪数量")]
    public float currentNumberCats;

    [Header("关卡挑战次数")]
    public int NumberLevelChallenges;


    #region  游戏保存KEY

    public static string NumberLevelChallengesKey = "NumberLevelChallengesKEY";         //关卡挑战次数
    public static string CurrentGameLevelKey = "CurrentGameLevelKEY";                   //当前游戏关卡


    #endregion

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

    //检查保存数据
    public void CheckSaveData()
    {
        if (!PlayerPrefs.HasKey(CurrentGameLevelKey))
        {
            PlayerPrefs.SetInt(CurrentGameLevelKey, 0);
        }
    }

    //猫咪数量改变
    public void CatNumChange(int num = 1)
    {
        currentNumberCats += num;
    }

    //获取 文本中数字
    public string GetNumbersText(string _TMP, int numID)
    {
        string finalTMP = _TMP.Replace("<NUM>", numID.ToString());
        Debug.LogError(finalTMP);
        return finalTMP;
    }


    #region JSON 读取

    public void StartLoadConfigAsset()
    {

    }

    public void InitGameLevelJSON()
    { 
    
    }

    #endregion
}

//public class GameLevelFileData
//{
//    public int LevelID;
//    public int Target;
//    public int Amount;
//    public int TypeID;
//    public float ConveyorSpeed;
//    public float ClearTime;
//    public int ClearStep;
//}

//public class ProbabilityCardsAppeare
//{ 

//}


