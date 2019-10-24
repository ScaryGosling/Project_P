﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float Countdown { get; set; }
    public bool MethodHasRun { get; set; }
    private Action methodToRun;


    public void CountDown() {

        if (Countdown > 0)
        {

            Countdown -= Time.deltaTime;

        }
        else if (!MethodHasRun) {
            methodToRun();
            //MethodHasRun = true;
            Destroy(gameObject);
        }

    }

    public void RunCountDown(float time, Action action) {

        methodToRun = action;
        Countdown = time;

    }

    public void Update()
    {
        if(methodToRun != null)
            CountDown();

    }


}