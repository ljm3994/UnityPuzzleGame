using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public PuzzleGrid puzzle_grid = null;
    public List<PuzzleTile> tiles;
    public List<int> tile_selected;

    public PUZZLE_STATE puzzle_state;
    public delegate void StateEndAction();
    Dictionary<PUZZLE_STATE, StateEndAction> mStateEndActions;
   
    void Awake()
    {
        instance = this;
        tiles = puzzle_grid.GetTiles();
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
                if (Input.GetMouseButton(0))
                    PuzzleSelect();
                if (Input.GetMouseButtonUp(0))
                    PuzzleSelectEnd();
                break;
            case PUZZLE_STATE.WAIT:

                break;
        }


        Vector2 mousePos = puzzle_grid.GetMousePos();

        if(ConsoleDebug)
            Debug.Log("mouse pos : " + mousePos.ToString());
    }

    void PuzzleSelect()
    {
        
        float distance = 99999f;
        Vector2 mousePos = puzzle_grid.GetMousePos();

        if(ConsoleDebug)
            Debug.Log("mouse pos : " + mousePos.ToString());

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
            if(ConsoleDebug)
                Debug.Log("Tile Selected : " + target.index_x.ToString() + ", " + target.index_y.ToString()
                 + " Tile Position : " + target.GetPosition().ToString());
            return;
        }

        foreach (int selected in tile_selected)
        {
            if (selected == target.index)
                return;
        }


        tile_selected.Add(target.index);
        if(ConsoleDebug)
            Debug.Log("Tile Selected : " + target.index_x.ToString() + ", " + target.index_y.ToString()
            + " Tile Position : " + target.GetPosition().ToString());
    }

    void PuzzleSelectEnd()
    {
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
            if (node.GetNodeType() == PUZZLE_NODE_TYPE.ITEM)
            {
                item_nodes.Add(node);
                continue;
            }

            selected_nodes[groupCount].Add(selected);
        }

        if (selected_nodes[groupCount].Count < 3)
            selected_nodes.RemoveAt(groupCount);

        List<Character> character_match = CharacterManager.instance.CheckPuzzle(selected_nodes);


        tile_selected.Clear();
        // Match False
        if (character_match.Count == 0) return;

        // Match at Least one of Characters
        puzzle_grid.StartMatching(character_match, selected_nodes);
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

    public void SetStateEnd()
    {
        Debug.Log("PuzzleManager State(" + puzzle_state + ") End!");
        StateEndAction noti;
        if (mStateEndActions.TryGetValue(puzzle_state, out noti))
        {
            Util.BeginLog();
            Util.Log("Puzzle State End fired! " + puzzle_state);
            PUZZLE_STATE state = puzzle_state;
            noti.Invoke();
            mStateEndActions.Remove(state);

            Util.PopLog();
        }
    }


    public void ChangeState(PUZZLE_STATE state, StateEndAction endAction)
    {
        if(ConsoleDebug)
            Debug.Log("PuzzleManager : ChangeState : " + state.ToString());

        if(endAction != null)
            AddEndAction(state, endAction);

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
