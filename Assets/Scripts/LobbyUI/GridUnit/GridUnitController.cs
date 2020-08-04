using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public abstract class GridUnitController : MonoBehaviour
{
    public GameObject Cover;
    public Button button;
    public bool isCovered;
    public bool bCover
    {
        get => isCovered;
        set
        {
            isCovered = value;
            if (Cover != null)
            {
                Cover.SetActive(isCovered);
                button.interactable = !isCovered;
            }
        }
    }


    public abstract void Setup<T>(T t);
}
