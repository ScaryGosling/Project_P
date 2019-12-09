using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodVignette : MonoBehaviour
{

    [SerializeField] private float rangeValue;

    private Coroutine flash;
    private Image image;

    public void Start()
    {
        rangeValue = rangeValue / 255;
        image = GetComponent<Image>();
    }

    public IEnumerator Transition(float baseValue)
    {

        yield return null;
        StopCoroutine("Flash");
        StartCoroutine(Flash(image.color.a, baseValue));
    }

    public IEnumerator Flash(float startValue, float baseValue)
    {
        baseValue = baseValue / 255;

        while (image.color.a < baseValue + rangeValue)
        {
            image.color += new Color(0, 0, 0, Time.deltaTime * 0.5f);
            yield return null;
        }

        while (true)
        {
            while (image.color.a > baseValue - rangeValue)
            {
                image.color -= new Color(0, 0, 0, Time.deltaTime * 0.5f);
                yield return null;
            }

            while (image.color.a < baseValue + rangeValue)
            {
                image.color += new Color(0, 0, 0, Time.deltaTime * 0.5f);
                yield return null;
            }

        }


    }


    public IEnumerator EndFlash()
    {
        StopCoroutine("Flash");

        while (image.color.a > 0)
        {
            image.color -= new Color(0, 0, 0, Time.deltaTime * 0.5f);
            yield return null;
        }
    }

}
