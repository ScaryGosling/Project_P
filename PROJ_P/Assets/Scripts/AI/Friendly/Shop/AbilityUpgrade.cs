using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUpgrade : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private Image abilityImage;
    [SerializeField] private Text abilityName;
    [SerializeField] private Text nextUpgradeCost;
    private int currentAbilityLevel;
    [SerializeField] private Text currentAbilityLevelText;
    [SerializeField] private PlayerAttack ability;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void EmptyRow()
    {
        abilityImage.sprite = null;
        abilityName.text = "";
        nextUpgradeCost.text = "-";
        currentAbilityLevelText.text = "-";
        ability = null;
    }


    public void SetElements(PlayerAttack ability)
    {
        this.ability = ability;
        InstantiateRow();
    }
    void InstantiateRow()
    {
        abilityName.text = ability.ToString();
        abilityImage.sprite = ability.GetImage();
        //ability.ResetLevel();
    }
    public AbilityCat GetAbilityCat()
    {
        return ability.AbilityCatProp;
    }
    public PlayerAttack GetAbility()
    {
        return ability;
    }

    Color32 tempColor;
    void Update()
    {
        if (ability != null)
        {
            tempColor = image.color;
            currentAbilityLevel = ability.CurrentLevel;
            if (currentAbilityLevel == -1)
            {
                currentAbilityLevelText.text = "U";
                image.color = new Color32(tempColor.r, tempColor.g, tempColor.b, 68);
            }
            else
            {

                currentAbilityLevelText.text = currentAbilityLevel + "";
                image.color = new Color32(tempColor.r, tempColor.g, tempColor.b, 168);
            }
            if (ability.GetNextLevelCost(currentAbilityLevel) != -2)
            {
                nextUpgradeCost.text = ability.GetNextLevelCost(currentAbilityLevel) + "$";
            }
            else
            {
                nextUpgradeCost.text = "-";
            }

        }

    }
    int nextLevelCost;
    public void OnPointerClick(PointerEventData eventData)
    {
        nextLevelCost = ability.GetNextLevelCost(currentAbilityLevel);
        if (ability != null && ability.UpgradePossible())
        {
            if (Player.instance.GoldProp >= nextLevelCost)
            {
                ability.UpgradeAttack();
                Player.instance.GoldProp -= nextLevelCost;
            }
        }
    }
    GameObject clone;
    public void OnDrag(PointerEventData eventData)
    {
        if (clone != null)
        {

            clone.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (clone != null)
        {
            Destroy(clone);

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ability.IsLocked())
        {
            clone = Instantiate(gameObject, GameObject.Find("Canvas").transform);

        }

    }
}
