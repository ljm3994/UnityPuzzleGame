using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class Spine_Controller : MonoBehaviour
{
    #region Inspector - Animation
    public bool ConsoleDebug = false;
    public enum SPINE_ANIMATION_TYPE
    {
        SPINE_IDLE,
        SPINE_RUN,
        SPINE_ATTACK,
        SPINE_SKILL,
        SPINE_HURT,
        SPINE_DIE,
    }

    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string runAnimationName;

    [SpineAnimation]
    public string attackAnimationName;

    [SpineAnimation]
    public string hurtAnimationName;

    [SpineAnimation]
    public string dieAnimationName;

    [SpineAnimation]
    public string skillAnimationName;

    #endregion

    SkeletonAnimation mSkeletonAnimation;

    public Spine.AnimationState mSpineAnimationState;
    public Spine.Skeleton mSkeleton;
    SPINE_ANIMATION_TYPE mCurAnimationType;
    public SPINE_ANIMATION_TYPE CurAnimationType { get => mCurAnimationType; }

    private void Awake()
    {
        mSkeletonAnimation = GetComponent<SkeletonAnimation>();
        mSpineAnimationState = mSkeletonAnimation.AnimationState;
        mSkeleton = mSkeletonAnimation.skeleton;

        mSpineAnimationState.Data.DefaultMix = 0.0f;

        SetAnimation(SPINE_ANIMATION_TYPE.SPINE_IDLE, true);
    }

    #region GETTER & SETTER
    public void SetAnimation(SPINE_ANIMATION_TYPE animation_type, bool bLoop)
    {
        Spine.TrackEntry trackEntry;
        switch (animation_type)
        {
            case SPINE_ANIMATION_TYPE.SPINE_IDLE:
                trackEntry = mSpineAnimationState.SetAnimation(0, idleAnimationName, bLoop);
                mCurAnimationType = SPINE_ANIMATION_TYPE.SPINE_IDLE;
                break;
            case SPINE_ANIMATION_TYPE.SPINE_RUN:
                trackEntry = mSpineAnimationState.SetAnimation(0, runAnimationName, bLoop);
                mCurAnimationType = SPINE_ANIMATION_TYPE.SPINE_RUN;
                break;
            case SPINE_ANIMATION_TYPE.SPINE_ATTACK:
                trackEntry = mSpineAnimationState.SetAnimation(0, attackAnimationName, bLoop);
                mCurAnimationType = SPINE_ANIMATION_TYPE.SPINE_ATTACK;
                break;
            case SPINE_ANIMATION_TYPE.SPINE_HURT:
                trackEntry = mSpineAnimationState.SetAnimation(0, hurtAnimationName, bLoop);
                mCurAnimationType = SPINE_ANIMATION_TYPE.SPINE_HURT;
                break;
            case SPINE_ANIMATION_TYPE.SPINE_DIE:
                trackEntry = mSpineAnimationState.SetAnimation(0, dieAnimationName, bLoop);
                mCurAnimationType = SPINE_ANIMATION_TYPE.SPINE_DIE;
                break;
            case SPINE_ANIMATION_TYPE.SPINE_SKILL:
                trackEntry = mSpineAnimationState.SetAnimation(0, skillAnimationName, bLoop);
                mCurAnimationType = SPINE_ANIMATION_TYPE.SPINE_SKILL;
                break;
            default:
                break;
        }
    }

    public float GetAnimationLength(SPINE_ANIMATION_TYPE animation_type)
    {
        switch (animation_type)
        {
            case SPINE_ANIMATION_TYPE.SPINE_IDLE: return mSkeleton.Data.FindAnimation(idleAnimationName).Duration;
            case SPINE_ANIMATION_TYPE.SPINE_RUN: return mSkeleton.Data.FindAnimation(runAnimationName).Duration;
            case SPINE_ANIMATION_TYPE.SPINE_ATTACK: return mSkeleton.Data.FindAnimation(attackAnimationName).Duration;
            case SPINE_ANIMATION_TYPE.SPINE_HURT: return mSkeleton.Data.FindAnimation(hurtAnimationName).Duration;
            case SPINE_ANIMATION_TYPE.SPINE_DIE: return mSkeleton.Data.FindAnimation(dieAnimationName).Duration;
            case SPINE_ANIMATION_TYPE.SPINE_SKILL: return mSkeleton.Data.FindAnimation(skillAnimationName).Duration;
            default: return 0;
        }

    }

    public Vector3 GetAnimationRelativePosition(SPINE_ANIMATION_TYPE animation_type, Vector3 target_position)
    {
        Vector3 animation_pos;
        switch (animation_type)
        {
            case SPINE_ANIMATION_TYPE.SPINE_ATTACK:
                animation_pos = transform.Find("AttackPosition").localPosition;
                break;
            case SPINE_ANIMATION_TYPE.SPINE_SKILL:
                animation_pos = transform.Find("SkillPosition").localPosition;
                break;
            case SPINE_ANIMATION_TYPE.SPINE_IDLE:
            case SPINE_ANIMATION_TYPE.SPINE_HURT:
            case SPINE_ANIMATION_TYPE.SPINE_DIE:
            case SPINE_ANIMATION_TYPE.SPINE_RUN:
            default:
                animation_pos = transform.position;
                break;
        }

        animation_pos *= transform.lossyScale.x;


        return target_position - animation_pos;
    }
    #endregion





    /*
     * 스파인 등장
     * TODO : 후에 Spawn 관련 효과로 대체
     */
    public IEnumerator SpineAppear()
    {
        while(mSkeleton.A < 1.0f)
        {
            if(mSkeleton.A > 1.0f)
            {
                mSkeleton.A = 1.0f;
                break;
            }

            mSkeleton.A += 0.03f;
            yield return new WaitForEndOfFrame();
        }

        mSkeleton.A = 1;
    }


}
