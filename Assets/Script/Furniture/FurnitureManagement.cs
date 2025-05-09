using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureManagement : MonoBehaviour
{

    #region  家具点击
    //射线检查
    public void OnPointClick()
    {
        Debug.Log("OnPointClick ---- ");
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    Debug.Log("OnPointClick ---- ");
    //}
    #endregion

}

public class FurnitureData
{ 
    public string FurnitureName;
}

public class Award
{
    public int ID;
    public string FurnitureNmae;
    public AwardType AwardType;
    public FurnitureFloor FurnitureFloor;   
    
}