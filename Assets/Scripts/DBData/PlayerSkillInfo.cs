using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSkillInfo
{
    public static int GetGoogleSheetGID() { return 1343042784; }

    #region 플레이어 스킬 기본 정보 변수
    [SerializeField]
    private int _iSkillId;   //스킬 ID
    [SerializeField]
    private string _strSkillName;  //스킬 이름
    [SerializeField]
    private SKILLTYPE _Type;    //스킬의 종류
    [SerializeField]
    private string _strSkillIcon;   //스킬 아이콘 명
    [SerializeField]
    private string _strSkillDesc;   //스킬 설명
    #endregion
    #region 1번쨰 효과 변수
    // 1번쨰 효과 변수
    [SerializeField]
    private TARGET _Target1;   // 스킬 효과 적용 대상
    [SerializeField]
    private TARGETOBJECT _TargetObj1;  //스킬 효과 적용 대상 방식(1번쨰 효과)
    [SerializeField]
    private RANGE _TargetNumber1; // 스킬의 효과 적용 대상 수
    [SerializeField]
    private int _iSkillEffectID1;   //스킬의 효과 아이디
    [SerializeField]
    private int _iSkillEffectTurn1;  //스킬의 효과 적용 턴(효과가 데미지/회복인 경우 0고정, 효과가 무적인 경우 피해 무시 횟수, 나머지 효과는 지속 턴)
    [SerializeField]
    private int _iSkillEffectValue1; //효과 값(백분율, 면역/무적/은신의 경우는 0 고정)
    #endregion
    #region 2번쨰 효과 변수
    // 2번쨰 효과 변수
    [SerializeField]
    private TARGET _Target2;   // 스킬 효과 적용 대상
    [SerializeField]
    private TARGETOBJECT _TargetObj2;  //스킬 효과 적용 대상 방식(2번쨰 효과)
    [SerializeField]
    private RANGE _TargetNumber2; // 스킬의 효과 적용 대상 수
    [SerializeField]
    private int _iSkillEffectID2;   //스킬의 효과 아이디
    [SerializeField]
    private int _iSkillEffectTurn2;  //스킬의 효과 적용 턴(효과가 데미지/회복인 경우 0고정, 효과가 무적인 경우 피해 무시 횟수, 나머지 효과는 지속 턴)
    [SerializeField]
    private int _iSkillEffectValue2; //효과 값(백분율, 면역/무적/은신의 경우는 0 고정)
    #endregion
    #region 3번쨰 효과 변수
    // 3번쨰 효과 변수
    [SerializeField]
    private TARGET _Target3;   // 스킬 효과 적용 대상
    [SerializeField]
    private TARGETOBJECT _TargetObj3;  //스킬 효과 적용 대상 방식(3번쨰 효과)
    [SerializeField]
    private RANGE _TargetNumber3; // 스킬의 효과 적용 대상 수
    [SerializeField]
    private int _iSkillEffectID3;   //스킬의 효과 아이디
    [SerializeField]
    private int _iSkillEffectTurn3;  //스킬의 효과 적용 턴(효과가 데미지/회복인 경우 0고정, 효과가 무적인 경우 피해 무시 횟수, 나머지 효과는 지속 턴)
    [SerializeField]
    private int _iSkillEffectValue3; //효과 값(백분율, 면역/무적/은신의 경우는 0 고정)
    #endregion

    #region 필드 캡슐화
        #region 플레이어 스킬 기본 정보 변수 캡슐화
    /// <summary>
    /// 스킬의 ID
    /// </summary>
    public int ISkillId { get => _iSkillId; set => _iSkillId = value; }
    /// <summary>
    /// 스킬의 이름
    /// </summary>
    public string StrSkillName { get => _strSkillName; set => _strSkillName = value; }
    /// <summary>
    /// 스킬의 종류(일반 공격, 스킬)
    /// </summary>
    public SKILLTYPE Type { get => _Type; set => _Type = value; }
    /// <summary>
    /// 아이콘 명
    /// </summary>
    public string StrSkillIcon { get => _strSkillIcon; set => _strSkillIcon = value; }
    /// <summary>
    /// 스킬 설명
    /// </summary>
    public string StrSkillDesc { get => _strSkillDesc; set => _strSkillDesc = value; }
    #endregion
        #region 1번쨰 효과 변수 초기화
    /// <summary>
    /// 1번쨰 스킬 효과 적용 대상(아군, 적군, 아군/적군, 플레이어)
    /// </summary>
    public TARGET Target1 { get => _Target1; set => _Target1 = value; }
    /// <summary>
    /// 1번쨰 스킬 효과 적용 대상 방식(시전자 대상, 체력이 가장 낮은 대상, 체력이 가장 높은 대상, 전열에 있는 대상, 중열에 있는 대상, 후열에 있는 대상)
    /// </summary>
    public TARGETOBJECT TargetObj1 { get => _TargetObj1; set => _TargetObj1 = value; }
    /// <summary>
    /// 1번쨰 스킬 효과 적용 대상 수(단일, 범위)
    /// </summary>
    public RANGE TargetNumber1 { get => _TargetNumber1; set => _TargetNumber1 = value; }
    /// <summary>
    /// 1번쨰 스킬의 효과 ID
    /// </summary>
    public int ISkillEffectID1 { get => _iSkillEffectID1; set => _iSkillEffectID1 = value; }
    /// <summary>
    /// 1번쨰 효과 적용 턴
    /// </summary>
    public int ISkillEffectTurn1 { get => _iSkillEffectTurn1; set => _iSkillEffectTurn1 = value; }
    /// <summary>
    /// 1번쨰 효과 값
    /// </summary>
    public int ISkillEffectValue1 { get => _iSkillEffectValue1; set => _iSkillEffectValue1 = value; }
    #endregion
        #region 2번쨰 효과 변수 초기화
    /// <summary>
    /// 2번쨰 스킬 효과 적용 대상(아군, 적군, 아군/적군, 플레이어)
    /// </summary>
    public TARGET Target2 { get => _Target2; set => _Target2 = value; }
    /// <summary>
    /// 2번쨰 스킬 효과 적용 대상 방식(시전자 대상, 체력이 가장 낮은 대상, 체력이 가장 높은 대상, 전열에 있는 대상, 중열에 있는 대상, 후열에 있는 대상)
    /// </summary>
    public TARGETOBJECT TargetObj2 { get => _TargetObj2; set => _TargetObj2 = value; }
    /// <summary>
    /// 2번쨰 스킬 효과 적용 대상 수(단일, 범위)
    /// </summary>
    public RANGE TargetNumber2 { get => _TargetNumber2; set => _TargetNumber2 = value; }
    /// <summary>
    /// 2번쨰 스킬의 효과 ID
    /// </summary>
    public int ISkillEffectID2 { get => _iSkillEffectID2; set => _iSkillEffectID2 = value; }
    /// <summary>
    /// 2번쨰 효과 적용 턴
    /// </summary>
    public int ISkillEffectTurn2 { get => _iSkillEffectTurn2; set => _iSkillEffectTurn2 = value; }
    /// <summary>
    /// 2번쨰 효과 값
    /// </summary>
    public int ISkillEffectValue2 { get => _iSkillEffectValue2; set => _iSkillEffectValue2 = value; }
    #endregion
        #region 3번쨰 효과 변수 초기화
    /// <summary>
    /// 3번쨰 스킬 효과 적용 대상(아군, 적군, 아군/적군, 플레이어)
    /// </summary>
    public TARGET Target3 { get => _Target3; set => _Target3 = value; }
    /// <summary>
    /// 3번쨰 스킬 효과 적용 대상 방식(시전자 대상, 체력이 가장 낮은 대상, 체력이 가장 높은 대상, 전열에 있는 대상, 중열에 있는 대상, 후열에 있는 대상)
    /// </summary>
    public TARGETOBJECT TargetObj3 { get => _TargetObj3; set => _TargetObj3 = value; }
    /// <summary>
    /// 3번쨰 스킬 효과 적용 대상 수(단일, 범위)
    /// </summary>
    public RANGE TargetNumber3 { get => _TargetNumber3; set => _TargetNumber3 = value; }
    /// <summary>
    /// 3번쨰 스킬의 효과 ID
    /// </summary>
    public int ISkillEffectID3 { get => _iSkillEffectID3; set => _iSkillEffectID3 = value; }
    /// <summary>
    /// 3번쨰 효과 적용 턴
    /// </summary>
    public int ISkillEffectTurn3 { get => _iSkillEffectTurn3; set => _iSkillEffectTurn3 = value; }
    /// <summary>
    /// 3번쨰 효과 값
    /// </summary>
    public int ISkillEffectValue3 { get => _iSkillEffectValue3; set => _iSkillEffectValue3 = value; }
    #endregion
    #endregion

    #region 생성자
    public PlayerSkillInfo(string SkillID, string SkillName, string SkillType, 
        string SkillTarget1, string SkillTargetObj1, string SkillTargetNumber1, string SkillEffectID1, string EffectTurn1, string EffectValue1,
        string SkillTarget2, string SkillTargetObj2, string SkillTargetNumber2, string SkillEffectID2, string EffectTurn2, string EffectValue2,
        string SkillTarget3, string SkillTargetObj3, string SkillTargetNumber3, string SkillEffectID3, string EffectTurn3, string EffectValue3,
        string SkillIcon, string SkillDesc)
    {
        #region 플레이어 스킬 기본 정보 초기화
        ISkillId = DataProcess.stringToint(SkillID);
        StrSkillName = DataProcess.stringToNull(SkillName);
        Type = (SKILLTYPE)DataProcess.stringToint(SkillType);
        StrSkillIcon = DataProcess.stringToNull(SkillIcon);
        StrSkillDesc = DataProcess.stringToNull(SkillDesc);
        #endregion
        #region 스킬 효과 1번쨰
        Target1 = (TARGET)DataProcess.stringToint(SkillTarget1);
        TargetObj1 = (TARGETOBJECT)DataProcess.stringToint(SkillTargetObj1);
        TargetNumber1 = (RANGE)DataProcess.stringToint(SkillTargetNumber1);
        ISkillEffectID1 = DataProcess.stringToint(SkillEffectID1);
        ISkillEffectTurn1 = DataProcess.stringToint(EffectTurn1);
        ISkillEffectValue1 = DataProcess.stringToint(EffectValue1);
        #endregion

        #region 스킬 효과 2번쨰
        Target2 = (TARGET)DataProcess.stringToint(SkillTarget2);
        TargetObj2 = (TARGETOBJECT)DataProcess.stringToint(SkillTargetObj2);
        TargetNumber2 = (RANGE)DataProcess.stringToint(SkillTargetNumber2);
        ISkillEffectID2 = DataProcess.stringToint(SkillEffectID2);
        ISkillEffectTurn2 = DataProcess.stringToint(EffectTurn2);
        ISkillEffectValue2 = DataProcess.stringToint(EffectValue2);
        #endregion

        #region 스킬 효과 3번쨰
        Target3 = (TARGET)DataProcess.stringToint(SkillTarget3);
        TargetObj3 = (TARGETOBJECT)DataProcess.stringToint(SkillTargetObj3);
        TargetNumber3 = (RANGE)DataProcess.stringToint(SkillTargetNumber3);
        ISkillEffectID3 = DataProcess.stringToint(SkillEffectID3);
        ISkillEffectTurn3 = DataProcess.stringToint(EffectTurn3);
        ISkillEffectValue3 = DataProcess.stringToint(EffectValue3);
        #endregion

    }
    #endregion
}

[System.Serializable]
public class PlayerSkillDataBase : SerializableDictionary<int, PlayerSkillInfo> { }