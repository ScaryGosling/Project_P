using System;
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
    [SerializeField] private GameObject unlockObject;
    [SerializeField] private GameObject dragAbility;
    [SerializeField] private GameObject dragDots;
 
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabSound;
    [SerializeField] private AudioClip releaseSound;
    [SerializeField] private AudioClip upgradeSound;
    [SerializeField] private AudioClip unlockSound;

    private Animator anim;

    public static event Action<bool> VibrateOnDrag = delegate { };
    public static event Action<bool> FadeOnDrag = delegate { };

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
        if (ability!=null && !ability.IsLocked())
        {
            dragDots.SetActive(true);
        }
        anim = GetComponent<Animator>();
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
        //if (ability.IsLocked())
        //{
        //    lockObject.SetActive(true);
        //}
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

            }
            else
            {
                currentAbilityLevelText.text = currentAbilityLevel + 1 + "";
                //lockObject.SetActive(false);
            }
            if (ability.IsLocked() == false/* && (Player.instance.GoldProp < ability.GetNextLevelCost(currentAbilityLevel)|| ability.GetNextLevelCost(currentAbilityLevel) == -2)*/) //-2 is maxLevel
            {
                tempColor.a = 255;         
            }
            else /*if (ability.IsLocked() == true)*/
            {
                tempColor.a = 68;
            }
            //else if (Player.instance.GoldProp < ability.GetNextLevelCost(currentAbilityLevel)) 
            //{
            //    tempColor.a = 68;
            //}
            //else
            //{
            //    tempColor.a = 200;
            //}
                image.color = tempColor;
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
            //currentAbilityLevelText.transform.parent.gameObject.SetActive(false);
            currentAbilityLevelText.text = potion.GetResourceHandler().ToString();
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
                    if (Player.instance.GetSettings().UseSFX)
                        audioSource.Play();
                    ability.UpgradeAttack();
                    OnPointerUp(eventData);
                    Player.instance.GoldProp -= nextLevelCost;
                    dragDots.SetActive(true);
                    AutoEquipAbility.instance.AutoEquip(ability);
                }
            }
            if (ability.IsLocked() == false)
            {
                unlockObject.SetActive(false);
            }
        }
        else if (potion != null)
        {
            nextLevelCost = potion.GetPotionCost();
            if (Player.instance.GoldProp >= nextLevelCost)
            {
                audioSource.clip = unlockSound;
                if (Player.instance.GetSettings().UseSFX)
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
            if (Player.instance.GetSettings().UseSFX)
                audioSource.Play();
            Destroy(clone);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ability != null && !ability.IsLocked())
        {
            audioSource.clip = grabSound;
            if (Player.instance.GetSettings().UseSFX)
                audioSource.Play();
            clone = Instantiate(dragAbility, GameObject.Find("Canvas Variant").transform);
            clone.transform.GetChild(0).GetComponent<Image>().sprite = ability.GetImage();
            StartDragging(true);
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
                anim.SetTrigger("Hover");
            }
            else
            {
                Cursor.SetCursor(shopHand, Vector2.zero, CursorMode.Auto);
            }
            //if (ability && ability.IsLocked() && Player.instance.GoldProp >= ability.GetNextLevelCost(currentAbilityLevel))
            //{
            //    lockObject.SetActive(false);
            //    unlockObject.SetActive(true);
            //}
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
            //if (ability && ability.IsLocked())
            //{
            //    lockObject.SetActive(true);
            //    unlockObject.SetActive(false);
            //}
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ability && !ability.IsLocked())
        {
            Cursor.SetCursor(closedHand, Vector2.zero, CursorMode.Auto);
        }
    }
    private void StartDragging(bool toggle)
    {
        VibrateOnDrag(toggle);
        FadeOnDrag(toggle);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        StartDragging(false);
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
