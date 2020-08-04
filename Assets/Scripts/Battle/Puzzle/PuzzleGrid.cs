using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleUnit;
using UnityEngine.EventSystems;

namespace BattlePuzzle
{
    public class PuzzleGrid : MonoBehaviour
    {
        #region inspector
        public bool ConsoleDebug = false;

        #endregion
        #region Puzzle Gird Info Constant
        private const float ROOT3 = 1.732f;
        public const int PUZZLE_NUM_X = 7;
        public const int PUZZLE_NUM_Y = 5;
        private const int NUM_RADIUS_X = ((PUZZLE_NUM_X) / 2 + ((PUZZLE_NUM_X + 1) / 2) * 2);

        private float TILE_RADIUS;
        private float START_POS_Y;
        private GameObject tile_prefab;
        private RectTransform rt_grid;
        private Vector2 root_canvas_size;
        #endregion


        bool pointerDown = false;

        List<PuzzleTile> mTiles = new List<PuzzleTile>();
        public List<PuzzleTile> Tiles{ get=>mTiles; }

        List<int> mTilesSelected = new List<int>();
        public List<int> GetTileSelected() { return mTilesSelected; }

        //생성할 Puzzle 담당
        List<Queue<int>> mNewNodes;

        // Start is called before the first frame update
        private void Awake()
        {
            RectTransform rtRootCanvas = GetComponent<Canvas>().rootCanvas.GetComponent<RectTransform>();
            root_canvas_size = new Vector2(rtRootCanvas.rect.width, rtRootCanvas.rect.height);
        }

        void Start()
        {
            tile_prefab = Resources.Load("Prefabs/Puzzle/PuzzleNode") as GameObject;

            rt_grid = GetComponent<RectTransform>();
            TILE_RADIUS = rt_grid.rect.width/ NUM_RADIUS_X;
            START_POS_Y = rt_grid.rect.height + TILE_RADIUS;

            mNewNodes = new List<Queue<int>>(PUZZLE_NUM_X);

            for (int i = 0; i < PUZZLE_NUM_X; ++i)
                mNewNodes.Add(new Queue<int>());


            float offset_x = -NUM_RADIUS_X / 2f * TILE_RADIUS;
            PuzzleTile.offsetX = offset_x;
            for (int y = 0; y < PUZZLE_NUM_Y; ++y)
                for (int x = 0; x < PUZZLE_NUM_X; ++x)
                {
                    int dy = x % 2;
                     Vector2 centerPosition = new Vector2(
                         ((x * 3) + 2) / 2f * TILE_RADIUS + offset_x,
                         (((2 * y + 1) * ROOT3 / 2f) + (dy * ROOT3 / 2f)) * TILE_RADIUS);


                    PuzzleTile tile = new PuzzleTile(y * PUZZLE_NUM_X + x, x, y, centerPosition);
                    tile.node = null;
                    mTiles.Add(tile);
                }
        }


        public void FillPuzzle()
        {
            for (int col = 0; col < PUZZLE_NUM_X; ++col)
            {
                Queue<int> queue = new Queue<int>();

                for (int y = 0; y < PUZZLE_NUM_Y; ++y)
                {
                    int index = y * PUZZLE_NUM_X + col;

                    if (mTiles[index].node == null)
                    {
                        queue.Enqueue(index);
                        continue;
                    }

                    if (queue.Count == 0) continue;

                    int new_index = queue.Dequeue();

                    // tile node 새로 지정
                    PuzzleNode node = mTiles[index].node;
                    mTiles[index].node = null;
                    queue.Enqueue(index);

                    node.mIndex = new_index;
                    node.mIndexX = col;
                    node.mIndexY = new_index / PUZZLE_NUM_X;

                    node.mPosition = mTiles[new_index].position;

                    mTiles[new_index].node = node;


                    StartCoroutine(node.MoveToMyTile());
                }


                
                while (queue.Count != 0)
                {
                    int tile_index = queue.Dequeue();

                    GameObject obj = Instantiate(tile_prefab);

                    obj.transform.SetParent(GameObject.Find("PuzzleNodes").transform);
                    obj.transform.localPosition = new Vector2(mTiles[col].position.x, START_POS_Y);
                    obj.transform.localScale = new Vector3(1, 1, 1);

                    PuzzleNode node = obj.GetComponent<PuzzleNode>();
                    node.mPosition = mTiles[tile_index].position;
                    node.mIndexX = col;
                    node.mIndexY = tile_index / PUZZLE_NUM_X;
                    node.mIndex = tile_index;
                    float size = TILE_RADIUS;
                    node.SetNodeSize(new Vector2(size, size));

                    mTiles[tile_index].node = node;
                    mNewNodes[col].Enqueue(tile_index);
                }
            }

            StartCoroutine(MoveNewPuzzles());
        }

        IEnumerator MoveNewPuzzles()
        {
            int maxCount = 0;
            for (int i = 0; i < mNewNodes.Count; i++)
            {
                maxCount = Mathf.Max(maxCount, mNewNodes[i].Count);
            }

            for (int low = 0; low < maxCount; ++low)
            {
                bool wait = false;
                for (int col = 0; col < PUZZLE_NUM_X; ++col)
                {
                    if (mNewNodes[col].Count == 0) continue;
                    wait = true;
                    int tile_index = mNewNodes[col].Dequeue();
                
                    StartCoroutine(mTiles[tile_index].node.MoveToMyTile());
                }

                if(wait)
                {
                    yield return new WaitForSeconds(0.2f);
                }

            }

            PuzzleManager.instance.SetStateEnd();
        }

        public IEnumerator MatchingRecipe(List<Unit> units, List<List<int>> node_matched)
        {
            for (int i = 0; i < units.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int tile_index = node_matched[i][j];
                    
                    yield return StartCoroutine(mTiles[tile_index].node.MatchRecipeEffect(units[i], j));
                }
            }

            PuzzleManager.instance.SetStateEnd();
        }

        public void ClearPuzzle()
        {
            foreach (var item in mTiles)
            {
                Destroy(item.node.gameObject);
                item.node = null;
            }
        }

        public void ClearPuzzle(int index)
        {
            Destroy(mTiles[index].node.gameObject);
            mTiles[index].node = null;
        }

        #region UTIL
        // 좌표 계산
        public bool GetMousePos(out Vector2 mousePos)
        {
            mousePos = Input.mousePosition;
            mousePos.x = Mathf.Clamp01(mousePos.x / Screen.width) * root_canvas_size.x;
            mousePos.y = Mathf.Clamp01(mousePos.y / Screen.height) * root_canvas_size.y;
            if (mousePos.y > rt_grid.anchorMax.y * root_canvas_size.y)
            {
                return false;
            }

            return true;
        }
        #endregion

    }

}