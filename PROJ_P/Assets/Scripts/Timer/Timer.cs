using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public static Timer instance;


    public void Awake()
    {
        instance = this;
    }

    public void Run(float time, Action methodToRun) {

        StartCoroutine(RunTimer(time, methodToRun));

    }

    private IEnumerator RunTimer(float time, Action methodToRun) {

        while (true) {

            yield return new WaitForSeconds(time);
            methodToRun();

        }
    }


}
