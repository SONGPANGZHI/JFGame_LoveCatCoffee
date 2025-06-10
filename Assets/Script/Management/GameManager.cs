using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("关卡数据")]
    public Dictionary<int, GameLevelInfo> gameLevelDic = new Dictionary<int, GameLevelInfo>();
    public List<GameLevelInfo> gameLevelInfos = new List<GameLevelInfo>();

    [Header("家具数据")]
    public List<FurnitureItem> currentFurnitureData = new List<FurnitureItem>();
    public static List<FurnitureItem> AllFurnitureData = new List<FurnitureItem>();            //保存数据 
    public Dictionary<string, FurnitureItem> FurniturePosDic = new Dictionary<string, FurnitureItem>();       //原皮 记录位置
    public List<string> awardFurniturePool = new List<string>();



    [Header("游戏暂停")]
    public bool pauseGame = true;

    [Header("当前猫咪猫咪数量")]
    public float currentNumberCats;

    [Header("关卡挑战次数")]
    public int NumberLevelChallenges;


    public GameLevelInfo currentGameLevel;

    public GameSaveData CurrentData;

    public bool _hasLoaded = false;

    public List<GuidanceConfig> guidanceData;

    #region  游戏保存KEY

    public static string NumberLevelChallengesKey = "NumberLevelChallengesKEY";         //关卡挑战次数
    public static string CurrentGameLevelKey = "CurrentGameLevelKEY";                   //当前游戏关卡
    public static string SaveImageKey = "SaveImageKEY";

    public const string musicSetKey = "MusicSettingKey";
    public const string soundSetKey = "SoundSettingKey";
    public const string shakeSetKey = "ShakeSettingKey";
    public const string propUserKey = "PropUserKey";

    private static string SavePath => Path.Combine(Application.persistentDataPath, "GameSaveData.json");

    public string LogOutTimeKey;
    public string LogInTimeKey;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        if (!_hasLoaded)
        {
            //加载loadConfig
            StartLoadConfigAsset();
            LoadFurnitureItemJson();
            LoadAwardFurniturePoolJson();
            _hasLoaded = false;
        }
       

        //初始化本地数据
        InitializeData();
        //LogInTime();
        //检查关卡
        CheckSaveData();

        if (!_hasLoaded)
        {
            _hasLoaded = true;
            //初始化家具
            FirstLoadAllFurniture();
        }
        
    }

    private void Start()
    {
        ////初始化家具
        //FirstLoadAllFurniture();
        //获得当前 关卡
        GetGameLevelData();

        //LoadSaveGameLeveData();

        //GetUnLockDefaultFurniture();
    }

    //初始化家具
    public void AddCurrentFurnitureData()
    {
        for (int i = 0; i < CurrentData.AllFurniture.Count; i++)
        {
            if (CurrentData.AllFurniture[i].IsDefault)
                currentFurnitureData.Add(CurrentData.AllFurniture[i]);
        }
    }

    //家具分类


    public void FirstLoadAllFurniture()
    {
        //把JSON文件 转换到本地数据 第一次登陆时加载
        if (!PlayerPrefs.HasKey("LoadAllFurnitureKey"))
        {
            for (int i = 0; i < AllFurnitureData.Count; i++)
            {
                CurrentData.AllFurniture.Add(AllFurnitureData[i]);
            }

            for (int i = 0; i < awardFurniturePool.Count; i++)
            {
                CurrentData.AwardFurniturePool.Add(awardFurniturePool[i]);
            }

            SaveData();
            PlayerPrefs.SetString("LoadAllFurnitureKey", "LoadAllFurniture");
        }
        
        for (int i = 0; i < AllFurnitureData.Count; i++)
        {
            FurniturePosDic.Add(AllFurnitureData[i].Id, AllFurnitureData[i]);
        }
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
        Debug.Log("当前关卡" + currentGameLevel.LevelID);
        return currentGameLevel;
    }

    public GameLevelInfo GetGameLevelData_TEMP(int ID)
    {
        currentGameLevel = gameLevelDic[ID];
        Debug.Log("当前关卡" + currentGameLevel.LevelID);
        return currentGameLevel;
    }

    #region 时间获取

    ////登录
    //public void LogInTime()
    //{
    //    GetCurrentTime();
    //    if (LogInTimeKey != PlayerPrefs.GetString("LogOutTimeKey"))
    //    {
    //        NumberLevelChallenges = 0;
    //        Debug.LogError("新的一天--");
    //    }
    //}

    ////获取时间
    //public void GetCurrentTime()
    //{
    //    DateTime now = DateTime.Now;

    //    LogInTimeKey = now.ToString("yyyy年MM月dd日");
    //    PlayerPrefs.SetString("LogInTimeKey", LogInTimeKey);

    //}

    //退出
    //public void OnApplicationQuit()
    //{
    //    DateTime now = DateTime.Now;
    //    //UnityEditor.EditorApplication.isPlaying = false;
    //    string LogOutTime = now.ToString("yyyy年MM月dd日");
    //    PlayerPrefs.SetString("LogOutTimeKey", LogOutTime);
    //}
    #endregion

    #region 数据保存

    public void LoadSaveDate()
    {
        NumberLevelChallenges = PlayerPrefs.GetInt(NumberLevelChallengesKey);
    }


    //保存关卡等级
    public void SavaGameLevel()
    {
        if (!UIManagement.Instance._isChallengBool)
        {
            int saveID = PlayerPrefs.GetInt(CurrentGameLevelKey);
            PlayerPrefs.SetInt(CurrentGameLevelKey, saveID + 1);
        }
    }

    //保存挑战次数
    public void SavaChallengTime()
    {
        //int saveID = PlayerPrefs.GetInt(NumberLevelChallengesKey);
        //PlayerPrefs.SetInt(NumberLevelChallengesKey, saveID + 1);
        //NumberLevelChallenges = PlayerPrefs.GetInt(NumberLevelChallengesKey);
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
            CurrentData.AllFurniture = CurrentData.AllFurniture ?? new List<FurnitureItem>();
            CurrentData.collectionFurnitureName = CurrentData.collectionFurnitureName ?? new List<string>();
            CurrentData.AwardFurniturePool = CurrentData.AwardFurniturePool ?? new List<string>();

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



    #region 关卡数据加载

    public void StartLoadConfigAsset()
    {
        string localUrl = "Json/GameLevelData";
        ParsingContent(Resources.Load<TextAsset>(localUrl).text);
    }

    void ParsingContent(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<List<object>>(_data);

        for (int i = 0; i < rootData.Count; i++)
        {
            GameLevelInfo gameLevelInfo = JsonConvert.DeserializeObject<GameLevelInfo>(rootData[i].ToString());
            gameLevelDic.Add(gameLevelInfo.LevelID, gameLevelInfo);
        }
        Debug.Log("关卡数据加载成功---");

    }

    #endregion

    #region 家具奖励池

    //读取奖励池
    public void LoadFurnitureItemJson()
    {
        string localUrl = "Json/FurnitureData";
        StartLoadFurnitureItam(Resources.Load<TextAsset>(localUrl).text);
    }


    public void StartLoadFurnitureItam(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<List<object>>(_data);

        for (int i = 0; i < rootData.Count; i++)
        {
            FurnitureItem furnitureInfo = JsonConvert.DeserializeObject<FurnitureItem>(rootData[i].ToString());
            AllFurnitureData.Add(furnitureInfo);
        }

        Debug.Log("家具奖励池加载成功---");
    }

    #endregion

    #region 读取位置奖励池
    //读取奖励池
    public void LoadAwardFurniturePoolJson()
    {
        string localUrl = "Json/FurnitureReward";
        StartLoadAwardFurniturePool(Resources.Load<TextAsset>(localUrl).text);
    }

    public void StartLoadAwardFurniturePool(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<List<object>>(_data);

        for (int i = 0; i < rootData.Count; i++)
        {
            FurnitureReward furnitureInfo = JsonConvert.DeserializeObject<FurnitureReward>(rootData[i].ToString());
            awardFurniturePool.Add(furnitureInfo.name);
        }

        Debug.Log("奖励池加载成功---");
    }



    #endregion


}






#region 工具

public static class ListExtensions
{
    private static System.Random rng = new System.Random();
    private const string DEFAULT_AWARD_PATH = "Images/Hall_Brown/";
    private const string ICON_PATH = "Images/DressUpIcon/";
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

    //加载家具图片
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

    //加载家具图片Icon
    public static Sprite LoadFurnitureIconSprite(string spriteKey, string imagePath = ICON_PATH)
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

    public static string LoadSprite(string strKey)
    {
        string changeText = strKey.Replace("<M>", "<sprite name=\"Game_Manghe_String\"verticalOffset=1em horizontalOffset=1emsize=100>");
        return changeText.Replace("<N>", "\n");
    }
}


#endregion




