using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class SelectStageInfo
{
    public List<StageInfo> StageInfos;
    public int StageNum;
}
public class MainPanelController : PanelController
{
    [System.Serializable]
    public class TowerButton
    {
        public Button button;
        public Image iMain;
    }

    #region inspector

    public TopGSBarController topGSBar;
    public Image iWorldMap;
    public List<TowerButton> TowerButtons;
    public Button InfoMenuBtn;
    public Button ShopBtn;

    #endregion


    public override void Setup()
    {
        base.Setup();


        UIManager.instance.currentPanel = this;

        InfoMenuBtn.onClick.AddListener(()=> { UIManager.instance.SwapPanel(transform, "CharacterPanel"); });
        ShopBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "Shop_GoldStemina_Panel"); });

        int currentTower = PlayerDataManager.PlayerData.Pdata.ICurrentTopNum;

        iWorldMap.sprite = UICommon.LoadSprite(UIDataProcess.MainLobbyPath + "Image_WorldMap_" + currentTower);

        for (int i =0; i< TowerButtons.Count; ++i)
        {
            int towerNum = i + 1;

            if (towerNum <= currentTower)
            {
                if(towerNum < currentTower)
                {
                    TowerButtons[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.MainLobbyPath + "Image_StageTower_" + towerNum + "_Clear");
                }
                else
                {
                    TowerButtons[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.MainLobbyPath + "Image_StageTower_" + towerNum + "_NonClear");
                }

                TowerButtons[i].button.onClick.AddListener(
                   () => {
                    /// TODO:
                    /// 스테이지 진입방법?
                    SelectStageInfo selectStageInfo = new SelectStageInfo();
                       selectStageInfo.StageInfos = UIDataProcess.GetStageNumInfo(towerNum);
                       selectStageInfo.StageNum = towerNum;
                       UIManager.instance.Popup("StageEnter_Popup", selectStageInfo);
                   });
            }
            else
            {
                TowerButtons[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.MainLobbyPath + "Image_StageTower_" + towerNum + "_NonEnter");
                TowerButtons[i].button.interactable = false;
            }
                
        }

        Load();
    }
    
    public override void Load()
    {
        ClearGrid();
        topGSBar.Load();
        /* Nothing */
    }

    public override void ClearGrid()
    {
        /* Nothing */
    }
}
