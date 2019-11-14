using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutCellResize : MonoBehaviour
{
    private float width = 100;
    private RectTransform container;
    private Vector2 newCellSize = Vector2.one * 100;
    private GridLayoutGroup layoutGroup;

    private void Start()
    {
        layoutGroup = GetComponent<GridLayoutGroup>();
        container = GetComponent<RectTransform>();
    }
    void Update()
    {

        width = container.rect.width;
        newCellSize = Vector2.one * (width) / ((layoutGroup.constraintCount - 1) / 3.75f + layoutGroup.constraintCount);
        layoutGroup.cellSize = newCellSize;
        layoutGroup.spacing = Vector2.one * newCellSize / 3.75f;

    }
}
