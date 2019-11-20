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
    [SerializeField] Settings keybindSet;

    private Image buttonImage;

    public Keybind GetKeybind() { return keyBind; }
    public void Start()
    {
        text.text = keyBind.GetFeatureName() + ":";
        buttonText.text = keybindSet.GetBindString(keyBind.GetFeature());
        buttonImage = GetComponentInChildren<Button>().GetComponent<Image>();
    }

    public void ResetBind()
    {
        keyBind.ResetKey();
        buttonText.text = keybindSet.GetBindString(keyBind.GetFeature());
    }

    public void ToggleKeyListener()
    {
        listenToKey = !listenToKey;

        if(!listenToKey)
            buttonImage.color = Color.white;
        else
            buttonImage.color = new Color32(0, 155, 255, 255);

    }

    public void Update()
    {
        if (listenToKey)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    keybindSet.OverrideBind(keyCode);
                    keyBind.SetBind(keyCode);
                    buttonText.text = keybindSet.GetBindString(keyBind.GetFeature());
                    ToggleKeyListener();

                }
            }
        }
    }




}
