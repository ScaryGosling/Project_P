using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindowFade : MonoBehaviour
{
    [SerializeField] private GameObject fade;
    void Start()
    {

        AbilityUpgrade.FadeOnDrag += ToggleFade;
    }
    private void ToggleFade(bool toggle)
    {
       fade.gameObject.SetActive(toggle);
    }
}
