using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ShopItemController : GridUnitController
{
    #region inspector

    public Image iMain;

    public Text tPrice;
    public Text tName;
    #endregion

    public override void Setup<T>(T t)
    {
        if (t.GetType() == typeof(ShopInfo))
        {
            var inputData = t as ShopInfo;

            var itemData = UIDataProcess.GetItemInfo(inputData.IItemID);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + itemData.StrIcon.Replace("[ItemID]",inputData.IItemID.ToString()));
            tPrice.text = inputData.IItemValue.ToString();
            tName.text = inputData.StrItemName;

            button.onClick.AddListener(
                () => {
                    UIManager.instance.Popup("Popup_ShopItem", inputData);
                });
        }
    }
}
