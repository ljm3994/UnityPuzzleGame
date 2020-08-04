using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class InvenItemController : GridUnitController
{
    #region inspector
    public Image   iMain;
    public Text    tNum;
    #endregion

    
    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(CharterItem))
        {
            var inputData = t as CharterItem;
            var itemData = UIDataProcess.GetItemInfo(inputData.iItemIndex);
            switch (itemData.Type)
            {
                case ITEMTYPE.STUFF_TYPE: iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + itemData.StrIcon.Replace("[ItemID]",inputData.iItemIndex.ToString())); break;
                case ITEMTYPE.EXPENDABLES_TYPE: iMain.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + itemData.StrIcon.Replace("[ItemID]", inputData.iItemIndex.ToString())); break;
                default: break;
            }
            tNum.text = inputData.iCount.ToString();

            button.onClick.AddListener(
                () => {
                    UIManager.instance.Popup("Popup_ETC_Item", inputData);
                });
        }

        if(t.GetType() == typeof(ItemInfo))
        {
            var inputData = t as ItemInfo;
            switch (inputData.Type)
            {
                case ITEMTYPE.STUFF_TYPE: iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + inputData.StrIcon.Replace("[ItemID]", inputData.IItemId.ToString())); break;
                case ITEMTYPE.EXPENDABLES_TYPE: iMain.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + inputData.StrIcon.Replace("[ItemID]", inputData.IItemId.ToString())); break;
                default: break;
            }
        }
    }

}
