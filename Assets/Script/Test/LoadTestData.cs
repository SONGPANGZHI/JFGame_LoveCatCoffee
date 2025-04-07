using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTestData : MonoBehaviour
{
    public Dictionary<int,TestInfo> keyValuePairs= new Dictionary<int,TestInfo>();

    public Transform spriteTrans;

    private void Awake()
    {
        StartLoadTestConfigAsset();
    }

    private void Start()
    {
       
        InitFurnitureItem();
    }



    //public void CreateSprite()
    //{
    //    for (int i = 0; i < keyValuePairs.Count; i++)
    //    {
    //        Texture2D ga = Instantiate(Resources.Load<Texture2D>("Xiaowu/"+ keyValuePairs[i].name), spriteTrans) ;
    //        //ga.transform.position = new Vector3(keyValuePairs[i].width, keyValuePairs[i].height);
    //        GameObject imageObject = new GameObject(keyValuePairs[i].name);
    //        imageObject.transform.SetParent(spriteTrans);
    //        SpriteRenderer renderer = imageObject.AddComponent<SpriteRenderer>();
    //        Sprite sprite = Sprite.Create(ga, new Rect(0, 0, keyValuePairs[i].width, keyValuePairs[i].height), new Vector2(0.5f, 0.5f));
    //        renderer.sprite = sprite;


    //        Vector3 configVect = new Vector3(keyValuePairs[i].x, keyValuePairs[i].y,0);

    //        Camera camera = Camera.main; // 获取主相机

    //        Vector3 screenPosition = camera.ScreenToWorldPoint(configVect);

    //        imageObject.transform.localPosition = screenPosition;
    //        imageObject.transform.localEulerAngles = Vector3.zero;
    //        imageObject.transform.localScale = new Vector3(1,1,1);
    //    }
    //}

    //public void LoadTest()
    //{

    //    string testInfo = Resources.Load<TextAsset>("Json/Test").text;
    //    TestList testList = JsonUtility.FromJson<TestList>(testInfo);
    //    for (int i = 0; i < testList.Test.Count; i++)
    //    {
    //        keyValuePairs.Add(i, testList.Test[i]);
    //    }

    //    Debug.LogError("keyValuePairs---" + keyValuePairs.Count);
    //    Debug.LogError(testInfo);

    //}
    public string imagePath = "";
    public Dictionary<string, FurnitureItemData> FurnitureList = new Dictionary<string, FurnitureItemData>();
    public GameObject pre;
    public void InitFurnitureItem()
    {
        //for (int i = 0; i < FurnitureList.Count; i++)
        //{
        //    Texture2D ga = Instantiate(Resources.Load<Texture2D>("Hall1/" + FurnitureList[i].skinName), spriteTrans);
        //}

        //Sprite itemSprite = Resources.Load(imagePath + currentItemName, typeof(Sprite)) as Sprite;




        foreach (var item in FurnitureList)
        {
            GameObject ga = Instantiate(pre, spriteTrans);
            ga.GetComponent<FurnitureData>().InitFurniture(item.Value);


            //Texture2D ga = Instantiate(Resources.Load<Texture2D>("Hall1/" + item.Key), spriteTrans);
            ////ga.transform.position = new Vector3(keyValuePairs[i].width, keyValuePairs[i].height);
            //GameObject imageObject = new GameObject(item.Key);
            //imageObject.transform.SetParent(spriteTrans);
            //SpriteRenderer renderer = imageObject.AddComponent<SpriteRenderer>();
            //Sprite sprite = Sprite.Create(ga, new Rect(0, 0, FurnitureList[item.Key].width, FurnitureList[item.Key].height), new Vector2(0.5f, 0.5f));
            //renderer.sprite = sprite;
            //imageObject.transform.localPosition = FurnitureList[item.Key].localPostion;
        }

    }

    public void StartLoadTestConfigAsset()
    {
        Debug.Log("-------------------当前为本地测试状态-------------------");
        string jsonContent;
        //string localUrl = "Config/rabbithomeData";
        string localUrl = "Config/Hall1";
        TextAsset galleryLocalJson = Resources.Load<TextAsset>(localUrl);
        jsonContent = galleryLocalJson.text;
        ParsingContent(jsonContent);
    }


    void ParsingContent(string _data)
    {
        var rootData = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data.ToString());

        //1.获取image地址3
        var skeleton = JsonConvert.DeserializeObject<Dictionary<string, object>>(rootData["skeleton"].ToString());
        if (skeleton.ContainsKey("images"))
        {
            object tempStr;
            skeleton.TryGetValue("images", out tempStr);
            imagePath = tempStr.ToString();
        }

        //3.获取image的skins信息
        var skins = JsonConvert.DeserializeObject<Dictionary<string, object>>(rootData["skins"].ToString());
        var defaultSkin = JsonConvert.DeserializeObject<Dictionary<string, object>>(skins["default"].ToString());
        //  int layoutNum = defaultSkin.Count + 8;
        int layoutNum = 8;
        foreach (var skinItem in defaultSkin)
        {
            layoutNum++;
            string nameTemp = skinItem.Key;
            Dictionary<string, object> valuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(skinItem.Value.ToString());
            foreach (var skinDataItem in valuePairs)
            {
                var skinData = JsonConvert.DeserializeObject<Dictionary<string, object>>(skinDataItem.Value.ToString());
                var skinsItem = new FurnitureItemData(skinData, nameTemp, layoutNum);
                FurnitureList.Add(nameTemp, skinsItem);
            }
        }

        Debug.LogError(defaultSkin);
        //homeLoadFinished = true;
        // ToEquence();
    }
}

public class TestList
{
    public List<TestInfo> Test;
}

[Serializable]
public class TestInfo
{ 
    public string name;
    public float x;
    public float y;
    public int width;
    public int height;
}

[Serializable]
public class FurnitureItemData
{
    public string skinName;
    public string Parent;
    public float pos_x;
    public float pos_y;
    public float width;
    public float height;
    public Vector3 localPostion;
    public bool CanClick;
    public bool IsDefault;
    public int orderLayer;

    public FurnitureItemData(Dictionary<string, object> data, string name, int layoutNum)
    {
        skinName = name;
        Parent = data.ContainsKey("Parent") ? data["Parent"].ToString() : "";
        pos_x = data.ContainsKey("x") ? float.Parse(data["x"].ToString()) : 0;
        pos_y = data.ContainsKey("y") ? float.Parse(data["y"].ToString()) : 0;
        width = data.ContainsKey("width") ? float.Parse(data["width"].ToString()) : 0;
        height = data.ContainsKey("height") ? float.Parse(data["height"].ToString()) : 0;
        orderLayer = layoutNum;
        CanClick = (bool)(data.ContainsKey("CanClick") ? data["CanClick"] : false);
        IsDefault = (bool)(data.ContainsKey("CanClick") ? true : false);
        Vector3 configVect = new Vector3(pos_x, pos_y, 0);
        Vector3 resultVect = Camera.main.ScreenToWorldPoint(configVect);
        localPostion = new Vector3(resultVect.x, resultVect.y, 0);

    }
}
