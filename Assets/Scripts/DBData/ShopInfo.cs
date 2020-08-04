using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopInfo
{
    public static int GetGoogleSheetGID() { return 1237531873; }
    [SerializeField]
    private int _iItemIndex;
    [SerializeField]
    private SHOPITEM_TYPE _ItemType;
    [SerializeField]
    private int _iItemID;
    [SerializeField]
    private string _strItemName;
    [SerializeField]
    private bool _bPurchaseType;
    [SerializeField]
    private int _iItemValue;
    [SerializeField]
    private string _strItemDesc;
    [SerializeField]
    private int _iItemGetValue;
    /// <summary>
    /// 항목 인덱스
    /// </summary>
    public int IItemIndex { get => _iItemIndex; set => _iItemIndex = value; }
    /// <summary>
    /// 상점에서 판매하는 항목 종류
    /// </summary>
    public SHOPITEM_TYPE ItemType { get => _ItemType; set => _ItemType = value; }
    /// <summary>
    /// 항목의 ID(1=캐릭터ID, 4=아이템ID)
    /// </summary>
    public int IItemID { get => _iItemID; set => _iItemID = value; }
    /// <summary>
    /// 아이템 이름
    /// </summary>
    public string StrItemName { get => _strItemName; set => _strItemName = value; }
    /// <summary>
    /// 구매 재화 종류 (골드/원화)
    /// </summary>
    public bool BPurchaseType { get => _bPurchaseType; set => _bPurchaseType = value; }
    /// <summary>
    /// 아이템 가격
    /// </summary>
    public int IItemValue { get => _iItemValue; set => _iItemValue = value; }
    /// <summary>
    /// 아이템 설명
    /// </summary>
    public string StrItemDesc { get => _strItemDesc; set => _strItemDesc = value; }
    /// <summary>
    /// 아이템 구매시 증가 값(골드, 스테미너의 경우)
    /// </summary>
    public int IItemGetValue { get => _iItemGetValue; set => _iItemGetValue = value; }

    public ShopInfo(string Index, string Type, string ID, string Name, string Purchase, string Value, string Desc, string GetValue)
    {
        IItemIndex = DataProcess.stringToint(Index);
        ItemType = (SHOPITEM_TYPE)DataProcess.stringToint(Type);
        IItemID = DataProcess.stringToint(ID);
        StrItemName = DataProcess.stringToNull(Name);
        BPurchaseType = DataProcess.stringTobool(Purchase);
        IItemValue = DataProcess.stringToint(Value);
        StrItemDesc = DataProcess.stringToNull(Desc);
        IItemGetValue = DataProcess.stringToint(GetValue);
    }
}

[System.Serializable] 
public class ShopDataBase : SerializableDictionary<int, ShopInfo> { }