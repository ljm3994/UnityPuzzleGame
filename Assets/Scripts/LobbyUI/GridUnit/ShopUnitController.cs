using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ShopUnitController : GridUnitController
{
    #region inspector

    public Image iMain;
    public Image iPosition;

    public Text tPrice;
    public Text tName;
    #endregion

    public override void Setup<T>(T t)
    {
        if (t.GetType() == typeof(ShopInfo))
        {
            var inputData = t as ShopInfo;
            var unitData = UIDataProcess.GetUnitInfo(inputData.IItemID);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            switch (unitData.Position)
            {
                case UNITPOSITION.TANKER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                case UNITPOSITION.DEALER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                case UNITPOSITION.SUPPORTER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
            }
            tName.text = inputData.StrItemName;
            tPrice.text = UIDataProcess.GetUnitPrice(inputData.IItemID).ToString();

            button.onClick.AddListener(
                () => {
                    UIManager.instance.Popup("Popup_ShopUnit", inputData);
                });
        }
    }
}
