using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Attacks/Attack Set")]
public class AttackSet : ScriptableObject
{
    public Sprite classIcon;
    [TextArea] public string description;
    public PlayerClass playerClass;
    public PlayerAttack[] list = new PlayerAttack[4];
    public Potion[] potionList = new Potion[0];

    public PlayerStats originalStats;
}
