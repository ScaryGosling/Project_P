using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class KeyBindingButton : MonoBehaviour
{
    private bool listenToKey = false;
    [SerializeField] Keybind keyBind;
    [SerializeField] Text text;
    [SerializeField] Text buttonText;

    public void Start()
    {
        text.text = keyBind.GetFeatureName() + ":";
        buttonText.text = keyBind.GetBind().ToString();
    }

    public void ResetBind()
    {
        keyBind.ResetKey();
        buttonText.text = keyBind.GetBind().ToString();
    }


    public void ToggleKeyListener()
    {
        listenToKey = !listenToKey;
    }

    public void Update()
    {
        if (listenToKey)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    keyBind.SetBind(keyCode);
                    buttonText.text = keyBind.GetBind().ToString();
                    ToggleKeyListener();

                }
            }
        }
    }
}
