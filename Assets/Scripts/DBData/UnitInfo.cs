using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// 유닛 테이블
public class UnitInfo
{
    // 해당 시트의 ID 값을 반환한다. 시트에 따라 ID값이 달라지므로 시트마다 클래스를 만들어줘야 할듯
    public static int GetGoogleSheetGID() { return 0; }
    #region 유닛 기본 정보 변수
    [SerializeField]
    private int _iID;         // 유닛 아이디 값
    [SerializeField]
    private string _strName;  // 유닛 이름
    [SerializeField]
    private UNITPOSITION _Position;  // 유닛 역할
    [SerializeField]
    private int _iMaxLevel;         // 최대 레벨
    [SerializeField]
    private string _strUnitImage;    // 유닛 이미지 명
    #endregion
    #region 유닛 첫번쨰 재료 변수
    [SerializeField]
    private int _iItemCondition1;         // 재료 아이템 사용 레벨  1 = lv1 -> lv2
    [SerializeField]
    private int _iItemID1;          // 재료 아이템 아이디
    [SerializeField]
    private int _iItemNumber1;    // 재료 아이템 초기 갯수
    [SerializeField]
    private int _iItemNumberIncrease1;    // 레벨 당 증가하는 재료 아이템
    #endregion
    #region 유닛 두번쨰 재료 변수
    [SerializeField]
    private int _iItemCondition2;    // 재료 아이템 사용 레벨   11 = lv11->lv12
    [SerializeField]
    private int _iItemID2;    // 재료 아이템 아이디
    [SerializeField]
    private int _iItemNumber2;    // 재료 아이템 초기 갯수
    [SerializeField]
    private int _iItemNumberIncrease2;    // 레벨 당 증가하는 재료 아이템
    #endregion
    #region 유닛 세번쨰 재료 변수
    [SerializeField]
    private int _iItemCondition3;    // 재료 아이템 사용 레벨   21 = lv21->lv22
    [SerializeField]
    private int _iItemID3;    // 재료 아이템 아이디
    [SerializeField]
    private int _iItemNumber3;    // 재료 아이템 초기 갯수
    [SerializeField]
    private int _iItemNumberIncrease3;    // 레벨 당 증가하는 재료 아이템
    #endregion
    #region 유닛 스탯 변수
    [SerializeField]
    private int _iUnitHealth;    // 캐릭터 체력
    [SerializeField]
    private int _iUnitAtk;    // 캐릭터 공격력
    [SerializeField]
    private int _iUnitDef;    // 캐릭터 방어력
    [SerializeField]
    private int _iStateIncreaseValue;    // 레벨 당 증가하는 스텟 값 백분율
    #endregion

    #region 캡슐화
        #region 유닛 기본 정보 변수 캡슐화
    /// <summary>
    /// 유닛 아이디 값
    /// </summary>
    public int iID { get => _iID; set => _iID = value; }
    /// <summary>
    /// 유닛 이름
    /// </summary>
    public string strName { get => _strName; set => _strName = value; }
    /// <summary>
    /// 유닛 역할(탱커, 딜러, 서포터)
    /// </summary>
    public UNITPOSITION Position { get => _Position; set => _Position = value; }
    /// <summary>
    /// 유닛 최대 레벨
    /// </summary>
    public int iMaxLevel { get => _iMaxLevel; set => _iMaxLevel = value; }
    /// <summary>
    /// 유닛 이미지 이름
    /// </summary>
    public string StrUnitImage { get => _strUnitImage; set => _strUnitImage = value; }
    #endregion
        #region 유닛 첫번쨰 재료 변수 캡슐화
    /// <summary>
    /// 1번쨰 재료 아이템 사용 레벨  1 = lv1 -> lv2
    /// </summary>
    public int IItemCondition1 { get => _iItemCondition1; set => _iItemCondition1 = value; }
    /// <summary>
    /// 1번쨰 재료 아이템 아이디
    /// </summary>
    public int IItemID1 { get => _iItemID1; set => _iItemID1 = value; }
    /// <summary>
    /// 1번쨰 재료 아이템 초기 갯수
    /// </summary>
    public int IItemNumber1 { get => _iItemNumber1; set => _iItemNumber1 = value; }
    /// <summary>
    /// 1번쨰 레벨 당 증가하는 재료 아이템
    /// </summary>
    public int IItemNumberIncrease1 { get => _iItemNumberIncrease1; set => _iItemNumberIncrease1 = value; }
    #endregion
        #region 유닛 두번쨰 재료 변수 캡슐화
    /// <summary>
    /// 2번쨰 재료 아이템 사용 레벨  11 = lv11->lv12
    /// </summary>
    public int IItemCondition2 { get => _iItemCondition2; set => _iItemCondition2 = value; }
    /// <summary>
    /// 2번쨰 재료 아이템 아이디
    /// </summary>
    public int IItemID2 { get => _iItemID2; set => _iItemID2 = value; }
    /// <summary>
    /// 2번쨰 재료 아이템 초기 갯수
    /// </summary>
    public int IItemNumber2 { get => _iItemNumber2; set => _iItemNumber2 = value; }
    /// <summary>
    /// 2번쨰 레벨 당 증가하는 재료 아이템
    /// </summary>
    public int IItemNumberIncrease2 { get => _iItemNumberIncrease2; set => _iItemNumberIncrease2 = value; }
    #endregion
        #region 유닛 세번쨰 재료 변수 캡슐화
    /// <summary>
    /// 3번쨰 재료 아이템 사용 레벨 21 = lv21->lv22
    /// </summary>
    public int IItemCondition3 { get => _iItemCondition3; set => _iItemCondition3 = value; }
    /// <summary>
    /// 3번쨰 재료 아이템 아이디
    /// </summary>
    public int IItemID3 { get => _iItemID3; set => _iItemID3 = value; }
    /// <summary>
    /// 3번쨰 재료 아이템 초기 갯수
    /// </summary>
    public int IItemNumber3 { get => _iItemNumber3; set => _iItemNumber3 = value; }
    /// <summary>
    /// 3번쨰 레벨 당 증가하는 재료 아이템
    /// </summary>
    public int IItemNumberIncrease3 { get => _iItemNumberIncrease3; set => _iItemNumberIncrease3 = value; }
    #endregion
        #region 유닛 스텟 정보 변수 캡슐화
    /// <summary>
    /// 캐릭터 체력
    /// </summary>
    public int IUnitHealth { get => _iUnitHealth; set => _iUnitHealth = value; }
    /// <summary>
    /// 캐릭터 공격력
    /// </summary>
    public int IUnitAtk { get => _iUnitAtk; set => _iUnitAtk = value; }
    /// <summary>
    /// 캐릭터 방어력
    /// </summary>
    public int IUnitDef { get => _iUnitDef; set => _iUnitDef = value; }
    /// <summary>
    /// 레벨 당 증가하는 스텟 값 백분율
    /// </summary>
    public int IStateIncreaseValue { get => _iStateIncreaseValue; set => _iStateIncreaseValue = value; }
    #endregion
    #endregion

    #region 생성자
    public UnitInfo(string _ID, string _Name, string _Position, string _MaxLevel, string _IItemCondition1, string _IItemID1, 
        string _IItemNumber1, string _IItemNumberIncrease1, string _IItemCondition2, string _IItemID2, string _IItemNumber2, 
        string _IItemNumberIncrease2, string _IItemCondition3, string _IItemID3, string _IItemNumber3, string _IItemNumberIncrease3,
        string Health, string Atk, string Def, string StateIncrease, string Image)
    {
        #region 유닛 기본 정보 초기화
        iID = DataProcess.stringToint(_ID);
        strName = DataProcess.stringToNull(_Name);
        Position = (UNITPOSITION)DataProcess.stringToint(_Position);
        iMaxLevel = DataProcess.stringToint(_MaxLevel);
        StrUnitImage = DataProcess.stringToNull(Image);
        #endregion

        #region 재료1 설정
        IItemCondition1 = DataProcess.stringToint(_IItemCondition1);
        IItemID1 = DataProcess.stringToint(_IItemID1);
        IItemNumber1 = DataProcess.stringToint(_IItemNumber1);
        IItemNumberIncrease1 = DataProcess.stringToint(_IItemNumberIncrease1);
        #endregion

        #region 재료2 설정
        IItemCondition2 = DataProcess.stringToint(_IItemCondition2);
        IItemID2 = DataProcess.stringToint(_IItemID2);
        IItemNumber2 = DataProcess.stringToint(_IItemNumber2);
        IItemNumberIncrease2 = DataProcess.stringToint(_IItemNumberIncrease2);
        #endregion

        #region 재료3 설정
        IItemCondition3 = DataProcess.stringToint(_IItemCondition3);
        IItemID3 = DataProcess.stringToint(_IItemID3);
        IItemNumber3 = DataProcess.stringToint(_IItemNumber3);
        IItemNumberIncrease3 = DataProcess.stringToint(_IItemNumberIncrease3);
        #endregion

        #region 유닛 스텟 정보 초기화
        IUnitHealth = DataProcess.stringToint(Health);
        IUnitAtk = DataProcess.stringToint(Atk);
        IUnitDef = DataProcess.stringToint(Def);
        IStateIncreaseValue = DataProcess.stringToint(StateIncrease);
        #endregion
    }
    #endregion
}

[System.Serializable]
public class UnitDataBase : SerializableDictionary<int, UnitInfo> { }
