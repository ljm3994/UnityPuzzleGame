using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UICommons;

public class UnitLvUpPopController : PopupController
{
    #region inspector

    public List<ItemCount> itemCounts;
    public List<Image> plusIcons;
    public Text tCost;

    public Button backGroundBtn;
    public Button lvUpBtn;
    public Button goBackBtn;
    #endregion
    PlayerUnit inputData;

    public override void Setup<T>(T t)
    {
        /// TODO:
        /// 레벨업 팝업 구성
        /// 
        if (t.GetType() == typeof(PlayerUnit))
        {
            inputData = t as PlayerUnit;
            var unitData = UIDataProcess.GetUnitInfo(inputData.iIndex);
            List<int> ItemIndexList = new List<int>();
            List<int> ItemNeedCountList = new List<int>();
            int level = inputData.iLevel;
            bool canLvUp = true;

            for(int i = 2; i >= 0; i--)
            {
                string Key = string.Format("_iItemCondition{0}", i + 1);
                int ConditionLevel = (int)UIDataProcess.GetStringTypeNameValue(unitData, Key);

                if(level < ConditionLevel)
                {
                    Destroy(itemCounts[i].iMain.gameObject);
                    itemCounts.RemoveAt(i);
                    if(i > 0)
                    {
                        Destroy(plusIcons[i - 1].gameObject);
                        plusIcons.RemoveAt(i - 1);
                    }
                }
                else
                {
                    string ItemKey = string.Format("_iItemID{0}", i + 1);
                    string InceaseKey = string.Format("_iItemNumberIncrease{0}", i + 1);
                    string NumberKey = string.Format("_iItemNumber{0}", i + 1);

                    int ItemIndex = (int)UIDataProcess.GetStringTypeNameValue(unitData, ItemKey);
                    int Increase = (int)UIDataProcess.GetStringTypeNameValue(unitData, InceaseKey);
                    int NumBer = (int)UIDataProcess.GetStringTypeNameValue(unitData, NumberKey);
                    ItemIndex += 1000;
                    ItemIndexList.Add(ItemIndex);
                    var itemData = UIDataProcess.GetItemInfo(ItemIndex);
                    itemCounts[0].iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + itemData.StrIcon.Replace("[ItemID]", ItemIndex.ToString()));
                    int NeedCount = (NumBer + Increase * (level - 1));
                    itemCounts[0].tNum.text = NeedCount.ToString();
                    itemCounts[0].tNum.color = new Color(0, 0, 0, 1);
                    ItemNeedCountList.Add(NeedCount);
                    int Count = PlayerDataManager.PlayerData.InventoryETCItemData.FindItemIndexSelectCount(ItemIndex);

                    if(Count < NeedCount)
                    {
                        canLvUp = false;
                        itemCounts[0].tNum.color = new Color(1, 0, 0, 1);
                    }
                }
            }


            /// ???? 레벨업 시 필요한 재화 데이터
            int NeedMoney = GameDataBase.Instance.UnitExpTable[level + 1].INeedMoney;
            tCost.text = NeedMoney.ToString();

            if(PlayerDataManager.PlayerData.Pdata.iCoin < NeedMoney)
            {
                canLvUp = false;
                tCost.color = new Color(1, 0, 0, 1);
            }

            backGroundBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });
            goBackBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });

            if(canLvUp)
            {
                lvUpBtn.enabled = true;
                lvUpBtn.onClick.AddListener(
                    () =>
                    {
                        /// TODO :
                        /// 유닛 레벨업 구현
                        inputData.IExp -= GameDataBase.Instance.UnitExpTable[level + 1].INeedEXP;
                        lvUpBtn.enabled = false;
                        for (int i = 0; i < ItemIndexList.Count; i++)
                        {
                            if (!PlayerDataManager.PlayerData.InventoryETCItemData.ItemUse(ItemIndexList[i], ItemNeedCountList[i]))
                            {
                                UIManager.instance.CloseTopPopup();
                                return;
                            }
                        }

                        PlayerDataManager.PlayerData.Pdata.iCoin -= NeedMoney;
                        inputData.iLevel += 1;
                        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.ETCITEM_DATAFILE | PLAYERDATAFILE.UNIT_DATAFILE | PLAYERDATAFILE.USER_DATAFILE, (succed) => {
                            if(succed)
                            {
                                lvUpBtn.enabled = true;
                                UIManager.instance.CloseCountPopup(2);
                            }
                        });
                        
                    });
            }
            else
            {
                /// 버튼 비활성화 처리
                lvUpBtn.enabled = false;
                lvUpBtn.onClick.RemoveAllListeners();
            }

        }

    }

    public override void Load()
    {
        if(inputData != null)
        {
            var unitData = UIDataProcess.GetUnitInfo(inputData.iIndex);
            List<int> ItemIndexList = new List<int>();
            List<int> ItemNeedCountList = new List<int>();
            int level = inputData.iLevel;
            bool canLvUp = true;

            for (int i = 2; i >= 0; i--)
            {
                string Key = string.Format("_iItemCondition{0}", i + 1);
                int ConditionLevel = (int)UIDataProcess.GetStringTypeNameValue(unitData, Key);

                if (level < ConditionLevel)
                {
                    Destroy(itemCounts[i].iMain.gameObject);
                    itemCounts.RemoveAt(i);
                    if (i > 0)
                    {
                        Destroy(plusIcons[i - 1].gameObject);
                        plusIcons.RemoveAt(i - 1);
                    }
                }
                else
                {
                    string ItemKey = string.Format("_iItemID{0}", i + 1);
                    string InceaseKey = string.Format("_iItemNumberIncrease{0}", i + 1);
                    string NumberKey = string.Format("_iItemNumber{0}", i + 1);

                    int ItemIndex = (int)UIDataProcess.GetStringTypeNameValue(unitData, ItemKey);
                    int Increase = (int)UIDataProcess.GetStringTypeNameValue(unitData, InceaseKey);
                    int NumBer = (int)UIDataProcess.GetStringTypeNameValue(unitData, NumberKey);
                    ItemIndex += 1000;
                    ItemIndexList.Add(ItemIndex);
                    var itemData = UIDataProcess.GetItemInfo(ItemIndex);
                    itemCounts[0].iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + itemData.StrIcon.Replace("[ItemID]", ItemIndex.ToString()));
                    int NeedCount = (NumBer + Increase * (level - 1));
                    itemCounts[0].tNum.text = NeedCount.ToString();
                    itemCounts[0].tNum.color = new Color(0, 0, 0, 1);
                    ItemNeedCountList.Add(NeedCount);
                    int Count = PlayerDataManager.PlayerData.InventoryETCItemData.FindItemIndexSelectCount(ItemIndex);

                    if (Count < NeedCount)
                    {
                        canLvUp = false;
                        itemCounts[0].tNum.color = new Color(1, 0, 0, 1);
                    }
                }
            }


            /// ???? 레벨업 시 필요한 재화 데이터
            int NeedMoney = GameDataBase.Instance.UnitExpTable[level + 1].INeedMoney;
            tCost.text = NeedMoney.ToString();

            if (PlayerDataManager.PlayerData.Pdata.iCoin < NeedMoney)
            {
                canLvUp = false;
                tCost.color = new Color(1, 0, 0, 1);
            }

            if (canLvUp)
            {
                lvUpBtn.enabled = true;
                lvUpBtn.onClick.RemoveAllListeners();
                lvUpBtn.onClick.AddListener(
                    () =>
                    {
                        /// TODO :
                        /// 유닛 레벨업 구현
                        inputData.IExp -= GameDataBase.Instance.UnitExpTable[level + 1].INeedEXP;
                        lvUpBtn.enabled = false;
                        for (int i = 0; i < ItemIndexList.Count; i++)
                        {
                            if (!PlayerDataManager.PlayerData.InventoryETCItemData.ItemUse(ItemIndexList[i], ItemNeedCountList[i]))
                            {
                                UIManager.instance.CloseTopPopup();
                                return;
                            }
                        }

                        PlayerDataManager.PlayerData.Pdata.iCoin -= NeedMoney;
                        inputData.iLevel += 1;
                        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.ETCITEM_DATAFILE | PLAYERDATAFILE.UNIT_DATAFILE | PLAYERDATAFILE.USER_DATAFILE, (succed) => {
                            if (succed)
                            {
                                lvUpBtn.enabled = true;
                                UIManager.instance.CloseCountPopup(2);
                            }
                        });

                    });
            }
            else
            {
                /// 버튼 비활성화 처리
                lvUpBtn.enabled = false;
                lvUpBtn.onClick.RemoveAllListeners();
            }
        }
    }
}
