using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text[] hotkeyTexts;
    [SerializeField] private KeybindSet keybindSet;


    public void Start()
    {

        hotkeyTexts[0].text = keybindSet.GetBind(KeyFeature.BASE_ATTACK).ToString();
        hotkeyTexts[1].text = keybindSet.GetBind(KeyFeature.ABILITY_1).ToString();
        hotkeyTexts[2].text = keybindSet.GetBind(KeyFeature.ABILITY_2).ToString();
        hotkeyTexts[3].text = keybindSet.GetBind(KeyFeature.ABILITY_3).ToString();

    }
}
