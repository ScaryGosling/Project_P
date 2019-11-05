using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class AbilityDropHandler : MonoBehaviour, IDropHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
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
                image.sprite = ability.GetImage();
                Player.instance.SetAbility(attackOnButton - 1, ability);
                canvasIcon.sprite = image.sprite;
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
}
