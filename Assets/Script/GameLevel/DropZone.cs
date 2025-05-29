using UnityEngine;
using UnityEngine.UI;

public class DropZone : MonoBehaviour
{
    public Image iconSprite;
    public BlockPropTypeNew blockPropTypeNew;
    public bool isMiddle = false;
    public BlockDataConfigNew blockPropDataNew;
    public GameObject effect;

    public void DropZoneInitNew(BlockDataConfigNew _blockPropData,bool middle)
    {
        effect.SetActive(false);
        blockPropDataNew = _blockPropData;
        isMiddle = middle;
        iconSprite.sprite = blockPropDataNew.DorpZoneSprite;
        blockPropTypeNew = blockPropDataNew.blockPropType;
    }

    //播放特效 然后销毁
    public void PlayEffect()
    {
        effect.SetActive(true);
        iconSprite.gameObject.SetActive(false);
        Invoke("DestroyObject",1);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
