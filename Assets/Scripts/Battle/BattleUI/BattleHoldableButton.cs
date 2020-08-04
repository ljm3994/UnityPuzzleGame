using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleHoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool ClickActive;
    public bool HoldActive;

    public float mMinHoldTime;
    public bool bHoldDown;
    public bool bInvoked;
    public float mHoldDownTimer;

    public delegate void HoldAction();
    public delegate void HoldOutAction();
    public delegate void ClickAction();

    public HoldAction holdAction;
    public HoldOutAction holdOutAction;
    public ClickAction clickAction;

    
    public void ResetHold()
    {
        bHoldDown = false;
        bInvoked = false;
        mHoldDownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (bHoldDown)
        {
            mHoldDownTimer += Time.deltaTime;
            if (mHoldDownTimer > mMinHoldTime)
            {
                if (bInvoked == false)
                {
                    if(holdAction != null && HoldActive)
                        holdAction.Invoke();

                    bInvoked = true;
                }
            }
        }
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        bHoldDown = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (mHoldDownTimer < mMinHoldTime && ClickActive)
        {
            if (clickAction != null) clickAction.Invoke();
        }
        else if (HoldActive)
        {
            if (holdOutAction != null) holdOutAction.Invoke();
        }
        ResetHold();
    }
}
