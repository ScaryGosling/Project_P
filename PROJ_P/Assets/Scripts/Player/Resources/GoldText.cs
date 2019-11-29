using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldText : MonoBehaviour
{
    private Text text;
    private TMP_Text textMesh;

    void Start()
    {
        text = GetComponent<Text>();
        textMesh = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (textMesh != null)
        {
            textMesh.text = Player.instance.GoldProp + "";
        }
        else if (text != null)
        {
        text.text = Player.instance.GoldProp + "";
        }

    }
}
