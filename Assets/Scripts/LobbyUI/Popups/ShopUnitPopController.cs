using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ShopUnitPopController : PopupController
{
    #region inspector

    public Image iMain;
    public Image iPosition;
    public Text tName;
    public Text tDesc;
    public Text tCost;
    public Text SkillDesc;
    public Image SkillImg;
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
            var unitData = UIDataProcess.GetUnitInfo(inputData.IItemID);
            int[] skillIndex = UIDataProcess.GetUnitSkillKeys(inputData.IItemID);
            UnitSkillInfo skillData = null;
            if (skillIndex.Length > 1)
            {
                skillData = UIDataProcess.GetUnitSkillInfo(skillIndex[1]);
            }
            iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            switch (unitData.Position)
            {
                case UNITPOSITION.TANKER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                case UNITPOSITION.DEALER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                case UNITPOSITION.SUPPORTER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
            }
            tName.text = inputData.StrItemName;
            tDesc.text = "체력 : " + unitData.IUnitHealth.ToString() +
                        " / " + "공격력 : " + unitData.IUnitAtk.ToString();
             if (skillData != null)
            {
                SkillImg.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", skillData.ISkillId.ToString()));
                SkillDesc.text = UIDataProcess.GetUnitSkiilDesc(skillData.ISkillId);
            }


            tCost.text = UIDataProcess.GetUnitPrice(inputData.IItemID).ToString();

            backgroundBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
            goBackBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
            purchaseBtn.onClick.AddListener(
                () => {
                    /// TODO:
                    /// Unit(inputData) 구매
                    int Price = DataProcess.stringToint(tCost.text);
                    
                    if (UIDataProcess.PurchasePrice(Price, inputData.BPurchaseType))
                    {
                        purchaseBtn.enabled = false;
                        goBackBtn.enabled = false;
                        backgroundBtn.enabled = false;
                        PlayerDataManager.PlayerData.UnitInventory.UnitAdd(inputData.IItemID);
                        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.USER_DATAFILE | PLAYERDATAFILE.UNIT_DATAFILE, (Succed) => {

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
            var unitData = UIDataProcess.GetUnitInfo(inputData.IItemID);
            int[] skillIndex = UIDataProcess.GetUnitSkillKeys(inputData.IItemID);
            UnitSkillInfo skillData = null;
            if (skillIndex.Length > 1)
            {
                skillData = UIDataProcess.GetUnitSkillInfo(skillIndex[1]);
            }
            iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitData.StrUnitImage.Replace("[CharacterID]", unitData.iID.ToString()));
            switch (unitData.Position)
            {
                case UNITPOSITION.TANKER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionTankerPath); break;
                case UNITPOSITION.DEALER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionDealerPath); break;
                case UNITPOSITION.SUPPORTER_POSITION: iPosition.sprite = UICommon.LoadSprite(UIDataProcess.UnitPositionSupporterPath); break;
            }
            tName.text = inputData.StrItemName;
            tDesc.text = "체력 : " + unitData.IUnitHealth.ToString() +
                        " / " + "공격력 : " + unitData.IUnitAtk.ToString();
            if (skillData != null)
            {
                SkillImg.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", skillData.ISkillId.ToString()));
                SkillDesc.text = UIDataProcess.GetUnitSkiilDesc(skillData.ISkillId);
            }

            tCost.text = UIDataProcess.GetUnitPrice(inputData.IItemID).ToString();
        }
    }
}
