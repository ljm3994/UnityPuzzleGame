using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace BattleUnit
{
    public class UnitSpineEventHandler : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private Unit mUnit;
        [SerializeField]
        private SkeletonAnimation mSkeletonAnimation;

        [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string eAttack;
        [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string eSkill;

        [Space]
        public bool ConsoleDebug = true;
        #endregion

        Unit unitBase;
        UnitSpineController mSpineController;

        public delegate void AnimNoti();
        Dictionary<SPINE_ANIMATION_TYPE, AnimNoti> mAnimEndActions;
        Dictionary<SPINE_ANIMATION_TYPE, AnimNoti> mAnimEventActions;

        public void AddEndAction(SPINE_ANIMATION_TYPE animType, AnimNoti animNoti)
        {
            if (mAnimEndActions == null) mAnimEndActions = new Dictionary<SPINE_ANIMATION_TYPE, AnimNoti>();

            AnimNoti noti;
            if (mAnimEndActions.TryGetValue(animType, out noti))
            {
                bool isIn = false;
                foreach (var n in noti.GetInvocationList())
                {
                    if (n.Equals(animNoti))
                    {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn) noti += animNoti;
            }
            else
            {
                mAnimEndActions.Add(animType, animNoti);
            }
        }

        public void AddEventNoti(SPINE_ANIMATION_TYPE animType, AnimNoti animNoti)
        {
            if (mAnimEventActions == null) mAnimEventActions = new Dictionary<SPINE_ANIMATION_TYPE, AnimNoti>();

            AnimNoti noti;
            if (mAnimEventActions.TryGetValue(animType, out noti))
            {
                bool isIn = false;
                foreach (var n in noti.GetInvocationList())
                {
                    if (n.Equals(animNoti))
                    {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn) noti += animNoti;
            }
            else
            {
                mAnimEventActions.Add(animType, animNoti);
            }
        }


        public void Setup(Unit unit)
        {
            unitBase = unit;
            mSpineController = GetComponent<UnitSpineController>();

            mAnimEndActions = new Dictionary<SPINE_ANIMATION_TYPE, AnimNoti>();
            mAnimEventActions = new Dictionary<SPINE_ANIMATION_TYPE, AnimNoti>();

            mSkeletonAnimation.AnimationState.Event += Event_Handler;
            mSkeletonAnimation.AnimationState.Complete += Complete_Handler;
        }


        private void Complete_Handler(Spine.TrackEntry trackEntry)
        {
            AnimNoti noti;
            if (mAnimEndActions.TryGetValue(mSpineController.CurAnimationType, out noti))
            {
                mAnimEndActions.Remove(mSpineController.CurAnimationType);

                noti.Invoke();
            }
        }

        private void Event_Handler(Spine.TrackEntry trackEntry, Spine.Event e)
        {
            AnimNoti noti;
            if (mAnimEventActions.TryGetValue(mSpineController.CurAnimationType, out noti))
            {
                mAnimEventActions.Remove(mSpineController.CurAnimationType);

                noti.Invoke();
            }
        }
    }

}

