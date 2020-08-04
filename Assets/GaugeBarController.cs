using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GaugeBarController : MonoBehaviour
{
    #region inspector
    public bool fillXdir;
    public bool showText;

    public RawImage mMainFrontBar;
    public RawImage mBackBar;
    public RawImage mSubFrontBar;

    public Text guideText;

    [SerializeField]
    [Range(0, 1)]
    float mFillPercent;

    #endregion

    float maxValue;
    public float Max
    {
        get => maxValue;
        set
        {
            maxValue = value;
            SetGaugePercent(curValue / maxValue);
        }
    }
    float curValue;
    public float Value
    {
        get => curValue;
        set
        {
            curValue = Mathf.Clamp(value,0,maxValue);
            SetGaugePercent(curValue / maxValue);
        } }
    

    void Update()
    {
        SetGaugePercent(mFillPercent);
    }

    public void SetGaugePercent(float percent)
    {
        mFillPercent = Mathf.Clamp01(percent);
        RectTransform front_rt = mMainFrontBar.GetComponent<RectTransform>();

        if (fillXdir) front_rt.anchorMax = new Vector2(mFillPercent, front_rt.anchorMax.y);
        else front_rt.anchorMax = new Vector2(front_rt.anchorMax.x, mFillPercent);

        if (showText) guideText.text = (int)(mFillPercent*100) + "/100";
    }

    public void SetAlpha(float alpha)
    {
        Color c;
        c = mMainFrontBar.color;
        c.a = alpha;
        mMainFrontBar.color = c;

        c = mSubFrontBar.color;
        c.a = alpha;
        mSubFrontBar.color = c;

        c = mBackBar.color;
        c.a = alpha;
        mBackBar.color = c;
    }

    public IEnumerator IncreaseValue(float val,float time)
    {
        float deltaTime = 0;
        float targetValue = Mathf.Clamp(curValue + val, 0, maxValue);
        while(time > deltaTime)
        {
            deltaTime += Time.deltaTime;

            Value = Mathf.Lerp(curValue, targetValue, deltaTime / time);

            yield return null;
        }

        Value = targetValue;
    }
}
