using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupController : MonoBehaviour
{
    public abstract void Setup<T>(T t);

    public abstract void Load();
}
