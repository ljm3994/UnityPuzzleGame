using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataBase : MonoBehaviour
{
    #region INSPECTOR
    [SerializeField]
    public GoogleSheatData GoogleSheat;
    [SerializeField]
    public float MaxSheetNum;
    #endregion

    [SerializeField]
    private UnitDataBase _UnitTable;
    [SerializeField]
    private CharterDataBase _CharterTable;
    [SerializeField]
    private UnitSkillDataBase _SkillTable;
    [SerializeField]
    private MonsterDataBase _MonsterTable;
    [SerializeField]
    private EffectDataBase _EffectTable;
    [SerializeField]
    private UnitSkillDataBase _MonsterSkillTable;
    [SerializeField]
    private ItemDataBase _ItemTable;
    [SerializeField]
    private PlayerSkillDataBase _PlayerSkillTable;
    [SerializeField]
    private StageDataBase _StageTable;
    [SerializeField]
    private ShopDataBase _ShopTable;
    [SerializeField]
    private UnitEXPDataBase _UnitExpTable;
    [SerializeField]
    private CharterPriceDataBase _CharterPriceTable;
    private static GameDataBase _Instance;
    public static GameDataBase Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = FindObjectOfType(typeof(GameDataBase)) as GameDataBase;
                if (!_Instance)
                {
                    GameObject container = new GameObject();
                    container.name = "GameDataBaseContainer";
                    _Instance = container.AddComponent(typeof(GameDataBase)) as GameDataBase;

                }
            }
            return _Instance;
        }
    }
    /// <summary>
    /// 유닛 데이터(Key값은 UnitID)유닛별 공격력 방어력 등등의 데이터를 저장하고 있음
    /// </summary>
    public UnitDataBase UnitTable { get => _UnitTable; set => _UnitTable = value; }
    /// <summary>
    /// Player 데이터(Key값은 레벨)레벨별 스테미너나 경험치 쿨타임 등 저장하고 있음
    /// </summary>
    public CharterDataBase CharterTable { get => _CharterTable; set => _CharterTable = value; }
    /// <summary>
    /// 유닛 스킬 데이터(Key값은 유닛 스킬ID)각 유닛 스킬별 데이터를 저장하고 있음
    /// </summary>
    public UnitSkillDataBase SkillTable { get => _SkillTable; set => _SkillTable = value; }
    /// <summary>
    /// Monster 데이터(Key값은 MonsterID)각 Monster 데이터를 저장하고 있음
    /// </summary>
    public MonsterDataBase MonsterTable { get => _MonsterTable; set => _MonsterTable = value; }
    /// <summary>
    /// 효과 데이터(Key값은 EffectID)각 Effect 데이터를 저장하고 있음
    /// </summary>
    public EffectDataBase EffectTable { get => _EffectTable; set => _EffectTable = value; }
    /// <summary>
    /// Monster 스킬 데이터(Key값은 Monster 스킬ID)각 Monster 스킬 데이터를 저장하고 있음
    /// </summary>
    public UnitSkillDataBase MonsterSkillTable { get => _MonsterSkillTable; set => _MonsterSkillTable = value; }
    /// <summary>
    /// Item 데이터(Key값은 ItemID)각 Item 데이터를 저장하고 있음
    /// </summary>
    public ItemDataBase ItemTable { get => _ItemTable; set => _ItemTable = value; }
    /// <summary>
    /// Player Skill 데이터(Key값은 Player SkillID)각 Player Skill 데이터를 저장하고 있음
    /// </summary>
    public PlayerSkillDataBase PlayerSkillTable { get => _PlayerSkillTable; set => _PlayerSkillTable = value; }
    /// <summary>
    /// Stage 데이터(Key값은 StageID)각 Stage 데이터를 저장하고 있음
    /// </summary>
    public StageDataBase StageTable { get => _StageTable; set => _StageTable = value; }
    /// <summary>
    /// Shop 데이터(Key값은 Shopindex)각 Shop에서 판매하는 아이템 정보를 저장하고 있음
    /// </summary>
    public ShopDataBase ShopTable { get => _ShopTable; set => _ShopTable = value; }
    /// <summary>
    /// 유닛 필요 경험치 테이블(Key값은 레벨)
    /// </summary>
    public UnitEXPDataBase UnitExpTable { get => _UnitExpTable; set => _UnitExpTable = value; }
    /// <summary>
    /// 캐릭터 상점 가격 테이블(Key값은 구매 횟수)
    /// </summary>
    public CharterPriceDataBase CharterPriceTable { get => _CharterPriceTable; set => _CharterPriceTable = value; }

    public GameDataBase()
    {
        UnitTable = new UnitDataBase();
        SkillTable = new UnitSkillDataBase();
        CharterTable = new CharterDataBase();
        MonsterTable = new MonsterDataBase();
        EffectTable = new EffectDataBase();
        MonsterSkillTable = new UnitSkillDataBase();
        ItemTable = new ItemDataBase();
        PlayerSkillTable = new PlayerSkillDataBase();
        StageTable = new StageDataBase();
        UnitExpTable = new UnitEXPDataBase();
        ShopTable = new ShopDataBase();
        CharterPriceTable = new CharterPriceDataBase();
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _Instance = this;
        UnitTable = new UnitDataBase();
        SkillTable = new UnitSkillDataBase();
        CharterTable = new CharterDataBase();
        MonsterTable = new MonsterDataBase();
        EffectTable = new EffectDataBase();
        MonsterSkillTable = new UnitSkillDataBase();
        ItemTable = new ItemDataBase();
        PlayerSkillTable = new PlayerSkillDataBase();
        StageTable = new StageDataBase();
        UnitExpTable = new UnitEXPDataBase();
        ShopTable = new ShopDataBase();
        CharterPriceTable = new CharterPriceDataBase();
        if (!GoogleSheat)
        {
            GoogleSheat = FindObjectOfType(typeof(GoogleSheatData)) as GoogleSheatData;
            if (!GoogleSheat)
            {
                GameObject container = new GameObject();
                container.name = "GoogleSheatDataContainer";
                GoogleSheat = container.AddComponent(typeof(GoogleSheatData)) as GoogleSheatData;
            }
        }
    }

    public bool LoadData(System.Action<float> Rspone)
    {
        int m_CurrentNum = 0;

        // 유닛 테이블 불러오기
        GoogleSheat.StartLoad<UnitInfo>(UnitInfo.GetGoogleSheetGID(), UnitTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });
        // 캐릭터 테이블 불러오기
        GoogleSheat.StartLoad<CharterInfo>(CharterInfo.GetGoogleSheetGID(), CharterTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });

        // 캐릭터 스킬 테이블 불러오기
        GoogleSheat.StartLoad<UnitSkillInfo>(CharterSkillInfo.GetGoogleSheetGID(), SkillTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });
        // 몬스터 테이블 불러오기
        GoogleSheat.StartLoad<MonsterInfo>(MonsterInfo.GetGoogleSheetGID(), MonsterTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });
        //몬스터 스킬 테이블 불러오기
        GoogleSheat.StartLoad<UnitSkillInfo>(MonsterSkillInfo.GetGoogleSheetGID(), MonsterSkillTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });
        //효과 테이블 불러오기
        GoogleSheat.StartLoad<EffectInfo>(EffectInfo.GetGoogleSheetGID(), EffectTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });
        // 아이템 테이블 불러오기
        GoogleSheat.StartLoad<ItemInfo>(ItemInfo.GetGoogleSheetGID(), ItemTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });
        // 플레이어 스킬 테이블 불러오기
        GoogleSheat.StartLoad<PlayerSkillInfo>(PlayerSkillInfo.GetGoogleSheetGID(), PlayerSkillTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });

        //스테이지 정보 테이블 불러오기
        GoogleSheat.StartLoad<StageInfo>(StageInfo.GetGoogleSheetGID(), StageTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });

        //Shop 정보 테이블 불러오기
        GoogleSheat.StartLoad<ShopInfo>(ShopInfo.GetGoogleSheetGID(), ShopTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });

        //UnitEXP 정보 테이블 불러오기
        GoogleSheat.StartLoad<UnitEXPInfo>(UnitEXPInfo.GetGoogleSheetGID(), UnitExpTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });

        // 캐릭터 가격 테이블 불러오기
        GoogleSheat.StartLoad<CharterPriceInfo>(CharterPriceInfo.GetGoogleSheetGID(), CharterPriceTable, (bSucced) =>
        {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {

            }
        });

        PlayerDataManager.PlayerData.PlayerDataLoad(PLAYERDATAFILE.ALL_DATAFILE, (bSucced) => {
            if (bSucced)
            {
                Debug.Log("다운 성공");
                m_CurrentNum++;
                Rspone(m_CurrentNum / MaxSheetNum);
            }
            else
            {
                Debug.Log("로드 실패");
            }
        });
        
        return false;
    }
}
