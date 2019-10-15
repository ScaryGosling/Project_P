using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenericTimer : MonoBehaviour
{
    public float absoluteTime { get; set; }
    private float currentTime;
    public bool timerTaskCompleted { get; set; }
    //public float setAbsoluteTime { get { return absoluteTime; } set { absoluteTime = value; } }

    public void startTimer()
    {
        currentTime = absoluteTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
            timerTaskCompleted = true;
        
    }
}


