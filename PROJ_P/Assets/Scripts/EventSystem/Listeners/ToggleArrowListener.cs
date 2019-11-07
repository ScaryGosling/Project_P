using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleArrowListener : MonoBehaviour
{
    [SerializeField] private GameObject arrowObject;
    private ShopIndicatorArrow arrow;
    private List<GameObject> queue = new List<GameObject>();

    void Start()
    {
        EventSystem.Current.RegisterListener<ToggleArrowEvent>(ToggleArrow);
        arrow = arrowObject.GetComponentInParent<ShopIndicatorArrow>();
    }

    private void ToggleArrow(ToggleArrowEvent toggleArrowEvent)
    {
        if (toggleArrowEvent.toggle == false)
        {
            if (queue.Count > 0)
            {
                queue.Remove(toggleArrowEvent.goal);
                arrow.SetGoal(queue[0]);
            }
            else
            {
                arrowObject.SetActive(false);
            }

        }
        else
        {
            queue.Add(toggleArrowEvent.goal);
            arrowObject.SetActive(true);
            arrow.SetGoal(queue[0]);
        }
    }


}
