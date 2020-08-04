using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2번이나 날라갔었음ㅠㅠ
[System.Serializable]
public class StageInfo
{
    public static int GetGoogleSheetGID() { return 2088914405; }

    #region 변수
        #region 스테이지 기본 정보 변수
    [SerializeField]
    private int _iStageId; // 스테이지의 아이디 값
    [SerializeField]
    private int _iStageNumber; // 탑 넘버
    [SerializeField]
    private int _iStageFloor; // 탑 층수
    [SerializeField]
    private int _iStageEncounter; // 1회 전투 횟수
    [SerializeField]
    private int _iStageNeedStamina; // 스테이지 진입에 필요한 스테미나 양
    [SerializeField]
    private int _iStagePlayerLinkNumber; // 한 턴 당 플레이어가 링크할 수 있는 퍼즐 횟수
    [SerializeField]
    private int _iStageMonsterMinAtkNum; // 한 턴 당 몬스터 최소 공격 횟수
    [SerializeField]
    private int _iStageMonsterMaxAtkNum; // 한 턴 당 몬스터 최대 공격 횟수
    [SerializeField]
    private int _iStageLevel;   //스테이지 레벨 (출현 몬스터 레벨)
    #endregion

        #region 출현 몬스터 정보 변수
    [SerializeField]
    private int _iStageMonsterID1; // 1번쨰 몬스터 ID
    [SerializeField]
    private int _iStageMonsterID2; // 2번쨰 몬스터 ID
    [SerializeField]
    private int _iStageMonsterID3; // 3번쨰 몬스터 ID
    [SerializeField]
    private int _iStageMonsterID4; // 4번쨰 몬스터 ID
    [SerializeField]
    private int _iStageMonsterID5; // 5번쨰 몬스터 ID
    [SerializeField]
    private int _iStageMonsterID6; // 6번쨰 몬스터 ID
    #endregion

        #region 드랍 아이템 정보 변수
    [SerializeField]
    private int _iStageDropItemID1;       // 1번쨰 드랍 아이템 ID
    [SerializeField]
    private int _iStageDropPercent1;       // 1번쨰 드랍 확률
    [SerializeField]
    private int _iStageDropItemNumber1;       // 1번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    [SerializeField]
    private int _iStageDropItemID2;       // 2번쨰 드랍 아이템 ID
    [SerializeField]
    private int _iStageDropPercent2;       // 2번쨰 드랍 확률
    [SerializeField]
    private int _iStageDropItemNumber2;       // 2번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    [SerializeField]
    private int _iStageDropItemID3;       // 3번쨰 드랍 아이템 ID
    [SerializeField]
    private int _iStageDropPercent3;       // 3번쨰 드랍 확률
    [SerializeField]
    private int _iStageDropItemNumber3;       // 3번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    [SerializeField]
    private int _iStageDropItemID4;       // 4번쨰 드랍 아이템 ID
    [SerializeField]
    private int _iStageDropPercent4;       // 4번쨰 드랍 확률
    [SerializeField]
    private int _iStageDropItemNumber4;       // 4번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    [SerializeField]
    private int _iStageDropItemID5;       // 5번쨰 드랍 아이템 ID
    [SerializeField]
    private int _iStageDropPercent5;       // 5번쨰 드랍 확률
    [SerializeField]
    private int _iStageDropItemNumber5;       // 5번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    [SerializeField]
    private int _iStageDropItemID6;       // 6번쨰 드랍 아이템 ID
    [SerializeField]
    private int _iStageDropPercent6;       // 6번쨰 드랍 확률
    [SerializeField]
    private int _iStageDropItemNumber6;       // 6번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    #endregion

        #region 드랍 경험치, 골드 변수
    [SerializeField]
    private int _iStageDropEXP;       // 인카운터 1회 클리어 당 지급 경험치
    [SerializeField]
    private int _iStageDropMoney;       // 인카운터 1회 클리어 당 지급 골드
    #endregion

        #region grid 몬스터 리스트
    [SerializeField]
    private List<int> _ListStageEncounter1;  // 1 : Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터
    [SerializeField]
    private List<int> _ListStageEncounter2;  // 2 : Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터
    [SerializeField]
    private List<int> _ListStageEncounter3;  // 3 : Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터
    [SerializeField]
    private List<int> _ListStageEncounter4;  // 4 : Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터
    [SerializeField]
    private List<int> _ListStageEncounter5;  // 5 : Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터
    [SerializeField]
    private List<int> _ListStageEncounter6;  // 6 : Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터
    #endregion
    #endregion

    #region 캡슐화
        #region 스테이지 기본 정보 변수 캡슐화
    /// <summary>
    /// 스테이지 ID값
    /// </summary>
    public int IStageId { get => _iStageId; set => _iStageId = value; }
    /// <summary>
    /// 스테이지 번호 (대륙 번호)
    /// </summary>
    public int IStageNumber { get => _iStageNumber; set => _iStageNumber = value; }
    /// <summary>
    /// 스테이지 층
    /// </summary>
    public int IStageFloor { get => _iStageFloor; set => _iStageFloor = value; }
    /// <summary>
    /// 전투 횟수
    /// </summary>
    public int IStageEncounter { get => _iStageEncounter; set => _iStageEncounter = value; }
    /// <summary>
    /// 스테이지 진입에 필요한 스테미너 양
    /// </summary>
    public int IStageNeedStamina { get => _iStageNeedStamina; set => _iStageNeedStamina = value; }
    /// <summary>
    /// 한 턴 당 플레이어가 링크할 수 있는 퍼즐 횟수
    /// </summary>
    public int IStagePlayerLinkNumber { get => _iStagePlayerLinkNumber; set => _iStagePlayerLinkNumber = value; }
    /// <summary>
    /// 한 턴 당 몬스터 최소 공격 횟수
    /// </summary>
    public int IStageMonsterMinAtkNum { get => _iStageMonsterMinAtkNum; set => _iStageMonsterMinAtkNum = value; }
    /// <summary>
    /// 한 턴 당 몬스터 최대 공격 횟수
    /// </summary>
    public int IStageMonsterMaxAtkNum { get => _iStageMonsterMaxAtkNum; set => _iStageMonsterMaxAtkNum = value; }
    /// <summary>
    /// 스테이지 레벨 (출현 몬스터 레벨)
    /// </summary>
    public int IStageLevel { get => _iStageLevel; set => _iStageLevel = value; }
    #endregion
        #region 출현 몬스터 정보 변수 캡슐화
    /// <summary>
    /// 1번쨰 출현 몬스터 ID
    /// </summary>
    public int IStageMonsterID1 { get => _iStageMonsterID1; set => _iStageMonsterID1 = value; }
    /// <summary>
    /// 2번쨰 출현 몬스터 ID
    /// </summary>
    public int IStageMonsterID2 { get => _iStageMonsterID2; set => _iStageMonsterID2 = value; }
    /// <summary>
    /// 3번쨰 출현 몬스터 ID
    /// </summary>
    public int IStageMonsterID3 { get => _iStageMonsterID3; set => _iStageMonsterID3 = value; }
    /// <summary>
    /// 4번쨰 출현 몬스터 ID
    /// </summary>
    public int IStageMonsterID4 { get => _iStageMonsterID4; set => _iStageMonsterID4 = value; }
    /// <summary>
    /// 5번쨰 출현 몬스터 ID
    /// </summary>
    public int IStageMonsterID5 { get => _iStageMonsterID5; set => _iStageMonsterID5 = value; }
    /// <summary>
    /// 6번쨰 출현 몬스터 ID
    /// </summary>
    public int IStageMonsterID6 { get => _iStageMonsterID6; set => _iStageMonsterID6 = value; }
    #endregion
        #region 드랍 아이템 정보 변수 캡슐화
    /// <summary>
    /// 1번쨰 드랍 아이템 ID
    /// </summary>
    public int IStageDropItemID1 { get => _iStageDropItemID1; set => _iStageDropItemID1 = value; }
    /// <summary>
    /// 1번쨰 인카운터 1회 클리어 당 아이템 드랍 확률 (100=100%)
    /// </summary>
    public int IStageDropPercent1 { get => _iStageDropPercent1; set => _iStageDropPercent1 = value; }
    /// <summary>
    /// 1번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    /// </summary>
    public int IStageDropItemNumber1 { get => _iStageDropItemNumber1; set => _iStageDropItemNumber1 = value; }
    /// <summary>
    /// 2번쨰 드랍 아이템 ID
    /// </summary>
    public int IStageDropItemID2 { get => _iStageDropItemID2; set => _iStageDropItemID2 = value; }
    /// <summary>
    /// 2번쨰 인카운터 1회 클리어 당 아이템 드랍 확률 (100=100%)
    /// </summary>
    public int IStageDropPercent2 { get => _iStageDropPercent2; set => _iStageDropPercent2 = value; }
    /// <summary>
    /// 2번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    /// </summary>
    public int IStageDropItemNumber2 { get => _iStageDropItemNumber2; set => _iStageDropItemNumber2 = value; }
    /// <summary>
    /// 3번쨰 드랍 아이템 ID
    /// </summary>
    public int IStageDropItemID3 { get => _iStageDropItemID3; set => _iStageDropItemID3 = value; }
    /// <summary>
    /// 3번쨰 인카운터 1회 클리어 당 아이템 드랍 확률 (100=100%)
    /// </summary>
    public int IStageDropPercent3 { get => _iStageDropPercent3; set => _iStageDropPercent3 = value; }
    /// <summary>
    /// 3번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    /// </summary>
    public int IStageDropItemNumber3 { get => _iStageDropItemNumber3; set => _iStageDropItemNumber3 = value; }
    /// <summary>
    /// 4번쨰 드랍 아이템 ID
    /// </summary>
    public int IStageDropItemID4 { get => _iStageDropItemID4; set => _iStageDropItemID4 = value; }
    /// <summary>
    /// 4번쨰 인카운터 1회 클리어 당 아이템 드랍 확률 (100=100%)
    /// </summary>
    public int IStageDropPercent4 { get => _iStageDropPercent4; set => _iStageDropPercent4 = value; }
    /// <summary>
    /// 4번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    /// </summary>
    public int IStageDropItemNumber4 { get => _iStageDropItemNumber4; set => _iStageDropItemNumber4 = value; }
    /// <summary>
    /// 5번쨰 드랍 아이템 ID
    /// </summary>
    public int IStageDropItemID5 { get => _iStageDropItemID5; set => _iStageDropItemID5 = value; }
    /// <summary>
    /// 5번쨰 인카운터 1회 클리어 당 아이템 드랍 확률 (100=100%)
    /// </summary>
    public int IStageDropPercent5 { get => _iStageDropPercent5; set => _iStageDropPercent5 = value; }
    /// <summary>
    /// 5번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    /// </summary>
    public int IStageDropItemNumber5 { get => _iStageDropItemNumber5; set => _iStageDropItemNumber5 = value; }
    /// <summary>
    /// 6번쨰 드랍 아이템 ID
    /// </summary>
    public int IStageDropItemID6 { get => _iStageDropItemID6; set => _iStageDropItemID6 = value; }
    /// <summary>
    /// 6번쨰 인카운터 1회 클리어 당 아이템 드랍 확률 (100=100%)
    /// </summary>
    public int IStageDropPercent6 { get => _iStageDropPercent6; set => _iStageDropPercent6 = value; }
    /// <summary>
    /// 6번쨰 드랍 아이템 갯수 (DropPercent 확률로 1~x개 획득)
    /// </summary>
    public int IStageDropItemNumber6 { get => _iStageDropItemNumber6; set => _iStageDropItemNumber6 = value; }
    #endregion
        #region 드랍 경험치, 골드 변수 캡슐화
    /// <summary>
    /// 인카운터 1회 클리어 당 지급 경험치
    /// </summary>
    public int IStageDropEXP { get => _iStageDropEXP; set => _iStageDropEXP = value; }
    /// <summary>
    /// 인카운터 1회 클리어 당 지급 골드
    /// </summary>
    public int IStageDropMoney { get => _iStageDropMoney; set => _iStageDropMoney = value; }
    #endregion
        #region grid 몬스터 리스트 캡슐화
    /// <summary>
    /// 1번쨰 Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터 Ex) 1,2,2,3,3
    /// </summary>
    public List<int> ListStageEncounter1 { get => _ListStageEncounter1; set => _ListStageEncounter1 = value; }
    /// <summary>
    /// 2번쨰 Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터 Ex) 1,2,2,3,3
    /// </summary>
    public List<int> ListStageEncounter2 { get => _ListStageEncounter2; set => _ListStageEncounter2 = value; }
    /// <summary>
    /// 3번쨰 Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터 Ex) 1,2,2,3,3
    /// </summary>
    public List<int> ListStageEncounter3 { get => _ListStageEncounter3; set => _ListStageEncounter3 = value; }
    /// <summary>
    /// 4번쨰 Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터 Ex) 1,2,2,3,3
    /// </summary>
    public List<int> ListStageEncounter4 { get => _ListStageEncounter4; set => _ListStageEncounter4 = value; }
    /// <summary>
    /// 5번쨰 Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터 Ex) 1,2,2,3,3
    /// </summary>
    public List<int> ListStageEncounter5 { get => _ListStageEncounter5; set => _ListStageEncounter5 = value; }
    /// <summary>
    /// 6번쨰 Grid0 몬스터, Grid1 몬스터, Grid2 몬스터, Grid3 몬스터, Grid4 몬스터 Ex) 1,2,2,3,3
    /// </summary>
    public List<int> ListStageEncounter6 { get => _ListStageEncounter6; set => _ListStageEncounter6 = value; }

    #endregion
    #endregion

    #region 생성자
    public StageInfo(string ID, string Number, string Floor, string Encounter,
        string NeedStamina, string PlayerLinkNum, string MonsterMinAtkNum, string MonsterMaxAtkNum, string Level,
        string MonsterID1, string MonsterID2, string MonsterID3, string MonsterID4,  string MonsterID5, string MonsterID6,
        string DropItemID1, string DropPercent1, string DropItemNumber1, string DropItemID2, string DropPercent2, string DropItemNumber2,
        string DropItemID3, string DropPercent3, string DropItemNumber3, string DropItemID4, string DropPercent4, string DropItemNumber4,
        string DropItemID5, string DropPercent5, string DropItemNumber5, string DropItemID6, string DropPercent6, string DropItemNumber6,
        string DropEXP, string DropMoney,
        string List1, string List2, string List3, string List4, string List5, string List6)
    {
        #region 스테이지 기본 정보 초기화
        IStageId = DataProcess.stringToint(ID);
        IStageNumber = DataProcess.stringToint(Number);
        IStageFloor = DataProcess.stringToint(Floor);
        IStageEncounter = DataProcess.stringToint(Encounter);
        IStageNeedStamina = DataProcess.stringToint(NeedStamina);
        IStagePlayerLinkNumber = DataProcess.stringToint(PlayerLinkNum);
        IStageMonsterMinAtkNum = DataProcess.stringToint(MonsterMinAtkNum);
        IStageMonsterMaxAtkNum = DataProcess.stringToint(MonsterMaxAtkNum);
        IStageLevel = DataProcess.stringToint(Level);
        #endregion

        #region 스테이지 몬스터 정보 초기화
        IStageMonsterID1 = DataProcess.stringToint(MonsterID1);
        IStageMonsterID2 = DataProcess.stringToint(MonsterID2);
        IStageMonsterID3 = DataProcess.stringToint(MonsterID3);
        IStageMonsterID4 = DataProcess.stringToint(MonsterID4);
        IStageMonsterID5 = DataProcess.stringToint(MonsterID5);
        IStageMonsterID6 = DataProcess.stringToint(MonsterID6);
        #endregion
        
        #region 드랍 아이템 정보 초기화
        IStageDropItemID1 = DataProcess.stringToint(DropItemID1);
        IStageDropPercent1 = DataProcess.stringToint(DropPercent1);
        IStageDropItemNumber1 = DataProcess.stringToint(DropItemNumber1);
        IStageDropItemID2 = DataProcess.stringToint(DropItemID2);
        IStageDropPercent2 = DataProcess.stringToint(DropPercent2);
        IStageDropItemNumber2 = DataProcess.stringToint(DropItemNumber2);
        IStageDropItemID3 = DataProcess.stringToint(DropItemID3);
        IStageDropPercent3 = DataProcess.stringToint(DropPercent3);
        IStageDropItemNumber3 = DataProcess.stringToint(DropItemNumber3);
        IStageDropItemID4 = DataProcess.stringToint(DropItemID4);
        IStageDropPercent4 = DataProcess.stringToint(DropPercent4);
        IStageDropItemNumber4 = DataProcess.stringToint(DropItemNumber4);
        IStageDropItemID5 = DataProcess.stringToint(DropItemID5);
        IStageDropPercent5 = DataProcess.stringToint(DropPercent5);
        IStageDropItemNumber5 = DataProcess.stringToint(DropItemNumber5);
        IStageDropItemID6 = DataProcess.stringToint(DropItemID6);
        IStageDropPercent6 = DataProcess.stringToint(DropPercent6);
        IStageDropItemNumber6 = DataProcess.stringToint(DropItemNumber6);
        #endregion

        #region 드랍 경험치, 골드 정보 초기화
        IStageDropEXP = DataProcess.stringToint(DropEXP);
        IStageDropMoney = DataProcess.stringToint(DropMoney);
        #endregion

        #region Grid 몬스터 리스트 정보 초기화
        ListStageEncounter1 = DataProcess.stringToListint(List1, ",");
        ListStageEncounter2 = DataProcess.stringToListint(List2, ",");
        ListStageEncounter3 = DataProcess.stringToListint(List3, ",");
        ListStageEncounter4 = DataProcess.stringToListint(List4, ",");
        ListStageEncounter5 = DataProcess.stringToListint(List5, ",");
        ListStageEncounter6 = DataProcess.stringToListint(List6, ",");
        #endregion
    }
    #endregion
}

[System.Serializable]
public class StageDataBase : SerializableDictionary<int, StageInfo> { }