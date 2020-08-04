using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//몬스터 테이블
public class MonsterInfo
{
    // 해당 시트의 ID 값을 반환한다. 시트에 따라 ID값이 달라지므로 시트마다 클래스를 만들어줘야 할듯
    public static int GetGoogleSheetGID() { return 1202119852; }
    #region 몬스터 기본 정보 변수
    [SerializeField]
    private int _iID; //몬스터의 아이디
    [SerializeField]
    private string _strName;  // 이름
    [SerializeField]
    private UNITPOSITION _Postion;  //역할
    [SerializeField]
    private int _iMaxLevel;  // 최대 레벨
    [SerializeField]
    private int _iMonsterHealth;  // 몬스터 체력
    [SerializeField]
    private int _iMonsterAtk;  // 몬스터 공격력
    [SerializeField]
    private int _iMonsterDef;  // 몬스터 방어력
    [SerializeField]
    private int _iStateIncreaseValue;  // 레벨당 스탯 증가값
    [SerializeField]
    private string _strMonsterImage;  // 몬스터 이미지
    #endregion
    #region 캡슐화
    /// <summary>
    /// 몬스터의 ID
    /// </summary>
    public int iID { get => _iID; set => _iID = value; }
    /// <summary>
    /// 몬스터의 이름
    /// </summary>
    public string strName { get => _strName; set => _strName = value; }
    /// <summary>
    /// 몬스터의 포지션(탱커, 딜러, 서포터)
    /// </summary>
    public UNITPOSITION Postion { get => _Postion; set => _Postion = value; }
    /// <summary>
    /// 몬스터의 최대 레벨
    /// </summary>
    public int iMaxLevel { get => _iMaxLevel; set => _iMaxLevel = value; }
    /// <summary>
    /// 기본 몬스터 체력
    /// </summary>
    public int IMonsterHealth { get => _iMonsterHealth; set => _iMonsterHealth = value; }
    /// <summary>
    /// 기본 몬스터 공격력
    /// </summary>
    public int IMonsterAtk { get => _iMonsterAtk; set => _iMonsterAtk = value; }
    /// <summary>
    /// 기본 몬스터 방어력
    /// </summary>
    public int IMonsterDef { get => _iMonsterDef; set => _iMonsterDef = value; }
    /// <summary>
    /// 레벨 당 증가하는 스텟 비율
    /// </summary>
    public int IStateIncreaseValue { get => _iStateIncreaseValue; set => _iStateIncreaseValue = value; }
    /// <summary>
    /// 몬스터 이미지 명
    /// </summary>
    public string StrMonsterImage { get => _strMonsterImage; set => _strMonsterImage = value; }
    #endregion
    #region 생성자
    public MonsterInfo(string _iID, string _strName, string _Postion, string _iMaxLevel, string Health, string Atk,
        string Def, string IcreaseValue, string Image)
    {
        iID = DataProcess.stringToint(_iID);
        strName = DataProcess.stringToNull(_strName);
        Postion = (UNITPOSITION)DataProcess.stringToint(_Postion);
        iMaxLevel = DataProcess.stringToint(_iMaxLevel);
        IMonsterHealth = DataProcess.stringToint(Health);
        IMonsterAtk = DataProcess.stringToint(Atk);
        IMonsterDef = DataProcess.stringToint(Def);
        IStateIncreaseValue = DataProcess.stringToint(IcreaseValue);
        StrMonsterImage = DataProcess.stringToNull(Image);
    }
    #endregion
}

[System.Serializable]
public class MonsterDataBase : SerializableDictionary<int, MonsterInfo> { }
