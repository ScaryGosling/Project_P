using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopIndicatorArrow : MonoBehaviour
{
    [SerializeField]private GameObject shopKeeper;
    private Camera mainCamera;
    private Vector3 viewportPoint;
    private float angle;
    [SerializeField] private GameObject arrow;

    private Vector3 tempPlayerPosition, tempShopPosition;
    private Quaternion temp;
    private Vector3 arrowDirection;
    void Start()
    {
        mainCamera = Camera.main;
    }


    void Update()
    {

        if (shopKeeper.gameObject.activeInHierarchy && !IsVisible())
        {
            arrow.gameObject.SetActive(true);
            arrowDirection = (shopKeeper.transform.position - transform.position).normalized;
            temp = Quaternion.LookRotation(arrowDirection);
            temp = Quaternion.Euler(90, temp.eulerAngles.y, temp.eulerAngles.z);
            arrow.transform.rotation = temp;
        }
        else
        {
            arrow.gameObject.SetActive(false);
        }




        
    }
    private bool IsVisible()
    {
        viewportPoint = mainCamera.WorldToViewportPoint(shopKeeper.transform.position);
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            return false;
        }
        return true;
    }
}
