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
        CatData.CreateCatAction += CreateCat;

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
    }

    //随机生成一只猫咪
    public void CreateCat()
    {
        int randonCat = Random.Range(0, catDatas_Temp.Count);
        CheckUsedLocation(catPos[3]);
        GameObject GO = Instantiate(catDatas_Temp[randonCat].gameObject, catPos[3]);
        GO.GetComponent<CatData>().InitCatData();
        GameManager.Instance.CheckCatRequirements(GO.GetComponent<CatData>());
        catPos.Shuffle();
    }

    //添加猫咪数组
    public void AddCatList()
    {
        for (int i = 0; i < GameManager.Instance.catDataAll.Count; i++)
        {
            catDatas_Temp.Add(GameManager.Instance.catDataAll[i]);
        }
    }

    //查看位置
    public void CheckUsedLocation(Transform trans)
    {
        while (trans.childCount > 0)
        {
            catPos.Shuffle();
            trans = catPos[0];
        }
        posTran = trans;
    }
}
