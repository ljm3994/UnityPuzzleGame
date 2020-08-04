using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// 스킬 테이블
public class CharterSkillInfo : UnitSkillInfo
{
    // 해당 시트의 ID 값을 반환한다. 시트에 따라 ID값이 달라지므로 시트마다 클래스를 만들어줘야 할듯
    public static int GetGoogleSheetGID() { return 701490297; }

    #region 생성자
    public CharterSkillInfo(string SkillID, string CharterID, string SkillName, string SkillType, string SkillRange,
        string SkillTarget1, string SkillTargetObj1, string SkillTargetNumber1, string SkillEffectID1, string EffectTurn1, string EffectValue1,
        string SkillTarget2, string SkillTargetObj2, string SkillTargetNumber2, string SkillEffectID2, string EffectTurn2, string EffectValue2,
        string SkillTarget3, string SkillTargetObj3, string SkillTargetNumber3, string SkillEffectID3, string EffectTurn3, string EffectValue3,
        string SkillIcon, string SkillDesc)
        : base(SkillID, CharterID, SkillName, SkillType, SkillRange,
        SkillTarget1, SkillTargetObj1, SkillTargetNumber1, SkillEffectID1, EffectTurn1, EffectValue1,
        SkillTarget2, SkillTargetObj2, SkillTargetNumber2, SkillEffectID2, EffectTurn2, EffectValue2,
        SkillTarget3, SkillTargetObj3, SkillTargetNumber3, SkillEffectID3, EffectTurn3, EffectValue3,
        SkillIcon, SkillDesc)
    {

    }
    #endregion
    
}

[System.Serializable] 
public class CharterSkillDataBase : SerializableDictionary<int, CharterSkillInfo> { }