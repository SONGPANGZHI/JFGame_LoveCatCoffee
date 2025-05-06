using DG.Tweening;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class BaseTools : MonoBehaviour
{
    public Camera MainSceneCamera;
    public static BaseTools Instance = null;

    public GameObject tipsObject;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// 通用提示
    /// </summary>
    /// <param name="_tipsContent"></param>
    public void GeneralTips(string _tipsContent)
    {
        float X = Input.mousePosition.x - Screen.width / 2f;
        float Y = Input.mousePosition.y - Screen.height / 2f;
        Vector3 tranPos = new Vector3(X, Y, 0);

        tipsObject.SetActive(true);
        tipsObject.transform.localPosition = tranPos;
        tipsObject.transform.Find("TipText").GetComponent<Text>().text = _tipsContent;

        float defaultY = tipsObject.GetComponent<RectTransform>().anchoredPosition.y;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(tipsObject.GetComponent<RectTransform>().DOAnchorPosY(defaultY + 235f, 2.5f));
        mySequence.Join(tipsObject.transform.Find("TipText").GetComponent<Text>().DOColor(new Color32(255, 255, 255, 0), 2.5f));

        mySequence.onComplete = () =>
        {
            tipsObject.SetActive(false);
            tipsObject.transform.Find("TipText").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        };
    }

    /// <summary>
    /// 获取UI适配参数
    /// </summary>
    /// <returns></returns>
    public float GetAdaptationScale()
    {
        float scaleParm;
        float ration = 2560f / 1440f;
        float d = ((float)Screen.height / (float)Screen.width);
        scaleParm = ration / d;
        scaleParm = Mathf.Min(scaleParm, 1);
        return scaleParm;
    }

    /// <summary>
    /// 判断url是否可用
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public bool IsCanConnect(string url)
    {
        HttpWebRequest req = null;
        HttpWebResponse res = null;
        bool CanCn = true;   //设成可以连接；
        try
        {
            req = (HttpWebRequest)WebRequest.Create(url);
            res = (HttpWebResponse)req.GetResponse();
        }
        catch (Exception)
        {
            CanCn = false;   //无法连接
        }
        finally
        {
            if (res != null)
            {
                res.Close();
            }
        }
        return CanCn;
    }

    /// <summary>
    /// 等比缩放精灵
    /// </summary>
    /// <param name="pendingSprite"></param>
    /// <param name="tagreatTrans"></param>
    public void SetFurnitureSize(Sprite pendingSprite, Transform tagreatTrans)
    {
        Vector3 pendingSize = pendingSprite.bounds.size;
        Vector2 tagreatRect = tagreatTrans.GetComponent<RectTransform>().sizeDelta;
        float pendingFloat;
        if (pendingSize.x > pendingSize.y)
        {
            pendingFloat = pendingSize.y / pendingSize.x;
            tagreatTrans.GetComponent<RectTransform>().sizeDelta = new Vector2(tagreatRect.x, tagreatRect.y * pendingFloat);
        }
        else
        {
            pendingFloat = pendingSize.x / pendingSize.y;
            tagreatTrans.GetComponent<RectTransform>().sizeDelta = new Vector2(tagreatRect.x * pendingFloat, tagreatRect.y);
        }
    }

    public float AdaptationCamreaSize()
    {
        Debug.Log("device Screen height:   " + Screen.height);
        float cameraSize = (float)Screen.height / 2 / 100;
        return cameraSize;
    }

    /// <summary>
    /// 设置摄像机显示
    /// </summary>
    /// <param name="type"></param>
    public void SetCameraShowByType(int type)
    {
        MainSceneCamera.gameObject.SetActive(type == 0);
        //TutorCamera.gameObject.SetActive(type == 1);
    }

    ///// <summary>
    ///// 判断是否家具是否解锁
    ///// </summary>
    ///// <param name="furnitureName"></param>
    ///// <returns>解锁:True   未解锁:False</returns>
    //public bool FurnitureUnlockState(string furnitureName)
    //{
    //    return !CreateHomeScene.Instance.marketObjectDic.ContainsKey(furnitureName);
    //}

    /// <summary>
    /// 房间解锁状态
    /// </summary>
    /// <returns></returns>
    //public bool GetRoom2State()
    //{
    //    int unlockValue = PlayerPrefs.GetInt(PlayerPrefsKeys.room2UnlockKey, 0);
    //    return unlockValue == 1;
    //}
    //public bool GetRoom3State()
    //{
    //    int unlockValue = PlayerPrefs.GetInt(PlayerPrefsKeys.room3UnlockKey, 0);
    //    return unlockValue == 1;
    //}

}
