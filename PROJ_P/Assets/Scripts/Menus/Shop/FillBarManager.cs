using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBarManager : MonoBehaviour
{

    [SerializeField] private Image healthBar, resourceBar;

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Player.instance.HealthProp * 0.01f;
        resourceBar.fillAmount = Player.instance.Resource.Value;
    }
}
