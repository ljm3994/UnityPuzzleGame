using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleUnit;
using BattleCommon;
using UICommons;

namespace BattleUnit
{
    public class UnitUIController : MonoBehaviour
    {

        #region inspector
        [SerializeField]
        public GaugeBarController mHpGaugeBar;
        [SerializeField]
        public List<Image> iRecipes;

        [SerializeField]
        public List<Image> iSkillBubble;

        [SerializeField]
        public List<Transform> buffIconTRs;

        [SerializeField]
        public GameObject damageFontPrefab;
        [SerializeField]
        public Transform damageFontTr;
        [SerializeField]
        public Transform buffFontTr;

        [SerializeField]
        public Button button;
        #endregion

        Unit unitBase;
        public void Setup(Unit unit)
        {
            unitBase = unit;
            unitBase.mStatus.sumStatus.AddHPNoti((float val) => { mHpGaugeBar.Max = val; });
            unitBase.mStatus.curStatus.AddHPNoti((float val) => { mHpGaugeBar.Value = val; });

            if (unitBase.Camp == CHARACTER_CAMP.PLAYER)
            {
                for (int i = 0; i < iRecipes.Count; ++i)
                {
                    int index = i;
                    unitBase.mStatus.puzzleRecipe.AddElementNoti(index,
                        (BattlePuzzle.PUZZLE_NODE_TYPE type) =>
                        {
                            SetRecipe(index, type);
                        });
                }
            }

            unitBase.mStatus.skillBubble.AddNoti(
                (int numBubble) =>
                {
                    for (int i = 0; i < unitBase.mStatus.MAX_SKILL_BUBBLE; ++i)
                    {
                        if(i<numBubble)
                        {
                            iSkillBubble[i].color = Color.magenta;
                        }
                        else
                        {
                            iSkillBubble[i].color = Color.white;
                        }
                    }
                });

            unitBase.mStatus.buffs.AddNoti(SetBuffIcon);
            unitBase.mStatus.debuffs.AddNoti(SetBuffIcon);

            if (unitBase.Camp == CHARACTER_CAMP.ENEMY)
            {
                foreach (var item in iRecipes)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        public void ShowHPGaugeBar(bool b)
        {
            mHpGaugeBar.SetAlpha(b ? 1 : 0);
        }


        void SetBuffIcon()
        {
            foreach (var tr in buffIconTRs)
            {
                for (int i = tr.childCount - 1; i >= 0; --i)
                {
                    Destroy(tr.GetChild(i).gameObject);
                }
            }

            var effectTable = GameDataBase.Instance.EffectTable;
            var debuffs = unitBase.mStatus.debuffs.Effects;
            var buffs = unitBase.mStatus.buffs.Effects;

            if (debuffs.Count > 0)
            {
                foreach (var item in debuffs)
                {
                    GameObject obj = new GameObject("debuffIcon", typeof(Image));
                    obj.transform.SetParent(buffIconTRs[0]);
                    obj.transform.localPosition = Vector3.zero;
                    

                    var effectData = effectTable[(int)item];
                    obj.name = effectData.strName;
                    Image img = obj.GetComponent<Image>();
                    img.sprite = UICommon.LoadSprite(UIDataProcess.EffectImagePath + effectData.strEffectIcon);
                }

                foreach (var item in buffs)
                {
                    GameObject obj = new GameObject("buffIcon", typeof(Image));
                    obj.transform.SetParent(buffIconTRs[1]);
                    obj.transform.localPosition = Vector3.zero;

                    var effectData = effectTable[(int)item];
                    obj.name = effectData.strName;
                    Image img = obj.GetComponent<Image>();
                    img.sprite = UICommon.LoadSprite(UIDataProcess.EffectImagePath + effectData.strEffectIcon);
                }
            }
            else
            {
                foreach (var item in buffs)
                {
                    GameObject obj = new GameObject("buffIcon", typeof(Image));
                    obj.transform.SetParent(buffIconTRs[0]);
                    obj.transform.localPosition = Vector3.zero;

                    var effectData = effectTable[(int)item];
                    obj.name = effectData.strName;
                    Image img = obj.GetComponent<Image>();
                    img.sprite = UICommon.LoadSprite(UIDataProcess.EffectImagePath + effectData.strEffectIcon);
                }
            }
        }

        void SetRecipe(int i,BattlePuzzle.PUZZLE_NODE_TYPE type)
        {
            switch (type)
            {
                case BattlePuzzle.PUZZLE_NODE_TYPE.RED:
                    iRecipes[i].color = Color.red;
                break;
                case BattlePuzzle.PUZZLE_NODE_TYPE.GREEN:
                    iRecipes[i].color = Color.green;
                break;
                case BattlePuzzle.PUZZLE_NODE_TYPE.BLUE:
                    iRecipes[i].color = Color.blue;
                break;
                case BattlePuzzle.PUZZLE_NODE_TYPE.END:
                case BattlePuzzle.PUZZLE_NODE_TYPE.ITEM:
                case BattlePuzzle.PUZZLE_NODE_TYPE.NONE:
                default:
                    iRecipes[i].color = new Color(0,0,0,0);
                break;
            }
        }

        public void GenerateDamageFont(float damage)
        {
            GameObject go = GameObject.Instantiate(damageFontPrefab, null);
            go.transform.position = damageFontTr.position;
            Vector3 scale = damageFontTr.lossyScale;
            scale.x = Mathf.Abs(scale.x);

            go.transform.localScale = scale;
            go.GetComponentInChildren<DamageFontController>().SetText( ((int)damage).ToString());
        }

        public void GenerateDamageFont(float damage,Color color)
        {
            GameObject go = GameObject.Instantiate(damageFontPrefab, null);
            go.transform.position = damageFontTr.position;
            Vector3 scale = damageFontTr.lossyScale;
            scale.x = Mathf.Abs(scale.x);

            go.transform.localScale = scale;
            go.GetComponentInChildren<DamageFontController>().SetText(((int)damage).ToString(), color);
        }

        public void GenerateBuffFont(EFFECT effect)
        {
            GameObject go = GameObject.Instantiate(damageFontPrefab, null);
            go.transform.position = buffFontTr.position;
            Vector3 scale = buffFontTr.lossyScale;
            scale.x = Mathf.Abs(scale.x);

            

            go.transform.localScale = scale;

            if(effect == EFFECT.REINCARNATION_EFFECT)
            {
                go.GetComponentInChildren<DamageFontController>().SetText(GameDataBase.Instance.EffectTable[(int)effect].strName,Color.green);
            }
            else
            {
                go.GetComponentInChildren<DamageFontController>().SetText(GameDataBase.Instance.EffectTable[(int)effect].strName);
            }
        }
    }
}
