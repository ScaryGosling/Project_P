using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    private TMP_Text text;
    [SerializeField] private MenuButtonController buttonController;
    [SerializeField]private int thisIndex;
    private bool mouseMoved;


    public void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void ActivateUnderLine()
    {
        text.fontStyle = FontStyles.Underline | FontStyles.SmallCaps | FontStyles.Bold;
    }
    public void ActivateStandard()
    {
        text.fontStyle = FontStyles.SmallCaps | FontStyles.Bold;
    }
    private void Update()
    {
        if (buttonController.index == thisIndex && buttonController.mouseTakesOver)
        {
            ActivateUnderLine();
            if (Input.GetAxis("Submit") == 1)
            {
                PointerEventData eventData = new PointerEventData(EventSystem.InternalCurrent);
                ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }
            mouseMoved = false;
        }
        else if (!mouseMoved)
        {
            ActivateStandard();
            mouseMoved = true;
        }
    }
}