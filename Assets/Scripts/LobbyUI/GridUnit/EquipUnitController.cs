﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class EquipUnitController : GridUnitController
{
    #region inspector

    public Image iMain;
    public Image iPosition;
    public Text LevelUpText;
    public Text tLv;
    #endregion

    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(PlayerUnit))
        {
            var inputData = t as PlayerUnit;
            var unitData = UIDataProcess.GetUnitInfo(inputData.iIndex);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            tLv.text = inputData.iLevel.ToString();

            if (inputData.iLevel < GameDataBase.Instance.UnitTable[inputData.iIndex].iMaxLevel && inputData.IExp >= GameDataBase.Instance.UnitExpTable[inputData.iLevel + 1].INeedEXP)
            {
                LevelUpText.text = "!";
            }
            else
            {
                LevelUpText.text = "";
            }

            switch (unitData.Position)
            {
                case UNITPOSITION.TANKER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                case UNITPOSITION.DEALER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                case UNITPOSITION.SUPPORTER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
            }

            button.onClick.AddListener(() => {
                UIManager.instance.Popup("Popup_EquipUnit", inputData);
            });
        }
    }
}
