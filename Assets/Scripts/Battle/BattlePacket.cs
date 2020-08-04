using BattleUnit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace BattleCommon
{
    public enum BUFFTIME
    {
        NONE,
        STARTTURN,
        ATTACK,
        HIT,
        ENDTURN,
    }



    public class Buffs : MonoBehaviour
    {
        Unit owner;
        public delegate void Noti();
        Noti noti;

        #region staticField
        // false = 중첩 불가 true = 중첩가능
        static bool[] overlab = 
        {
            false,false,false,
            false,true,true,
            true,false,false,
            false,false,false,true
        };

        static BUFFTIME[] durationTime =
        {
            BUFFTIME.NONE, BUFFTIME.NONE, BUFFTIME.NONE,                             // NONE,        ATK,        HEAL
            BUFFTIME.ENDTURN,BUFFTIME.ENDTURN,BUFFTIME.ENDTURN,               // STUN,        ADDICTION,  BLEED, 
            BUFFTIME.ENDTURN,BUFFTIME.ENDTURN,BUFFTIME.ENDTURN,               // BURN,        IMMUNE,     COUNTER
            BUFFTIME.HIT,BUFFTIME.STARTTURN,BUFFTIME.STARTTURN,BUFFTIME.STARTTURN   // INVINCIBLE,  FOCUS,      HIDE,       REGENERATION
        };

        static BUFFTIME[] executeTime =
        {
            BUFFTIME.NONE, BUFFTIME.NONE, BUFFTIME.NONE,                     // NONE,        ATK,        HEAL
            BUFFTIME.NONE, BUFFTIME.STARTTURN, BUFFTIME.ATTACK,             // STUN,        ADDICTION,  BLEED, 
            BUFFTIME.STARTTURN, BUFFTIME.NONE, BUFFTIME.ATTACK,             // BURN,        IMMUNE,     COUNTER
            BUFFTIME.NONE, BUFFTIME.NONE, BUFFTIME.NONE, BUFFTIME.STARTTURN // INVINCIBLE,  FOCUS,      HIDE,           REGENERATION
        };
        #endregion

        /*
            NO_EFFECT,  //효과 없음
            DAMAGE_EFFECT,   // 데미지
            HEAL_EFFECT,     //회복
            FAINT_EFFECT,  // 기절            // 턴 끝날때 --
            POISON_EFFECT,   // 중독          // 턴 시작시 효과 후 --
            BLOODY_EFFECT,  // 출혈           // 턴 중 행동마다 효과/ 턴 시작시 --
            FIRE_EFFECT,    // 화상           // 턴 시작시 효과 후 --
            IMMUNE_EFFECT,   //면역           // 턴 끝날때 --
            COUNTERATTACK_EFFECT, //반격      //턴 끝날때 --
            INVINCIBILITY_EFFECT,  // 무적    // 공격받는 횟수만큼 --
            CONCENTRATION_EFFECT,  // 집중    // 턴 끝날때 --
            STEALTH_EFFECT,        // 은신    // 턴 끝날때 --
            REINCARNATION_EFFECT,  // 재생    // 턴 시작시 효과 후 --
            LINKNUMBERINCREASE_EFFECT// 링크 횟수 증가
         */

        Dictionary<EFFECT, List<BattlePacket>> buffMap;

        public int Count { get => GetCount(); }
        public List<EFFECT> Effects { get => GetContainBuff(); }

        public Buffs(Unit unit)
        {
            owner = unit;
            buffMap = new Dictionary<EFFECT, List<BattlePacket>>();
            for(int i = (int)EFFECT.FAINT_EFFECT; i <= (int)EFFECT.REINCARNATION_EFFECT; ++i)
            {
                buffMap.Add((EFFECT)i, new List<BattlePacket>());
            }
        }

        public void AddBuff(BattlePacket buff)
        {
            bool bAdd = overlab[buff.eID];

            var buffList = buffMap[(EFFECT)buff.eID];
            if(buffList != null)
            {
                if(buffList.Count == 0)
                {
                    buffList.Add(BattlePacket.CopyBuff(buff));
                    noti.Invoke();
                }
                else
                {
                    if(bAdd)
                    {
                        buffList.Add(BattlePacket.CopyBuff(buff));
                        noti.Invoke();
                    }
                    else
                    {
                        buffList.Clear();
                        buffList.Add(BattlePacket.CopyBuff(buff));
                        noti.Invoke();
                    }
                }
            }
        }

        public IEnumerator Execute(BUFFTIME time)
        {
            foreach (var buffLists in buffMap)
            {
                if (owner.mSpineController.CurAnimationType == SPINE_ANIMATION_TYPE.SPINE_DIE) break;

                EFFECT effect = EFFECT.NO_EFFECT;
                float sumValue = 0;
                for(int i = buffLists.Value.Count - 1; i >= 0; --i)
                {
                    var item = buffLists.Value[i];

                    if (executeTime[(int)buffLists.Key] == time)
                    {
                        float value = 0;
                        effect = item.Execute(owner,ref value);
                        sumValue += value;
                    }

                    if (durationTime[(int)buffLists.Key] == time)
                    {
                        item.Duration--;

                        if(item.Duration <= 0)
                        {
                            buffLists.Value.Remove(item);
                            noti.Invoke();
                        }
                    }
                }

                yield return new WaitForSeconds(owner.mStatus.SetBuffEffect(effect, sumValue));

            }
        }

        public IEnumerator Execute(BUFFTIME time, EFFECT effect)
        {
            var buffLists = buffMap[effect];

            EFFECT returnEffect = EFFECT.NO_EFFECT;
            float sumValue = 0;
            for (int i = buffLists.Count - 1; i >= 0; --i)
            {
                var item = buffLists[i];

                if (executeTime[(int)effect] == time)
                {
                    float value = 0;
                    returnEffect = item.Execute(owner, ref value);
                    sumValue += value;
                }

                if (durationTime[(int)effect] == time)
                {
                    item.Duration--;

                    if (item.Duration <= 0)
                    {
                        buffLists.Remove(item);
                        noti.Invoke();
                    }
                }
            }

            yield return new WaitForSeconds(owner.mStatus.SetBuffEffect(returnEffect, sumValue));

        }

        int GetCount()
        {
            int count = 0;
            foreach (var item in buffMap)
            {
                count += item.Value.Count > 0 ? 1 : 0;
            }
            return count;
        }

        List<EFFECT> GetContainBuff()
        {
            List<EFFECT> effects = new List<EFFECT>();

            foreach (var item in buffMap)
            {
                if(item.Value.Count > 0)
                {
                    effects.Add(item.Key);
                }
            }

            return effects;
        }

        public bool Contains(EFFECT effect)
        {
            if(effect >= EFFECT.LINKNUMBERINCREASE_EFFECT || effect <= EFFECT.HEAL_EFFECT)
            {
                return false;
            }

            return buffMap[effect].Count > 0;
        }

        public void AddNoti(Noti n)
        {
            noti += n;
        }
    }

    public class BattlePacket
    {
        public class CasterInfo
        {
            private CASTER type;
            private int unitIndex;
            private int skillID;
            private float hp;
            private float atk;

            public CASTER Type { get => type; }
            public int uIndex { get => unitIndex; }
            public int sID { get => skillID; }
            public float HP { get => hp; }
            public float ATK { get => atk; }

            public CasterInfo(){}
            public CasterInfo(CasterInfo caster)
            {
                type = caster.type;
                unitIndex = caster.unitIndex;
                skillID = caster.skillID;
                hp = caster.hp;
                atk = caster.atk;
            }
            public CasterInfo(CASTER casterType, int sID = -1,int uIndex = -1,  float Hp = -1, float Atk = -1)
            {
                type = casterType;
                unitIndex = uIndex;
                skillID = sID;
                hp = Hp;
                atk = Atk;
            }
        }

        private CasterInfo casterInfo;
        private int effectID;
        private int duration;
        private float effectValue;

        #region Getter & Setter
        public CasterInfo Caster { get => casterInfo; set => casterInfo = value; }
        public int eID { get => effectID; }
        public int Duration { get => duration; set => duration = value; }
        public float eValue { get => effectValue; }

        #endregion

        #region Constructor
        public static BattlePacket CopyBuff(BattlePacket target)
        {
            BattlePacket buff = new BattlePacket();
            buff.Caster = new CasterInfo(target.Caster);

            buff.effectID = target.effectID;
            buff.duration = target.duration;
            buff.effectValue = target.effectValue;
            return buff;
        }
        public BattlePacket() { }

        public BattlePacket(int eid, int effectTurn,int eValue) { effectID = eid; duration = effectTurn; effectValue = eValue;
            casterInfo = new CasterInfo();
        }
        public BattlePacket(Unit caster, int sID, int eID, int effectTurn, int eValue)
        {
            ;

            if (caster != null)
            {
                casterInfo = new CasterInfo(
                    caster.mStatus.camp == CHARACTER_CAMP.PLAYER ? CASTER.UNIT : CASTER.ENEMY,
                    sID, caster.mStatus.unitIndex,
                    caster.mStatus.HP, caster.mStatus.ATK);
            }
            else
            {
                casterInfo = new CasterInfo(CASTER.PLAYER, sID);
            }

            effectID = eID;
            duration = effectTurn;
            effectValue = eValue;
        }


        #endregion

        /// <summary>
        /// 버프 실행
        /// </summary>
        /// <param name="value">DAMAGEorHEAL value</param>
        /// <returns>DAMAGE_EFFECT or HEAL_EFFECT</returns>
        public EFFECT Execute(Unit unit,ref float value)
        {
            unit.mUIController.GenerateBuffFont((EFFECT)eID);
            switch ((EFFECT)effectID)
            {
                case EFFECT.POISON_EFFECT:
                    value = unit.mStatus.MAXHP * (effectValue / 100);
                    return EFFECT.DAMAGE_EFFECT;
                break;
                case EFFECT.BLOODY_EFFECT:
                    value = unit.mStatus.MAXHP * (effectValue / 100);
                    return EFFECT.DAMAGE_EFFECT;
                break;
                case EFFECT.FIRE_EFFECT:
                    value = unit.mStatus.MAXHP * (effectValue / 100);
                    return EFFECT.DAMAGE_EFFECT;
                break;
                case EFFECT.REINCARNATION_EFFECT:
                    value = unit.mStatus.MAXHP * (effectValue / 100);
                    return EFFECT.HEAL_EFFECT;
                break;
                default:
                    value = 0;
                    return EFFECT.NO_EFFECT;
                break;
            }
        }

    }
}