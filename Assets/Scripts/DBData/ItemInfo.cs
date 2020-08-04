using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemInfo
{
    public static int GetGoogleSheetGID() { return 1470457592; }

    #region 아이템 정보 변수
    [SerializeField]
    private int _iItemId;    // 아이템 ID
    [SerializeField]
    private string _strItemName;   //아이템 이름
    [SerializeField]
    private ITEMTYPE _Type;  //아이템 타입
    [SerializeField]
    private int _iItemTier;  //아이템 타겟 대상
    [SerializeField]
    private TARGET _Target;  //아이템 타겟
    [SerializeField]
    private TARGETOBJECT _TargetSelect;  //아이템 타겟 대상
    [SerializeField]
    private RANGE _ItemRange;  //아이템 타겟 범위
    [SerializeField]
    private bool _bisRemove;  //소모 아이템 여부
    [SerializeField]
    private int _iEffectID;  //아이템 효과 종류
    [SerializeField]
    private int _iEffectValue;  //아이템 효과 값
    [SerializeField]
    private string _strIcon;  //아이템 아이콘 명
    [SerializeField]
    private string _strItemDesc;  //아이템 설명
    #endregion

    #region 캡슐화
    /// <summary>
    /// 아이템의 ID
    /// </summary>
    public int IItemId { get => _iItemId; set => _iItemId = value; }
    /// <summary>
    /// 아이템의 이름
    /// </summary>
    public string StrItemName { get => _strItemName; set => _strItemName = value; }
    /// <summary>
    /// 아이템의 종류(잡화, 소모품)
    /// </summary>
    public ITEMTYPE Type { get => _Type; set => _Type = value; }
    /// <summary>
    /// 아이템 등급
    /// </summary>
    public int IItemTier { get => _iItemTier; set => _iItemTier = value; }
    /// <summary>
    /// 아이템 사용 시 효과 적용 대상(아군, 적군, 아군/적군, 플레이어)
    /// </summary>
    public TARGET Target { get => _Target; set => _Target = value; }
    /// <summary>
    /// 아이템 사용 시 효과 적용 대상 선택 방식(일반대상[랜덤], 시전자 대상, 체력이 가장 낮은 대상, 체력이 가장 높은 대상, 전열에 있는 대상, 중열에 있는 대상, 후열에 있는 대상)
    /// </summary>
    public TARGETOBJECT TargetSelect { get => _TargetSelect; set => _TargetSelect = value; }
    /// <summary>
    /// 아이템 사용 시 효과 적용 대상 수(단일, 범위)
    /// </summary>
    public RANGE ItemRange { get => _ItemRange; set => _ItemRange = value; }
    /// <summary>
    /// 전투 중에 사용 가능한 아이템인가? (No/Yes)
    /// </summary>
    public bool BisRemove { get => _bisRemove; set => _bisRemove = value; }
    /// <summary>
    /// 아이템 효과 ID
    /// </summary>
    public int IEffectID { get => _iEffectID; set => _iEffectID = value; }
    /// <summary>
    /// 아이템 효과 값
    /// </summary>
    public int IEffectValue { get => _iEffectValue; set => _iEffectValue = value; }
    /// <summary>
    /// 아이콘 명
    /// </summary>
    public string StrIcon { get => _strIcon; set => _strIcon = value; }
    /// <summary>
    /// 아이템 설명
    /// </summary>
    public string StrItemDesc { get => _strItemDesc; set => _strItemDesc = value; }
    #endregion

    #region 생성자
    public ItemInfo(string Id, string ItemName, string ItemType, string ItemTier, string ItemisRemove, string itemTarget, string ItemTargetSelect,
        string strItemRange, string ItemEffectID, string ItemEffectValue, string ItemIcon, string ItemDesc)
    {
        IItemId = DataProcess.stringToint(Id);
        StrItemName = DataProcess.stringToNull(ItemName);
        Type = (ITEMTYPE)DataProcess.stringToint(ItemType);
        IItemTier = DataProcess.stringToint(ItemTier);
        Target = (TARGET)DataProcess.stringToint(itemTarget);
        TargetSelect = (TARGETOBJECT)DataProcess.stringToint(ItemTargetSelect);
        ItemRange = (RANGE)DataProcess.stringToint(strItemRange);
        BisRemove = DataProcess.stringTobool(ItemisRemove);
        IEffectID = DataProcess.stringToint(ItemEffectID);
        IEffectValue = DataProcess.stringToint(ItemEffectValue);
        StrIcon = DataProcess.stringToNull(ItemIcon);
        StrItemDesc = DataProcess.stringToNull(ItemDesc);
    }
    #endregion
}

[System.Serializable]
public class ItemDataBase : SerializableDictionary<int, ItemInfo> { }