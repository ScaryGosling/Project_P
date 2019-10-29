using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenericTimer : MonoBehaviour
{
    private float absoluteTime;
    private float currentTime;
    public bool TimeTask { get; set; } = false;
    
    public void SetTimer(float t)
    {
        TimeTask = false;
        absoluteTime = t;
        currentTime = absoluteTime;
    }

    void Update()
    {
        if (currentTime >= 0)
            currentTime -= Time.deltaTime;
        else
        {
            TimeTask = true;
        }

    }
}


