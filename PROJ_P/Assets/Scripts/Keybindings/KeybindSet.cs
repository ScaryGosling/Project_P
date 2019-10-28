using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Keybinding/Set")]
public class KeybindSet : ScriptableObject
{
    [SerializeField] private Keybind[] keybinds;

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


}
