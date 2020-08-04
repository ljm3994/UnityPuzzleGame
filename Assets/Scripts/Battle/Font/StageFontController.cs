using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageFontController : MonoBehaviour
{
    public Text text;
    public Animator animator;

    public delegate void StageFontFinishAction();
    StageFontFinishAction mStageFontEndAction;

    public void AddStageFontAction(StageFontFinishAction action)
    {
        mStageFontEndAction += action;
    }


    void OnAnimationEnd()
    {
        if(mStageFontEndAction != null)
            mStageFontEndAction.Invoke();

        Destroy(gameObject);
    }


    public void SetText(string s)
    {
        GetComponent<Text>().text = s;
    }
}
