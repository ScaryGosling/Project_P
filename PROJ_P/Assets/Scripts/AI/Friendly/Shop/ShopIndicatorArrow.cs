using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopIndicatorArrow : MonoBehaviour
{
    [SerializeField]private GameObject goal;
    private Camera mainCamera;
    private Vector3 viewportPoint;
    [SerializeField] private GameObject arrow;
    private Quaternion temp;
    private Vector3 arrowDirection;
    private float scaleFactor;
    [SerializeField] private float arrowScaleDistance = 35;
    void Start()
    {
        mainCamera = Camera.main;
    }


    void Update()
    {

        if (goal != null && goal.gameObject.activeInHierarchy)
        {
            arrowDirection = (goal.transform.position - transform.position).normalized;
            temp = Quaternion.LookRotation(arrowDirection);
            temp = Quaternion.Euler(90, temp.eulerAngles.y, temp.eulerAngles.z);
            arrow.transform.rotation = temp;

            if (DistanceToGoal() > arrowScaleDistance)
            {
                scaleFactor = 1;
            }
            else
            {
                scaleFactor = DistanceToGoal() / arrowScaleDistance;
            }
            arrow.transform.localScale = Vector3.one * scaleFactor;
        }

    }
    private float DistanceToGoal()
    {
        return Vector3.Distance(transform.position, goal.transform.position);
    }
    public void SetGoal(GameObject goal)
    {
        this.goal = goal;
    }
    private bool IsVisible()
    {
        viewportPoint = mainCamera.WorldToViewportPoint(goal.transform.position);
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            return false;
        }
        return true;
    }
}
