using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float Countdown { get; set; }
    public bool MethodHasRun { get; set; }
    private Action methodToRun;
    public enum TimerType { WHILE, DELAY }
    private TimerType type;


    public void CountDown()
    {

        if (Countdown > 0)
        {

            Countdown -= Time.deltaTime;

        }
        else if (!MethodHasRun)
        {
            methodToRun();
            //MethodHasRun = true;
            Destroy(gameObject);
        }

    }

    public void RunCountDown(float time, Action action, TimerType type)
    {

        this.type = type;
        methodToRun = action;
        Countdown = time;

    }

    public void ExecuteWhile()
    {
        if (Countdown > 0)
        {
            Countdown -= Time.deltaTime;
            methodToRun();
        }
        else
            Destroy(gameObject);
    }



    public void Update()
    {
        if (methodToRun == null)
            return;

        if (type == TimerType.DELAY)
        {
            CountDown();
        }
        else if (type == TimerType.WHILE)
        {
            ExecuteWhile();
        }
    }


}