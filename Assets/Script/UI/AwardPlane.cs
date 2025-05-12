using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class AwardPlane : MonoBehaviour
{
    [SerializeField]
    public GameObject awardGrid;
    [SerializeField] 
    public Transform awardTran;
    [SerializeField]
    private Button ok_BTN;
    [SerializeField]
    private string imagePath;


    private void Awake()
    {
        ok_BTN.onClick.AddListener(OKClick);
    }

    //界面初始化
    public void AwardPlaneInit()
    {
        for (int i = 0; i < PlayGameManagement.Instance.furnitureName.Count; i++)
        {
            GameObject GO = Instantiate(awardGrid, awardTran);
            GO.GetComponent<AwardGrid>().InitAwardGrid(imagePath, PlayGameManagement.Instance.furnitureName[i]);
        }

        transform.GetChild(0).DOScale(new Vector3(1,1,1),0.7f);
    }

    //三个阶段回调
    public void OKClick()
    {
        MusicManagement.instance.ClickPlaySFX();
        transform.GetChild(0).DOScale(new Vector3(0,0,0), 0.7f).OnComplete(() =>
        {
            this.gameObject.SetActive(false);

        });
    }

}
