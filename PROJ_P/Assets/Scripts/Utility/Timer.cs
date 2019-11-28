//Author: Alexander Castman
//Secondary author: Emil Dahl
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float Countdown { get; set; }
    public float InitialTime;
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
            BowoniaPool.instance.AddToPool(PoolObject.TIMER, gameObject);
        }

    }

    public void CancelMethod()
    {
        methodToRun = null;
        Countdown = 0;
        BowoniaPool.instance.AddToPool(PoolObject.TIMER, gameObject);
    }

    public void RunCountDown(float time, Action action, TimerType type)
    {

        this.type = type;
        methodToRun = action;
        Countdown = time;
        InitialTime = Countdown;
    }

    /// <summary>
    /// Runs method for entered duration.
    /// </summary>
    public void ExecuteWhile()
    {
        if (Countdown > 0)
        {
            Countdown -= Time.deltaTime;
            methodToRun();
        }
        else
            BowoniaPool.instance.AddToPool(PoolObject.TIMER, gameObject);
    }

    public void ResetTimer()
    {
        Countdown = InitialTime;
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