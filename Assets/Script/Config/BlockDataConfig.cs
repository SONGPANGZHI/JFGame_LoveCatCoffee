using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDataConfig", menuName = "BlockData")]
public class BlockDataConfig : ScriptableObject
{
    public BlockPropType blockPropType;
    public Sprite DorpZoneSprite;
    public Sprite Icon;
}
