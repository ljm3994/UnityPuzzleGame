using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPopControll : PopupController
{
    #region inspector
    public Button ExitBtn;
    public Button CancelBtn;

    #endregion

    public override void Setup<T>(T t)
    {
        CancelBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
        ExitBtn.onClick.AddListener(() =>
        {
            ExitBtn.enabled = false;
            CancelBtn.enabled = false;
            PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.USER_DATAFILE, (succed) => {
                if(succed)
                {
                    ExitBtn.enabled = true;
                    CancelBtn.enabled = true;
                    Application.Quit(0);
                }
 
            });
            
        });
    }

    public override void Load()
    {
        
    }
}
