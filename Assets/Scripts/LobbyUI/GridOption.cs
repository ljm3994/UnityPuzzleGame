using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UICommons;

[ExecuteInEditMode]
public class GridOption : MonoBehaviour
{
    public GridLayoutGroup grid;
    public ScrollRect scroll;
    [HideInInspector]
    public int numX = 1;
    [HideInInspector]
    public int numY = 1;

    private void Update()
    {
        if(scroll != null)
        {
            RectTransform window = scroll.GetComponent<RectTransform>();

            if (scroll.horizontal && !scroll.vertical)
            {
                grid.cellSize = new Vector2(
                    (window.rect.width - grid.padding.left - grid.spacing.y - (numX - 1) * grid.spacing.x) / (float)numX,
                    (window.rect.height - grid.padding.top - grid.padding.bottom - (numY - 1) * grid.spacing.y) / (float)numY);
            }
            else if (scroll.vertical && !scroll.horizontal)
            {
                grid.cellSize = new Vector2(
                    (window.rect.width - grid.padding.left - grid.padding.right - (numX - 1) * grid.spacing.x) / (float)numX,
                    (window.rect.height - grid.padding.top - grid.spacing.y - (numY - 1) * grid.spacing.y) / (float)numY);
            }


            UICommon.FitGridSize(transform, transform.childCount);
        }
        else
        {
            RectTransform window = GetComponent<RectTransform>();
            grid.cellSize = new Vector2(
                (window.rect.width - grid.padding.left - grid.padding.right - (numX - 1) * grid.spacing.x) / (float)numX,
                (window.rect.height - grid.padding.top - grid.padding.bottom - (numY - 1) * grid.spacing.y) / (float)numY
                );
        }
    }
}
