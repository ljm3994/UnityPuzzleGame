using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupButton : MonoBehaviour
{
    [SerializeField]
    private Text TextButton;
    private System.Action ClickEvent;
    private GameObject @object;

    public void init(string text, GameObject target, System.Action Event)
    {
        TextButton.text = text;
        ClickEvent = Event;
        @object = target;
    }

    public void OnButton()
    {
        ClickEvent();
        Destroy(@object);
    }
}
