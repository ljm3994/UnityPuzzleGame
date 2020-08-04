using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

public class UIDataProcess
{
    #region 이미지 경로 변수들
    public static string GoldImagePath = "Images/PlayerDataImage/";
    public static string SteminaImagePath = "Images/PlayerDataImage/";
    public static string PlayerSkillPath = "Images/PlayerSkillImage/";
    public static string EtcItemPath = "Images/ETCItem/";
    public static string ConsumptionItemPath = "Images/ConsumptionImage/";
    public static string UnitPath = "Images/CharterImage/character_UnitInven_Images/";
    public static string UnitSkillPath = "Images/UnitSkillImage/";
    public static string UnitPositionDealerPath = "Images/PositionImage/Dealer";
    public static string UnitPositionSupporterPath = "Images/PositionImage/Supporter";
    public static string UnitPositionTankerPath = "Images/PositionImage/Tanker";
    public static string ShopGoldImage = "Images/ShopDataImage/";
    public static string ShopSteminaImage = "Images/ShopDataImage/";
    public static string PurChaseGoldImage = "Images/ShopDataImage/";
    public static string PurChaseDiamondImage = "Images/ShopDataImage/";
    public static string EnemyImagePath = "Images/CharterImage/Spc_Enemy_Image/";
    public static string EffectImagePath = "Images/EffectImage/";
    public static string UnitSpinePath = "Prefabs/Character/";
    public static string SpineDataPath = "Spine/";
    public static string MainLobbyPath = "Images/LobbyImage/";

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
                return (from item in PlayerDataManager.PlayerData.PlayerItem.ItemList where item.Value.iItemIndex == DBIndex select item.Value.iCount.ToString()).FirstOrDefault();
            case ITEMTYPE.STUFF_TYPE:
                return (from item in PlayerDataManager.PlayerData.InventoryETCItemData.ItemList where item.Value.iItemIndex == DBIndex select item.Value.iCount.ToString()).FirstOrDefault();
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
        return false;
    }
    public static bool PurchasePrice(int Price, int Count, bool PurchaseType)
    {
        if (!PurchaseType)
        {
            if (PlayerDataManager.PlayerData.Pdata.iCoin >= Price * Count)
            {
                PlayerDataManager.PlayerData.Pdata.iCoin -= Price * Count;
                return true;
            }
        }
        else
        {
            if (PlayerDataManager.PlayerData.Pdata.iDiamond >= Price * Count)
            {
                PlayerDataManager.PlayerData.Pdata.iDiamond -= Price * Count;
                return true;
            }
        }

        return false;
    }


    public static List<StageInfo> GetStageNumInfo(int Num) { return (from item in GameDataBase.Instance.StageTable where item.Value.IStageNumber == Num select item.Value).ToList(); }
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
                         where item.Value.IUnitId == iIndex
                         orderby item.Value.Type
                         select item.Key).ToArray();
        return UnitSkillKey;
    }

    public static int[] GetMonsterSkillKeys(int iIndex)
    {
        var MonsterSkillKeys = (from item in GameDataBase.Instance.MonsterSkillTable
                                where item.Value.IUnitId == iIndex
                                orderby item.Value.Type
                                select item.Key).ToArray();
        return MonsterSkillKeys;
    }

    public static UnitSkillInfo GetUnitSkillInfo(int key){ return GameDataBase.Instance.SkillTable[key]; }

    public static ItemInfo GetItemInfo(int dbIndex) {

        return dbIndex != 0 ? GameDataBase.Instance.ItemTable[dbIndex] : null;
    }

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
    /// <summary>
    /// 문자열 형식의 변수명을 통해 클래스의 멤버 변수의 값을 가져온다
    /// </summary>
    /// <typeparam name="T"> 타입</typeparam>
    /// <param name="t"> 클래스</param>
    /// <param name="TypeName"> 가져올 변수명</param>
    /// <returns>가져온 값</returns>
    public static object GetStringTypeNameValue<T>(T t, string TypeName)
    {
        FieldInfo field = t.GetType().GetField(TypeName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        return field.GetValue(t);
    }

    public static string GetUnitSkiilDesc(int Index)
    {
        var SkiilInfo = GameDataBase.Instance.SkillTable[Index];
        var UnitInfo = GameDataBase.Instance.UnitTable[SkiilInfo.IUnitId];
        return SkiilInfo.StrSkillDesc.Replace("[CharacterAtk]", UnitInfo.IUnitAtk.ToString()).Replace("[CharacterAtk*SkillEffectValue1]", (UnitInfo.IUnitAtk * SkiilInfo.ISkillEffectValue1).ToString())
            .Replace("[CharacterAtk*SkillEffectValue2]", (UnitInfo.IUnitAtk * SkiilInfo.ISkillEffectValue2).ToString()).Replace("[CharacterAtk * SkillEffectValue3]", (UnitInfo.IUnitAtk * SkiilInfo.ISkillEffectValue3).ToString())
            .Replace("[SkillEffectTurn1]", SkiilInfo.ISkillEffectTurn1.ToString()).Replace("[SkillEffectTurn2]", SkiilInfo.ISkillEffectTurn2.ToString())
            .Replace("[SkillEffectTurn3]", SkiilInfo.ISkillEffectTurn3.ToString()).Replace("[SkillEffectValue1]", SkiilInfo.ISkillEffectValue1.ToString())
            .Replace("[SkillEffectValue2]", SkiilInfo.ISkillEffectValue2.ToString()).Replace("[SkillEffectValue3]", SkiilInfo.ISkillEffectValue3.ToString());
    }

    public static string GetPlayerSkillDesc(int Index)
    {
        var SkillInfo = GameDataBase.Instance.PlayerSkillTable[Index];

        return SkillInfo.StrSkillDesc.Replace("[SkillEffectValue1]", SkillInfo.ISkillEffectValue1.ToString()).Replace("[SkillEffectValue2]", SkillInfo.ISkillEffectValue2.ToString())
            .Replace("[SkillEffectValue3]", SkillInfo.ISkillEffectValue3.ToString()).Replace("[SkillEffectTurn1]", SkillInfo.ISkillEffectTurn1.ToString())
            .Replace("[SkillEffectTurn2]", SkillInfo.ISkillEffectTurn2.ToString()).Replace("[SkillEffectTurn3]", SkillInfo.ISkillEffectTurn3.ToString());
    }

    public static string GetConsumptionItemDesc(int Index)
    {
        var ItemInfo = GameDataBase.Instance.ItemTable[Index];

        return ItemInfo.StrItemDesc.Replace("[ItemEffectValue]", ItemInfo.IEffectValue.ToString());
    }
}
