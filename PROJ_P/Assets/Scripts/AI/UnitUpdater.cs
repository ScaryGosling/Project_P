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

    }

    public void ActivateUI(int remaining)
    {
        unitText = "Units Left: " + remaining;
        gameObject.GetComponent<Text>().text = unitText;
        timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
        timer.GetComponent<Timer>().RunCountDown(5f, SetInactive, Timer.TimerType.DELAY);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }


}
