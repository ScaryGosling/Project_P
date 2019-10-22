using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleArrowListener : MonoBehaviour
{
    [SerializeField] private GameObject arrowObject;
    private ShopIndicatorArrow arrow;


    void Start()
    {
        EventSystem.Current.RegisterListener<ToggleArrowEvent>(ToggleArrow);
        arrow = arrowObject.GetComponentInParent<ShopIndicatorArrow>();
    }

    private void ToggleArrow(ToggleArrowEvent toggleArrowEvent)
    {
        arrowObject.SetActive(toggleArrowEvent.toggle);
        arrow.SetGoal(toggleArrowEvent.goal);
    }
}
