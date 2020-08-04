using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UICommons;
public class NetworkCanvas : MonoBehaviour
{
    public bool NetWorkError;
    IEnumerator RetryCo;
    public static NetworkCanvas Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SetCamera();
        NetWorkError = false;
    }

    private void SetCamera()
    {
        float TargetWidthAspect = 9.0f;
        float TargetHeightAspect = 16.0f;

        Camera main = Camera.main;

        main.aspect = TargetWidthAspect / TargetHeightAspect;

        float WidthRatio = Screen.width / TargetWidthAspect;
        float HeightRatio = Screen.height / TargetHeightAspect;

        float HeightAdd = ((WidthRatio / (HeightRatio / 100)) - 100) / 200;
        float WidthAdd = ((HeightRatio / (WidthRatio / 100)) - 100) / 200;

        if (HeightRatio > WidthRatio)
        {
            WidthAdd = 0.0f;
        }
        else
        {
            HeightAdd = 0.0f;
        }

        main.rect = new Rect(main.rect.x + Mathf.Abs(WidthAdd), main.rect.y + Mathf.Abs(HeightAdd), main.rect.width + (WidthAdd * 2),
            main.rect.height + (HeightAdd * 2));
    }

    public void Popup()
    {
        GameObject prefab = (GameObject)Resources.Load(UICommon.PopupPath + "Popup_NetworkError", typeof(GameObject));
        
        GameObject popObj = (GameObject)GameObject.Instantiate(prefab, this.transform);
        PopupController controller = popObj.GetComponent<PopupController>();

        controller.Setup(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!NetWorkCheck() && !NetWorkError)
        {
            NetWorkError = true;
            Popup();
        }
    }

    bool NetWorkCheck()
    {
        NETWORK_TYPE _TYPE = NetworkProcess.GetNetWorkConnectType();

        if (_TYPE == NETWORK_TYPE.NOTCONNECTED_TYPE)
        {
            return false;
        }

        return true;
    }
}
