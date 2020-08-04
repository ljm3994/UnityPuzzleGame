using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePuzzle
{
    #region Enum Datas
    public enum PUZZLE_NODE_TYPE
    {
        RED = 0,
        GREEN ,
        BLUE ,
        END ,
        ITEM ,
        NONE ,
    }
    #endregion

    #region classes

    [System.Serializable]
    public class PuzzleTile
    {
        public PuzzleTile(int index, int x, int y, Vector2 pos)
        {
            this.index = index;
            index_x = x; index_y = y;
            position = pos;
            node = null;
        }

        public Vector2 GetPosition() { return new Vector2(position.x - offsetX, position.y); }

        public static float offsetX;
        public int index, index_x, index_y;
        public Vector2 position;
        public PuzzleNode node;
    }
    #endregion
}
