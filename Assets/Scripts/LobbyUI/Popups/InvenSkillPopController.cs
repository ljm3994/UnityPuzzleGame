using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class InvenSkillPopController : PopupController
{
    #region inspector

    public Image iMain;
    public Text tName;
    public Text tDesc;

    public Button backGroundBtn;
    public Button changeBtn;

    #endregion

    PlayerSkill inputData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(PlayerSkill))
        {
            inputData = t as PlayerSkill;
            int EqpIndex = 0;
            PlayerDataManager.PlayerData.SkillInventory.FindEquipmentItem(inputData.iIndex, out EqpIndex);

            var skillData = UIDataProcess.GetPlayerSkillInfo(inputData.iIndex, EqpIndex);
            iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", inputData.iIndex.ToString()));
            tName.text = skillData.StrSkillName;
            tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillData.ISkillId);

            backGroundBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });
            changeBtn.onClick.AddListener(() => { UIManager.instance.Popup("Popup_ChangeESkill", inputData); });
        }
    }

    public override void Load()
    {
        if(inputData != null)
        {
            int EqpIndex = 0;
            PlayerDataManager.PlayerData.SkillInventory.FindEquipmentItem(inputData.iIndex, out EqpIndex);

            var skillData = UIDataProcess.GetPlayerSkillInfo(inputData.iIndex, EqpIndex);
            iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", inputData.iIndex.ToString()));
            tName.text = skillData.StrSkillName;
            tDesc.text = UIDataProcess.GetPlayerSkillDesc(skillData.ISkillId);
        }
    }
}
