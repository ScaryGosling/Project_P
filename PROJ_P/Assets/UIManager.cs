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

        hotkeyTexts[0].text = keybindSet.GetBindString(KeyFeature.BASE_ATTACK);
        hotkeyTexts[1].text = keybindSet.GetBindString(KeyFeature.ABILITY_1);
        hotkeyTexts[2].text = keybindSet.GetBindString(KeyFeature.ABILITY_2);
        hotkeyTexts[3].text = keybindSet.GetBindString(KeyFeature.ABILITY_3);

    }
}
