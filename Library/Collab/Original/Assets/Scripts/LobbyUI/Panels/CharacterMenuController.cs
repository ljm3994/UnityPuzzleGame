using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;


public class CharacterMenuController : PanelController
{
    #region inspector
    public TopGSBarController GSBar;

    public Button GoBackBtn;
    public Button UnitMenuBtn;
    public Button ItemMenuBtn;

    public List<EquipSkill> EquipSkills;

    public Transform GridSpace;
    public List<GridUnitController> InvenSkills = new List<GridUnitController>();

    #endregion


    public override void Setup()
    {
        base.Setup();
        UnitMenuBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "UnitPanel"); });
        ItemMenuBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "ItemPanel"); });
        GoBackBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "MainPanel"); });

        ClearGrid();
    }

    public override void Load()
    {
        base.Load();
        GSBar.Load();

        var inventory = UIDataProcess.GetSkillInventory();

        for (int i = 0; i < 3; ++i)
        {
            var playerSkill = inventory.playerEquipSkills[i];
            var skillInfo = UIDataProcess.GetPlayerSkillInfo(playerSkill.iIndex, i);
            EquipSkills[i].image.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillInfo.StrSkillIcon.Replace("[SkillID]", playerSkill.iIndex.ToString()));
            EquipSkills[i].button.onClick.AddListener(
                () => {
                    UIManager.instance.Popup("Popup_EquipSkill", playerSkill);
                });
        }

        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab("GridUnit_InvenSkill");
        if (gridUnitPrefab != null)
        {
            for (int i = 0; i < inventory.SkillList.Count; ++i)
            {
                var skillInfo = UIDataProcess.GetPlayerSkillInfo(inventory.SkillList[i].iIndex, inventory.SkillList[i].IEquipmentIndex);

                if(skillInfo == null)
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
        }
        else Debug.Log("GridUnitPrefab is Missing! name : GridUnit_InvenSkill");
    }

    public override void ClearGrid()
    {
        foreach (var unit in InvenSkills)
        {
            Destroy(unit.gameObject);
        }
        InvenSkills.Clear();
    }
}
