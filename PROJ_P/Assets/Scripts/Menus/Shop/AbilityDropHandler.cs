using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityDropHandler : MonoBehaviour, IDropHandler
{
    private RectTransform rectTransform;
    private Image image;
    private PlayerAttack ability;
    [SerializeField] private Image canvasIcon;
    [SerializeField] private int attackOnButton;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
        {
            ability = eventData.pointerDrag.GetComponent<AbilityUpgrade>().GetAbility();
            eventData.pointerDrag.GetComponent<AbilityUpgrade>().GetAbilityCat();
            image.sprite = ability.GetImage();
            Player.instance.SetAbility(attackOnButton-1 , ability);
            canvasIcon.sprite = image.sprite;
        }
        
    }

}
