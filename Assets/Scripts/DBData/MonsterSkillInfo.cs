using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//몬스터 스킬 정보 테이블
[System.Serializable]
public class MonsterSkillInfo : UnitSkillInfo
{
    public static int GetGoogleSheetGID() { return 1999398297; }

    #region 생성자
    public MonsterSkillInfo(string SkillID, string MonsterID, string SkillName, string SkillType, string SkillRange,
        string SkillTarget1, string SkillTargetObj1, string SkillTargetNumber1, string SkillEffectID1, string EffectTurn1, string EffectValue1,
        string SkillTarget2, string SkillTargetObj2, string SkillTargetNumber2, string SkillEffectID2, string EffectTurn2, string EffectValue2,
        string SkillTarget3, string SkillTargetObj3, string SkillTargetNumber3, string SkillEffectID3, string EffectTurn3, string EffectValue3,
        string SkillIcon, string SkillDesc)
        : base(SkillID, MonsterID, SkillName, SkillType, SkillRange,
        SkillTarget1, SkillTargetObj1, SkillTargetNumber1, SkillEffectID1, EffectTurn1, EffectValue1,
        SkillTarget2, SkillTargetObj2, SkillTargetNumber2, SkillEffectID2, EffectTurn2, EffectValue2,
        SkillTarget3, SkillTargetObj3, SkillTargetNumber3, SkillEffectID3, EffectTurn3, EffectValue3,
        SkillIcon, SkillDesc)
    {
    }
    #endregion
}

[System.Serializable]
public class MonsterSkillDataBase : SerializableDictionary<int, MonsterSkillInfo> { }