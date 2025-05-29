using System.Collections.Generic;
using UnityEngine;

public class BlockGeneration : MonoBehaviour
{
    public List<Transform> prefabTrans;
   
    //生成所有数据
    public void GenerateAllBlock()
    {
        for (int i = 0; i < prefabTrans.Count; i++)
        {
            int elementCount = Random.Range(1,4);
            GenerateBlock(i, elementCount);
        }

        PlayGameManagement.Instance.UpdateMysteryBox();
    }

    //生成一组数据
    public void GenerateBlock(int transIndex,int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomBlockType = Random.Range(0,PlayGameManagement.Instance.blockTypeNum);
            GameObject element = Instantiate(PlayGameManagement.Instance.blockPrefab, prefabTrans[transIndex]);
            element.transform.localPosition = new Vector2(0, i * 15);
            element.GetComponent<BlockPropData>().ConveyorBlockInit(PlayGameManagement.Instance.blockTypes[randomBlockType]);

            if (i < count - 1)
                element.GetComponent<BlockPropData>().ButtonNotClickable();

        }
    }

    public void ClearAllBlock()
    {
        for (int i = 0; i < prefabTrans.Count; i++)
        {
            for (int j = 0; j < prefabTrans[i].childCount; j++)
            {
                Destroy(prefabTrans[i].GetChild(j).gameObject);
            }
        }
    }


}
