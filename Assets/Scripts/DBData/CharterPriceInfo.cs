using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharterPriceInfo
{
    public static int GetGoogleSheetGID() { return 1943740000; }
    [SerializeField]
    private int _BuyCounter;
    [SerializeField]
    private int _ItemPrice;
    /// <summary>
    /// 캐릭터 구매횟수
    /// </summary>
    public int iBuyCounter { get => _BuyCounter; set => _BuyCounter = value; }
    /// <summary>
    /// 캐릭터 가격
    /// </summary>
    public int iItemPrice { get => _ItemPrice; set => _ItemPrice = value; }

    public CharterPriceInfo(string Counter, string Price)
    {
        iBuyCounter = DataProcess.stringToint(Counter);
        iItemPrice = DataProcess.stringToint(Price);
    }
}

[System.Serializable]
public class CharterPriceDataBase : SerializableDictionary<int, CharterPriceInfo> { }