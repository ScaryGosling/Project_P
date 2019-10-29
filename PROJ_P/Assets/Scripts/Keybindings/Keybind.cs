using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Keybinding/Bind")]
public class Keybind : ScriptableObject
{
    [SerializeField] private string featureName;
    [SerializeField] private KeyFeature feature;
    [SerializeField] private KeyCode inputKey;
    [SerializeField] private KeyCode defaultKey;



    public KeyCode GetBind() { return inputKey; }
    public KeyFeature GetFeature() { return feature; }
    public string GetFeatureName() { return featureName; }
    public void SetBind(KeyCode inputKey) { this.inputKey = inputKey; }
    public void ResetKey() { inputKey = defaultKey; }
}

public enum KeyFeature
{
    FORWARD_MOVEMENT, BACKWARD_MOVEMENT, RIGHT_MOVEMENT, LEFT_MOVEMENT, BASE_ATTACK, ABILITY_1, ABILITY_2, ABILITY_3, EXECUTE, REFILL, TOGGLE_SHOP
}