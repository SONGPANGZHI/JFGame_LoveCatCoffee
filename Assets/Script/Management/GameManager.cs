using System;
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

    public const string musicSetKey = "MusicSettingKey";
    public const string soundSetKey = "SoundSettingKey";
    public const string shakeSetKey = "ShakeSettingKey";
    public const string propUserKey = "PropUserKey";

    public string LogOutTimeKey;
    public string LogInTimeKey;
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

        LogInTime();
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
        return finalTMP;
    }

    #region 时间获取

    //登录
    public void LogInTime()
    {
        GetCurrentTime();
        if (LogInTimeKey != PlayerPrefs.GetString("LogOutTimeKey"))
        {
            NumberLevelChallenges = 0;
            Debug.LogError("新的一天--");
        }
    }

    //获取时间
    public void GetCurrentTime()
    {
        DateTime now = DateTime.Now;

        LogInTimeKey = now.ToString("yyyy年MM月dd日");
        PlayerPrefs.SetString("LogInTimeKey", LogInTimeKey);

    }

    //退出
    public void OnApplicationQuit()
    {
        DateTime now = DateTime.Now;
        //UnityEditor.EditorApplication.isPlaying = false;
        string LogOutTime = now.ToString("yyyy年MM月dd日");
        PlayerPrefs.SetString("LogOutTimeKey", LogOutTime);
    }
    #endregion

    #region 数据保存

    public void LoadSaveDate()
    {
        NumberLevelChallenges = PlayerPrefs.GetInt(NumberLevelChallengesKey);
    }


    //保存关卡等级
    public void SavaGameLevel()
    {
        int saveID = PlayerPrefs.GetInt(CurrentGameLevelKey);
        PlayerPrefs.SetInt(CurrentGameLevelKey, saveID + 1);
    }

    //保存挑战次数
    public void SavaChallengTime()
    {
        int saveID = PlayerPrefs.GetInt(NumberLevelChallengesKey);
        PlayerPrefs.SetInt(NumberLevelChallengesKey, saveID + 1);
        NumberLevelChallenges = PlayerPrefs.GetInt(NumberLevelChallengesKey);
    }

    #endregion


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


