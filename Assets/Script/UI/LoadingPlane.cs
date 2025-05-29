using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPlane : MonoBehaviour
{
    [SerializeField]
    private Image changeValue;
    [SerializeField]
    private Image bg;
    [SerializeField]
    private TMP_Text load_TMP;
    [SerializeField]
    private Image valueBG;

    //界面初始化
    public void LoadingPlaneInit()
    {
        SetProgress();
    }

    public void SetProgress()
    {
        changeValue.fillAmount = 0;
        changeValue.DOFillAmount(1, 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            LoadNextSceneWithFade();
        }); 
    }

    void LoadNextSceneWithFade()
    {
        // 异步加载场景
        SceneManager.LoadSceneAsync(UIManagement.Instance.sceneName).completed += operation => {

            valueBG.DOFade(0, 0.5f).SetEase(Ease.Linear);
            load_TMP.DOFade(0, 0.5f).SetEase(Ease.Linear);
            changeValue.DOFade(0, 0.5f).SetEase(Ease.Linear);
            bg.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.Linear);
            bg.transform.GetChild(1).GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.Linear);
            bg.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => {

                if(UIManagement.Instance.sceneName == "DressUp")
                    UIManagement.Instance.OpenFurnitureUpgradePlane();
                else
                    UIManagement.Instance.OpenGamePlane();

                this.gameObject.SetActive(false);
            });
        };
    }



    public void LoadUIScene()
    {
        changeValue.fillAmount = 0;
        changeValue.DOFillAmount(1, 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            LoadUISceneFade();
        });
    }

    public void LoadUISceneFade()
    {
        SceneManager.LoadSceneAsync("DressUp").completed += operation => {

            valueBG.DOFade(0, 0.5f).SetEase(Ease.Linear);
            load_TMP.DOFade(0, 0.5f).SetEase(Ease.Linear);
            changeValue.DOFade(0, 0.5f).SetEase(Ease.Linear);
            bg.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.Linear);
            bg.transform.GetChild(1).GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.Linear);
            bg.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => {

                this.gameObject.SetActive(false);
                UIManagement.Instance.OpenMainPlane();
            });
        };
    }

    private void OnDisable()
    {
        valueBG.color = Color.white;
        load_TMP.color = new Color32(123,35,6,255);
        changeValue.color = Color.white;
        bg.color = Color.white;
        bg.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        bg.transform.GetChild(1).GetComponent<Image>().color = Color.white;
    }
}
