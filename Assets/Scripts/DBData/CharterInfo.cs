using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// 캐릭터 테이블
public class CharterInfo
{
    // 해당 시트의 ID 값을 반환한다. 시트에 따라 ID값이 달라지므로 시트마다 클래스를 만들어줘야 할듯
    public static int GetGoogleSheetGID() { return 2146065714; }
    [SerializeField]
    private int _iLevel;            // 캐릭터 레벨
    [SerializeField]
    private int _iNeedXp;           // 레벨 별 필요 경험치 양
    [SerializeField]
    private int _iStamina;          // 스태미나
    [SerializeField]
    private int _iStaminaCoolTime;              // 스태미나가 회복되는데 필요한 시간
    /// <summary>
    /// 플레이어(메인 캐릭터) 레벨
    /// </summary>
    public int iLevel { get => _iLevel; set => _iLevel = value; }
    /// <summary>
    /// 레벨을 올리는 데 필요한 경험치량
    /// </summary>
    public int iNeedXp { get => _iNeedXp; set => _iNeedXp = value; }
    /// <summary>
    /// 플레이어가 보유할 수 있는 최대 스테미너량
    /// </summary>
    public int iStamina { get => _iStamina; set => _iStamina = value; }
    /// <summary>
    /// 스테미너가 회복되는데 필요한 시간(s)
    /// </summary>
    public int iStaminaCoolTime { get => _iStaminaCoolTime; set => _iStaminaCoolTime = value; }

    public CharterInfo(string _Level, string _NeedXp, string _Stamina, string _StaminaCoolTime)
    {
        iLevel = DataProcess.stringToint(_Level);
        iNeedXp = DataProcess.stringToint(_NeedXp);
        iStamina = DataProcess.stringToint(_Stamina);
        iStaminaCoolTime = DataProcess.stringToint(_StaminaCoolTime);
    }
}

[System.Serializable]
public class CharterDataBase : SerializableDictionary<int, CharterInfo> { }
