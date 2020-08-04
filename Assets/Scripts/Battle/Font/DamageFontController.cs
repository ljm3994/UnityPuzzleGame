using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFontController : MonoBehaviour
{
    public Text text;
    public Animator animator;
    public GameObject canvas;

    void OnAnimationEnd()
    {
        Destroy(canvas);
    }

    public void SetText(string t)
    {
        text.text = t;
    }

    public void SetText(string t, Color color)
    {
        text.color = color;
        text.text = t;
    }

}
