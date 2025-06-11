using UnityEngine;
using UnityEngine.UI;

public class DropZone : MonoBehaviour
{
    public Image iconSprite;
    public BlockPropType blockPropType;
    public bool isMiddle = false;
    public BlockDataConfig blockPropData;
    public GameObject effect;

    public void DropZoneInit(BlockDataConfig _blockPropData,bool middle)
    {
        effect.SetActive(false);
        blockPropData = _blockPropData;
        isMiddle = middle;
        iconSprite.sprite = blockPropData.fruits_IMG;
        blockPropType = blockPropData.blockPropType;
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
