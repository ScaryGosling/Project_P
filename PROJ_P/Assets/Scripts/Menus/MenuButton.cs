using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButton : MonoBehaviour
{
    private TMP_Text text;

    public void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void ActivateUnderLine()
    {
        text.fontStyle = FontStyles.Underline | FontStyles.SmallCaps | FontStyles.Bold;
    }
    public void ActivateStandard()
    {
        text.fontStyle = FontStyles.SmallCaps | FontStyles.Bold;
    }

}