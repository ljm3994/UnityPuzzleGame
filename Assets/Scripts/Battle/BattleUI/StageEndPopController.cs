using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;


public class StageEndPopController : PopupController
{
    [System.Serializable]
    public class ExpUnits
    {
        public Transform panel;
        public Image iMain;
        public GaugeBarController expBar;
        public Text tName;
        public Text tLv;
        public Text tExpInfo;
    }

    #region Inspector
    public List<ExpUnits> unitPanels;

    public Transform GridSpace;
    public List<InvenItemController> rewardItems;

    public Button backgourndBtn;
    #endregion


    public override void Setup<T>(T t)
    {
        
        backgourndBtn.onClick.AddListener(
               () =>
               {
                   BattleDataManager.instance.moveTotalRewards();
                   BattleUIManager.instance.ClearAllPopup();
                   CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.PLAYER_UNIT_WALKOUT, () => { BattleManager.instance.ChangeState(EBattleState.STAGE_IN); });
               });

        float dropEXP = GameManager.instance.CurentStage.IStageDropEXP;
        for (int i = 0; i < unitPanels.Count; ++i)
        {
            var invenData = PlayerDataManager.PlayerData.UnitInventory.EquipmentUnit[i];
            if (invenData == null || invenData.iIndex == 0)
            {
                Destroy(unitPanels[i].panel.gameObject);
                continue;
            }

            var unitPanel = unitPanels[i];

            var unitData = GameDataBase.Instance.UnitTable[invenData.iIndex];
            int nextLevel = Mathf.Clamp(invenData.iLevel + 1, 0, unitData.iMaxLevel);
            unitPanel.expBar.Max = GameDataBase.Instance.UnitExpTable[nextLevel].INeedEXP;
            unitPanel.expBar.Value = invenData.IExp;
            StartCoroutine(unitPanel.expBar.IncreaseValue(dropEXP, 1));

            unitPanel.iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            unitPanel.tExpInfo.text = invenData.IExp.ToString() + " => " + Mathf.Clamp(invenData.IExp + dropEXP, 0, unitPanel.expBar.Max).ToString();
            invenData.IExp += (int)dropEXP;
            unitPanel.tLv.text = "Lv." + invenData.iLevel.ToString();
            unitPanel.tName.text = unitData.strName;
        }

        string name = "GridUnit_Item_Consume";
        GameObject gridUnitPrefab = (GameObject)Resources.Load(UICommon.GridUnitPath + name, typeof(GameObject));
        if (gridUnitPrefab != null)
        {
            var rewardList = BattleDataManager.instance.stageRewardItems;
            foreach (var item in rewardList)
            {
                var itemInfo = UIDataProcess.GetItemInfo(item.Key);

                GameObject gridUnit = GameObject.Instantiate(gridUnitPrefab, GridSpace);
                gridUnit.name = itemInfo.StrItemName;

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

        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(5);

        BattleDataManager.instance.moveTotalRewards();
        BattleUIManager.instance.ClearAllPopup();
        CharacterManager.instance.ChangeState(CharacterManager.CHARACTER_STATE.PLAYER_UNIT_WALKOUT, () => { BattleManager.instance.ChangeState(EBattleState.STAGE_IN); });
    }

    public override void Load()
    {
        
    }
}
