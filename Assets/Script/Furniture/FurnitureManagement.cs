using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FurnitureManagement : MonoBehaviour
{
    public static FurnitureManagement instance;
    public List<string> boxName;
    public GameObject furniturePrefab;
    public Transform furnitureTrans;

    public static string dialogueNoveicKey = "DialogueNoveicKEY";       //对话新手引导
    public FurnitureInfo currentClickFurniture;

    public List<GameObject> sceneFurniture;

    public Camera targetCamera;
    public int width = 1920;
    public int height = 1080;
    public string savePath = "CameraCaptures";
    private void Awake()
    {
        if (instance == null)
            instance = this;

        GameManager.Instance.AddCurrentFurnitureData();

        
    }

    private void Start()
    {
        DefaultFurnitureInit();
    }

    public void DefaultFurnitureInit()
    {
        for (int i = 0; i < GameManager.Instance.currentFurnitureData.Count; i++)
        {
            GameObject GO = Instantiate(furniturePrefab, furnitureTrans);
            GO.GetComponent<FurnitureInfo>().Init(GameManager.Instance.currentFurnitureData[i]);
            GO.name = GameManager.Instance.currentFurnitureData[i].Id;
            sceneFurniture.Add(GO);
        }
    }

    //生成建筑 
    public void CreateFurniture(string spriteKey)
    {
        GameObject GO = Instantiate(furniturePrefab, furnitureTrans);
        GO.GetComponent<FurnitureInfo>().Init(GetFurnitureItem(spriteKey));
        GO.name = spriteKey;
        sceneFurniture.Add(GO);

    }

    //新手关卡（第一关通关）
    public void NoviceLevel()
    {
        for (int i = 0; i < boxName.Count; i++)
        {
            ChangeFurnitureItemDefault(boxName[i],false);
            GetFurnitureNameDestory(boxName[i]);
        }
    }


    //根据家具名返回家具类型 
    public FurnitureItem GetFurnitureItem(string furnitureID)
    {
        if (GameManager.Instance.FurniturePosDic.ContainsKey(furnitureID))
        {
            return GameManager.Instance.FurniturePosDic[furnitureID];
        }

        return null;
    }

    //根据家具名 销毁对应家具
    public void GetFurnitureNameDestory(string furnitureID)
    {

        for (int i = 0; i < sceneFurniture.Count; i++)
        {
            if (sceneFurniture[i] == null)
            { 
                sceneFurniture.Remove(sceneFurniture[i]);
                return;
            }

            if (sceneFurniture[i].name == furnitureID)
            {
                Destroy(sceneFurniture[i]);
            }
        }

    }

    //修改默认家具Bool值
    public void ChangeFurnitureItemDefault(string furnitureID,bool changeBool)
    {
        if (GameManager.Instance.FurniturePosDic.ContainsKey(furnitureID))
        {
            GameManager.Instance.FurniturePosDic[furnitureID].IsDefault = changeBool; // 直接修改值
        }

        foreach (var item in GameManager.Instance.CurrentData.AllFurniture)
        {
            if (item.Id == furnitureID)
            {
                item.IsDefault = changeBool;
            }
        }

    }

    //修改家具 解锁
    public void ChangeFurnitureItemUnlock(string furnitureID)
    {
        FurnitureItem furnitureItem = GetFurnitureItem(furnitureID);

        GameManager.Instance.FurniturePosDic[furnitureID].IsUnlocked = true;
        for (int i = 0; i < GameManager.Instance.CurrentData.AllFurniture.Count; i++)
        {
            if (GameManager.Instance.CurrentData.AllFurniture[i].Id == furnitureItem.Id)
            {
                GameManager.Instance.CurrentData.AllFurniture[i].IsUnlocked = true;
            }
        }
    }

    public void CapturePhoto()
    {
        // 创建RenderTexture
        RenderTexture rt = new RenderTexture(width, height, 24);
        targetCamera.targetTexture = rt;

        // 创建Texture2D来保存图像
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

        // 渲染相机视图到RenderTexture
        targetCamera.Render();

        // 激活RenderTexture并读取像素
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        // 重置相机设置
        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // 将图像编码为PNG
        byte[] bytes = screenShot.EncodeToPNG();

        // 保存文件
        string filename = string.Format("{0}/capture_{1}.png",
                              savePath,
                              System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        File.WriteAllBytes(filename, bytes);

        Debug.Log("照片已保存: " + filename);
    }

}

