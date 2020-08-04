using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuBtnPopupController : PopupController
{
    #region inspector
    public Text tStage;
    public Button backgbBtn;
    public Button backBtn;
    public Button lobbyBtn;
    #endregion

    public override void Setup<T>(T t)
    {
        int stageNum = GameManager.instance.CurentStage.IStageNumber;
        tStage.text = "Stage " + (stageNum / 20 + 1).ToString() + " - " + (stageNum % 20 > 10 ? stageNum.ToString() : "0" + (stageNum % 20).ToString());

        backgbBtn.onClick.AddListener(BattleUIManager.instance.ClearAllPopup);
        backBtn.onClick.AddListener(BattleUIManager.instance.ClearAllPopup);
        lobbyBtn.onClick.AddListener(
            () =>
            {
                /// TODO : GOTO LOBBY SCENE
                SceneManager.LoadScene("Lobby");
            });
    }

    public override void Load()
    {
        
    }
}
