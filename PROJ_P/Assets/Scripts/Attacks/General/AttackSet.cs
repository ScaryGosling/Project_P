using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Attack Set")]
public class AttackSet : ScriptableObject
{
    public PlayerAttack[] list = new PlayerAttack[4];
}
