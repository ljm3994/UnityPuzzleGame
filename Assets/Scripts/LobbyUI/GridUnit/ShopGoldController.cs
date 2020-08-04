using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ShopGoldController : GridUnitController
{
    #region inspector

    public Image iMain;
    public Text tName;
    public Text tPrice;
    #endregion

    public override void Setup<T>(T t)
    {
        if (t.GetType() == typeof(ShopInfo))
        {
            var inputData = t as ShopInfo;

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.ShopSteminaImage);
            tName.text = inputData.StrItemName;
            tPrice.text = inputData.IItemValue.ToString();

            button.onClick.AddListener(
                () => {
                    UIManager.instance.Popup("Popup_ShopGold", inputData);
                });
        }
    }
}
