using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ShopGSPopController : PopupController
{
    #region inspector

    public Text tInfo;
    public Text tCost;

    public Button backgroundBtn;
    public Button purchaseBtn;
    public Button goBackBtn;
    #endregion
    ShopInfo inputData;

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
        if (t.GetType() == typeof(ShopInfo))
        {
            inputData = t as ShopInfo;

            tInfo.text = inputData.StrItemDesc;
            tCost.text = inputData.IItemValue.ToString();


            backgroundBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
            goBackBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
            purchaseBtn.onClick.AddListener(
                () => {
                    /// TODO:
                    /// Item(inputData) 구매
                    if(UIDataProcess.PurchasePrice(inputData.IItemValue, inputData.BPurchaseType))
                    {
                        purchaseBtn.enabled = false;
                        goBackBtn.enabled = false;
                        backgroundBtn.enabled = false;
                        switch (inputData.ItemType)
                        {
                            case SHOPITEM_TYPE.GOLD_TYPE:
                                PlayerDataManager.PlayerData.Pdata.iCoin += inputData.IItemGetValue;
                                break;
                            case SHOPITEM_TYPE.STEMINA_TYPE:
                                PlayerDataManager.PlayerData.Pdata.iStamina += inputData.IItemGetValue;
                                break;
                            default:
                                break;
                        }
                        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.USER_DATAFILE, (Succed) => {
                            if (Succed)
                            {
                                purchaseBtn.enabled = true;
                                goBackBtn.enabled = true;
                                backgroundBtn.enabled = true;
                                UIManager.instance.CloseAllPopup();
                            }
                        });
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
            tInfo.text = inputData.StrItemDesc;
            tCost.text = inputData.IItemValue.ToString();
        }
    }
}
