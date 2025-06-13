using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPlane : MonoBehaviour
{
    public GameObject LoadAnim;
 
    //界面初始化
    public void LoadingPlaneInit()
    {
        LoadAnim.SetActive(true);

        Invoke("LoadNextSceneWithFade",1.3f);
    }

  
    void LoadNextSceneWithFade()
    {
        // 异步加载场景
        SceneManager.LoadSceneAsync(UIManagement.Instance.sceneName).completed += operation => {


            if (UIManagement.Instance.sceneName == "DressUp")
                UIManagement.Instance.OpenFurnitureUpgradePlane();
            else
                UIManagement.Instance.OpenGamePlane();

            LoadAnim.SetActive(false);
            this.gameObject.SetActive(false);
        };
    }



    public void LoadUIScene()
    {
        LoadAnim.SetActive(true);

        Invoke("LoadUISceneFade", 1.3f);
    }

    public void LoadUISceneFade()
    {
        SceneManager.LoadSceneAsync("DressUp").completed += operation => {

            LoadAnim.SetActive(false);
            this.gameObject.SetActive(false);
            UIManagement.Instance.OpenMainPlane();
        };
    }

}
