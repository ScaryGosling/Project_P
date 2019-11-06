using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpdater : MonoBehaviour
{
    private string unitText;
    private GameObject timer;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActivateUI(int remaining)
    {
        unitText = "Units Left: " + remaining;
        gameObject.GetComponent<Text>().text = unitText;
        timer = new GameObject("UI_Timer");
        timer.AddComponent<Timer>().RunCountDown(5f, SetInactive, Timer.TimerType.DELAY);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }


}
