using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ShopItemPopController : PopupController
{
    #region inspector

    public Image iMain;

    public Text tName;
    public Text tDesc;

    public Text tNum;
    public Text tCost;

    public Button numUpBtn;
    public Button numDownBtn;

    public Button backgroundBtn;
    public Button purchaseBtn;
    public Button goBackBtn;
    #endregion
    ShopInfo inputData;
    // Start is called before the first frame update
    void Start()
    {
    }

    void addNum(int ItemCount, int val)
    {
        int Count = DataProcess.stringToint(tNum.text);
        if(99 > ItemCount + Count + val)
        {
            tNum.text = Mathf.Clamp(Count + val, 1, 99).ToString();
        }
    }

    void resetCost(int price)
    {
        int num;
        if(int.TryParse(tNum.text,out num))
        {
            tCost.text = (num * price).ToString();
        }
    }

    public override void Setup<T>(T t)
    {
        if (t.GetType() == typeof(ShopInfo))
        {
            inputData = t as ShopInfo;
            var itemData = UIDataProcess.GetItemInfo(inputData.IItemID);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + itemData.StrIcon.Replace("[ItemID]",inputData.IItemID.ToString()));
            tName.text = inputData.StrItemName;
            tDesc.text = UIDataProcess.GetConsumptionItemDesc(itemData.IItemId); ;

            tNum.text = "1";
            int Count = DataProcess.stringToint(UIDataProcess.ItemConut(itemData.Type, itemData.IItemId));
            int price = inputData.IItemValue;
            numUpBtn.onClick.AddListener(() => { addNum(Count, 1); resetCost(price); });
            numDownBtn.onClick.AddListener(() => { addNum(Count, - 1); resetCost(price); });
            resetCost(price);

            backgroundBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
            goBackBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
            purchaseBtn.onClick.AddListener(
                () => {
                    /// TODO:
                    /// Item(inputData, int.parse(tNum.text) 개 ) 구매
                    int Price = DataProcess.stringToint(tCost.text);
                    int PurchaseCount = DataProcess.stringToint(tNum.text);
                    if (UIDataProcess.PurchasePrice(Price, inputData.BPurchaseType))
                    {
                        purchaseBtn.enabled = false;
                        goBackBtn.enabled = false;
                        backgroundBtn.enabled = false;
                        switch (itemData.Type)
                        {
                            case ITEMTYPE.STUFF_TYPE:
                                PlayerDataManager.PlayerData.InventoryETCItemData.AddItem(inputData.IItemID, PurchaseCount);

                                PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.ETCITEM_DATAFILE | PLAYERDATAFILE.USER_DATAFILE, (succed) => {
                                    if (succed)
                                    {
                                        purchaseBtn.enabled = true;
                                        goBackBtn.enabled = true;
                                        backgroundBtn.enabled = true;
                                        UIManager.instance.CloseAllPopup();
                                    }
                                });
                                UIManager.instance.CloseAllPopup();
                                break;
                            case ITEMTYPE.EXPENDABLES_TYPE:
                                PlayerDataManager.PlayerData.PlayerItem.AddItem(inputData.IItemID, PurchaseCount);
                                PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.ITEM_DATAFILE | PLAYERDATAFILE.USER_DATAFILE, (succed) => {
                                    if (succed)
                                    {
                                        purchaseBtn.enabled = true;
                                        goBackBtn.enabled = true;
                                        backgroundBtn.enabled = true;
                                        UIManager.instance.CloseAllPopup();
                                    }
                                });
                                
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        UIManager.instance.CloseAllPopup();
                    }

                });
        }
    }

    public override void Load()
    {
        if(inputData != null)
        {
            var itemData = UIDataProcess.GetItemInfo(inputData.IItemID);

            iMain.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + itemData.StrIcon.Replace("[ItemID]", inputData.IItemID.ToString()));
            tName.text = inputData.StrItemName;
            tDesc.text = UIDataProcess.GetConsumptionItemDesc(itemData.IItemId); ;

            tNum.text = "1";
            int Count = DataProcess.stringToint(UIDataProcess.ItemConut(itemData.Type, itemData.IItemId));
            int price = inputData.IItemValue;
            numUpBtn.onClick.AddListener(() => { addNum(Count, 1); resetCost(price); });
            numDownBtn.onClick.AddListener(() => { addNum(Count, -1); resetCost(price); });
            resetCost(price);
        }
    }
}
