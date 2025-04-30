using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropZone : MonoBehaviour
{
    #region ÐÂ°æÍæ·¨

    public Image iconSprite;
    public BlockPropTypeNew blockPropTypeNew;
    public BlockDataConfigNew blockPropDataNew;

    public void DropZoneInitNew(BlockDataConfigNew _blockPropData)
    {
        blockPropDataNew = _blockPropData;
        iconSprite.sprite = blockPropDataNew.DorpZoneSprite;
        blockPropTypeNew = blockPropDataNew.blockPropType;
    }

    #endregion

    #region ¾É°æÍæ·¨

    public BlockPropType blockPropType;
    public BlockDataConfig blockPropData;
    public void DropZoneInit(BlockDataConfig _blockPropData)
    {
        blockPropData = _blockPropData;
        iconSprite.sprite = blockPropData.DorpZoneSprite;
        blockPropType = blockPropData.blockPropType;
    }
    #endregion


}
