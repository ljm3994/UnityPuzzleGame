using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class SwitchSkillPopController : PopupController
{
    #region inspector

    public SwitchSkillUnit unitSwitchTo;

    public List<SwitchSkillUnit> unitSwitchFrom;
    public GroupToggle groupToggle;

    public Button backgroundBtn;
    public Button changeBtn;
    public Button goBackBtn;

    #endregion
    PlayerSkill inputData;
    private void Awake()
    {
        groupToggle.Setup(1);
    }

    public override void Setup<T>(T t)
    {
        if (t.GetType() == typeof(PlayerSkill))
        {
            inputData = t as PlayerSkill;
            int EqpIndex = 0;
            bool bEqp = PlayerDataManager.PlayerData.SkillInventory.FindEquipmentItem(inputData.iIndex, out EqpIndex);
            var skillData = UIDataProcess.GetPlayerSkillInfo(inputData.iIndex, EqpIndex);

            unitSwitchTo.iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", inputData.iIndex.ToString()));
            unitSwitchTo.tName.text = skillData.StrSkillName;
            unitSwitchTo.tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillData.ISkillId);

            groupToggle.checkAction +=
                () => {
                    if(groupToggle.checkQ.Count != 0)
                    {
                        int equipIndex = groupToggle.checkQ[0];
                        /// TODO:
                        /// 바꿀 스킬(unitSwitchTo) 의 능력치가 equipIndex (0~2) 에 따라 다르게 로딩되도록
                        unitSwitchTo.tName.text = GameDataBase.Instance.PlayerSkillTable[skillData.ISkillId + equipIndex].StrSkillName;
                        unitSwitchTo.tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillData.ISkillId + equipIndex);
                    }
                };

            var inventory = UIDataProcess.GetSkillInventory().playerEquipSkills;
            for(int i = 0; i< 3; ++i)
            {
                if (inventory[i] != null)
                {
                    var skillInfo = UIDataProcess.GetPlayerSkillInfo(inventory[i].iIndex, i);
                    unitSwitchFrom[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillInfo.StrSkillIcon.Replace("[SkillID]", inventory[i].iIndex.ToString()));
                    unitSwitchFrom[i].tName.text = skillInfo.StrSkillName;
                    unitSwitchFrom[i].tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillInfo.ISkillId);
                }
                else
                {
                    unitSwitchFrom[i].tName.text = "";
                    unitSwitchFrom[i].tDesc.text = "";
                }
            }


            backgroundBtn.onClick.AddListener(() => { UIManager.instance.CloseCountPopup(2); });
            goBackBtn.onClick.AddListener(() => { UIManager.instance.CloseCountPopup(2); });
            changeBtn.onClick.AddListener(
                () => {
                    /// TODO:
                    /// inputSkillInfo(인벤토리) 를 Equip[groupToggle.checkQ[0]] 와 바꾸기
                    /// Switch Inventory Skill(unitSwitchTo) Equip Skill(unitSwitchFrom)
                    /// 

                    //정보 가져오기 
                    if (groupToggle.checkQ.Count != 0)
                    {
                        changeBtn.enabled = false;
                        goBackBtn.enabled = false;
                        backgroundBtn.enabled = false;
                        int equipIndex = groupToggle.checkQ[0];
                        int Index = PlayerDataManager.PlayerData.SkillInventory.FindItem(UIDataProcess.GetSkillInventory().playerEquipSkills[equipIndex]);
                        if (Index != -1)
                        {
                            PlayerDataManager.PlayerData.SkillInventory.SkillList[Index].BEquipped = false;
                            if(bEqp)
                            {
                                PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills[EqpIndex] = null;
                            }
                        }
                        inputData.BEquipped = true;
                        UIDataProcess.GetSkillInventory().playerEquipSkills[equipIndex] = inputData;
                        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.SKILL_DATAFILE, (Succed) => {
                            if(Succed)
                            {
                                changeBtn.enabled = true;
                                goBackBtn.enabled = true;
                                backgroundBtn.enabled = true;
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
            int EqpIndex = 0;
            bool bEqp = PlayerDataManager.PlayerData.SkillInventory.FindEquipmentItem(inputData.iIndex, out EqpIndex);
            var skillData = UIDataProcess.GetPlayerSkillInfo(inputData.iIndex, EqpIndex);

            unitSwitchTo.iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", inputData.iIndex.ToString()));
            unitSwitchTo.tName.text = skillData.StrSkillName;
            unitSwitchTo.tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillData.ISkillId);

            var inventory = UIDataProcess.GetSkillInventory().playerEquipSkills;
            for (int i = 0; i < 3; ++i)
            {
                if (inventory[i] != null)
                {
                    var skillInfo = UIDataProcess.GetPlayerSkillInfo(inventory[i].iIndex, i);
                    unitSwitchFrom[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillInfo.StrSkillIcon.Replace("[SkillID]", inventory[i].iIndex.ToString()));
                    unitSwitchFrom[i].tName.text = skillInfo.StrSkillName;
                    unitSwitchFrom[i].tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillInfo.ISkillId);
                }
                else
                {
                    unitSwitchFrom[i].tName.text = "";
                    unitSwitchFrom[i].tDesc.text = "";
                }
            }
        }
    }
}
