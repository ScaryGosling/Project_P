using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostileUI : MonoBehaviour
{
    private GameObject hostile;
    private Slider slider;
    [SerializeField] private GameObject healthBar;

    void Start()
    {
        hostile = gameObject.transform.parent.gameObject;
        slider = healthBar.GetComponent<Slider>();
        //genericTimer = hostile.GetComponent<Unit>().getGenericTimer;
    }

    public void ChangeHealth(float totalHealth, float newHealth)
    {
        gameObject.SetActive(true);
        slider.gameObject.SetActive(true);
        slider.value = newHealth / totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UIControl();
    }

    void UIControl()
    {
        gameObject.transform.LookAt(Camera.main.transform.position);
    }
}
