using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodVignette : MonoBehaviour
{

    [SerializeField] private float flashTime = 1;
    private Image image;
    private Coroutine flash;

    public void Start()
    {
        image = GetComponent<Image>();
    }

    public void RunFlash(float strength)
    {
        if(flash != null)
            StopCoroutine(flash);
        flash = StartCoroutine(Flash(strength));
    }

    public IEnumerator EndFlash() {

        if (flash != null)
            StopCoroutine(flash);
        while (image.color.a > 0)
        {
            image.color -= new Color(1, 0, 0, 0.1f);
            yield return null;
        }

    }

    private IEnumerator Flash(float strength)
    {
        float activeTime = 0;

        while (true)
        {
            activeTime = 0;

            while (activeTime < flashTime)
            {
                image.color = new Color(1, 0, 0, activeTime / flashTime * strength);
                activeTime += Time.deltaTime;
                yield return null;
            }

            while (activeTime > 0)
            {
                image.color = new Color(1, 0, 0, activeTime / flashTime * strength);
                activeTime -= Time.deltaTime;
                yield return null;
            }


        }

    }

}
