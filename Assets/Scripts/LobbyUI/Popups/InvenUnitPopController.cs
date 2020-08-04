﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class InvenUnitPopController : PopupController
{
    #region inspector
    public Image iMain;
    public Image iSkill;
    [Space]
    public Text tName;
    public Text tLv;
    public Text tDesc;
    public Text tSkillDesc;
    [Space]
    public GaugeBarController ExpGaugeBar;
    [Space]
    public Button BackGroundBtn;
    public Button ChangeBtn;
    public Button LvUpBtn;
    #endregion

    bool lvUpActive = false;
    PlayerUnit inputData;


    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(PlayerUnit))
        {
            inputData = t as PlayerUnit;
            var unitData = UIDataProcess.GetUnitInfo(inputData.iIndex);
            var skillKeys = UIDataProcess.GetUnitSkillKeys(inputData.iIndex);
            var skillData = UIDataProcess.GetUnitSkillInfo(skillKeys[1]);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            iSkill.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", skillKeys[1].ToString()));
            tName.text = unitData.strName;
            tLv.text = inputData.iLevel.ToString();
            tDesc.text = "체력 : " + unitData.IUnitHealth.ToString() +
                         "   공격력 : " + unitData.IUnitAtk.ToString() +
                         "   방어력 : " + unitData.IUnitDef.ToString();
            tSkillDesc.text = UIDataProcess.GetUnitSkiilDesc(skillKeys[1]);

            int MaxLevel = 20;
            if (inputData.iLevel < unitData.iMaxLevel)
            {
                MaxLevel = inputData.iLevel + 1;
            }
            else
            {
                MaxLevel = unitData.iMaxLevel;
            }
            ExpGaugeBar.Max = GameDataBase.Instance.UnitExpTable[MaxLevel].INeedEXP;
            ExpGaugeBar.Value = inputData.IExp;

            lvUpActive = false;

            if (inputData.iLevel < unitData.iMaxLevel)
            {
                if (inputData.IExp >= GameDataBase.Instance.UnitExpTable[MaxLevel].INeedEXP)
                {
                    lvUpActive = true;
                }
            }

            BackGroundBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });
            ChangeBtn.onClick.AddListener(() => { UIManager.instance.Popup("Popup_SelectSwapUnit", inputData); });
            {
                /// TODO:
                /// LvUp 버튼 활성화 조건 추가
                if (lvUpActive)
                {
                    LvUpBtn.enabled = true;
                    LvUpBtn.onClick.AddListener(() => { UIManager.instance.Popup("Popup_UnitLvUp", inputData); });
                }
                else
                {
                    LvUpBtn.onClick.RemoveAllListeners();
                    LvUpBtn.enabled = false;
                }
            }
        }
    }

    public override void Load()
    {
        if(inputData != null)
        {
            var unitData = UIDataProcess.GetUnitInfo(inputData.iIndex);
            var skillKeys = UIDataProcess.GetUnitSkillKeys(inputData.iIndex);
            var skillData = UIDataProcess.GetUnitSkillInfo(skillKeys[1]);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            iSkill.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", skillKeys[1].ToString()));
            tName.text = unitData.strName;
            tLv.text = inputData.iLevel.ToString();
            tDesc.text = "체력 : " + unitData.IUnitHealth.ToString() +
                         "   공격력 : " + unitData.IUnitAtk.ToString() +
                         "   방어력 : " + unitData.IUnitDef.ToString();
            tSkillDesc.text = UIDataProcess.GetUnitSkiilDesc(skillKeys[1]);

            int MaxLevel = 20;
            if (inputData.iLevel < unitData.iMaxLevel)
            {
                MaxLevel = inputData.iLevel + 1;
            }
            else
            {
                MaxLevel = unitData.iMaxLevel;
            }
            ExpGaugeBar.Max = GameDataBase.Instance.UnitExpTable[MaxLevel].INeedEXP;
            ExpGaugeBar.Value = inputData.IExp;

            lvUpActive = false;

            if (inputData.iLevel < unitData.iMaxLevel)
            {
                if (inputData.IExp >= GameDataBase.Instance.UnitExpTable[MaxLevel].INeedEXP)
                {
                    lvUpActive = true;
                }
            }

            ChangeBtn.onClick.AddListener(() => { UIManager.instance.Popup("Popup_SelectSwapUnit", inputData); });
            {
                /// TODO:
                /// LvUp 버튼 활성화 조건 추가
                if (lvUpActive)
                {
                    LvUpBtn.enabled = true;
                    LvUpBtn.onClick.AddListener(() => { UIManager.instance.Popup("Popup_UnitLvUp", inputData); });
                }
                else
                {
                    LvUpBtn.onClick.RemoveAllListeners();
                    LvUpBtn.enabled = false;
                }
            }
        }
    }
}
