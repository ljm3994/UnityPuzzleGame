using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Stack<GameObject> RootObject;
    public Stack<UI_DATA.UI_PARENT> RootType;
    public GameObject CurrentObject;
    static public GameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        RootObject = new Stack<GameObject>();
        RootType = new Stack<UI_DATA.UI_PARENT>();
    }

    // Update is called once per frame
    void Update()
    {
        BackButton();
    }

    public void BackButton()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (RootObject != null && RootObject.Count > 0)
                {
                    Debug.Log("뒤로가기");
                    BackPannel(RootObject.Pop(), RootType.Pop());
                }      
            }
        }
    }

    public void BackPannel(GameObject target, UI_DATA.UI_PARENT Type)
    {
        switch (Type)
        {
            case UI_DATA.UI_PARENT.PANEL:
                {
                    Debug.Log("패널뒤로가기");
                    SwapPannel((target.GetComponent<UI_Panel>()).panelBack.gameObject);
                }
                break;
            case UI_DATA.UI_PARENT.POPUP:
                {
                    Debug.Log("팝업뒤로가기");
                    target.GetComponent<UI_Popup>().CloseRootPopup();
                }
                break;
        }
        // In Screen
        // Panel A => Panel B
        
    }

    public void SwapPannel(GameObject @object)
    {
        UI_Panel panelA = CurrentObject.GetComponent<UI_Panel>();
        panelA.panelBack = null;
        UI_Panel panelB = @object.GetComponent<UI_Panel>();

        Vector3 temp_pos;
        temp_pos = panelA.transform.localPosition;
        panelA.transform.localPosition = panelB.transform.localPosition;
        panelB.transform.localPosition = temp_pos;
        panelB.panelBack = panelA;
        GameManager.instance.CurrentObject = @object;
        UI_DATA.StartLoadHierarchy(panelB.transform);
    }
    public void SetBattleScene()
    {
        SceneManager.LoadScene("BattleScene");

    }
    
    public void SetLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
