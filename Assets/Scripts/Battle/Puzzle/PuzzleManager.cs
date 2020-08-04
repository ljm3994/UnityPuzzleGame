using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleUnit;
using BattlePuzzle;

public class PuzzleManager : MonoBehaviour
{
    public bool ConsoleDebug = false;
    public enum PUZZLE_STATE
    {
        WAIT,
        FILL,
        MATCH,
        CHECK,
    }

    public static PuzzleManager instance;
    public Sprite[] node_sprite;
    public GameObject oCover;
    public bool isCover;
    public bool Cover { get => isCover; set { isCover = value; oCover.SetActive(isCover); } }

    public PuzzleGrid puzzle_grid = null;
    public List<PuzzleTile> tiles;
    public List<int> tile_selected;

    private PUZZLE_STATE puzzle_state;
    public PUZZLE_STATE State { get => puzzle_state; }
    public delegate void StateEndAction();
    Dictionary<PUZZLE_STATE, StateEndAction> mStateEndActions;
   
    void Awake()
    {
        instance = this;
        tiles = puzzle_grid.Tiles;
        tile_selected = puzzle_grid.GetTileSelected();
    }

    void Update()
    {
        switch (puzzle_state)
        {
            case PUZZLE_STATE.CHECK:

                break;
            case PUZZLE_STATE.FILL:

                break;
            case PUZZLE_STATE.MATCH:
                if(!isCover && BattleUIManager.instance.PopupCanvas.transform.childCount == 0)
                {
                    Vector2 mousePos;
                    bool isInCanvas = puzzle_grid.GetMousePos(out mousePos);

                    if (Input.GetMouseButton(0) && isInCanvas)
                        PuzzleSelect(mousePos);

                    if (Input.GetMouseButtonUp(0) || !isInCanvas)
                        PuzzleSelectEnd();
                }
                break;
            case PUZZLE_STATE.WAIT:

                break;
        }



    }

    void PuzzleSelect(Vector2 mousePos)
    {
        
        float distance = 99999f;


        PuzzleTile target = null;


        foreach (PuzzleTile tile in tiles)
        {

            Vector2 puzzlePos = tile.GetPosition();

            float tempDist = Vector2.Distance(mousePos, puzzlePos);

            if (tempDist < distance) // 제일 가까운거 찾아서 target 지정
            {
                distance = tempDist;
                target = tile;
            }
        }

        if (tile_selected.Count == 0)
        {
            tile_selected.Add(target.index);
            return;
        }

        for (int i = 0; i < tile_selected.Count; ++i)
        {
            if (tile_selected[i] == target.index)
            {
                if (i < tile_selected.Count - 1)
                {
                    tile_selected.RemoveRange(i + 1, tile_selected.Count - (i + 1));
                }
                return;
            }
        }

        var lastSelected = tiles[tile_selected[tile_selected.Count-1]];

        //Debug.Log("LastSelected: X(" + lastSelected.index_x + ") Y(" + lastSelected.index_y + ")\n" 
        //    +     "target      : X(" + target.index_x + ") Y(" + target.index_y + ")");

        bool bAdd = false;

        if (lastSelected.index_x == target.index_x)
        {
            if(Mathf.Abs(lastSelected.index_y - target.index_y) == 1)
            {
                bAdd = true;
            }
        }
        else if (target.index_x % 2 == 1)
        {
            if(Mathf.Abs(lastSelected.index_x - target.index_x) == 1)
            {
                if (lastSelected.index_y == target.index_y ||
                    lastSelected.index_y == target.index_y + 1)
                {
                    bAdd = true;
                }
            }
        }
        else
        {
            if (Mathf.Abs(lastSelected.index_x - target.index_x) == 1)
            {
                if (lastSelected.index_y == target.index_y ||
                    lastSelected.index_y == target.index_y - 1)
                {
                    bAdd = true;
                }
            }
        }
        
        if(bAdd)
        {
            tile_selected.Add(target.index);
        }

    }

    void PuzzleSelectEnd()
    {
        if (tile_selected.Count == 0) return;

        List<List<int>> selected_nodes = new List<List<int>>();
        List<PuzzleNode> item_nodes = new List<PuzzleNode>();

        int groupCount = 0;
        foreach (var selected in tile_selected)
        {
            if (selected_nodes.Count == 0)
            {
                selected_nodes.Add(new List<int>());
            }

            if (selected_nodes[groupCount].Count >= 3)
            {
                selected_nodes.Add(new List<int>());
                groupCount++;
            }

            PuzzleNode node = tiles[selected].node;
            if (node.Type == PUZZLE_NODE_TYPE.ITEM)
            {
                item_nodes.Add(node);
                continue;
            }

            selected_nodes[groupCount].Add(selected);
        }

        if (selected_nodes[groupCount].Count < 3)
            selected_nodes.RemoveAt(groupCount);

        List<Unit> character_match = CharacterManager.instance.CheckPuzzle(selected_nodes);


        tile_selected.Clear();
        // Match False
        if (character_match.Count == 0) return;

        // Match at Least one of Characters
        StartCoroutine(puzzle_grid.MatchingRecipe(character_match, selected_nodes));
    }

    public void ClearPuzzle()
    {
        puzzle_grid.ClearPuzzle();
    }

    public void ClearPuzzle(int index)
    {
        puzzle_grid.ClearPuzzle(index);
    }

    public void AddEndAction(PUZZLE_STATE state, StateEndAction endAction)
    {
        if (mStateEndActions == null) mStateEndActions = new Dictionary<PUZZLE_STATE, StateEndAction>();

        StateEndAction noti;
        if (mStateEndActions.TryGetValue(state, out noti))
        {
            bool isIn = false;
            foreach (var n in noti.GetInvocationList())
            {
                if (n.Equals(endAction))
                {
                    isIn = true;
                    break;
                }
            }
            if (!isIn) noti += endAction;
        }
        else
        {
            mStateEndActions.Add(state, endAction);
        }
    }

    public void ClearEndAction()
    {
        if (mStateEndActions == null) return;

        mStateEndActions.Clear();
    }

    public void ClearEndAction(PUZZLE_STATE state)
    {
        if (mStateEndActions == null) return;

        if(mStateEndActions.ContainsKey(state))
        {
            mStateEndActions.Remove(state);
        }
    }

    public void MoveEndAction(PUZZLE_STATE source, PUZZLE_STATE dest)
    {
        if (mStateEndActions == null) return;

        StateEndAction noti;
        if(mStateEndActions.TryGetValue(source, out noti))
        {
            AddEndAction(dest, noti);
            mStateEndActions.Remove(source);
        }
    }

    public void SetStateEnd()
    {
        StateEndAction noti;
        if (mStateEndActions.TryGetValue(puzzle_state, out noti))
        {
            PUZZLE_STATE state = puzzle_state;
            noti.Invoke();
            mStateEndActions.Remove(state);
        }
    }


    public void ChangeState(PUZZLE_STATE state, StateEndAction endAction)
    {

        if(endAction != null)
        {
            ClearEndAction();
            AddEndAction(state, endAction);
        }

        puzzle_state = state;
        switch (puzzle_state)
        {
            case PUZZLE_STATE.CHECK:
                break;
            case PUZZLE_STATE.FILL:
                puzzle_grid.enabled = true;
                puzzle_grid.FillPuzzle();
                break;
            case PUZZLE_STATE.MATCH:
                break;
            case PUZZLE_STATE.WAIT:
                break;
        }
    }

}
