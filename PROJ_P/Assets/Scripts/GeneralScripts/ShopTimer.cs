using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTimer : MonoBehaviour
{
    [SerializeField] private Text timer;
    private Timer genericTimer;
    public void SetTimer(Timer generic)
    {
        genericTimer = generic;
    }

    private void Update()
    {
        if (genericTimer != null)
        {
            timer.text = ((int)genericTimer.Countdown).ToString("00") + ":" + ((genericTimer.Countdown % 1) * 100).ToString("00");

        }
        if (genericTimer.Countdown <= 0)
        {
            timer.text = "-:-";
        }
    }
}
