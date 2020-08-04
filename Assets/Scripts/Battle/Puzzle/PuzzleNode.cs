using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleUnit;


namespace BattlePuzzle
{
    public class PuzzleNode : MonoBehaviour
    {
        public bool ConsoleDebug = false;
        #region PuzzleNode Member 변수
        Image puzzleImage = null;

        public float mMovingSpeed;

        [SerializeField]
        PUZZLE_NODE_TYPE mNodeType;
        public PUZZLE_NODE_TYPE Type { get => mNodeType; }

        public int mIndexX;
        public int mIndexY;
        public int mIndex;

        public Vector2 mPosition;
        #endregion
        RectTransform mRectTransform;


        private void Awake()
        {
            mRectTransform = GetComponent<RectTransform>();
            mMovingSpeed = 10f * (Screen.height / 480f);
        }
        private void Start()
        {
            mNodeType = (PUZZLE_NODE_TYPE)Random.Range(0, (int)PUZZLE_NODE_TYPE.END);
            puzzleImage = GetComponent<Image>();
            puzzleImage.sprite = PuzzleManager.instance.node_sprite[(int)mNodeType];
        }

        public void SetNodeSize(Vector2 size)
        {
            mRectTransform.sizeDelta = size;
        }

        public IEnumerator MoveToMyTile()
        {
            Vector3 pos = transform.localPosition;

            while (transform.localPosition.y > mPosition.y)
            {
                pos.y -= mMovingSpeed;
                transform.localPosition = pos;

                yield return new WaitForEndOfFrame();
            }

            pos.y = mPosition.y;
            transform.localPosition = pos;
        }

        public IEnumerator MatchRecipeEffect(Unit unit, int gemIndex)
        {
            unit.mStatus.puzzleRecipe.ClearRecipe(gemIndex);
            PuzzleManager.instance.tiles[mIndex].node = null;
            Destroy(this.gameObject);

            yield return new WaitForSeconds(0.2f);
        }
    }

}