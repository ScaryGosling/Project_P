using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindowFade : MonoBehaviour
{
    [SerializeField] private GameObject fade;
    void OnEnable()
    {

        AbilityUpgrade.FadeOnDrag += ToggleFade;
        fade.gameObject.SetActive(false) ;
    }
    private void OnDisable()
    {
        AbilityUpgrade.FadeOnDrag -= ToggleFade;
    }
    private void ToggleFade(bool toggle)
    {

       fade.gameObject.SetActive(toggle);
    }
}
