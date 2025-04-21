using UnityEngine;
using UnityEngine.UI;

public class ScreenAdaptation : MonoBehaviour
{
    //∆¡ƒª  ≈‰
    public Transform verticcalGroup;

    
    private void Awake()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float designRatio = 1080f / 1980f;

        if (screenRatio >= designRatio)
        {
            //±»¿˝ designRatio = 0.5625
            verticcalGroup.GetComponent<VerticalLayoutGroup>().padding.top = -100;
            verticcalGroup.GetComponent<VerticalLayoutGroup>().spacing = -20;
        }
        else
        {
            // ’≠∆¡…Ë±∏
            verticcalGroup.GetComponent<VerticalLayoutGroup>().padding.top = 0;
            verticcalGroup.GetComponent<VerticalLayoutGroup>().spacing = 0;
        }
    }
}
