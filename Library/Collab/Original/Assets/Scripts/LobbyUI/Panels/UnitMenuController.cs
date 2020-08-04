using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class UnitMenuController : PanelController
{
    #region inspector
    public TopGSBarController GSBar;

    public Button GoBackBtn;
    public Button SkillMenuBtn;
    public Button ItemMenuBtn;

    public List<EquipUnit> EquipUnits;

    public Transform GridSpace;
    public List<GridUnitController> InvenUnits = new List<GridUnitController>();
    #endregion

    public override void Setup()
    {
        base.Setup();
        SkillMenuBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "CharacterPanel"); });
        ItemMenuBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "ItemPanel"); });
        GoBackBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "MainPanel"); });

        Load();
    }

    public override void Load()
    {
        base.Load();
        GSBar.Load();

        var Inventory = UIDataProcess.GetUnitInventory();

        for(int i = 0; i< EquipUnits.Count; ++i)
        {
            var playerUnit = Inventory.EquipmentUnit[i];
            if(playerUnit != null)
            {
                var unitData = UIDataProcess.GetUnitInfo(playerUnit.iIndex);
                EquipUnits[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + unitData.StrUnitImage.Replace("[CharacterID]", playerUnit.iIndex.ToString()));
                switch (unitData.Position)
                {
                    case UNITPOSITION.TANKER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                    case UNITPOSITION.DEALER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                    case UNITPOSITION.SUPPORTER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
                }

                EquipUnits[i].tLv.text = playerUnit.iLevel.ToString();
                EquipUnits[i].button.onClick.AddListener(() => { UIManager.instance.Popup("Popup_EquipUnit", playerUnit); });
            }
        }

        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab("GridUnit_InvenUnit");
        if(gridUnitPrefab != null)
        {
            for (int i = 0; i < Inventory.UintList.Count; ++i)
            {
                var playerUnit = Inventory.UintList[i];
                if (playerUnit != null)
                {
                    var unitInfo = UIDataProcess.GetUnitInfo(playerUnit.iIndex);

                    if(unitInfo == null)
                    {
                        Debug.Log("GridUnit_InvenUnit " + i + " missing!");
                        continue;
                    }

                    GameObject gridUnit = GameObject.Instantiate(gridUnitPrefab, GridSpace);
                    gridUnit.name = "GridUnit_InvenUnit" + i;

                    var controller = gridUnit.GetComponent<GridUnitController>();
                    InvenUnits.Add(controller);
                    controller.Setup(playerUnit);
                }
            }
        }
        else Debug.Log("GridUnitPrefab is Missing! name : GridUnit_InvenUnit");
    }

    public override void ClearGrid()
    {
        foreach (var unit in InvenUnits)
        {
            Destroy(unit.gameObject);
        }
        InvenUnits.Clear();
    }
}
