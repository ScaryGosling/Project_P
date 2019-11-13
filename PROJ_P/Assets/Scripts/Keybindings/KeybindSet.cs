using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Keybinding/Set")]
public class KeybindSet : ScriptableObject
{
    [SerializeField] private Keybind[] keybinds;
    public PlayerClass playerClass {get; set;}
    public bool useWarnings { get; set; }
    public bool useInfo { get; set; }
    public bool useBonus { get; set; }

    public KeyCode GetBind(KeyFeature feature)
    {

        for (int i = 0; i < keybinds.Length; i++)
        {
            if (keybinds[i].GetFeature() == feature){

                return keybinds[i].GetBind();
            }
        }

        throw new System.Exception("Bind not found");
    }

    public string GetBindString(KeyFeature feature)
    {
        for (int i = 0; i < keybinds.Length; i++)
        {
            if (keybinds[i].GetFeature() == feature)
            {

                if (keybinds[i].GetBind().ToString().Contains("Alpha"))
                {
                    int startIndex = keybinds[i].GetBind().ToString().IndexOf("a") + 1;
                    return keybinds[i].GetBind().ToString().Substring(startIndex);
                }else
                    return keybinds[i].GetBind().ToString();

            }
        }

        throw new System.Exception("Bind not found");
    }

    public void OverrideBind(KeyCode code) {

        for (int i = 0; i < keybinds.Length; i++)
        {
            if (keybinds[i].GetBind() == code)
            {
                FindButtonOverride(keybinds[i].GetBind());
                keybinds[i].SetBind(KeyCode.None);

            }
        }
    }

    public void FindButtonOverride(KeyCode code) {

        KeyBindingButton[] buttons = FindObjectsOfType<KeyBindingButton>();

        foreach(KeyBindingButton button in buttons)
        {
            if(button.GetKeybind().GetBind() == code)
            {
                button.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KeyCode.None.ToString();
            }
        }

    }

}
