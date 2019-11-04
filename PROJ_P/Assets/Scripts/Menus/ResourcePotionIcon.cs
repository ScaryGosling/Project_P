using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePotionIcon : MonoBehaviour
{
    [SerializeField] private Sprite manaPotion;
    [SerializeField] private Sprite repairKit;
    void Start()
    {
        if (Player.instance.playerClass == PlayerClass.WIZARD)
        {
            GetComponent<Image>().sprite = manaPotion;
        }
        else
        {
            GetComponent<Image>().sprite = repairKit ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
