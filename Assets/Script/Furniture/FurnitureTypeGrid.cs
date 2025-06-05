using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class FurnitureTypeGrid : MonoBehaviour
{
    public GameObject gridItemPre;
    public Transform trans;
    //初始化格子
    public void GridInit(FurnitureItem Gird)
    {
        GameObject GO = Instantiate(gridItemPre, trans);
        GO.GetComponent<FurnitureItemGrid>().ItemInit(Gird);
        GO.name = Gird.Id + "_Grid";
    }

}
