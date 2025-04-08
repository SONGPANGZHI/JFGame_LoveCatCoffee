using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlockPropData : MonoBehaviour
{
    public BlockPropType blockPropType;
    public GameObject prefab;

    private void Awake()
    {
        prefab.GetComponent<Button>().onClick.AddListener(BlockClick);
    }

    //点击方块
    public void BlockClick()
    {
        gameObject.SetActive(false);
        GameManager.Instance.CreateDropZoneObject(this);
    }
   
}

