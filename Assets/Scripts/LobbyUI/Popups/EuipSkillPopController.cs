using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class EuipSkillPopController : PopupController
{
    #region inspector

    public Image iMain;
    public Text tName;
    public Text tDesc;

    public Button backGroundBtn;
    public Button releaseBtn;

    #endregion

    bool bClose = false;
    PlayerSkill inputData;
    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(PlayerSkill))
        {
            inputData = t as PlayerSkill;
            int EqpIndex = 0;
            bool bEqp = PlayerDataManager.PlayerData.SkillInventory.FindEquipmentItem(inputData.iIndex, out EqpIndex);
            var skillData = UIDataProcess.GetPlayerSkillInfo(inputData.iIndex, EqpIndex);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillData.StrSkillIcon.Replace("[SkillID]",inputData.iIndex.ToString()));
            tName.text = skillData.StrSkillName;
            tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillData.ISkillId);

            backGroundBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });
            releaseBtn.onClick.AddListener(
                () => {
                    /// TODO:
                    /// 장착된 스킬(skillInfo) 해제
                    if (bEqp)
                    {
                        releaseBtn.enabled = false;
                        backGroundBtn.enabled = false;
                        PlayerDataManager.PlayerData.SkillInventory.SetSkillEquipment(inputData.iIndex, false);
                        PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills[EqpIndex] = null;
                        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.SKILL_DATAFILE, (Succed) => {
                            if (Succed)
                            {
                                backGroundBtn.enabled = true;
                                releaseBtn.enabled = true;
                                UIManager.instance.CloseTopPopup();
                            }
                        });

                    }
                    else
                    {
                        UIManager.instance.CloseTopPopup();
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

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", inputData.iIndex.ToString()));
            tName.text = skillData.StrSkillName;
            tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillData.ISkillId);
        }
    }
}
