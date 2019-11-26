using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Settings")]
public class Settings : ScriptableObject
{
    [SerializeField] private Keybind[] keybinds;
    [SerializeField] private float aimAssistRange = 10;
    public PlayerClass playerClass {get; set;}
    public bool UseWarnings { get; set; }
    public bool UseInfo { get; set; }
    public bool UseBonus { get; set; }
    public bool UseAutoRefill { get; set; }
    public bool UseExtraShopTime { get; set; }
    public bool UseMusic { get; set; }
    public bool UseSFX { get; set; }
    public bool UseAimAssist { get; set; }
    public float enemyPace { get; set; }

    public float GetAimAssistRange() { return aimAssistRange; }

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
                }
                else if(keybinds[i].GetBind().ToString() == "Mouse0")
                {
                    return "LMB";
                }
                else if (keybinds[i].GetBind().ToString() == "Mouse1")
                {
                    return "RMB";
                }
                else
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
