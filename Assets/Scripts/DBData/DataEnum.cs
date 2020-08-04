using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// 유닛의 역할
public enum UNITPOSITION
{
    TANKER_POSITION = 1,      //탱커
    DEALER_POSITION,          //딜러
    SUPPORTER_POSITION        //서포터
}
[System.Serializable]
public enum SKILLTYPE
{
    NO_TYPE,        //값없음
    NORMAL_TYPE,    //일반 공격
    SKILL_TYPE          // 스킬 타입
}
[System.Serializable]
// 스킬, 아이템 등 사용 대상
// Unit의 경우 1(아군),2(적군)의 값만 가짐
public enum TARGET
{
    NO_TARGET,          //값 없음
    FRIENDLY_TARGET,      // 아군
    ENEMY_TARGET,           // 적군
    ALL_TARGET,             //아군, 적군
    PLAYER_TARGET           //플레이어
}
[System.Serializable]
// 스킬 및 아이템 등의 사용 범위
public enum RANGE
{
    NO_RANGE,       // 값 없음
    SINGLE_RANGE,   // 단일
    MULTI_RANGE        //  범위
}
[System.Serializable]
// 공격 거리
public enum ATTACKRANGE
{
    NEAR_ATTACKRANGE = 1,   // 근거리
    REMOTE_ATTACKRANGE      // 원거리
}
[System.Serializable]
//타겟 대상
// Unit의 경우 1(랜덤),2(시전자),3(체력이 낮은 대상) 값만 가짐
public enum TARGETOBJECT
{
    NO_TARGETOBJ,      //값 없음
    NORMAL_TARGETOBJ,   // 일반대상(랜덤)
    CASTER_TARGETOBJ,        // 시전자 대상
    LOWHEALTH_TARGETOBJ,     // 체력이 가장 낮은 대상
    HIGHHEALTH_TARGETOBJ,    // 체력이 가장 높은 대상
    FRONTLINE_TARGETOBJ,     // 전열에 있는 대상
    MIDDLELINE_TARGETOBJ,    // 중열에 있는 대상
    ENDLINE_TARGETOBJ,        // 후열에 있는 대상
    SELECT_TARGETOBJ
}
[System.Serializable]
// 효과
public enum EFFECT
{
    NO_EFFECT,  //효과 없음
    DAMAGE_EFFECT,   // 데미지
    HEAL_EFFECT,     //회복
    FAINT_EFFECT,  // 기절            // 턴 끝날때 --
    POISON_EFFECT,   // 중독          // 턴 시작시 효과 후 --
    BLOODY_EFFECT,  // 출혈           // 턴 중 행동마다 효과/ 턴 시작시 --
    FIRE_EFFECT,    // 화상           // 턴 시작시 효과 후 --
    IMMUNE_EFFECT,   //면역           // 턴 끝날때 --
    COUNTERATTACK_EFFECT, //반격      //턴 끝날때 --
    INVINCIBILITY_EFFECT,  // 무적    // 공격받는 횟수만큼 --
    CONCENTRATION_EFFECT,  // 집중    // 턴 끝날때 --
    STEALTH_EFFECT,        // 은신    // 턴 끝날때 --
    REINCARNATION_EFFECT,  // 재생    // 턴 시작시 효과 후 --
    LINKNUMBERINCREASE_EFFECT,// 링크 횟수 증가
    LINKNUMBERDECREASE_EFFECT,
    SKILLGAUGEINCREASE_EFFECT,
    SKILLGAUGEDECREASE_EFFECT,
    MANABUBBLEINCREASE_EFFECT,
    MANABUBBLEDECREASE_EFFECT,
    EQUALITY_EFFECT,
    PUZZLEREINITIALIZATION_EFFECT,
    PUZZLEREMOVE_EFFECT,
}
[System.Serializable]
// 이펙트 그룹(디버프/버프)
public enum EFFECTGROUP
{
    DEBUFF_EFFECT,
    BUFF_EFFECT
}
[System.Serializable]
//아이템 타입
public enum ITEMTYPE
{
    NO_TYPE,
    STUFF_TYPE,   //잡화
    EXPENDABLES_TYPE  //소모품
}
/// <summary>
/// 샵에서 판매하는 아이템 타입
/// </summary>
[System.Serializable] 
public enum SHOPITEM_TYPE
{
    NO_TYPE,
    /// <summary>
    /// 캐릭터
    /// </summary>
    CHARTER_TYPE,
    /// <summary>
    /// 골드
    /// </summary>
    GOLD_TYPE,
    /// <summary>
    /// 스테미너
    /// </summary>
    STEMINA_TYPE,
    /// <summary>
    /// 소모품
    /// </summary>
    EXPENDABLES_TYPE
}

[System.Serializable]
public enum CASTER
{
    NONE = 0,
    UNIT = 1,
    ENEMY = 2,
    PLAYER = 3,
    END = 4,
}