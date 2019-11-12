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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabSound;
    [SerializeField] private AudioClip releaseSound;
    [SerializeField] private Texture2D openHand;
    [SerializeField] private Texture2D closedHand;
    [SerializeField] private Texture2D shopHand;
    [SerializeField] private Image iconImage;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
    }
    public void OnDrop(PointerEventData eventData)
    {

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
        {
            abilityUpgrade = eventData.pointerDrag.GetComponent<AbilityUpgrade>();
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

        }
        
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

        if (image.sprite != defaultSprite)
        {
            audioSource.clip = grabSound;
            audioSource.Play();
            clone = Instantiate(gameObject, GameObject.Find("Canvas").transform);
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
        if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
        {
                image.sprite = defaultSprite;
                canvasIcon.sprite = null;
            iconImage.gameObject.SetActive(false);
            Player.instance.SetAbility(attackOnButton - 1, null);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (clone != null)
        {
            clone.transform.position = Input.mousePosition;

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            if (image.sprite != defaultSprite)
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
        if (image.sprite != defaultSprite)
        {
            Cursor.SetCursor(closedHand, Vector2.zero, CursorMode.Auto);
        }
    }
}
