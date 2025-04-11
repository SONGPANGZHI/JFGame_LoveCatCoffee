using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGeneration : MonoBehaviour
{
    public List<Transform> catPos;

    public List<CatData> catDatas_Temp;
    //刚开始生成3只猫咪

    private Transform posTran;
    private void Start()
    {
        AddCatList();
        CatData.CreateCatAction += DetermineCurrentProgressCat;

        CatInit();
    }

    public void CatInit()
    {
        catPos.Shuffle();
        catDatas_Temp.Shuffle();
        for (int i = 0; i < 3; i++)
        {
            GameObject GO = Instantiate(catDatas_Temp[i].gameObject, catPos[i]);
            GO.GetComponent<CatData>().InitCatData();
        }

        GameManager.Instance.CatNumChange(3);
    }

    //随机生成一只猫咪
    public void CreateCat()
    {
        int randonCat = Random.Range(0, catDatas_Temp.Count);
        GameObject GO = Instantiate(catDatas_Temp[randonCat].gameObject, CheckUsedLocation(catPos[3]));
        GO.GetComponent<CatData>().InitCatData();
        GameLevelManagement.Instance.CheckCatRequirements(GO.GetComponent<CatData>());
        catPos.Shuffle();
    }

    //添加猫咪数组
    public void AddCatList()
    {
        for (int i = 0; i < GameLevelManagement.Instance.catDataAll.Count; i++)
        {
            catDatas_Temp.Add(GameLevelManagement.Instance.catDataAll[i]);
        }
    }

    //查看位置
    public Transform CheckUsedLocation(Transform trans)
    {
        while (trans.childCount > 0)
        {
            catPos.Shuffle();
            trans = catPos[0];
        }
        posTran = trans;

        return posTran;
    }

    //判断 当前已生成几只猫
    public void DetermineCurrentProgressCat()
    {
        if (GameManager.Instance.currentNumberCats <= 30)
        {
            CreateCat();
        }
        else
        {
            //游戏结束
            UIManagement.Instance.OpenGameOverPlane(true);
        }
    }

}
