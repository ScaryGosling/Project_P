using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class AbilityDropHandler : MonoBehaviour, IDropHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Image image;
    private PlayerAttack ability;
    [SerializeField] private Image canvasIcon;
    [SerializeField] private int attackOnButton;
    private Sprite defaultSprite;
    private AbilityUpgrade abilityUpgrade;
    private AbilityDropHandler abilityDrop;
    [SerializeField] private GameObject dragAbility;

    private bool swapped = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabSound;
    [SerializeField] private AudioClip releaseSound;
    [SerializeField] private Texture2D openHand;
    [SerializeField] private Texture2D closedHand;
    [SerializeField] private Texture2D shopHand;
    [SerializeField] private Image iconImage;
    private Animator anim;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
        anim = GetComponent<Animator>();
        AbilityUpgrade.VibrateOnDrag += ToggleVibrating;
    }
    private void ToggleVibrating(bool toggle)
    {
        if (anim != null)
        {

            anim.SetBool("Vibrate", toggle);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
        {
            abilityUpgrade = eventData.pointerDrag.GetComponent<AbilityUpgrade>();
            abilityDrop = eventData.pointerDrag.GetComponent<AbilityDropHandler>();
            if (abilityUpgrade != null && abilityUpgrade.GetClone() != null)
            {


                ability = abilityUpgrade.GetAbility();
                eventData.pointerDrag.GetComponent<AbilityUpgrade>().GetAbilityCat();
                //image.sprite = ability.GetImage();
                Player.instance.SetAbility(attackOnButton - 1, ability);
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = ability.GetImage();
                canvasIcon.sprite = iconImage.sprite;
                OnPointerUp(eventData);
            }
            else if (abilityDrop != null && ability != null)
            {
                PlayerAttack temp = ability;
                ability = abilityDrop.GetAbility();
                Player.instance.SetAbility(attackOnButton - 1, ability);
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = ability.GetImage();
                canvasIcon.sprite = iconImage.sprite;
                abilityDrop.SwapAbility(temp);
     
            }

            else if (abilityDrop != null)
            {
                ability = abilityDrop.GetAbility();
                Player.instance.SetAbility(attackOnButton - 1, ability);
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = ability.GetImage();
                canvasIcon.sprite = iconImage.sprite;
                OnPointerUp(eventData);
            }

        }

    }


    public void SwapAbility(PlayerAttack ability)
    {
        this.ability = ability;
        Player.instance.SetAbility(attackOnButton - 1, ability);
        iconImage.gameObject.SetActive(true);
        iconImage.sprite = ability.GetImage();
        canvasIcon.sprite = iconImage.sprite;
        swapped = true;
    }

    public int GetAttackNumber()
    {
        return attackOnButton;
    }

    public PlayerAttack GetAbility()
    {
        return ability;
    }

    private void OnDisable()
    {
        if (clone != null)
        {
            Destroy(clone);
        }
    }
    GameObject clone;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (iconImage.sprite != null)
        {
            audioSource.clip = grabSound;

            clone = Instantiate(dragAbility, GameObject.Find("Canvas Variant").transform);
            clone.transform.GetChild(0).GetComponent<Image>().sprite = ability.GetImage();

        swapped = false;

            if (Player.instance.GetSettings().UseSFX)
                audioSource.Play();
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
        if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition) && !swapped)
        {
            image.sprite = defaultSprite;
            canvasIcon.sprite = null;
            ability = null;
            iconImage.sprite = null;
            iconImage.gameObject.SetActive(false);
            Player.instance.SetAbility(attackOnButton - 1, null);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        //anim.SetBool("Vibrate", true);
        if (clone != null)
        {
            clone.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            if (iconImage.sprite != null)
            {
                Cursor.SetCursor(openHand, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(shopHand, Vector2.zero, CursorMode.Auto);

            }

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            Cursor.SetCursor(shopHand, Vector2.zero, CursorMode.Auto);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (iconImage.sprite != null)
        {
            Cursor.SetCursor(closedHand, Vector2.zero, CursorMode.Auto);
        }
    }
}
