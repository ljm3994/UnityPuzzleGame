using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using UICommons;
using UnityEngine.SceneManagement;
using BattleUnit;

public class StageFinishPopController : PopupController
{
    #region inpector
    [System.Serializable]
    public class VictoryUnits
    {
        public RectTransform panel;
        public GaugeBarController expBar;
        public RectTransform spineSpace;
    }



    public List<VictoryUnits> unitPanels;

    public Transform GridSpace;
    public List<InvenItemController> rewardItems;

    public Button goNextStage;
    public Button goLobby;
    #endregion

    public override void Setup<T>(T t)
    {
        goNextStage.onClick.AddListener(
               () => {
                   /// TODO : NEXT STAGE
                   NextStageUp();
                   SceneManager.LoadScene("BattleScene");
               });
        goLobby.onClick.AddListener(
            () => {
                /// TODO : GO LOBBY
                NextStageUp();
                SceneManager.LoadScene("Lobby");

            });

        BattleDataManager.instance.moveTotalRewards();

        for (int i = 0; i < unitPanels.Count; ++i)
        {
            var invenData = PlayerDataManager.PlayerData.UnitInventory.EquipmentUnit[i];
            if (invenData == null)
            {
                Destroy(unitPanels[i].panel.gameObject);
                continue;
            }


            var unitData = GameDataBase.Instance.UnitTable[invenData.iIndex];
            unitPanels[i].expBar.Max = GameDataBase.Instance.UnitExpTable[invenData.iLevel].INeedEXP;
            unitPanels[i].expBar.Value = invenData.IExp;
            string spinePath = UIDataProcess.UnitSpinePath + "UI/" + unitData.iID;
            var prefab = (GameObject)Resources.Load(spinePath);
            if (prefab != null)
            {
                var obj = GameObject.Instantiate(prefab, unitPanels[i].spineSpace);
                RectTransform tr = obj.GetComponent<RectTransform>();

                float scale = unitPanels[i].spineSpace.rect.height / tr.rect.height;
                tr.localScale = new Vector2(scale, scale);
            }
        }

        string name = "GridUnit_Item_Consume";
        GameObject gridUnitPrefab = (GameObject)Resources.Load(UICommon.GridUnitPath + name, typeof(GameObject));
        if (gridUnitPrefab != null)
        {
            var rewardList = BattleDataManager.instance.totalRewardItems;
            foreach (var item in rewardList)
            {
                var itemInfo = UIDataProcess.GetItemInfo(item.Key);

                GameObject gridUnit = GameObject.Instantiate(gridUnitPrefab, GridSpace);
                gridUnit.name = itemInfo.StrItemName;
                if (itemInfo.Type == ITEMTYPE.STUFF_TYPE)
                {
                    PlayerDataManager.PlayerData.InventoryETCItemData.AddItem(item.Key, item.Value);
                }
                else if(itemInfo.Type == ITEMTYPE.EXPENDABLES_TYPE)
                {
                    PlayerDataManager.PlayerData.PlayerItem.AddItem(item.Key, item.Value);
                }
                var controller = gridUnit.GetComponent<InvenItemController>();
                rewardItems.Add(controller);
                controller.Setup(itemInfo);

                {
                    controller.button.onClick.RemoveAllListeners();
                    controller.button.gameObject.SetActive(false);
                    controller.tNum.text = rewardList.Values.Count.ToString();
                }
            }
            UICommon.FitGridSize(GridSpace, rewardItems.Count);
        }
    }

    public void NextStageUp()
    {
        if (PlayerDataManager.PlayerData.Pdata.ICurrentTopFloor == 20)
        {
            PlayerDataManager.PlayerData.Pdata.ICurrentTopNum++;

            if (PlayerDataManager.PlayerData.Pdata.ICurrentTopNum > 3)
            {
                PlayerDataManager.PlayerData.Pdata.ICurrentTopNum = 3;
            }
            else
            {
                PlayerDataManager.PlayerData.Pdata.ICurrentTopFloor = 1;
            }
        }
        else
        {
            PlayerDataManager.PlayerData.Pdata.ICurrentTopFloor++;
        }
    }
    public override void Load()
    {
        
    }
}
