using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoseFontController : MonoBehaviour
{
    #region inspector
    public StageLosePopup stageLosePopup;
    #endregion

    void OnAnimationEnd()
    {
        stageLosePopup.Setup(0);

        Destroy(gameObject);
    }
}
