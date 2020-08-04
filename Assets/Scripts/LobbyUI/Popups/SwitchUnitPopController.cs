using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class SwitchUnitPopController : PopupController
{
    #region inspector
    [Space]
    public List<EquipUnit> equipUnits;
    public GroupToggle groupToggle;

    [Space]
    public SwitchUnitUnit unitSwitchFrom;
    public SwitchUnitUnit unitSwitchTo;

    [Space]
    public Button changeBtn;
    public Button goBackBtn;

    #endregion
    PlayerUnit inputData;
    private void Awake()
    {
        groupToggle.Setup(1);
    }


    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(PlayerUnit))
        {
            inputData = t as PlayerUnit;
            var unitData = UIDataProcess.GetUnitInfo(inputData.iIndex);
            var skillKeys = UIDataProcess.GetUnitSkillKeys(inputData.iIndex);
            var skillData = UIDataProcess.GetUnitSkillInfo(skillKeys[1]);

            unitSwitchTo.iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            switch (unitData.Position)
            {
                case UNITPOSITION.TANKER_POSITION: unitSwitchTo.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                case UNITPOSITION.DEALER_POSITION: unitSwitchTo.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                case UNITPOSITION.SUPPORTER_POSITION: unitSwitchTo.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
            }
            unitSwitchTo.iSkill.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", skillKeys[1].ToString()));
            unitSwitchTo.tLv.text = inputData.iLevel.ToString();
            unitSwitchTo.tName.text = unitData.strName;
            unitSwitchTo.tStatus.text =  "체력 : " + unitData.IUnitHealth.ToString() +
                                        "   공격력 : " + unitData.IUnitAtk.ToString() +
                                        "   방어력 : " + unitData.IUnitDef.ToString();

            unitSwitchTo.SkillDesc.text = UIDataProcess.GetUnitSkiilDesc(skillKeys[1]);
            var Inventory = UIDataProcess.GetUnitInventory();
            for (int i = 0; i< equipUnits.Count; ++i)
            {
                if (Inventory.EquipmentUnit[i] != null)
                {
                    var playerUnit = Inventory.EquipmentUnit[i];

                    if (playerUnit != null && playerUnit.iIndex != 0)
                    {
                        equipUnits[i].iMain.enabled = true;
                        equipUnits[i].iPosition.enabled = true;
                        var unitInfo = UIDataProcess.GetUnitInfo(playerUnit.iIndex);
                        equipUnits[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitInfo.StrUnitImage.Replace("[CharacterID]", unitInfo.iID.ToString()));
                        switch (unitInfo.Position)
                        {
                            case UNITPOSITION.TANKER_POSITION: equipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                            case UNITPOSITION.DEALER_POSITION: equipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                            case UNITPOSITION.SUPPORTER_POSITION: equipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
                        }

                        equipUnits[i].tLv.text = playerUnit.iLevel.ToString();

                        if (inputData.iIndex == playerUnit.iIndex && inputData.ICount == playerUnit.ICount)
                        {
                            groupToggle.toggles[i].isActive = false;
                        }
                    }
                }
                else
                {
                    equipUnits[i].iMain.enabled = false;
                    equipUnits[i].iPosition.enabled = false;
                    equipUnits[i].tLv.text = "";
                }
            }

            groupToggle.checkAction += () =>
            {
                if (groupToggle.checkQ.Count != 0)
                {
                    var playerUnit = Inventory.EquipmentUnit[groupToggle.checkQ[0]];
                    if (playerUnit != null && playerUnit.iIndex != 0)
                    {
                        var unitInfo = UIDataProcess.GetUnitInfo(playerUnit.iIndex);
                        var skillKey = UIDataProcess.GetUnitSkillKeys(playerUnit.iIndex);
                        var skillInfo = UIDataProcess.GetUnitSkillInfo(skillKeys[1]);

                        unitSwitchFrom.iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitInfo.StrUnitImage.Replace("[CharacterID]", unitInfo.iID.ToString()));
                        switch (unitInfo.Position)
                        {
                            case UNITPOSITION.TANKER_POSITION: unitSwitchFrom.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                            case UNITPOSITION.DEALER_POSITION: unitSwitchFrom.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                            case UNITPOSITION.SUPPORTER_POSITION: unitSwitchFrom.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
                        }
                        unitSwitchFrom.iSkill.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillInfo.StrSkillIcon.Replace("[SkillID]", skillKey[1].ToString()));
                        unitSwitchFrom.tLv.text = playerUnit.iLevel.ToString();
                        unitSwitchFrom.tName.text = unitInfo.strName;
                        unitSwitchFrom.tStatus.text = "체력 : " + unitInfo.IUnitHealth.ToString() +
                                                    "   공격력 : " + unitInfo.IUnitAtk.ToString() +
                                                    "   방어력 : " + unitInfo.IUnitDef.ToString();
                        unitSwitchFrom.SkillDesc.text = UIDataProcess.GetUnitSkiilDesc(skillKeys[1]);

                        Color alpha = new Color(1,1,1,1);
                        unitSwitchFrom.iMain.color = alpha;
                        unitSwitchFrom.iPosition.color = alpha;
                        unitSwitchFrom.iSkill.color = alpha;
                    }
                    else
                    {
                        Color alpha = new Color(0, 0, 0, 0);
                        unitSwitchFrom.iMain.sprite = null;
                        unitSwitchFrom.iPosition.sprite = null;
                        unitSwitchFrom.iSkill.sprite = null;
                        unitSwitchFrom.iMain.color = alpha;
                        unitSwitchFrom.iPosition.color = alpha;
                        unitSwitchFrom.iSkill.color = alpha;
                        unitSwitchFrom.tLv.text = "";
                        unitSwitchFrom.tName.text = "";
                        unitSwitchFrom.tStatus.text = "";
                        unitSwitchFrom.SkillDesc.text = "";
                    }
                }
            };

            goBackBtn.onClick.AddListener(() => { UIManager.instance.CloseCountPopup(2); });
            changeBtn.onClick.AddListener(
                () =>
                {
                    /// TODO:
                    /// 유닛 교체 SwitchTo(InputData) 와  SwitchFrom(Inventory.EquipmentUnit[groupToggle.checkQ[0]))
                    if(groupToggle.checkQ.Count > 0)
                    {
                        goBackBtn.enabled = false;
                        changeBtn.enabled = false;
                        int CurrentIndex = PlayerDataManager.PlayerData.UnitInventory.UnitFind(Inventory.EquipmentUnit[groupToggle.checkQ[0]]);
                        if(CurrentIndex != -1)
                        {
                            PlayerDataManager.PlayerData.UnitInventory.UintList[CurrentIndex].bEquipped = false;
                            int i = 0;
                            int Eqpindex = 0;
                            if(PlayerDataManager.PlayerData.UnitInventory.UnitEquipmentFind(Inventory.EquipmentUnit[groupToggle.checkQ[0]].iIndex, Inventory.EquipmentUnit[groupToggle.checkQ[0]].ICount, out Eqpindex))
                            {
                                PlayerDataManager.PlayerData.UnitInventory.EquipmentUnit[Eqpindex] = null;
                            }
                        }
                        inputData.bEquipped = true;
                        Inventory.EquipmentUnit[groupToggle.checkQ[0]] = inputData;
                        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.UNIT_DATAFILE, (Suceed) => {
                            if(Suceed)
                            {
                                goBackBtn.enabled = true;
                                changeBtn.enabled = true;

                                UIManager.instance.CloseCountPopup(2);
                            }
                        });                       
                    }
                });
        }
    }

    public override void Load()
    {
        if(inputData != null)
        {
            var unitData = UIDataProcess.GetUnitInfo(inputData.iIndex);
            var skillKeys = UIDataProcess.GetUnitSkillKeys(inputData.iIndex);
            var skillData = UIDataProcess.GetUnitSkillInfo(skillKeys[1]);

            unitSwitchTo.iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            switch (unitData.Position)
            {
                case UNITPOSITION.TANKER_POSITION: unitSwitchTo.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                case UNITPOSITION.DEALER_POSITION: unitSwitchTo.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                case UNITPOSITION.SUPPORTER_POSITION: unitSwitchTo.iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
            }
            unitSwitchTo.iSkill.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", skillKeys[1].ToString()));
            unitSwitchTo.tLv.text = inputData.iLevel.ToString();
            unitSwitchTo.tName.text = unitData.strName;
            unitSwitchTo.tStatus.text = "체력 : " + unitData.IUnitHealth.ToString() +
                                        "   공격력 : " + unitData.IUnitAtk.ToString() +
                                        "   방어력 : " + unitData.IUnitDef.ToString();

            unitSwitchTo.SkillDesc.text = UIDataProcess.GetUnitSkiilDesc(skillKeys[1]);
            var Inventory = UIDataProcess.GetUnitInventory();
            for (int i = 0; i < equipUnits.Count; ++i)
            {
                if (Inventory.EquipmentUnit[i] != null)
                {
                    var playerUnit = Inventory.EquipmentUnit[i];

                    if (playerUnit != null && playerUnit.iIndex != 0)
                    {
                        equipUnits[i].iMain.enabled = true;
                        equipUnits[i].iPosition.enabled = true;
                        var unitInfo = UIDataProcess.GetUnitInfo(playerUnit.iIndex);
                        equipUnits[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitInfo.StrUnitImage.Replace("[CharacterID]", unitInfo.iID.ToString()));
                        switch (unitInfo.Position)
                        {
                            case UNITPOSITION.TANKER_POSITION: equipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                            case UNITPOSITION.DEALER_POSITION: equipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                            case UNITPOSITION.SUPPORTER_POSITION: equipUnits[i].iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
                        }

                        equipUnits[i].tLv.text = playerUnit.iLevel.ToString();

                        if (inputData.iIndex == playerUnit.iIndex && inputData.ICount == playerUnit.ICount)
                        {
                            groupToggle.toggles[i].isActive = false;
                        }
                    }
                }
                else
                {
                    equipUnits[i].iMain.enabled = false;
                    equipUnits[i].iPosition.enabled = false;
                    equipUnits[i].tLv.text = "";
                }
            }
        }
    }
}
