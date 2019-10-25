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
    private Potion potion;
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
    public void SetPotion(Potion potion)
    {
        this.potion = potion;
        InstantiatePotionRow();
    }
    void InstantiatePotionRow()
    {
        abilityName.text = potion.ToString();
        abilityImage.sprite = potion.GetImage();
        //ability.ResetLevel();
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
                tempColor.a = 68;
                image.color = tempColor;
            }
            else
            {

                currentAbilityLevelText.text = currentAbilityLevel + "";
                tempColor.a = 168;
                image.color = tempColor;
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
        else if(potion != null)
        {
            tempColor = image.color;
            tempColor.a = 168;
            image.color = tempColor;
            nextUpgradeCost.text = potion.GetPotionCost().ToString();
            currentAbilityLevelText.transform.parent.gameObject.SetActive(false);
        }

    }
    int nextLevelCost;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ability != null)
        {

            nextLevelCost = ability.GetNextLevelCost(currentAbilityLevel);
            if (ability.UpgradePossible())
            {
                if (Player.instance.GoldProp >= nextLevelCost)
                {
                    ability.UpgradeAttack();
                    Player.instance.GoldProp -= nextLevelCost;
                }
            }
        }
        else if (potion != null)
        {
                nextLevelCost = potion.GetPotionCost();
            if (Player.instance.GoldProp >= nextLevelCost)
            {
                potion.BuyPotion(nextLevelCost);
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
        if (ability != null && !ability.IsLocked())
        {
            clone = Instantiate(gameObject, GameObject.Find("Canvas").transform);

        }

    }
}
