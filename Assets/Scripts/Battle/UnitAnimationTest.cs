using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using BattleUnit;

public class UnitAnimationTest : MonoBehaviour
{

    [SerializeField]
    public UnitSpineController spineController;
    
    public float animationTime;

    #region TEST
    [Space]
    public SPINE_ANIMATION_TYPE testAnimationType;

    private void Start()
    {
        spineController.Setup(null);
    }

    public void Update()
    {
        if (testAnimationType != spineController.CurAnimationType)
        {
            spineController.SetAnimation(testAnimationType, true);
        }

        animationTime = spineController.GetCurAnimationTime();
    }
    #endregion


}
