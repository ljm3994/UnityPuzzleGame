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

    public Button button;

    #endregion

    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(PlayerSkill))
        {
            var inputData = t as PlayerSkill;
            var skillData = UIDataProcess.GetPlayerSkillInfo(inputData.iIndex, inputData.IEquipmentIndex);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", inputData.iIndex.ToString()));
            tName.text = skillData.StrSkillName;
            if(inputData.BEquipped)
            {
                iMain.color = new Color(0.0f, 0.0f, 0.0f, 0.3f);
            }
            else
            {
                iMain.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
            button.onClick.AddListener(
                () => {
                    UIManager.instance.Popup("Popup_InvenSkill", inputData);
                });
        }
    }
}
