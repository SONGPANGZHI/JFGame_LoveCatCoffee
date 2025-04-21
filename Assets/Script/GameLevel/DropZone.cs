using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropZone : MonoBehaviour
{
    public Image iconSprite; 
    public BlockPropType blockPropType;
    public BlockDataConfig blockPropData;

    public void DropZoneInit(BlockDataConfig _blockPropData)
    {
        blockPropData = _blockPropData;
        iconSprite.sprite = blockPropData.DorpZoneSprite;
        blockPropType = blockPropData.blockPropType;
    }

}
