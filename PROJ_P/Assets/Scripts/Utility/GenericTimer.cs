//Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This timer is currently only used for enemy attacks. Will probably be removed in the future. 
/// </summary>
public class GenericTimer : MonoBehaviour
{
    private float absoluteTime;
    private float currentTime;
    public bool TimeTask { get; set; } = false;
    
    /// <summary>
    /// Use to start timer with given time value.
    /// </summary>
    /// <param name="t"></param>
    public void SetTimer(float t)
    {
        TimeTask = false;
        absoluteTime = t;
        currentTime = absoluteTime;
    }

    void Update()
    {
        if (currentTime > 0)
            currentTime -= Time.deltaTime;
        else
        {
            TimeTask = true;
        }

    }
}


