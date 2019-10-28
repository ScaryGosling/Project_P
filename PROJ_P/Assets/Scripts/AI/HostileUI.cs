//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostileUI : MonoBehaviour
{
    private GameObject hostile;
    private Slider healthSlider;
    private GenericTimer genericTimer;
    private const float uiDuration = 0.8f;
    private float targetHealthFactor;
    private float oldHealthFactor;

    [SerializeField] private GameObject healthBar;

    void Start()
    {
        hostile = gameObject.transform.parent.gameObject;
        healthSlider = healthBar.GetComponent<Slider>();
        genericTimer = gameObject.GetComponent<GenericTimer>();
        gameObject.transform.LookAt(Camera.main.transform.position);
    }

    public void ChangeHealth(float totalHealth, float newHealth)
    {
        gameObject.SetActive(true);
        targetHealthFactor = newHealth / totalHealth;
        oldHealthFactor = totalHealth;
        healthSlider.value = targetHealthFactor;
    }

    // Update is called once per frame
    void Update()
    {
        UIControl();
    }

    void UIControl()
    {
        if (healthSlider.value == 0)
            gameObject.SetActive(false);

        if (genericTimer.timeTask)
        {
            genericTimer.SetTimer(uiDuration);
            gameObject.SetActive(false);
        }
    }


}
