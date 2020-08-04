using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 유닛 경험치 정보
/// </summary>
public class UnitEXPInfo
{
    public static int GetGoogleSheetGID() { return 866582062; }

    [SerializeField]
    private int _iLevel;
    [SerializeField]
    private int _iNeedEXP;
    [SerializeField]
    private int _iTotalEXP;
    [SerializeField]
    private int _iNeedMoney;
    [SerializeField]
    private int _iTotalMoney;
    /// <summary>
    /// 유닛 레벨
    /// </summary>
    public int ILevel { get => _iLevel; set => _iLevel = value; }
    /// <summary>
    /// 필요 경험치 양 
    /// 1=>2 레벨업 하기 위해서 필요한 경험치는 테이블 [2]에 있음
    /// </summary>
    public int INeedEXP { get => _iNeedEXP; set => _iNeedEXP = value; }
    /// <summary>
    /// 총 경험치 양
    /// </summary>
    public int ITotalEXP { get => _iTotalEXP; set => _iTotalEXP = value; }
    /// <summary>
    /// 레벨업시 필요한 돈
    /// </summary>
    public int INeedMoney { get => _iNeedMoney; set => _iNeedMoney = value; }
    /// <summary>
    /// 총 금액
    /// </summary>
    public int ITotalMoney { get => _iTotalMoney; set => _iTotalMoney = value; }

    public UnitEXPInfo(string Level, string NeedEXP, string TotalEXP, string NeedMoney, string TotalMoney)
    {
        ILevel = DataProcess.stringToint(Level);
        INeedEXP = DataProcess.stringToint(NeedEXP);
        ITotalEXP = DataProcess.stringToint(TotalEXP);
        INeedMoney = DataProcess.stringToint(NeedMoney);
        ITotalMoney = DataProcess.stringToint(TotalMoney);
    }
}
[System.Serializable]
public class UnitEXPDataBase : SerializableDictionary<int, UnitEXPInfo> { }