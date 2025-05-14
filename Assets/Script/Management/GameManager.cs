using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("关卡数据")]
    public Dictionary<int, GameLevelInfo> gameLevelDic = new Dictionary<int, GameLevelInfo>();
    public List<GameLevelInfo> gameLevelInfos = new List<GameLevelInfo>();

    [Header("家具数据")]
    public Dictionary<int, FurnitureReward> furnitureRewardDic = new Dictionary<int, FurnitureReward>();        //奖励池
    public List<FurnitureReward> furnitureRewards = new List<FurnitureReward>();            //保存数据 
    public List<string> furnitureName = new List<string>();                 //解锁家具

    public Dictionary<string, FurnitureInfos> FurniturePosDic = new Dictionary<string, FurnitureInfos>();       //原皮 记录位置
    public List<FurnitureInfos> unlockFurniture = new List<FurnitureInfos>();

    public FurnitureInfo currentClickFurniture;

    [Header("游戏暂停")]
    public bool pauseGame = true;

    [Header("当前猫咪猫咪数量")]
    public float currentNumberCats;

    [Header("关卡挑战次数")]
    public int NumberLevelChallenges;

    public GameLevelInfo currentGameLevel;

    public GameSaveData CurrentData;
    private static string SavePath => Path.Combine(Application.persistentDataPath, "GameSaveData.json");
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

        InitializeData();
        LogInTime();
        //检查关卡
        CheckSaveData();

    }

    private void Start()
    {
        //获得当前 关卡
        GetGameLevelData();

        //LoadSaveGameLeveData();

        //GetUnLockDefaultFurniture();
    }

    //检查保存数据
    public void CheckSaveData()
    {
        if (!PlayerPrefs.HasKey(CurrentGameLevelKey))
        {
            PlayerPrefs.SetInt(CurrentGameLevelKey, 1);
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

    //获取当前关卡
    public GameLevelInfo GetGameLevelData()
    {
        currentGameLevel = gameLevelDic[PlayerPrefs.GetInt(CurrentGameLevelKey)];
        Debug.LogError("当前关卡" + currentGameLevel.LevelID);
        return currentGameLevel;
    }

    //加载保存家具数据
    public void LoadSaveGameLeveData()
    {
        for (int i = 0; i < gameLevelDic.Count; i++)
        {
            if (i < currentGameLevel.LevelID && gameLevelDic[i + 1].FurnitureName != null)
            {
                for (int j = 0; j < gameLevelDic[i+1].FurnitureName.Count; j++)
                {
                    furnitureName.Add(gameLevelDic[i + 1].FurnitureName[j]);
                }
               
            }

        }
    }

    //在字典里找解锁家具
    public void GetUnLockDefaultFurniture()
    {
        for (int i = 0; i < furnitureName.Count; i++)
        {
            if (FurniturePosDic.ContainsKey(furnitureName[i]))
            {
                unlockFurniture.Add(FurniturePosDic[furnitureName[i]]);
            }
        }
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
    private void InitializeData()
    {
        if (File.Exists(SavePath))
        {
            LoadGameSaveData();
        }
        else
        {
            Debug.Log("无存档文件，创建新数据并保存初始文件");
            CurrentData = new GameSaveData();

            // 立即保存创建初始文件
            SaveData();
        }

    }

    public void LoadGameSaveData()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning($"存档文件不存在: {SavePath}");
                return;
            }

            string json = File.ReadAllText(SavePath);
            CurrentData = JsonUtility.FromJson<GameSaveData>(json);

            // 确保反序列化后的对象不为null
            if (CurrentData == null)
            {
                Debug.LogWarning("反序列化失败，创建新数据");
                CurrentData = new GameSaveData();
            }

            // 确保所有列表已初始化
            CurrentData.usedFurniture = CurrentData.usedFurniture ?? new List<FurnitureReward>();
            CurrentData.collectionFurnitureName = CurrentData.collectionFurnitureName ?? new List<string>();

            Debug.Log("游戏数据加载成功");
        }
        catch (Exception e)
        {
            Debug.LogError($"加载数据失败: {e.Message}");
            CurrentData = new GameSaveData();
        }
    
    }


    // 保存列表到JSON文件
    public void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(CurrentData, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"游戏数据保存成功: {SavePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"保存数据失败: {e.Message}");
        }
    }
}

public static class ListExtensions
{
    private static System.Random rng = new System.Random();
    private const string DEFAULT_AWARD_PATH = "Images/Hall_Brown/";
    private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();


    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Dictionary<int, List<T>> SplitIntoGroups<T>(this List<T> source, int groupCount)
    {
        Dictionary<int, List<T>> result = new Dictionary<int, List<T>>();

        int itemsPerGroup = Mathf.CeilToInt(source.Count / (float)groupCount);

        for (int i = 0; i < groupCount; i++)
        {
            int startIndex = i * itemsPerGroup;
            if (startIndex >= source.Count) break;

            int endIndex = Mathf.Min(startIndex + itemsPerGroup, source.Count);
            result.Add(i, source.GetRange(startIndex, endIndex - startIndex));
        }

        return result;
    }

    public static Sprite LoadFurnitureSprite(string spriteKey, string imagePath = DEFAULT_AWARD_PATH)
    {
        string cacheKey = imagePath + spriteKey;

        if (spriteCache.TryGetValue(cacheKey, out Sprite cachedSprite))
            return cachedSprite;

        Sprite newSprite = Resources.Load<Sprite>(Path.Combine(imagePath, spriteKey));

        if (newSprite != null)
            spriteCache[cacheKey] = newSprite;
        else
            Debug.LogWarning($"Sprite not found: {cacheKey}");

        return newSprite;
    }
}




