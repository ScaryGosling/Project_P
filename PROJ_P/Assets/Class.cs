using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Class : MonoBehaviour
{
    public Image icon;
    public AttackSet attackSet;
    CharacterSelection cs = CharacterSelection.instance;
    [SerializeField] private Sprite inactiveBG;
    [SerializeField] private Sprite activeBG;
    [SerializeField] private Image background;

    public void SetupClass() {

        cs.SetClass(attackSet);
        Clicked();
    }

    public void HoverOn()
    {
        background.sprite = activeBG;
    }
    public void HoverOff()
    {
        background.sprite = inactiveBG;
    }
    public void Clicked()
    {
        background.color = Color.white;
    }
    public void Unclicked()
    {
        background.color = Color.gray;
    }
}
