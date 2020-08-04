using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using BattleUnit;
using BattleCommon;

namespace BattleUnit
{
    public class Unit : MonoBehaviour
    {
        #region inspector
        public UnitUIController mUIController;
        public UnitStatus mStatus;
        public UnitSpineController mSpineController;
        public UnitSpineEventHandler mSpineEventHandler;
        
        #endregion

        CHARACTER_CAMP myCamp;
        public CHARACTER_CAMP Camp
        {
            get => myCamp;
            set
            {
                myCamp = value;
                if(myCamp == CHARACTER_CAMP.PLAYER)
                {
                    SetFlipX(true);
                }
                else
                {
                    SetFlipX(false);
                }
            }
        }


        public void SetUpCharacter(CHARACTER_CAMP camp, Vector3 position, UnitInitData data)
        {
            if (position != Vector3.zero)
                transform.position = position;

            mStatus.Setup(this, data);
            mSpineController.Setup(this);
            mSpineEventHandler.Setup(this);

            myCamp = camp;
            mUIController.Setup(this);
        }

        public void SetFlipX(bool flip)
        {
            Vector3 scale = Vector3.one;
            if (flip) scale.x *= -1;
            mSpineController.transform.localScale = scale;
        }

        public void ShowGaugeBar(bool b)
        {
            mUIController.ShowHPGaugeBar(b);
        }



        #region 캐릭터
        /// TODO : MANA BUBBLE 채우는 시기
        /// <summary>
        /// 캐릭터 공격
        /// 1. 타깃지정 2.이동(원거리시 이동 X) 3. 공격 4. 원위치로 이동
        /// </summary>
        /// <returns></returns>
        public IEnumerator Attack()
        {
            // 1. 타깃 지정
            List<BattlePacket> packets = new List<BattlePacket>();
            List<List<Unit>> targetLists = new List<List<Unit>>();
            ATTACKRANGE atkRange;
            SKILLTYPE skillType;
            bool flip_ori = mSpineController.transform.localScale.x > 0 ? false : true;
            int oldLayerOrder = mSpineController.SetDrawOrder(10);

            if (mStatus.SkillID != -1 && mStatus.skillBubble.Value == mStatus.MAX_SKILL_BUBBLE)
            {
                skillType = SKILLTYPE.SKILL_TYPE;
                mStatus.skillBubble.Value = 0;
            }
            else
            {
                skillType = SKILLTYPE.NORMAL_TYPE;
                mStatus.skillBubble.Value += 1;
            }
            SetTarget(skillType, ref packets, ref targetLists, out atkRange);

            Vector3 pos_origin = transform.position;
            Vector3 target_position;

            float counterDamage = 0;
            if (mStatus.ContainsBuff(EFFECT.COUNTERATTACK_EFFECT))
            {
                if (mStatus.PrevDamage > 0)
                {
                    mUIController.GenerateBuffFont(EFFECT.COUNTERATTACK_EFFECT);
                    counterDamage = mStatus.PrevDamage;
                    mStatus.PrevDamage = -1;
                }
                else
                {
                    skillType = SKILLTYPE.NORMAL_TYPE;
                }
            }


            SPINE_ANIMATION_TYPE animationType = skillType == SKILLTYPE.NORMAL_TYPE ? SPINE_ANIMATION_TYPE.SPINE_ATTACK : SPINE_ANIMATION_TYPE.SPINE_SKILL;
            //2. 이동 (targetLists[0] 을 기준으로 이동)
            if (atkRange == ATTACKRANGE.NEAR_ATTACKRANGE)
            {
                var targets = targetLists[0];
                if (targets.Count == 1)
                {
                    target_position = mSpineController.GetAnimationRelativePosition(animationType, targets[0].transform.position);
                }
                else
                {
                    target_position = mSpineController.GetAnimationRelativePosition(animationType, BattleCommons.GetCenterPosition(targets));
                }

                mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_RUN, true);
                yield return StartCoroutine(MoveToPos(target_position, 1));
            }

            if(mStatus.ContainsBuff(EFFECT.BLOODY_EFFECT))
            {
                yield return StartCoroutine(mStatus.ExecuteBuff(BUFFTIME.ATTACK, EFFECT.BLOODY_EFFECT));
                if(mSpineController.CurAnimationType == SPINE_ANIMATION_TYPE.SPINE_DIE)
                {
                    yield break;
                }
                mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_IDLE, true);
            }

            
            
            //3. 공격
            // 임시 :
            SetFlipX(flip_ori);
            mSpineController.SetAnimation(animationType, false);
            mSpineEventHandler.AddEventNoti(animationType,
                () =>
                {
                    for (int i = 0; i < targetLists.Count; ++i)
                    {
                        foreach (var item in targetLists[i])
                        {
                        /// TODO : 데미지
                            if(packets[i].eID == (int)EFFECT.DAMAGE_EFFECT)
                            {
                                item.mStatus.GetDamage(packets[i].eValue + counterDamage);
                            }
                            else if(packets[i].eID == (int)EFFECT.HEAL_EFFECT)
                            {
                                item.mStatus.GetHeal(item.mStatus.MAXHP*packets[i].eValue/100);
                            }
                            else
                            {
                                item.mStatus.AddBuff(packets[i]);
                            }
                        }
                    }
                });

            yield return new WaitForSeconds(mSpineController.GetAnimationLength(animationType));
            
            

            // 돌아오기
            if (atkRange == ATTACKRANGE.NEAR_ATTACKRANGE)
            {
                mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_RUN, true);

                yield return StartCoroutine(MoveToPos(pos_origin, 1));

                SetFlipX(flip_ori);
            }
            mSpineController.SetDrawOrder(oldLayerOrder);
            mSpineController.SetAnimation(SPINE_ANIMATION_TYPE.SPINE_IDLE, true);
        }

        public void SetTarget(SKILLTYPE skillType,ref List<BattlePacket> packets, ref List<List<Unit>> targetLists, out ATTACKRANGE range)
        {
            int skillID = skillType == SKILLTYPE.SKILL_TYPE ? mStatus.SkillID : mStatus.AtkID;
            
            var skillData = mStatus.camp == CHARACTER_CAMP.PLAYER ? GameDataBase.Instance.SkillTable[skillID] : GameDataBase.Instance.MonsterSkillTable[skillID];
            range = skillData.ARange;

            /// ???? 스킬효과 1,2,3 대상이 같아야하나?
            if (skillData.Target1 != 0)
            {
                if (skillData.TargetObj1 == TARGETOBJECT.CASTER_TARGETOBJ)
                {
                    var targets = new List<Unit>();
                    targets.Add(this);
                    targetLists.Add(targets);
                }
                else
                {
                    var targets = CharacterManager.instance.FindTarget(this, (EFFECT)skillData.ISkillEffectID1 ,skillData.Target1, skillData.TargetObj1, skillData.TargetNumber1);
                    targetLists.Add(targets);
                }
                packets.Add(new BattlePacket(this, skillID, skillData.ISkillEffectID1, skillData.ISkillEffectTurn1, skillData.ISkillEffectValue1));
            }

            if (skillData.Target2 != 0)
            {
                if (skillData.TargetObj2 == TARGETOBJECT.CASTER_TARGETOBJ)
                {
                    var targets = new List<Unit>();
                    targets.Add(this);
                    targetLists.Add(targets);
                }
                else
                {
                    List<Unit> targets;
                    if(skillData.TargetObj2 == skillData.TargetObj1)
                    {
                        targets = targetLists[0];
                    }
                    else
                    {
                        targets = CharacterManager.instance.FindTarget(this, (EFFECT)skillData.ISkillEffectID1, skillData.Target2, skillData.TargetObj2, skillData.TargetNumber2);
                    }
                    targetLists.Add(targets);
                }
                packets.Add(new BattlePacket(this, skillID, skillData.ISkillEffectID2, skillData.ISkillEffectTurn2, skillData.ISkillEffectValue2));
            }

            if (skillData.Target3 != 0)
            {
                if (skillData.TargetObj3 == TARGETOBJECT.CASTER_TARGETOBJ)
                {
                    var targets = new List<Unit>();
                    targets.Add(this);
                    targetLists.Add(targets);
                }
                else
                {
                    List<Unit> targets;
                    if (skillData.TargetObj2 == skillData.TargetObj1)
                    {
                        targets = targetLists[0];
                    }
                    else
                    {
                        targets = CharacterManager.instance.FindTarget(this, (EFFECT)skillData.ISkillEffectID1, skillData.Target3, skillData.TargetObj3, skillData.TargetNumber3);
                    }
                    targetLists.Add(targets);
                }
                packets.Add(new BattlePacket(this, skillID, skillData.ISkillEffectID3, skillData.ISkillEffectTurn3, skillData.ISkillEffectValue3));
            }
        }



        public IEnumerator MoveToPos(Vector3 targetPos, float targetSeconds)
        {
            Vector3 target_position = targetPos;

            // 랜더링 안겹치도록
            target_position.z -= 0.1f;

            Vector3 pos_origin = transform.position;
            Vector3 dir = targetPos - pos_origin;
            dir.Normalize();

            if (dir.x > 0) SetFlipX(true);
            else SetFlipX(false);

            float deltaTime = 0;

            while (deltaTime < targetSeconds)
            {
                deltaTime += Time.deltaTime;
                transform.position = Vector3.Lerp(pos_origin, targetPos, deltaTime / targetSeconds);
                yield return new WaitForEndOfFrame();
            }

            transform.position = targetPos;
            
        }
        #endregion
    }

}
