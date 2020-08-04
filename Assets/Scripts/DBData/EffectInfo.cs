using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//효과 테이블
public class EffectInfo
{
    // 해당 시트의 ID 값을 반환한다. 시트에 따라 ID값이 달라지므로 시트마다 클래스를 만들어줘야 할듯
    public static int GetGoogleSheetGID() { return 1037914649; }
    #region 효과 정보 변수
    [SerializeField]
    private int _iID;  // 이펙트 아이디
    [SerializeField]
    private string _strName;  // 이펙트 이름
    [SerializeField]
    private EFFECTGROUP _Group;  // 효과 종류
    [SerializeField]
    private string _EffectIcon;   // 효과 아이콘
    [SerializeField]
    private string _strDescription; // 효과 설명
    [SerializeField]
    private int _iEffectOverLap;
    #endregion

    #region 캡슐화
    /// <summary>
    /// 효과의 ID
    /// </summary>
    public int iID { get => _iID; set => _iID = value; }
    /// <summary>
    /// 효과의 이름
    /// </summary>
    public string strName { get => _strName; set => _strName = value; }
    /// <summary>
    /// 효과의 종류 (디버프, 버프)
    /// </summary>
    public EFFECTGROUP Group { get => _Group; set => _Group = value; }
    /// <summary>
    /// 아이콘 명
    /// </summary>
    public string strEffectIcon { get => _EffectIcon; set => _EffectIcon = value; }
    /// <summary>
    /// 효과 설명
    /// </summary>
    public string strDescription { get => _strDescription; set => _strDescription = value; }
    public int IEffectOverLap { get => _iEffectOverLap; set => _iEffectOverLap = value; }
    #endregion

    #region 생성자
    public EffectInfo(string _IID, string _StrName, string EffectGroup, string _strIcon, string strDes, string EffectOverLap)
    {
        iID = DataProcess.stringToint(_IID);
        strName = DataProcess.stringToNull(_StrName);
        Group = (EFFECTGROUP)DataProcess.stringToint(EffectGroup);
        strEffectIcon = DataProcess.stringToNull(_strIcon);
        strDescription = DataProcess.stringToNull(strDes);
        IEffectOverLap = DataProcess.stringToint(EffectOverLap);
    }
    #endregion
}

[System.Serializable] 
public class EffectDataBase : SerializableDictionary<int, EffectInfo> { }