using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AbilityUpgrade : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image abilityImage;
    [SerializeField] private Text abilityName;
    [SerializeField] private Text nextUpgradeCost;
    private int currentAbilityLevel;
    [SerializeField] private Text currentAbilityLevelText;
    [SerializeField] private PlayerAttack ability;
    private Potion potion;
    private Image image;
    private Text tooltipText;
    private GameObject tooltip;
    private float rowHeight;
    [SerializeField] private float hoverOffset = 5;
    private string abilityDescription;
    private float rowToCameraRatio;
    private float tooltipHeight;
    private static readonly float referenceWidth = 800;
    [SerializeField] private Texture2D openHand;
    [SerializeField] private Texture2D closedHand;
    [SerializeField] private Texture2D shopHand;
    [SerializeField] private Sprite normalBackground;
    [SerializeField] private Sprite hoverBackground;
    [SerializeField] private GameObject lockObject;
    [SerializeField] private GameObject dragAbility;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabSound;
    [SerializeField] private AudioClip releaseSound;
    [SerializeField] private AudioClip upgradeSound;
    [SerializeField] private AudioClip unlockSound;


    private void OnDisable()
    {
        if (clone != null)
        {
            Destroy(clone);
        }
    }
    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = normalBackground;

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
    public void SetTooltip(Text tooltipText)
    {
        this.tooltipText = tooltipText;
        tooltip = tooltipText.transform.parent.gameObject;
        rowHeight = GetComponent<RectTransform>().rect.height;
        tooltipHeight = tooltip.GetComponent<RectTransform>().rect.height;
    }
    public void SetPotion(Potion potion)
    {
        this.potion = potion;
        InstantiatePotionRow();
    }

    public Potion GetPotion()
    {
        return potion;
    }
    public void SetMeleeHack(MeleeHack melee)
    {
        ((RepairKit)potion).SetMeleeHack(melee);
    }
    void InstantiatePotionRow()
    {
        abilityName.text = potion.GetAbilityName();
        abilityImage.sprite = potion.GetImage();
        abilityDescription = potion.GetAbilityDescription();

        //ability.ResetLevel();
    }
    void InstantiateRow()
    {
        abilityName.text = ability.GetAbilityName();
        abilityImage.sprite = ability.GetImage();
        abilityDescription = ability.GetAbilityDescription();
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
                currentAbilityLevelText.text = "-";
                tempColor.a = 68;
                image.color = tempColor;
                lockObject.SetActive(true);
            }
            else
            {

                currentAbilityLevelText.text = currentAbilityLevel + "";
                tempColor.a = 255;
                image.color = tempColor;
                lockObject.SetActive(false);
            }
            if (ability.GetNextLevelCost(currentAbilityLevel) != -2)
            {
                nextUpgradeCost.text = ability.GetNextLevelCost(currentAbilityLevel) + "";
            }
            else
            {
                nextUpgradeCost.text = "-";
            }

        }
        else if (potion != null)
        {
            tempColor = image.color;
            tempColor.a = 255;
            image.color = tempColor;
            nextUpgradeCost.text = potion.GetPotionCost().ToString();
            currentAbilityLevelText.transform.parent.gameObject.SetActive(false);
            //currentAbilityLevelText.transform.parent.gameObject.SetActive(false);
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
                    if (ability.IsLocked())
                    {
                        audioSource.clip = unlockSound;
                    }
                    else
                    {
                        audioSource.clip = upgradeSound;
                    }
                    audioSource.Play();
                    ability.UpgradeAttack();
                    OnPointerUp(eventData);
                    Player.instance.GoldProp -= nextLevelCost;
                }
            }
        }
        else if (potion != null)
        {
            nextLevelCost = potion.GetPotionCost();
            if (Player.instance.GoldProp >= nextLevelCost)
            {
                audioSource.clip = unlockSound;
                audioSource.Play();
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
            audioSource.clip = releaseSound;
            audioSource.Play();
            Destroy(clone);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ability != null && !ability.IsLocked())
        {
            audioSource.clip = grabSound;
            audioSource.Play();
            clone = Instantiate(dragAbility, GameObject.Find("Canvas").transform);
            clone.transform.GetChild(0).GetComponent<Image>().sprite = ability.GetImage();
        }
        else
        {
            clone = null;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            image.sprite = hoverBackground;
            if (tooltip != null)
            {
                tooltip.SetActive(true);
                rowToCameraRatio = Screen.width / referenceWidth;
                tooltip.transform.position = new Vector2(transform.position.x, transform.position.y - rowToCameraRatio * (tooltipHeight / 2 + hoverOffset));
                tooltipText.text = abilityDescription;
            }
            if (ability && !ability.IsLocked())
            {
                Cursor.SetCursor(openHand, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(shopHand, Vector2.zero, CursorMode.Auto);
            }

        }


    }
    public GameObject GetClone()
    {
        return clone;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = normalBackground;
        if (tooltip != null)
            tooltip.SetActive(false);
        if (!eventData.dragging)
        {
            Cursor.SetCursor(shopHand, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ability && !ability.IsLocked())
        {
            Cursor.SetCursor(closedHand, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        PointerEventData pointer = new PointerEventData(EventSystem.InternalCurrent);
        ExecuteEvents.Execute(eventData.pointerPress, pointer, ExecuteEvents.pointerExitHandler);
        GameObject test = eventData.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < 3; i++)
        {
            if (test != clone && (test.GetComponent<AbilityUpgrade>() != null || test.GetComponent<AbilityDropHandler>() != null))
            {
                ExecuteEvents.Execute(test, pointer, ExecuteEvents.pointerEnterHandler);
                break;
            }
            else
            {
                if (test.transform.parent != null)
                {
                    test = test.transform.parent.gameObject;

                }
            }
        }
    }
}
