using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleUnit;

public class AnimationTest : MonoBehaviour
{
    [Space]
    public SPINE_ANIMATION_TYPE testAnimationType;

    List<UnitSpineController> spineControllers;

    // Start is called before the first frame update
    void Start()
    {
        spineControllers = new List<UnitSpineController>(GetComponentsInChildren<UnitSpineController>());
        foreach (var item in spineControllers)
        {
            item.Setup(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var spineController in spineControllers)
        {
            if (testAnimationType != spineController.CurAnimationType)
            {
                spineController.SetAnimation(testAnimationType, true);
            }
        }
    }
}
