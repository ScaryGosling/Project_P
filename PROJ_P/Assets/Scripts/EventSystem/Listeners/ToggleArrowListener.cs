using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleArrowListener : MonoBehaviour
{
    [SerializeField] private GameObject arrowObject;
    private ShopIndicatorArrow arrow;
    private List<GameObject> queue = new List<GameObject>();
    private Dictionary<GameObject, Color> dictionary = new Dictionary<GameObject, Color>();

    void Start()
    {
        EventSystem.Current.RegisterListener<ToggleArrowEvent>(ToggleArrow);
        arrow = arrowObject.GetComponentInParent<ShopIndicatorArrow>();
    }

    private void ToggleArrow(ToggleArrowEvent toggleArrowEvent)
    {
        if (toggleArrowEvent.toggle == false)
        {
            queue.Remove(toggleArrowEvent.goal);
            dictionary.Remove(toggleArrowEvent.goal);
            if (queue.Count > 0)
            {
                arrow.SetGoal(queue[0]);
                arrow.SetColor(dictionary[queue[0]]);
            }
            else
            {
                arrowObject.SetActive(false);
            }
        }
        else
        {
            queue.Add(toggleArrowEvent.goal);
            dictionary.Add(toggleArrowEvent.goal, toggleArrowEvent.arrowColor);
            arrowObject.SetActive(true);
            arrow.SetColor(dictionary[queue[0]]);
            arrow.SetGoal(queue[0]);
        }

    }


}
