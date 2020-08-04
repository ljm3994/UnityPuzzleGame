using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ItemPopController : PopupController
{
    #region inspector

    public Image iMain;
    public Text tName;
    public Text tDesc;
    public Text tNum;

    public Button backgroundBtn;
    public Button button;
    #endregion
    CharterItem inputData;

    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(CharterItem))
        {
            inputData = t as CharterItem;
            var itemData = UIDataProcess.GetItemInfo(inputData.iItemIndex);

            switch (itemData.Type)
            {
                case ITEMTYPE.STUFF_TYPE: iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + itemData.StrIcon.Replace("[ItemID]",inputData.iItemIndex.ToString())); break;
                case ITEMTYPE.EXPENDABLES_TYPE: iMain.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + itemData.StrIcon.Replace("[ItemID]", inputData.iItemIndex.ToString())); break;
            }
            tName.text = itemData.StrItemName;
            tDesc.text = UIDataProcess.GetConsumptionItemDesc(inputData.iItemIndex);
            tNum.text = "보유 : " + inputData.iCount.ToString();
        }

        backgroundBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
        button.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
    }

    public override void Load()
    {
        if(inputData!= null)
        {
            var itemData = UIDataProcess.GetItemInfo(inputData.iItemIndex);

            switch (itemData.Type)
            {
                case ITEMTYPE.STUFF_TYPE: iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + itemData.StrIcon.Replace("[ItemID]", inputData.iItemIndex.ToString())); break;
                case ITEMTYPE.EXPENDABLES_TYPE: iMain.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + itemData.StrIcon.Replace("[ItemID]", inputData.iItemIndex.ToString())); break;
            }
            tName.text = itemData.StrItemName;
            tDesc.text = UIDataProcess.GetConsumptionItemDesc(inputData.iItemIndex);
            tNum.text = "보유 : " + inputData.iCount.ToString();
        }
    }
}
