using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GuidanceConfig", menuName = "Guidance")]
public class GuidanceConfig : ScriptableObject
{
    public int ID;
    public Sprite maskSprite;
    public string dialogueStr;
    public bool _isDialogue;
    public string saveKey;
}
