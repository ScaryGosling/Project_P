using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    private bool keyDown;
    public int index { get; set; }
    private int maxIndex = 2;
    public bool mouseTakesOver { get; private set; }
    GameObject objectHovered;
    private List<MenuButton> children = new List<MenuButton>();
    PointerEventData eventData;
    [SerializeField] private CustomStandaloneInputModule inputModule;
    private void Start()
    {
        maxIndex = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                maxIndex++;
                children.Add(child.GetComponent<MenuButton>());
            }
        }

    }
    private void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (!keyDown)
            {
                Cursor.visible = false;
                foreach (MenuButton button in children)
                {

                    button.ActivateStandard();

                }
                if (Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 1;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Horizontal") < 0)
                {
                    if (index > 1)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }

                keyDown = true;
                mouseTakesOver = true;
            }

        }
        else
        {
            keyDown = false;
        }
        MoveMouse();
    }
    private void MoveMouse()
    {

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            if (mouseTakesOver)
            {
                Cursor.visible = true;

                eventData = inputModule.GetPointerData();
                objectHovered = eventData.pointerCurrentRaycast.gameObject;
                ExecuteEvents.Execute(objectHovered, eventData, ExecuteEvents.pointerEnterHandler);

                mouseTakesOver = false;
            }
        }
    }

}