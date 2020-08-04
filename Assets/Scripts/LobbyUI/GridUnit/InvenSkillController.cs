using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class InvenSkillController : GridUnitController
{
    #region inspector

    public Image iMain;
    public Text tName;
    #endregion

    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(PlayerSkill))
        {
            var inputData = t as PlayerSkill;
            int EqpIndex = 0;
            PlayerDataManager.PlayerData.SkillInventory.FindEquipmentItem(inputData.iIndex, out EqpIndex);

            var skillData = UIDataProcess.GetPlayerSkillInfo(inputData.iIndex, EqpIndex);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", inputData.iIndex.ToString()));
            tName.text = skillData.StrSkillName;

            bCover = inputData.BEquipped;

            button.onClick.AddListener(
                () => {
                    UIManager.instance.Popup("Popup_InvenSkill", inputData);
                });
        }
    }
}
