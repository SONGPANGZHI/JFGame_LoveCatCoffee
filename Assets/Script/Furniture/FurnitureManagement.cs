using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureManagement : MonoBehaviour
{
    public static FurnitureManagement instance;
    public List<string> boxName;
    public GameObject furniturePrefab;
    public Transform furnitureTrans;

    public static string dialogueNoveicKey = "DialogueNoveicKEY";       //对话新手引导

    public FurnitureInfo currentClickFurniture;
    public FurnitureInfo lastClickFurniture;

    public List<GameObject> sceneFurniture;
    public Camera MainSceneCamera;

    public List<FurnitureItem> firstFloorFurniture;     //一楼家具
    public List<FurnitureItem> secondFloorFurniture;    //二楼家具


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        GameManager.Instance.AddCurrentFurnitureData();
        BaseTools.Instance.ScreenAdaptation(MainSceneCamera);

        //家具分类
        FurnitureCategory();

        DefaultFurnitureInit();
    }

    //家具分类
    public void FurnitureCategory()
    {
        for (int i = 0; i < GameManager.Instance.CurrentData.AllFurniture.Count; i++)
        {
            if (GameManager.Instance.CurrentData.AllFurniture[i].FurnitureFloor == FurnitureFloor.FirstFloor)
                firstFloorFurniture.Add(GameManager.Instance.CurrentData.AllFurniture[i]);
            else if (GameManager.Instance.CurrentData.AllFurniture[i].FurnitureFloor == FurnitureFloor.SecondFloor)
                secondFloorFurniture.Add(GameManager.Instance.CurrentData.AllFurniture[i]);
        }
    }

    public void DefaultFurnitureInit()
    {
        for (int i = 0; i < GameManager.Instance.currentFurnitureData.Count; i++)
        {
            GameObject GO = Instantiate(furniturePrefab, furnitureTrans);
            GO.GetComponent<FurnitureInfo>().Init(GameManager.Instance.currentFurnitureData[i]);
            GO.name = GameManager.Instance.currentFurnitureData[i].Id;
            GO.transform.localScale = Vector3.one;
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
        GO.transform.DOScale(1.2f, 0.3f).SetEase(Ease.Linear).OnComplete(() => 
        {
            GO.transform.localScale = Vector3.one; // 动画完成后恢复原始大小
        });
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

    //判断当前点击家具 改变上一个点击家具状态
    public void JudgeCurrentClickFurniture()
    {
        if (lastClickFurniture == null)
        {
            lastClickFurniture = currentClickFurniture;
            return;
        }

        //不相同
        if (lastClickFurniture != currentClickFurniture)
        {
            lastClickFurniture.UseDefualtMaterial();
            lastClickFurniture = currentClickFurniture;
        }
    }

    //保存时 恢复默认 材质
    public void SaveFurnitureDefualtMaterial()
    {
        if (lastClickFurniture == null)
        {
            return;
        }
        else
        {
            lastClickFurniture.UseDefualtMaterial();
        }
    }
}

