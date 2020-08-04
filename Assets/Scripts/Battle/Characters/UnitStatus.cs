using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleCommon;

namespace BattleUnit
{
    public class UnitStatus : MonoBehaviour
    {
        #region inspector

        // general_status 기본 수치
        [Header("General Status")]
        [SerializeField]
        GeneralStatus basicStatus;

        #endregion
        Unit unitBase;

        public int unitIndex;
        public int unitID;
        public CHARACTER_CAMP camp;
        public int AtkID;
        public int SkillID;
        public int level;

        public GeneralStatus sumStatus;
        public GeneralStatus curStatus;
        public PuzzleRecipe puzzleRecipe;

        public int MAX_SKILL_BUBBLE = 3;
        public Observe<int> skillBubble = new Observe<int>(0);

        public Buffs buffs;
        public Buffs debuffs;

        List<float> damagesFromThisTurn = new List<float>();

        public void Setup(Unit unit, UnitInitData data)
        {
            buffs = new Buffs(unit);
            debuffs = new Buffs(unit);

            unitBase = unit;
            unitIndex = data.position_index;
            AtkID = data.AtkID;
            SkillID = data.SkillID;

            unitID = data.unitID;
            level = data.level;

            curStatus = new GeneralStatus(data.basicStatus);
            sumStatus = new GeneralStatus(curStatus);


            camp = data.camp;
            if (camp == CHARACTER_CAMP.PLAYER)
            {
                puzzleRecipe = new PuzzleRecipe();
            }
        }


        public void GetHeal(float value)
        {
            curStatus.HP = Mathf.Clamp(curStatus.HP + value, 0, sumStatus.HP);
        }

        public void GetDamage(float damage)
        {
            //무적
            if(ContainsBuff(EFFECT.INVINCIBILITY_EFFECT))
            {
                StartCoroutine(buffs.Execute(BUFFTIME.HIT, EFFECT.INVINCIBILITY_EFFECT));
            }

            damagesFromThisTurn.Add(damage);
            curStatus.HP = Mathf.Clamp(curStatus.HP - damage, 0, sumStatus.HP);
            unitBase.mUIController.GenerateDamageFont(damage);
            if (Mathf.Abs(curStatus.HP) < Mathf.Epsilon)
            {
                unitBase.mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_DIE, false);
                CharacterManager.instance.SetCharacterDie(unitBase);
            }
            else
            {
                unitBase.mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_HURT, false);
                unitBase.mSpineEventHandler.AddEndAction(SPINE_ANIMATION_TYPE.SPINE_HURT,
                    () =>
                    {
                        unitBase.mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_IDLE, true);
                    });
            }
        }

        public IEnumerator ExecuteBuff(BUFFTIME time)
        {
            if (time == BUFFTIME.ENDTURN)
                damagesFromThisTurn.Clear();

            yield return StartCoroutine(buffs.Execute(time));

            yield return StartCoroutine(debuffs.Execute(time));
        }

        public IEnumerator ExecuteBuff(BUFFTIME time, EFFECT effect)
        {
            yield return StartCoroutine(buffs.Execute(time,effect));

            yield return StartCoroutine(debuffs.Execute(time,effect));
        }

        public float SetBuffEffect(EFFECT effect, float value)
        {
            float waitSeconds = 0;
            if(effect == EFFECT.DAMAGE_EFFECT)
            {
                curStatus.HP = Mathf.Clamp(curStatus.HP - value, 0, sumStatus.HP);
                unitBase.mUIController.GenerateDamageFont(value);
                if (Mathf.Abs(curStatus.HP) < Mathf.Epsilon)
                {
                    unitBase.mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_DIE, false);
                    CharacterManager.instance.SetCharacterDie(unitBase);
                    waitSeconds = 0;
                }
                else
                {
                    unitBase.mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_HURT, false);
                    waitSeconds = unitBase.mSpineController.GetAnimationLength(SPINE_ANIMATION_TYPE.SPINE_HURT) * 2;

                    unitBase.mSpineEventHandler.AddEndAction(SPINE_ANIMATION_TYPE.SPINE_HURT, () => { unitBase.mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_IDLE, true); });
                }
            }
            else if(effect == EFFECT.HEAL_EFFECT)
            {
                unitBase.mUIController.GenerateDamageFont(value,Color.green);
                curStatus.HP = Mathf.Clamp(curStatus.HP + value, 0, sumStatus.HP);
                waitSeconds = 0.5f;
            }
            return waitSeconds;
        }


        public void AddBuff(BattlePacket packet)
        {
            unitBase.mUIController.GenerateBuffFont((EFFECT)packet.eID);
            if (GameDataBase.Instance.EffectTable[packet.eID].Group == EFFECTGROUP.BUFF_EFFECT)
            {
                buffs.AddBuff(packet);
            }
            else
            {
                if(ContainsBuff(EFFECT.IMMUNE_EFFECT))
                {
                    unitBase.mUIController.GenerateBuffFont(EFFECT.IMMUNE_EFFECT);
                }
                else
                {
                    debuffs.AddBuff(packet);
                }
            }
        }

        public bool ContainsBuff(EFFECT effect)
        {
            if(debuffs.Contains(effect))
            {
                return true;
            }
            if(buffs.Contains(effect))
            {
                return true;
            }
            return false;
        }


        public float PrevDamage {
            get
            {
                float damage = 0;
                foreach (var item in damagesFromThisTurn)
                {
                    damage += item;
                }
                return damage;
            }

            set
            {
                if(value < 0)
                {
                    damagesFromThisTurn.Clear();
                }
            }
        }
        public float MAXHP { get => sumStatus.HP; }
        public float HP { get => curStatus.HP; }
        public float ATK { get => curStatus.ATK; }
        public float DEF { get => curStatus.DEF; }
        
        public int SkillBubble
        {
            get => skillBubble.Value;
            set
            {
                skillBubble.Value = Mathf.Clamp(value, 0, MAX_SKILL_BUBBLE);
            }
        }


    }

}

