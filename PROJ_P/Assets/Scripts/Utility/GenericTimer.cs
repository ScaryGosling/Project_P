using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenericTimer : MonoBehaviour
{
    private float absoluteTime;
    private float currentTime;
    public bool timeTask { get; set; }
    


    public void SetTimer(float t)
    {
        timeTask = false;
        absoluteTime = t;
        currentTime = absoluteTime;
    }

    void Update()
    {
        if (currentTime >= 0)
            currentTime -= Time.deltaTime;
        else
        {
            timeTask = true;
        }


    }
}


