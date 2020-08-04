using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NetWorkPopupControll : PopupController
{
    #region inspector
    public Button RetryBtn;
    public Button CancelBtn;
    public Text Desc;
    #endregion

    IEnumerator RetryCo;
    public override void Setup<T>(T t)
    {
        CancelBtn.onClick.AddListener(() => { Application.Quit(0); });
        RetryBtn.onClick.AddListener(() =>
        {
            RetryCo = RetryCorutin();
            StartCoroutine(RetryCo);
        });
    }

    IEnumerator RetryCorutin()
    {
        int WaitTimer = 5;
        RetryBtn.enabled = false;
        CancelBtn.enabled = false;
        while (WaitTimer > 0)
        {
            Desc.text = "네트워크 재연결 중입니다....(" + WaitTimer.ToString() + ")";

            if (NetWorkCheck())
            {
                SceneManager.LoadSceneAsync("Login");
                Destroy(gameObject);
                StopCoroutine(RetryCo);
                yield return null;
            }

            WaitTimer--;
            yield return new WaitForSeconds(1.0f);
        }

        RetryBtn.enabled = true;
        CancelBtn.enabled = true;
        Desc.text = "네트워크 연결이 끊겼습니다";
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

    public override void Load()
    {
        
    }
}
