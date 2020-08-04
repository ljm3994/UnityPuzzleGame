using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICommons;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager instance;

    #region inspector
    public Canvas PopupCanvas;
    [Space]
    public BattleUIController BattleUI;
    public CenteralUIController CentralUI;
    #endregion

    Stack<PopupController> PopupStack;
    public Dictionary<string, GameObject> PopupPrefabs;

    private void Awake()
    {
        instance = this;
        PopupStack = new Stack<PopupController>();
        PopupPrefabs = new Dictionary<string, GameObject>();
    }

    public void Popup<T>(string PopupName, T tData)
    {
        GameObject prefab;
        if (!PopupPrefabs.TryGetValue(PopupName, out prefab))
        {
            prefab = (GameObject)Resources.Load(UICommon.BattlePopupPath + PopupName, typeof(GameObject));
            if (prefab == null) Debug.Log("Popup Prefab Path missing! name : " + PopupName);
            else PopupPrefabs.Add(PopupName, prefab);
        }

        GameObject popObj = (GameObject)GameObject.Instantiate(prefab, PopupCanvas.transform);
        PopupController controller = popObj.GetComponent<PopupController>();

        controller.Setup(tData);

        PopupStack.Push(controller);
    }

    public void ClearAllPopup()
    {
        foreach (var popup in PopupStack)
        {
            Destroy(popup.gameObject);
        }
        PopupStack.Clear();
    }

    public void ClearTopPopup()
    {
        var popup = PopupStack.Pop();
        Destroy(popup.gameObject);
    }
}
