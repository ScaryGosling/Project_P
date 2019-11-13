using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Class : MonoBehaviour
{
    public Image icon;
    public AttackSet attackSet;
    CharacterSelection cs = CharacterSelection.instance;

    public void SetupClass() {

        cs.SetClass(attackSet);

    }
}
