using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCommon;


namespace BattleUnit
{
    #region Enums
    public enum CHARACTER_CAMP
    {
        PLAYER,
        ENEMY,
        END
    }
    #endregion

    #region Classes
    public class PuzzleRecipe : MonoBehaviour
    {
        public List<Observe<BattlePuzzle.PUZZLE_NODE_TYPE>> elements;

        public PuzzleRecipe()
        {
            elements = new List<Observe<BattlePuzzle.PUZZLE_NODE_TYPE>>();
            for(int i = 0; i< 3; ++i)
            {
                Observe<BattlePuzzle.PUZZLE_NODE_TYPE> element = new Observe<BattlePuzzle.PUZZLE_NODE_TYPE>(BattlePuzzle.PUZZLE_NODE_TYPE.NONE);
                elements.Add(element);
            }
        }

        public bool CheckRecipe(BattlePuzzle.PUZZLE_NODE_TYPE r0, BattlePuzzle.PUZZLE_NODE_TYPE r1, BattlePuzzle.PUZZLE_NODE_TYPE r2)
        {
            return elements[0].Value == r0 && elements[1].Value == r1 && elements[2].Value == r2;
        }

        /// <summary>
        /// GenerateRandom Recipe
        /// </summary>
        /// <param name="units">do not generate same with "units" recipes</param>
        public IEnumerator GenerateRandom(List<Unit> units)
        {
            List<BattlePuzzle.PUZZLE_NODE_TYPE> myRecipe = new List<BattlePuzzle.PUZZLE_NODE_TYPE>();
            for (int i = 0; i < elements.Count; ++i)
            {
                myRecipe.Add(elements[i].Value);
            }

            int whileCount = 0;
            while (true)
            {
                whileCount++;
                for (int i = 0; i < elements.Count; ++i)
                {
                    myRecipe[i] = (BattlePuzzle.PUZZLE_NODE_TYPE)Random.Range((int)(BattlePuzzle.PUZZLE_NODE_TYPE.RED), (int)(BattlePuzzle.PUZZLE_NODE_TYPE.END));
                }


                bool overlabed = false;
                for (int i = 0; i < units.Count; ++i)
                {
                    if (units[i] == null) continue;

                    var recipes = units[i].mStatus.puzzleRecipe.elements;

                    bool recipeCheck = true;
                    for (int j = 0; j < recipes.Count; ++j)
                    {
                        if (recipes[j].Value != myRecipe[j])
                        {
                            recipeCheck = false;
                            break;
                        }
                    }

                    if (recipeCheck)
                    {
                        overlabed = true;
                        break;
                    }
                }

                if (whileCount > 100) break;
                if (overlabed == false) break;
            }

            for (int i = 0; i < elements.Count; ++i)
            {
                elements[i].Value = myRecipe[i];
                yield return new WaitForSeconds(0.2f);
            }
        }


        public void ClearRecipe()
        {
            foreach (var item in elements)
            {
                item.Value = BattlePuzzle.PUZZLE_NODE_TYPE.NONE;
            }
        }

        public void ClearRecipe(int index)
        {
            elements[index].Value = BattlePuzzle.PUZZLE_NODE_TYPE.NONE;
        }


        public void AddElementNoti(int i,Observe<BattlePuzzle.PUZZLE_NODE_TYPE>.Noti noti)
        {
            if(elements != null)
                elements[i].AddNoti(noti);
        }

    }


    [System.Serializable]
    public class GeneralStatus
    {
        public GeneralStatus(float hp, float atk, float def)
        {
            mHP = new Observe<float>(hp);
            mATK = new Observe<float>(atk);
            mDEF = new Observe<float>(def);
        }

        public GeneralStatus(GeneralStatus status)
        {
            mHP = new Observe<float>(status.HP);
            mATK = new Observe<float>(status.ATK);
            mDEF = new Observe<float>(status.DEF);
        }

        // 체력(HP) 공격력(Atk) 방어력(Def) 명중률(ACC) 회피율(AVD) 치명타(CRT)
        [SerializeField]
        Observe<float> mHP;
        [SerializeField]
        Observe<float> mATK;
        [SerializeField]
        Observe<float> mDEF;

        public float HP { get { return mHP.Value; } set { mHP.Value = value; } }
        public float ATK { get { return mATK.Value; } set { mATK.Value = value; } }
        public float DEF { get { return mDEF.Value; } set { mDEF.Value = value; } }

        public void AddHPNoti(Observe<float>.Noti noti)
        {
            mHP.AddNoti(noti);
        }

        public void AddATKNoti(Observe<float>.Noti noti)
        {
            mATK.AddNoti(noti);
        }
        public void AddDefNoti(Observe<float>.Noti noti)
        {
            mDEF.AddNoti(noti);
        }

        public void SetStatus(GeneralStatus status) { mHP.Value = status.mHP.Value; mATK.Value = status.mATK.Value; mDEF.Value = status.mDEF.Value; }
    }
    #endregion

}

