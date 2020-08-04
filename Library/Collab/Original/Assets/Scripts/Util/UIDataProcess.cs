using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UIDataProcess
{
    #region 이미지 경로 변수들
    public static string GoldImagePath = "Images/PlayerDataImage/";
    public static string SteminaImagePath = "Images/PlayerDataImage/";
    public static string PlayerSkillPath = "Images/PlayerSkillImage/";
    public static string EtcItemPath = "Images/ETCItem/";
    public static string ConsumptionItemPath = "Images/ConsumptionImage/";
    public static string UnitPath = "Images/UnitImage/";
    public static string UnitSkillPath = "Images/UnitSkillImage/";
    public static string UnitPositionDealerPath = "Images/PositionImage/Dealer";
    public static string UnitPositionSupporterPath = "Images/PositionImage/Supporter";
    public static string UnitPositionTankerPath = "Images/PositionImage/Tanker";
    public static string ShopGoldImage = "Images/ShopDataImage/";
    public static string ShopSteminaImage = "Images/ShopDataImage/";
    public static string PurChaseGoldImage = "Images/ShopDataImage/";
    public static string PurChaseDiamondImage = "Images/ShopDataImage/";
    #endregion

    #region 플레이어 골드 정보
    /// <summary>
    /// 플레이어의 골드 정보를 가져온다
    /// </summary>
    /// <param name="element"> 어떠한 요소를 가져올지 변수</param>
    /// <returns></returns>
    public static string GetGoldInfo(UI_DATA.LOAD_ELEMENT_BASIC element)
    {
        switch (element)
        {
            case UI_DATA.LOAD_ELEMENT_BASIC.IMAGE:
                return GoldImagePath;
            case UI_DATA.LOAD_ELEMENT_BASIC.TEXT:
                return PlayerDataManager.PlayerData.Pdata.iCoin.ToString();
            default:
                return "";
        }
    }
    #endregion
    #region 플레이어 스테미너 정보
    /// <summary>
    /// 플레이어의 스테미너 정보를 가져온다
    /// </summary>
    /// <param name="element"> 어떠한 요소를 가져올지 변수</param>
    /// <returns></returns>
    public static string GetSteminaInfo(UI_DATA.LOAD_ELEMENT_BASIC element)
    {
        switch (element)
        {
            case UI_DATA.LOAD_ELEMENT_BASIC.IMAGE:
                return SteminaImagePath;
            case UI_DATA.LOAD_ELEMENT_BASIC.TEXT:
                 return PlayerDataManager.PlayerData.Pdata.iStamina.ToString() + "/" + GameDataBase.Instance.CharterTable[PlayerDataManager.PlayerData.Pdata.ILevel].iStamina.ToString();
            case UI_DATA.LOAD_ELEMENT_BASIC.TIMERTEXT:
                if(SteminaManager.Instance.SteminaChargeTimer > 0)
                {
                    int Time = SteminaManager.Instance.SteminaChargeTimer;
                    return (Time / 60).ToString() + "m " + (Time % 60).ToString() + "s";
                }
                return "";
            default:
                return "";
        }
    }
    #endregion
    #region 플레이어 장착 스킬 정보
    /// <summary>
    /// 플레이어가 장착한 스킬 정보
    /// </summary>
    /// <param name="Index">장착 스킬의 DB 인덱스</param>
    /// <param name="element"> 어떤 요소를 가져올지 변수</param>
    /// <returns></returns>
    public static string GetUserSkillInfo(int DBIndex, int EquipIndex, UI_DATA.LOAD_ELEMENT_USER_SKILL element)
    {
        //PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills[index].iIndex,EquipIndex
        //PlayerDataManager.PlayerData.SkillInventory.SkillList[index].iIndex,EquipIndex
        //-------------------------(PlayerSkill)inventory.SkillList[i]
        switch ((UI_DATA.LOAD_ELEMENT_USER_SKILL)element)
        {
            case UI_DATA.LOAD_ELEMENT_USER_SKILL.NAME:
                return GameDataBase.Instance.PlayerSkillTable[DBIndex + EquipIndex].StrSkillName;
            case UI_DATA.LOAD_ELEMENT_USER_SKILL.IMAGE:
                return PlayerSkillPath + GameDataBase.Instance.PlayerSkillTable[DBIndex + EquipIndex].StrSkillIcon.Replace("[SkillID]", DBIndex.ToString());
            case UI_DATA.LOAD_ELEMENT_USER_SKILL.DESC:
                return GameDataBase.Instance.PlayerSkillTable[DBIndex + EquipIndex].StrSkillDesc;
            case UI_DATA.LOAD_ELEMENT_USER_SKILL.EQUIPTEXT:
                bool Equip = (from item in PlayerDataManager.PlayerData.SkillInventory.SkillList where item.Value.iIndex == GameDataBase.Instance.PlayerSkillTable[DBIndex].ISkillId select item.Value.BEquipped).Single();
                if(Equip)
                {
                    return "해제";
                }
                else
                {
                    return "장착";
                }
            case UI_DATA.LOAD_ELEMENT_USER_SKILL.EQUIP:
                bool Equipstr = (from item in PlayerDataManager.PlayerData.SkillInventory.SkillList where item.Value.iIndex == DBIndex select item.Value.BEquipped).First();
                return Equipstr.ToString();
            default:
                return "";
        }
    }
    #endregion
    #region 유저 데이터
    /// <summary>
    /// 유저 데이터를 가져온다
    /// </summary>
    /// <param name="element">어떠한 요소를 가져올지 변수</param>
    /// <returns></returns>
    public static string GetUserDataInfo(UI_DATA.LOAD_ELEMENT_USER element)
    {
        switch ((UI_DATA.LOAD_ELEMENT_USER)element)
        {
            case UI_DATA.LOAD_ELEMENT_USER.NAME:
                return PlayerDataManager.PlayerData.Pdata.StrName;
            case UI_DATA.LOAD_ELEMENT_USER.IMAGE3D:
                return "";
            case UI_DATA.LOAD_ELEMENT_USER.LEVEL:
                return PlayerDataManager.PlayerData.Pdata.ILevel.ToString();
            case UI_DATA.LOAD_ELEMENT_USER.EXP:
                return PlayerDataManager.PlayerData.Pdata.IPlayerEXP.ToString();
            default:
                return "";
        }
    }
    #endregion
    #region 유닛 정보
    /// <summary>
    /// 유닛의 정보를 가져온다
    /// </summary>
    /// <param name="playerUnit">플레이어 유닛 데이터</param>
    /// <param name="element">가져올 요소</param>
    /// <returns></returns>
    public static string GetUnitInfo(PlayerUnit playerUnit, int InventoryIndex, UI_DATA.LOAD_ELEMENT_UNIT element)
    {
        //playerUnit
        //
        if (playerUnit == null) return "";
        int UnitIndex = playerUnit.iIndex;
        var UnitSkill = (from item in GameDataBase.Instance.SkillTable
                         where item.Value.ICharteId == UnitIndex
                         orderby item.Value.Type
                         select item.Key).ToArray();
        switch ((UI_DATA.LOAD_ELEMENT_UNIT)element)
        {
            case UI_DATA.LOAD_ELEMENT_UNIT.NAME:
                return GameDataBase.Instance.UnitTable[UnitIndex].strName;
            case UI_DATA.LOAD_ELEMENT_UNIT.IMAGE:
                return UnitPath + GameDataBase.Instance.UnitTable[UnitIndex].StrUnitImage.Replace("[CharacterID]", UnitIndex.ToString());
            case UI_DATA.LOAD_ELEMENT_UNIT.STATUS_DESC:
                return "체력 : " + GameDataBase.Instance.UnitTable[UnitIndex].IUnitHealth.ToString() +
                    "   공격력 : " + GameDataBase.Instance.UnitTable[UnitIndex].IUnitAtk.ToString() +
                    "   방어력 : " + GameDataBase.Instance.UnitTable[UnitIndex].IUnitDef.ToString();
            case UI_DATA.LOAD_ELEMENT_UNIT.LEVEL:
                return "lv." + playerUnit.iLevel.ToString();
            case UI_DATA.LOAD_ELEMENT_UNIT.EXP:
                return playerUnit.IExp.ToString();
            case UI_DATA.LOAD_ELEMENT_UNIT.MAXEXP:
                return GameDataBase.Instance.UnitExpTable[playerUnit.iLevel + 1].INeedEXP.ToString();
            case UI_DATA.LOAD_ELEMENT_UNIT.SKILL_NAME:
                return GameDataBase.Instance.SkillTable[UnitSkill[1]].StrSkillName;
            case UI_DATA.LOAD_ELEMENT_UNIT.SKILL_IMAGE:
                return UnitSkillPath + GameDataBase.Instance.SkillTable[UnitSkill[1]].StrSkillIcon.Replace("[SkillID]", UnitSkill[1].ToString());
            case UI_DATA.LOAD_ELEMENT_UNIT.SKILL_DESC:
                return GameDataBase.Instance.SkillTable[UnitSkill[1]].StrSkillDesc;
            case UI_DATA.LOAD_ELEMENT_UNIT.ATTACK_NAME:
                return GameDataBase.Instance.SkillTable[UnitSkill[0]].StrSkillName;
            case UI_DATA.LOAD_ELEMENT_UNIT.ATTACK_IMAGE:
                return UnitSkillPath + GameDataBase.Instance.SkillTable[UnitSkill[0]].StrSkillIcon.Replace("[SkillID]", UnitSkill[0].ToString());
            case UI_DATA.LOAD_ELEMENT_UNIT.ATTACK_DESC:
                return GameDataBase.Instance.SkillTable[UnitSkill[0]].StrSkillDesc;
            case UI_DATA.LOAD_ELEMENT_UNIT.POSITION:
                return GameDataBase.Instance.UnitTable[UnitIndex].Position.ToString();
            case UI_DATA.LOAD_ELEMENT_UNIT.EQUIPMENT:
                if(playerUnit.bEquipped)
                {
                    return "해제";
                }
                else
                {
                    return "장착";
                }
            case UI_DATA.LOAD_ELEMENT_UNIT.IMG_POSITION:
                {
                    switch (GameDataBase.Instance.UnitTable[UnitIndex].Position)
                    {
                        case UNITPOSITION.DEALER_POSITION:
                            return UnitPositionDealerPath;
                        case UNITPOSITION.SUPPORTER_POSITION:
                            return UnitPositionSupporterPath;
                        case UNITPOSITION.TANKER_POSITION:
                            return UnitPositionTankerPath;
                        default:
                            return "";
                    }
                }
            default:
                return "";
        }
    }
    #endregion
    #region 아이템 정보
    public static string DBItemInfo(int DBIndex, UI_DATA.LOAD_ELEMENT_ITEM element)
    {
        //PlayerDataManager.PlayerData.PlayerItem.ItemList[index].iItemIndex
        //-----------------------(CharterItem)inventory[index]
        switch ((UI_DATA.LOAD_ELEMENT_ITEM)element)
        {
            case UI_DATA.LOAD_ELEMENT_ITEM.NAME:
                return GameDataBase.Instance.ItemTable[DBIndex].StrItemName;
            case UI_DATA.LOAD_ELEMENT_ITEM.IMAGE:
                if (GameDataBase.Instance.ItemTable[DBIndex].Type == ITEMTYPE.STUFF_TYPE)
                {
                    return EtcItemPath + GameDataBase.Instance.ItemTable[DBIndex].StrIcon.Replace("[ItemID]", DBIndex.ToString());
                }
                else if(GameDataBase.Instance.ItemTable[DBIndex].Type == ITEMTYPE.EXPENDABLES_TYPE)
                {
                    return ConsumptionItemPath + GameDataBase.Instance.ItemTable[DBIndex].StrIcon.Replace("[ItemID]", DBIndex.ToString());
                }
                else
                {
                    return "";
                }
            case UI_DATA.LOAD_ELEMENT_ITEM.DESC:
                return GameDataBase.Instance.ItemTable[DBIndex].StrItemDesc;
            case UI_DATA.LOAD_ELEMENT_ITEM.NUM:
                return ItemConut(GameDataBase.Instance.ItemTable[DBIndex].Type, DBIndex);
            default:
                return "";
        }
    }
    #endregion
    #region 상점 유닛 정보
    public static string GetShopUnitInfo(int index, UI_DATA.LOAD_ELEMENT_SHOP_UNIT element)
    {
        // GameDataBase.Instance.ShopTable[index]
        //----------------------------(ShopInfo)
        int DBIndex = GameDataBase.Instance.ShopTable[index].IItemID;
        var UnitSkill = (from item in GameDataBase.Instance.SkillTable
                         where item.Value.ICharteId == DBIndex && item.Value.Type == SKILLTYPE.SKILL_TYPE
                         orderby item.Value.Type
                         select item.Key).ToArray();
        switch (element)
        {
            case UI_DATA.LOAD_ELEMENT_SHOP_UNIT.NAME:
                return GameDataBase.Instance.ShopTable[index].StrItemName;
            case UI_DATA.LOAD_ELEMENT_SHOP_UNIT.IMAGE:
                return UnitPath + GameDataBase.Instance.UnitTable[DBIndex].StrUnitImage.Replace("[CharacterID]", DBIndex.ToString());
            case UI_DATA.LOAD_ELEMENT_SHOP_UNIT.STATUS_DESC:
                {
                    string str = "";
                    str = "체력 : " + GameDataBase.Instance.UnitTable[DBIndex].IUnitHealth.ToString() +
                        " / " + "공격력 : " + GameDataBase.Instance.UnitTable[DBIndex].IUnitAtk.ToString();
                    if (UnitSkill.Length > 0)
                    {
                        if (GameDataBase.Instance.SkillTable.ContainsKey(UnitSkill[0]))
                        {
                            str += "\r\n" + "스킬 명 : " + GameDataBase.Instance.SkillTable[UnitSkill[0]].StrSkillName;
                            str += "\r\n" + "스킬 정보 : " + GameDataBase.Instance.SkillTable[UnitSkill[0]].StrSkillDesc;
                        }
                        else
                        {
                            str += "\r\n" + "스킬 명 : ";
                            str += "\r\n" + "스킬 정보 : ";
                        }
                    }
                    else
                    {
                        str += "\r\n" + "스킬 명 : ";
                        str += "\r\n" + "스킬 정보 : ";
                    }
                    return str;
                }
            case UI_DATA.LOAD_ELEMENT_SHOP_UNIT.LEVEL:
                return "1";
            case UI_DATA.LOAD_ELEMENT_SHOP_UNIT.IMG_POSITION:
                {
                    switch(GameDataBase.Instance.UnitTable[DBIndex].Position)
                    {
                        case UNITPOSITION.DEALER_POSITION:
                            return UnitPositionDealerPath;
                        case UNITPOSITION.SUPPORTER_POSITION:
                            return UnitPositionSupporterPath;
                        case UNITPOSITION.TANKER_POSITION:
                            return UnitPositionTankerPath;
                        default:
                            return "";
                    }
                }
            case UI_DATA.LOAD_ELEMENT_SHOP_UNIT.PRICE:
                var UnitObject = (from item in PlayerDataManager.PlayerData.UnitInventory.UintList where item.Value.iIndex == DBIndex select item.Key).ToArray();
                if(UnitObject.Length > 0 && UnitObject.Length < GameDataBase.Instance.CharterPriceTable.Count)
                {
                    return GameDataBase.Instance.CharterPriceTable[UnitObject.Length].iItemPrice.ToString();
                }
                else if(UnitObject.Length >= GameDataBase.Instance.CharterPriceTable.Count)
                {
                    return GameDataBase.Instance.CharterPriceTable[GameDataBase.Instance.CharterPriceTable.Count].iItemPrice.ToString();
                }
                else
                {
                    return GameDataBase.Instance.CharterPriceTable[0].iItemPrice.ToString();
                }
            case UI_DATA.LOAD_ELEMENT_SHOP_UNIT.PRICETYPE:
                {
                    if (GameDataBase.Instance.ShopTable[index].BPurchaseType)
                    {
                        return PurChaseGoldImage;
                    }
                    return PurChaseDiamondImage;
                }
            default:
                return "";
        }
    }
    #endregion
    #region 상점 아이템 정보
    public static string GetShopItemInfo(int Index, UI_DATA.LOAD_ELEMENT_SHOP_ITEM elemnt)
    {
        //GameDataBase.Instance.ShopTable[Index]
        //----------------------------(ShopInfo)
        int DBIndex = GameDataBase.Instance.ShopTable[Index].IItemID;
        switch (elemnt)
        {
            case UI_DATA.LOAD_ELEMENT_SHOP_ITEM.NAME:
                return GameDataBase.Instance.ShopTable[Index].StrItemName;
            case UI_DATA.LOAD_ELEMENT_SHOP_ITEM.IMAGE:
                return ConsumptionItemPath + GameDataBase.Instance.ItemTable[DBIndex].StrIcon.Replace("[ItemID]", DBIndex.ToString());
            case UI_DATA.LOAD_ELEMENT_SHOP_ITEM.DESC:
                return GameDataBase.Instance.ItemTable[DBIndex].StrItemDesc;
            case UI_DATA.LOAD_ELEMENT_SHOP_ITEM.PRICE:
                return GameDataBase.Instance.ShopTable[Index].IItemValue.ToString();
            case UI_DATA.LOAD_ELEMENT_SHOP_ITEM.PRICETYPE:
                {
                    if (GameDataBase.Instance.ShopTable[Index].BPurchaseType)
                    {
                        return PurChaseGoldImage;
                    }
                    return PurChaseDiamondImage;
                }
            case UI_DATA.LOAD_ELEMENT_SHOP_ITEM.NAMEANDCOUNT:
                return GameDataBase.Instance.ShopTable[Index].StrItemName + " (보유 : " + PlayerDataManager.PlayerData.PlayerItem.FindItemCount(DBIndex).ToString() + ")";
            default:
                return "";
        }
    }
    #endregion
    #region 상점 골드 정보
    public static string GetShopGoldInfo(int index, UI_DATA.LOAD_ELEMENT_SHOP_GOLD element)
    {
        int DBIndex = GameDataBase.Instance.ShopTable[index].IItemID;
        switch (element)
        {
            case UI_DATA.LOAD_ELEMENT_SHOP_GOLD.NAME:
                return GameDataBase.Instance.ShopTable[index].StrItemName;
            case UI_DATA.LOAD_ELEMENT_SHOP_GOLD.IMAGE:
                return ShopGoldImage;
            case UI_DATA.LOAD_ELEMENT_SHOP_GOLD.PRICE:
                return GameDataBase.Instance.ShopTable[index].IItemValue.ToString();
            case UI_DATA.LOAD_ELEMENT_SHOP_GOLD.PRICETYPE:
                {
                    if (GameDataBase.Instance.ShopTable[index].BPurchaseType)
                    {
                        return PurChaseGoldImage;
                    }
                    return PurChaseDiamondImage;
                }
            case UI_DATA.LOAD_ELEMENT_SHOP_GOLD.DESC:
                {
                    return GameDataBase.Instance.ShopTable[index].StrItemDesc;
                }
            default:
                return "";
        }
    }
    #endregion
    #region 상점 스테미나 정보
    public static string GetShopSteminaInfo(int index, UI_DATA.LOAD_ELEMENT_SHOP_STEMINA element)
    {
        switch (element)
        {
            case UI_DATA.LOAD_ELEMENT_SHOP_STEMINA.NAME:
                return GameDataBase.Instance.ShopTable[index].StrItemName;
            case UI_DATA.LOAD_ELEMENT_SHOP_STEMINA.IMAGE:
                return ShopSteminaImage;
            case UI_DATA.LOAD_ELEMENT_SHOP_STEMINA.PRICE:
                return GameDataBase.Instance.ShopTable[index].IItemValue.ToString();
            case UI_DATA.LOAD_ELEMENT_SHOP_STEMINA.PRICETYPE:
                {
                    if (GameDataBase.Instance.ShopTable[index].BPurchaseType)
                    {
                        return PurChaseGoldImage;
                    }
                    return PurChaseDiamondImage;
                }
            case UI_DATA.LOAD_ELEMENT_SHOP_STEMINA.DESC:
                {
                    return GameDataBase.Instance.ShopTable[index].StrItemDesc;
                }
            default:
                return "";
        }

    }
    #endregion

    #region 상점 유닛 구매
    public static bool ShopUnitPurchase(int Index)
    {
        int DBindex = GameDataBase.Instance.ShopTable[Index].IItemID;
        Debug.Log(Index.ToString() + " - Index");
        Debug.Log(DBindex.ToString() + " - DBIndex");
        int Price = GameDataBase.Instance.ShopTable[Index].IItemValue;
        var UnitObject = (from item in PlayerDataManager.PlayerData.UnitInventory.UintList where item.Value.iIndex == DBindex select item.Key).ToArray();
        if (UnitObject.Length > 0 && UnitObject.Length < GameDataBase.Instance.CharterPriceTable.Count)
        {
            Price = GameDataBase.Instance.CharterPriceTable[UnitObject.Length].iItemPrice;
        }
        else if (UnitObject.Length >= GameDataBase.Instance.CharterPriceTable.Count)
        {
            Price = GameDataBase.Instance.CharterPriceTable[GameDataBase.Instance.CharterPriceTable.Count].iItemPrice;
        }
        else
        {
            Price = GameDataBase.Instance.CharterPriceTable[0].iItemPrice;
        }

        if (PurchasePrice(Price, GameDataBase.Instance.ShopTable[Index].BPurchaseType))
        {
            Debug.Log(DBindex.ToString() + " - 2DBIndex");
            PlayerDataManager.PlayerData.UnitInventory.UnitAdd(DBindex);
            return true;
        }

        return false;
    }
    #endregion
    #region 상점 아이템 구매
    public static bool ShopItemPurchase(int Index, int Count)
    {
        int DBindex = GameDataBase.Instance.ShopTable[Index].IItemID;
        int Price = GameDataBase.Instance.ShopTable[Index].IItemValue;
        Price *= Count;
        if (PurchasePrice(Price, GameDataBase.Instance.ShopTable[Index].BPurchaseType) && Price > 0)
        {
            PlayerDataManager.PlayerData.PlayerItem.AddItem(DBindex, Count);
            return true;
        }

        return false;
    }
    #endregion
    #region 상점 골드 구매
    public static bool ShopGoldPurchase(int Index)
    {
        int Price = GameDataBase.Instance.ShopTable[Index].IItemValue;
        int Value = GameDataBase.Instance.ShopTable[Index].IItemGetValue;
        if (PurchasePrice(Price, GameDataBase.Instance.ShopTable[Index].BPurchaseType))
        {
            PlayerDataManager.PlayerData.Pdata.iCoin += Value;
            return true;
        }
        return false;
    }
    #endregion
    #region 상점 스테미너 구매
    public static bool ShopSteminaPurchase(int Index)
    {
        int Price = GameDataBase.Instance.ShopTable[Index].IItemValue;
        int Value = GameDataBase.Instance.ShopTable[Index].IItemGetValue;
        if (PurchasePrice(Price, GameDataBase.Instance.ShopTable[Index].BPurchaseType))
        {
            PlayerDataManager.PlayerData.Pdata.iStamina += Value;
            return true;
        }
        return false;
    }
    #endregion
    public static string ItemConut(ITEMTYPE Type, int DBIndex)
    {
        switch (Type)
        {
            case ITEMTYPE.EXPENDABLES_TYPE:
                return (from item in PlayerDataManager.PlayerData.PlayerItem.ItemList where item.Value.iItemIndex == DBIndex select item.Value.iCount.ToString()).First();
            case ITEMTYPE.STUFF_TYPE:
                return (from item in PlayerDataManager.PlayerData.InventoryETCItemData.ItemList where item.Value.iItemIndex == DBIndex select item.Value.iCount.ToString()).First();
            default:
                return "";
        }
    }
    public static bool PurchasePrice(int Price, bool PurchaseType)
    {
        if (!PurchaseType)
        {
            if (PlayerDataManager.PlayerData.Pdata.iCoin >= Price)
            {
                PlayerDataManager.PlayerData.Pdata.iCoin -= Price;
                return true;
            }
        }
        else
        {
            if (PlayerDataManager.PlayerData.Pdata.iDiamond >= Price)
            {
                PlayerDataManager.PlayerData.Pdata.iDiamond -= Price;
                return true;
            }
        }
        PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.USER_DATAFILE);
        return false;
    }
    public static bool PurchasePrice(int Price, int Count, bool PurchaseType)
    {
        if (!PurchaseType)
        {
            if (PlayerDataManager.PlayerData.Pdata.iCoin >= Price * Count)
            {
                return true;
            }
        }
        else
        {
            if (PlayerDataManager.PlayerData.Pdata.iDiamond >= Price * Count)
            {
                return true;
            }
        }

        return false;
    }



    public static InventorSkill GetSkillInventory() { return PlayerDataManager.PlayerData.SkillInventory; }
    public static UintInventory GetUnitInventory() { return PlayerDataManager.PlayerData.UnitInventory; }
    public static InventoryItem GetConsumeItemInventory() { return PlayerDataManager.PlayerData.PlayerItem; }
    public static InventoryETCItemData GetETCItemInventory() { return PlayerDataManager.PlayerData.InventoryETCItemData; }
    public static PlayerData GetPlayerData() { return PlayerDataManager.PlayerData.Pdata; }

    public static PlayerSkillInfo GetPlayerSkillInfo(int DBIndex, int EquipIndex) { return GameDataBase.Instance.PlayerSkillTable[DBIndex + EquipIndex]; }
    public static UnitInfo GetUnitInfo(int iIndex) { return GameDataBase.Instance.UnitTable[iIndex]; }
    /// <summary>
    /// 유닛의 스킬 정보 가져오기
    /// </summary>
    /// <param name="iIndex">PlayerData.iIndex</param>
    /// <returns>array 0 : 공격  1: 스킬</returns>
    public static int[] GetUnitSkillKeys(int iIndex)
    {
        var UnitSkillKey = (from item in GameDataBase.Instance.SkillTable
                         where item.Value.ICharteId == iIndex
                         orderby item.Value.Type
                         select item.Key).ToArray();
        return UnitSkillKey;
    }

    public static CharterSkillInfo GetUnitSkillInfo(int key){ return GameDataBase.Instance.SkillTable[key]; }

    public static ItemInfo GetItemInfo(int dbIndex) { return GameDataBase.Instance.ItemTable[dbIndex]; }

    public static ShopInfo GetShopInfo(int dbIndex) { return GameDataBase.Instance.ShopTable[dbIndex]; }

    public static int GetUnitPrice(int dbIndex)
    {
        var UnitObject = (from item in PlayerDataManager.PlayerData.UnitInventory.UintList where item.Value.iIndex == dbIndex select item.Key).ToArray();
        if (UnitObject.Length > 0 && UnitObject.Length < GameDataBase.Instance.CharterPriceTable.Count)
        {
            return GameDataBase.Instance.CharterPriceTable[UnitObject.Length].iItemPrice;
        }
        else if (UnitObject.Length >= GameDataBase.Instance.CharterPriceTable.Count)
        {
            return GameDataBase.Instance.CharterPriceTable[GameDataBase.Instance.CharterPriceTable.Count].iItemPrice;
        }
        else
        {
            return GameDataBase.Instance.CharterPriceTable[0].iItemPrice;
        }
    }
}
