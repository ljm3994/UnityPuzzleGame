using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlePuzzle;
using UnityEngine.UI.Extensions;


public class PuzzleLineController : MonoBehaviour
{
    public RectTransform PuzzleCanvas;

    private UILineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<UILineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        List<int> selected = PuzzleManager.instance.tile_selected;

        List<Vector2> points = new List<Vector2>();


        for (int i = 0; i < selected.Count; ++i)
        {
            Vector3 puzzlePos = PuzzleManager.instance.tiles[selected[i]].node.GetComponent<RectTransform>().anchoredPosition;
            Vector2 point = new Vector2(puzzlePos.x/PuzzleCanvas.rect.width,puzzlePos.y/PuzzleCanvas.rect.height);

            points.Add(point);
        }

        lineRenderer.Points = points.ToArray();
    }
}
