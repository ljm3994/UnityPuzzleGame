using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Dictionary<string, GameObject> PopupPrefabs;
    public Dictionary<string, GameObject> GridUnitPrefabs;

    public Dictionary<string, PanelController> Panels;

    public Stack<PopupController> PopupStack;

    public PanelController currentPanel;
    
    private void Awake()
    {
        instance = this;
        Panels = new Dictionary<string, PanelController>();
        PopupPrefabs = new Dictionary<string, GameObject>();
        GridUnitPrefabs = new Dictionary<string, GameObject>();
        PopupStack = new Stack<PopupController>();
    }

    public void Update()
    {
        if (!NetworkCanvas.Instance.NetWorkError)
        {
            BackButton();
        }
    }
    public void BackButton()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentPanel.name != "MainPanel")
                {
                    if (PopupStack.Count > 0)
                    {
                        CloseTopPopup();
                    }
                    else
                    {
                        SwapPanel(currentPanel.transform, "MainPanel");
                    }
                }
                else
                {
                    Popup<int>("Popup_ExitPopup", 0);
                }
            }
        }
    }
    public void CloseTopPopup()
    {
        var topPopup = PopupStack.Pop();
        Destroy(topPopup.gameObject);

        if(PopupStack.Count == 0)
        {
            currentPanel.Load();
        }
        else
        {
            PopupStack.Peek().Load();
        }
    }

    public void CloseAllPopup()
    {
        foreach (var popup in PopupStack)
        {
            Destroy(popup.gameObject);
        }

        PopupStack.Clear();

        currentPanel.Load();
    }

    public void CloseCountPopup(int Count)
    {
        int ThisCount = Count;
        if (PopupStack.Count < Count)
        {
            ThisCount = PopupStack.Count;
        }

        for(int i = 0; i < ThisCount; i++)
        {
            var topPopup = PopupStack.Pop();
            Destroy(topPopup.gameObject);
        }

        if (PopupStack.Count == 0)
        {
            currentPanel.Load();
        }
        else
        {
            PopupStack.Peek().Load();
        }
    }

    public void SwapPanel(Transform swapFrom, string swapTo)
    {
        PanelController targetPanel;
        if (Panels.TryGetValue(swapTo, out targetPanel))
        {
            currentPanel = targetPanel;

            Vector3 temp = swapFrom.position;
            swapFrom.position = targetPanel.transform.position;
            targetPanel.transform.position = temp;

            targetPanel.Load();
        }
    }

    public void Popup<T>(string PopupName,T tData)
    {
        GameObject prefab;
        if (!PopupPrefabs.TryGetValue(PopupName, out prefab))
        {
            prefab = (GameObject)Resources.Load(UICommon.PopupPath + PopupName, typeof(GameObject));
            if (prefab == null) Debug.Log("Popup Prefab Path missing! name : " + PopupName);
            else PopupPrefabs.Add(PopupName, prefab);
        }
        else
        {
            if (PopupStack.Count > 0)
            {
                if (PopupStack.Peek().gameObject.name == prefab.name + "(Clone)")
                {
                    return;
                }
            }
        }
        GameObject popObj = (GameObject)GameObject.Instantiate(prefab, currentPanel.transform);
        PopupController controller = popObj.GetComponent<PopupController>();

        controller.Setup(tData);

        PopupStack.Push(controller);
    }

    public GameObject GetGridUnitPrefab(string unitName)
    {
        GameObject prefab;
        if(!GridUnitPrefabs.TryGetValue(unitName,out prefab))
        {
            prefab = (GameObject)Resources.Load(UICommon.GridUnitPath + unitName, typeof(GameObject));
            if (prefab == null) Debug.Log("GridUnit Prefab Path missing! name : " + unitName);
            else GridUnitPrefabs.Add(unitName, prefab);
        }

        return prefab;
    }
}
