using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class PopupUnitSelect : PopupController
{
    #region inspector
    public Button ApplyBtn;

    public List<EquipUnit> EquipUnits;

    public Transform GridSpace;
    public List<GridUnitController> InvenUnits = new List<GridUnitController>();
    #endregion

    bool selfdestroy = false;
    public override void Setup<T>(T t)
    {
        selfdestroy = false;
        ClearGrid();

        var Inventory = UIDataProcess.GetUnitInventory();

        for (int i = 0; i < EquipUnits.Count; ++i)
        {
            var playerUnit = Inventory.EquipmentUnit[i];
            if (playerUnit != null && playerUnit.iIndex != 0)
            {
                EquipUnits[i].iMain.enabled = true;
                var unitData = UIDataProcess.GetUnitInfo(playerUnit.iIndex);
                EquipUnits[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
                switch (unitData.Position)
                {
                    case UNITPOSITION.TANKER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                    case UNITPOSITION.DEALER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                    case UNITPOSITION.SUPPORTER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
                }

                EquipUnits[i].tLv.text = playerUnit.iLevel.ToString();
                EquipUnits[i].button.onClick.AddListener(() => { UIManager.instance.Popup("Popup_EquipUnit", playerUnit); });
            }
            else
            {
                EquipUnits[i].iMain.enabled = false;
                EquipUnits[i].iPosition.sprite = null;
                EquipUnits[i].tLv.text = "";
                EquipUnits[i].button.onClick.RemoveAllListeners();
            }
        }

        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab("GridUnit_InvenUnit");
        if (gridUnitPrefab != null)
        {
            for (int i = 0; i < Inventory.UintList.Count; i++)
            {
                var playerUnit = Inventory.UintList[i];
                if (playerUnit != null)
                {
                    var unitInfo = UIDataProcess.GetUnitInfo(playerUnit.iIndex);

                    if (unitInfo == null)
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
            UICommon.FitGridSize(GridSpace, InvenUnits.Count);
        }
        else Debug.Log("GridUnitPrefab is Missing! name : GridUnit_InvenUnit");

        ApplyBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });
    }

    public void ClearGrid()
    {
        foreach (var unit in InvenUnits)
        {
            Destroy(unit.gameObject);
        }
        InvenUnits.Clear();
    }

    public override void Load()
    {
        ClearGrid();

        var Inventory = UIDataProcess.GetUnitInventory();

        for (int i = 0; i < EquipUnits.Count; ++i)
        {
            var playerUnit = Inventory.EquipmentUnit[i];
            if (playerUnit != null && playerUnit.iIndex != 0)
            {
                EquipUnits[i].iMain.enabled = true;
                var unitData = UIDataProcess.GetUnitInfo(playerUnit.iIndex);
                EquipUnits[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
                switch (unitData.Position)
                {
                    case UNITPOSITION.TANKER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                    case UNITPOSITION.DEALER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                    case UNITPOSITION.SUPPORTER_POSITION: EquipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
                }

                EquipUnits[i].tLv.text = playerUnit.iLevel.ToString();
                EquipUnits[i].button.onClick.AddListener(() => { UIManager.instance.Popup("Popup_EquipUnit", playerUnit); });
            }
            else
            {
                EquipUnits[i].iMain.enabled = false;
                EquipUnits[i].iPosition.sprite = null;
                EquipUnits[i].tLv.text = "";
                EquipUnits[i].button.onClick.RemoveAllListeners();
            }
        }

        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab("GridUnit_InvenUnit");
        if (gridUnitPrefab != null)
        {
            for (int i = 0; i < Inventory.UintList.Count; i++)
            {
                var playerUnit = Inventory.UintList[i];
                if (playerUnit != null)
                {
                    var unitInfo = UIDataProcess.GetUnitInfo(playerUnit.iIndex);

                    if (unitInfo == null)
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
            UICommon.FitGridSize(GridSpace, InvenUnits.Count);
        }
        else Debug.Log("GridUnitPrefab is Missing! name : GridUnit_InvenUnit");

        ApplyBtn.onClick.RemoveAllListeners();
        ApplyBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });
    }
}
