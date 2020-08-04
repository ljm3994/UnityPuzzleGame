using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelController : MonoBehaviour
{
    private void Start()
    {
        Setup();
    }

    

    public virtual void Setup()
    {
        if(!UIManager.instance.Panels.ContainsKey(name))
            UIManager.instance.Panels.Add(name, this);
    }


    public virtual void Load()
    {
        ClearGrid();
    }

    public abstract void ClearGrid();
}
