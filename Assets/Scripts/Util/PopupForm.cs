using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupForm : MonoBehaviour
{
    [SerializeField]
    private Text _TextTitle;
    [SerializeField]
    private Text _TextDesc;
    [SerializeField]
    private GameObject ObjectButton;
    [SerializeField]
    private GameObject ObjectButtonPrefabs;

    public string TextTitle { set => _TextTitle.text = value; }
    public string TextDesc { set => _TextDesc.text = value; }
    /// <summary>
    /// 팝업창에 버튼을 설정한다.
    /// </summary>
    /// <param name="list">설정할 버튼 리스트</param>
    public void SetButton(List<WindowButton> list)
    {
        foreach (var item in list)
        {
            GameObject buttonobject = Instantiate(ObjectButtonPrefabs);
            buttonobject.transform.SetParent(ObjectButton.transform, false);
            PopupButton popupButton = buttonobject.GetComponent<PopupButton>();

            popupButton.init(item.StrText, gameObject, item.ClickEvent1);
        }
    }
    
    public void DestroyForm()
    {
        Destroy(gameObject);
    }
}