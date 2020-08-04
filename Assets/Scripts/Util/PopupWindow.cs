using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowButton
{
    private string strText;
    private System.Action ClickEvent;
    /// <summary>
    /// 버튼의 이름과 이벤트를 등록한다.
    /// </summary>
    /// <param name="text">버튼 내용</param>
    /// <param name="Event">클릭시 연결할 이벤트 함수</param>
    public WindowButton(string text, System.Action Event)
    {
        StrText = text;
        // 만약 이벤트가 널값이 온다면 해당 함수의 내용을 정의 해준다.
        ClickEvent1 = Event == null ? () => { } : Event;
    }

    public string StrText { get => strText; set => strText = value; }
    public Action ClickEvent1 { get => ClickEvent; set => ClickEvent = value; }
}
public class PopupWindow
{
    private Transform Target;
    PopupForm popupForm;
    private string _strTitle;
    private string _strDesc;
    private List<WindowButton> ListButton;
    /// <summary>
    /// 생성자 위치 정보를 인자로 받는다.
    /// </summary>
    /// <param name="transform">부모의 위치 정보</param>
    public PopupWindow(Transform transform)
    {
        Target = transform;
        StrTitle = "";
        StrDesc = "";
        ListButton = new List<WindowButton>();
    }
    /// <summary>
    /// 팝업창 타이틀
    /// </summary>
    public string StrTitle { get => _strTitle; set => _strTitle = value; }
    /// <summary>
    /// 팝업창 안에 설명
    /// </summary>
    public string StrDesc { get => _strDesc; set => _strDesc = value; }

    /// <summary>
    /// 팝업창을 생성하는 함수
    /// </summary>
    public void Bulid()
    {
        //리소스의 경로에서 해당 프리팹을 가져온다.
        GameObject popobject = GameObject.Instantiate(Resources.Load("Fbx/" + "Popup", typeof(GameObject))) as GameObject;
        //해당 오브젝트의 부모설정
        popobject.transform.SetParent(Target, false);
        popupForm = popobject.GetComponent<PopupForm>();
        popobject.transform.SetAsLastSibling();
        popupForm.TextTitle = StrTitle;
        popupForm.TextDesc = StrDesc;
        popupForm.SetButton(ListButton);
    }
    public void Bulid(Transform DataTrans, string PrefabPath)
    {
    }
    public void DestroyForm()
    {
        popupForm.DestroyForm();
    }

    public void SetDesc(string text)
    {
        popupForm.TextDesc = text;
    }

    public void SetTitle(string Text)
    {
        popupForm.TextTitle = Text;
    }
    /// <summary>
    /// 버튼을 추가하는 함수
    /// </summary>
    /// <param name="Text">버튼의 텍스트 내용</param>
    /// <param name="Event">버튼 클릭시 발생시킬 이벤트 함수</param>
    public void SetButton(string Text, Action Event = null)
    {
        ListButton.Add(new WindowButton(Text, Event));
    }
}
