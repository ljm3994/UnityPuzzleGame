using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DATAFILEName
{
    User_Data,
    User_Item,
    User_Skill,
    User_Unit,
    User_ETCItem
}

[Flags]
public enum PLAYERDATAFILE
{
    USER_DATAFILE = 1 << 0,
    ITEM_DATAFILE = 1 << 1,
    ETCITEM_DATAFILE = 1 << 2,
    SKILL_DATAFILE = 1 << 3,
    UNIT_DATAFILE = 1 << 4,
    ALL_DATAFILE = USER_DATAFILE | ITEM_DATAFILE | ETCITEM_DATAFILE | SKILL_DATAFILE | UNIT_DATAFILE
}
[System.Serializable]
public class PlayerData
{
    [SerializeField]
    private string _strCurrentTimer;
    [SerializeField]
    private string strName;
    [SerializeField]
    private int iPlayerEXP;
    [SerializeField]
    // 플레이어 골드 재화
    private int _Coin;
    [SerializeField]
    private int _iCurrentTopNum;
    [SerializeField]
    private int _iCurrentTopFloor;
    [SerializeField]
    private int _iNextCoolTime;
    /// <summary>
    /// 플레이어 골드 재화
    /// </summary>
    public int iCoin
    {
        get
        {
            return _Coin;
        }
        set
        {
            _Coin = value;
        }
    }
    [SerializeField]
    // 플레이어 다이아몬드 재화
    private int _Diamond;
    /// <summary>
    /// 플레이어 다이아몬드 재화
    /// </summary>
    public int iDiamond
    {
        get
        {
            return _Diamond;
        }
        set
        {
            _Diamond = value;
        }
    }
    /// <summary>
    /// 플레이어 현재 스테미너
    /// </summary>
    public int iStamina { get => _Stamina; set => _Stamina = value; }
    /// <summary>
    /// 플레이어 레벨
    /// </summary>
    public int ILevel { get => _iLevel; set => _iLevel = value; }
    /// <summary>
    /// 플레이어 이름(구글 아이디)
    /// </summary>
    public string StrName { get => strName; set => strName = value; }
    /// <summary>
    /// 플레이어 현재 경험치
    /// </summary>
    public int IPlayerEXP { get => iPlayerEXP; set => iPlayerEXP = value; }
    /// <summary>
    /// 세이브 시간
    /// </summary>
    public string StrCurrentTimer { get => _strCurrentTimer; set => _strCurrentTimer = value; }
    public int ICurrentTopNum { get => _iCurrentTopNum; set => _iCurrentTopNum = value; }
    public int ICurrentTopFloor { get => _iCurrentTopFloor; set => _iCurrentTopFloor = value; }
    public int INextCoolTime { get => _iNextCoolTime; set => _iNextCoolTime = value; }

    [SerializeField]
    private int _Stamina;
    [SerializeField]
    private int _iLevel;

    public PlayerData()
    {
        IPlayerEXP = 0;
        iCoin = 100000;
        iDiamond = 100000;
        ILevel = 1;
        iStamina = 12;
        StrName = "Temp";
        ICurrentTopNum = 1;
        ICurrentTopFloor = 1;
    }
}

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager _PlayerData;
    public static PlayerDataManager PlayerData
    {
        get
        {
            if(!_PlayerData)
            {
                _PlayerData = FindObjectOfType(typeof(PlayerDataManager)) as PlayerDataManager;
                if(!_PlayerData)
                {

                    GameObject container = new GameObject();
                    container.name = "PlayerDataManagerContainer";
                    _PlayerData = container.AddComponent(typeof(PlayerDataManager)) as PlayerDataManager;
                }
            }

            return _PlayerData;
        }
    }
    /// <summary>
    /// 플레이어 정보 데이터
    /// </summary>
    public PlayerData Pdata { get => _pdata; set => _pdata = value; }
    /// <summary>
    /// 플레이어 소지 아이템 데이터
    /// </summary>
    public InventoryItem PlayerItem { get => _PlayerItem; set => _PlayerItem = value; }
    /// <summary>
    /// 플레이어 소지 유닛 데이터
    /// </summary>
    public UintInventory UnitInventory { get => _UnitInventory; set => _UnitInventory = value; }
    /// <summary>
    /// 플레이어 소지 스킬 데이터
    /// </summary>
    public InventorSkill SkillInventory { get => _SkillInventory; set => _SkillInventory = value; }
    /// <summary>
    /// 플레이어 소지 잡화 아이템 데이터
    /// </summary>
    public InventoryETCItemData InventoryETCItemData { get => _inventoryETCItemData; set => _inventoryETCItemData = value; }
    DateTime PauseTime;
    bool bPause;
    [SerializeField]
    private InventorSkill _SkillInventory;
    [SerializeField]
    private UintInventory _UnitInventory;
    [SerializeField]
    private InventoryItem _PlayerItem;
    [SerializeField]
    private PlayerData _pdata;
    [SerializeField]
    private InventoryETCItemData _inventoryETCItemData;
    public PlayerDataManager()
    {
        Pdata = new PlayerData();
        _PlayerItem = new InventoryItem();
        UnitInventory = new UintInventory();
        SkillInventory = new InventorSkill();
        InventoryETCItemData = new InventoryETCItemData();
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);
        PauseTime = DateTime.Now.ToLocalTime();
        bPause = false;
        _PlayerData = this;
        Pdata = new PlayerData();
        _PlayerItem = new InventoryItem();
        UnitInventory = new UintInventory();
        SkillInventory = new InventorSkill();
        InventoryETCItemData = new InventoryETCItemData();
    }
    public string GetDataString(DATAFILEName FileName)
    {
        string str = "";
        switch (FileName)
        {
            case DATAFILEName.User_Data:
                Pdata.StrCurrentTimer = DateTime.Now.ToLocalTime().ToBinary().ToString();
                str = JsonUtility.ToJson(Pdata, true);
                break;
            case DATAFILEName.User_Item:
                str = JsonUtility.ToJson(PlayerItem, true);
                for(int i = 0; i < PlayerItem.EquipmentItemList.Length; i++)
                {
                    if(PlayerItem.EquipmentItemList[i] != null)
                    {
                        if(PlayerItem.EquipmentItemList[i].iItemIndex == 0)
                        {
                            PlayerItem.EquipmentItemList[i] = null;
                        }
                    }
                }
                break;
            case DATAFILEName.User_Skill:
                str = JsonUtility.ToJson(SkillInventory, true);
                for (int i = 0; i < SkillInventory.playerEquipSkills.Length; i++)
                {
                    if (SkillInventory.playerEquipSkills[i] != null)
                    {
                        if (SkillInventory.playerEquipSkills[i].iIndex == 0)
                        {
                            SkillInventory.playerEquipSkills[i] = null;
                        }
                    }
                }
                break;
            case DATAFILEName.User_Unit:
                str = JsonUtility.ToJson(UnitInventory, true);
                for (int i = 0; i < UnitInventory.EquipmentUnit.Length; i++)
                {
                    if (UnitInventory.EquipmentUnit[i] != null)
                    {
                        if (UnitInventory.EquipmentUnit[i].iIndex == 0)
                        {
                            UnitInventory.EquipmentUnit[i] = null;
                        }
                    }
                }
                break;
            case DATAFILEName.User_ETCItem:
                str = JsonUtility.ToJson(InventoryETCItemData, true);
                break;
        }
        
        return str;
    }
    /// <summary>
    /// string형식의 값을 데이터 클래스에 맞게 변형시켜주는 함수
    /// </summary>
    /// <param name="Data">데이터 유형</param>
    /// <param name="strData">데이터 문자열</param>
    public void SetDataString(DATAFILEName FileName, string strData)
    {
        if (strData != "0")
        {
            switch (FileName)
            {
                case DATAFILEName.User_Data:
                    JsonUtility.FromJsonOverwrite(strData, _pdata);
                    string strTimer = _pdata.StrCurrentTimer;
                    SteminaManager.Instance.SetLoadTimer(DateTime.FromBinary(Convert.ToInt64(strTimer)), _pdata.INextCoolTime);
                    break;
                case DATAFILEName.User_Item:
                    JsonUtility.FromJsonOverwrite(strData, PlayerItem);
                    break;
                case DATAFILEName.User_Skill:
                    JsonUtility.FromJsonOverwrite(strData, SkillInventory);
                    break;
                case DATAFILEName.User_Unit:
                    JsonUtility.FromJsonOverwrite(strData, UnitInventory);
                    break;
                case DATAFILEName.User_ETCItem:
                    JsonUtility.FromJsonOverwrite(strData, InventoryETCItemData);
                    break;
            }
        }
        else
        {
            //TestDefaultData();
        }
    }
    /// <summary>
    /// 플레이 데이터를 저장하는 함수
    /// </summary>
    /// <param name="Data">저장할 데이터의 유형</param>
    public void PlayerDataSave(PLAYERDATAFILE Data, Action<bool> action)
    { 
        List<DATAFILEName> FileName = new List<DATAFILEName>();

        if((Data & PLAYERDATAFILE.USER_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_Data);
        }
        if((Data & PLAYERDATAFILE.ITEM_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_Item);
        }
        if((Data & PLAYERDATAFILE.SKILL_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_Skill);
        }
        if((Data & PLAYERDATAFILE.UNIT_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_Unit);
        }
        if((Data & PLAYERDATAFILE.ETCITEM_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_ETCItem);
        }

        int MaxName = FileName.Count;
        int CurrentNum = 0;
        foreach (var item in FileName)
        {
            gpgsmanager.Instance.SaveOpenFile(item, true, (succed) => {
                 CurrentNum++;

                if (CurrentNum >= MaxName)
                {
                    action(true);
                }
            });
        }
        
    }

    // 앱 종료시 저장한다.
    private void OnApplicationQuit()
    {
    }

    // 플레이 도중 홈버튼을 누르거나 기타 방법등으로 앱을 비활성화 시키면 호출 된다.
    // OnApplicationFocus함수는 첫 실행시에도 호출 되나 
    // OnApplicationPause함수는 첫 실행시에는 호출 되지 않는다.
    private void OnApplicationPause(bool pause)
    { 
        //앱 비활성화 시 세이브 실행
        if(pause)
        {
            if (!bPause)
            {
                bPause = true;
                PauseTime = DateTime.Now.ToLocalTime();
                Debug.Log(PauseTime);
            }
        }
        else
        {
            if (bPause)
            {
                bPause = false;
                if (GameDataBase.Instance.CharterTable.Count > 0)
                {
                    Debug.Log(DateTime.Now.ToLocalTime());
                    SteminaManager.Instance.SetLoadTimer(PauseTime, PlayerDataManager.PlayerData.Pdata.INextCoolTime);

                }
            }
        }
    }
    /// <summary>
    /// 데이터 로드시 사용되는 함수
    /// </summary>
    /// <param name="Data">로드할 데이터 유형</param>
    public void PlayerDataLoad(PLAYERDATAFILE Data, Action<bool> action)
    {
        List<DATAFILEName> FileName = new List<DATAFILEName>();

        if ((Data & PLAYERDATAFILE.USER_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_Data);
        }
        if ((Data & PLAYERDATAFILE.ITEM_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_Item);
        }
        if ((Data & PLAYERDATAFILE.SKILL_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_Skill);
        }
        if ((Data & PLAYERDATAFILE.UNIT_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_Unit);
        }
        if ((Data & PLAYERDATAFILE.ETCITEM_DATAFILE) != 0)
        {
            FileName.Add(DATAFILEName.User_ETCItem);
        }

        int MaxName = FileName.Count;
        int CurrentNum = 0;
        foreach (var item in FileName)
        {
            gpgsmanager.Instance.SaveOpenFile(item, false, (succed) => {
                if (!succed)
                {
                    action(false);
                }
                else
                {
                    CurrentNum++;
                }

                if (CurrentNum >= MaxName)
                {
                    action(true);
                }
            });
        }
    }

    public void SkillDefalultData()
    {
        PlayerSkill playerSkill = new PlayerSkill();
        playerSkill.iIndex = 32001;
        playerSkill.BEquipped = false;
        PlayerSkill playerSkill2 = new PlayerSkill();
        playerSkill2.iIndex = 32004;
        playerSkill2.BEquipped = false;
        PlayerSkill playerSkill3 = new PlayerSkill();
        playerSkill3.iIndex = 32010;
        playerSkill3.BEquipped = false;
        PlayerSkill playerSkill4 = new PlayerSkill();
        playerSkill4.iIndex = 32007;
        playerSkill4.BEquipped = false;
        PlayerSkill playerSkill5 = new PlayerSkill();
        playerSkill5.iIndex = 32013;
        playerSkill5.BEquipped = false;
        PlayerSkill playerSkill6 = new PlayerSkill();
        playerSkill6.iIndex = 32016;
        playerSkill6.BEquipped = false;
        PlayerSkill playerSkill7 = new PlayerSkill();
        playerSkill7.iIndex = 32019;
        playerSkill7.BEquipped = false;

        SkillInventory.SkillList.Clear();
        SkillInventory.playerEquipSkills[0] = null;
        SkillInventory.playerEquipSkills[1] = null;
        SkillInventory.playerEquipSkills[2] = null;
        SkillInventory.AddItem(playerSkill);
        SkillInventory.AddItem(playerSkill2);
        SkillInventory.AddItem(playerSkill3);
        SkillInventory.AddItem(playerSkill4);
        SkillInventory.AddItem(playerSkill5);
        SkillInventory.AddItem(playerSkill6);
        SkillInventory.AddItem(playerSkill7);
    }

    public void UnitDefaultData()
    {
        PlayerUnit unit = new PlayerUnit();
        unit.iLevel = 1;
        unit.iIndex = 11201;
        unit.IExp = 20;
        unit.ICount = 1;
        unit.bEquipped = false;
        UnitInventory.EquipmentUnit[4] = null;
        PlayerUnit unit2 = new PlayerUnit();
        unit2.iLevel = 10;
        unit2.iIndex = 52102;
        unit2.ICount = 1;
        unit2.IExp = 1169;
        unit2.bEquipped = false;
        UnitInventory.EquipmentUnit[2] = null;
        PlayerUnit unit3 = new PlayerUnit();
        unit3.iLevel = 30;
        unit3.iIndex = 13203;
        unit3.ICount = 1;
        unit3.IExp = 15000;
        unit3.bEquipped = false;
        UnitInventory.EquipmentUnit[3] = null;
        PlayerUnit unit4 = new PlayerUnit();
        unit4.iLevel = 30;
        unit4.iIndex = 11204;
        unit4.ICount = 1;
        unit4.IExp = 22072;
        unit4.bEquipped = false;
        UnitInventory.EquipmentUnit[0] = null;
        PlayerUnit unit5 = new PlayerUnit();
        unit5.iLevel = 1;
        unit5.iIndex = 52106;
        unit5.ICount = 1;
        unit5.IExp = 100;
        unit5.bEquipped = false;
        UnitInventory.EquipmentUnit[1] = null;
        UnitInventory.UintList.Clear();
        UnitInventory.UintList.Add(0, unit);
        UnitInventory.UintList.Add(1, unit2);
        UnitInventory.UintList.Add(2, unit3);
        UnitInventory.UintList.Add(3, unit4);
        UnitInventory.UintList.Add(4, unit5);
    }

    public void PlayerDataDefault()
    {
        Pdata.iCoin = 100000;
        Pdata.ILevel = 5;
        Pdata.iStamina = 24;
        Pdata.iDiamond = 100000;
        Pdata.IPlayerEXP = 10;
        Pdata.ICurrentTopFloor = 10;
        Pdata.ICurrentTopNum = 2;
        if (Social.localUser.authenticated)
        {
            Pdata.StrName = Social.localUser.userName;
        }
        else
        {
            Pdata.StrName = "TempName";
        }
    }
    /// <summary>
    /// 디버깅용 기본 데이터 구성 함수
    /// </summary>
    public void ItemDefaultData()
    {
        PlayerItem.ItemList.Clear();
        InventoryETCItemData.ItemList.Clear();

        for (int i = 0; i < 7; ++i)
        {
            int iItemIndex = 2001 + i;
            InventoryETCItemData.AddItem(iItemIndex, 1);
        }
    }

}
