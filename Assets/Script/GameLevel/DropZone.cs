using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropZone : MonoBehaviour
{
    public Image iconSprite; 
    public BlockPropType blockPropType;
    public BlockPropData blockPropData;

    public void DropZoneInit(BlockPropData _blockPropData)
    {
        blockPropData = _blockPropData;
        iconSprite.sprite = blockPropData.dorpZoneSprite;
        blockPropType = blockPropData.blockPropType;
    }

}
