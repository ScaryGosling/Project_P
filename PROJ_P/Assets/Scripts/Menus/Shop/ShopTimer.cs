using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTimer : MonoBehaviour
{
    [SerializeField] private Text timer;
    //private Timer genericTimer;
    private Action methodToRun;
    private float time;
    public void SetTimer(Action methodToRun, float time)
    {
        this.methodToRun = methodToRun;
        this.time = time;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (methodToRun != null)
        {
            timer.text = ((int)time).ToString("00")/* + ":" + ((genericTimer.Countdown % 1) * 100).ToString("00")*/;

        }

        if (time <= 0)
        {
            methodToRun();
            timer.text = "00:00";
            gameObject.SetActive(false);
        }
    }
}
