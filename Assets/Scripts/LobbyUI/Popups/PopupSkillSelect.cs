using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class PopupSkillSelect : PopupController
{
    #region inspector
    public List<EquipSkill> EquipSkills;
    public Button ApplyBtn;
    public Transform GridSpace;
    public List<GridUnitController> InvenSkills = new List<GridUnitController>();
    #endregion

    public override void Load()
    {
        ClearGrid();
        var inventory = UIDataProcess.GetSkillInventory();

        for (int i = 0; i < 3; ++i)
        {
            var playerSkill = inventory.playerEquipSkills[i];
            if (playerSkill != null && playerSkill.iIndex != 0)
            {
                EquipSkills[i].image.enabled = true;
                var skillInfo = UIDataProcess.GetPlayerSkillInfo(playerSkill.iIndex, i);
                EquipSkills[i].image.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillInfo.StrSkillIcon.Replace("[SkillID]", playerSkill.iIndex.ToString()));
                EquipSkills[i].button.onClick.AddListener(
                    () =>
                    {
                        UIManager.instance.Popup("Popup_EquipSkill", playerSkill);
                    });
            }
            else
            {
                EquipSkills[i].image.enabled = false;
                EquipSkills[i].button.onClick.RemoveAllListeners();
            }
        }

        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab("GridUnit_InvenSkill");
        if (gridUnitPrefab != null)
        {
            for (int i = 0; i < inventory.SkillList.Count; ++i)
            {
                var skillInfo = UIDataProcess.GetPlayerSkillInfo(inventory.SkillList[i].iIndex, inventory.SkillList[i].IEquipmentIndex);

                if (skillInfo == null)
                {
                    Debug.Log("GridUnit_InvenSkill " + i + " missing!");
                    continue;
                }

                GameObject gridUnit = GameObject.Instantiate(gridUnitPrefab, GridSpace);
                gridUnit.name = "GridUnit_InvenSkill" + i;

                var controller = gridUnit.GetComponent<GridUnitController>();
                InvenSkills.Add(controller);
                controller.Setup(inventory.SkillList[i]);
            }
            UICommon.FitGridSize(GridSpace, InvenSkills.Count);
        }
        else Debug.Log("GridUnitPrefab is Missing! name : GridUnit_InvenSkill");
    }

    public override void Setup<T>(T t)
    {
        ClearGrid();
        var inventory = UIDataProcess.GetSkillInventory();

        for (int i = 0; i < 3; ++i)
        {
            var playerSkill = inventory.playerEquipSkills[i];
            if (playerSkill != null && playerSkill.iIndex != 0)
            {
                EquipSkills[i].image.enabled = true;
                var skillInfo = UIDataProcess.GetPlayerSkillInfo(playerSkill.iIndex, i);
                EquipSkills[i].image.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillInfo.StrSkillIcon.Replace("[SkillID]", playerSkill.iIndex.ToString()));
                EquipSkills[i].button.onClick.AddListener(
                    () =>
                    {
                        UIManager.instance.Popup("Popup_EquipSkill", playerSkill);
                    });
            }
            else
            {
                EquipSkills[i].image.enabled = false;
                EquipSkills[i].button.onClick.RemoveAllListeners();
            }
        }

        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab("GridUnit_InvenSkill");
        if (gridUnitPrefab != null)
        {
            for (int i = 0; i < inventory.SkillList.Count; ++i)
            {
                var skillInfo = UIDataProcess.GetPlayerSkillInfo(inventory.SkillList[i].iIndex, inventory.SkillList[i].IEquipmentIndex);

                if (skillInfo == null)
                {
                    Debug.Log("GridUnit_InvenSkill " + i + " missing!");
                    continue;
                }

                GameObject gridUnit = GameObject.Instantiate(gridUnitPrefab, GridSpace);
                gridUnit.name = "GridUnit_InvenSkill" + i;

                var controller = gridUnit.GetComponent<GridUnitController>();
                InvenSkills.Add(controller);
                controller.Setup(inventory.SkillList[i]);
            }
            UICommon.FitGridSize(GridSpace, InvenSkills.Count);
        }
        else Debug.Log("GridUnitPrefab is Missing! name : GridUnit_InvenSkill");

        ApplyBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });
    }

    public void ClearGrid()
    {
        foreach (var unit in InvenSkills)
        {
            Destroy(unit.gameObject);
        }
        InvenSkills.Clear();
    }
}
