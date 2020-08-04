using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class PlayerUnit
{
    [SerializeField]
    // 유닛의 인덱스 번호
    private int _iIndex;
    [SerializeField]
    // 유닛의 레벨
    private int _iLevel;
    [SerializeField]
    // 장착 상태인지의 여부
    private bool _Equipped;
    [SerializeField]
    // 유닛 경험치
    private int _iExp;
    [SerializeField]
    private int _iCount;
    /// <summary>
    /// 유닛의 인덱스 번호
    /// </summary>
    public int iIndex { get => _iIndex; set => _iIndex = value; }
    /// <summary>
    /// 유닛의 레벨
    /// </summary>
    public int iLevel { get => _iLevel; set => _iLevel = value; }
    /// <summary>
    /// 장착 상태인지의 여부
    /// </summary>
    public bool bEquipped { get => _Equipped; set => _Equipped = value; }
    /// <summary>
    /// 유닛의 경험치
    /// </summary>
    public int IExp { get => _iExp; set => _iExp = value; }
    /// <summary>
    /// 유닛 중복시 구분할 유닛 키
    /// </summary>
    public int ICount { get => _iCount; set => _iCount = value; }
}
[System.Serializable]
public class PlayerUnitData : SerializableDictionary<int, PlayerUnit> { }

[System.Serializable]
public class UintInventory : ISerializationCallbackReceiver
{
    [SerializeField]
    private PlayerUnit[] _EquipmentUnit;
    [SerializeField]
    // 유닛 리스트
    private PlayerUnitData _UintList;
    /// <summary>
    /// 유닛 리스트
    /// </summary>
    public PlayerUnitData UintList { get => _UintList; set => _UintList = value; }
    /// <summary>
    /// 장착 유닛 리스트(0~1 후열, 2~3 중열, 4 전열)
    /// </summary>
    public PlayerUnit[] EquipmentUnit { get => _EquipmentUnit; set => _EquipmentUnit = value; }
    public UintInventory()
    {
        UintList = new PlayerUnitData();
        EquipmentUnit = new PlayerUnit[5];
    }

    public void UnitAdd(int Index)
    {
        PlayerUnit unit = new PlayerUnit();
        unit.bEquipped = false;
        unit.IExp = 0;
        unit.iLevel = 1;
        unit.iIndex = Index;
        int Count = GetUnitCount(Index);
        unit.ICount = Count + 1;
        UintList.Add(UintList.Count, unit);
    }

    public int EquipmentUnitCount()
    {
        return (from item in EquipmentUnit where item != null select item.iIndex).Count();
    }
    public int UnitFind(PlayerUnit unit)
    {
        if (unit != null)
        {
            foreach (var item in UintList)
            {
                if (item.Value.iIndex == unit.iIndex && unit.ICount == item.Value.ICount)
                {
                    return item.Key;
                }
            }
        }

        return -1;
    }

    public bool UnitEquipmentFind(int Index, int Count , out int OutIndex)
    {
        OutIndex = 0;
        for (int i = 0; i < EquipmentUnit.Length; i++)
        {
            if(EquipmentUnit[i] != null)
            {
                if(EquipmentUnit[i].iIndex == Index && EquipmentUnit[i].ICount == Count)
                {
                    OutIndex = i;
                    return true;
                }
            }
        }
        return false;
    }

    public int GetUnitCount(int index)
    {
        var unit = (from item in UintList where item.Value.iIndex == index select item.Key).ToArray();

        return unit.Length;
    }
    
    public int GetUnitLevelUpCount()
    {
        var LevelUpCount = (from item in UintList where item.Value.iLevel < GameDataBase.Instance.UnitTable[item.Value.iIndex].iMaxLevel && item.Value.IExp >= GameDataBase.Instance.UnitExpTable[item.Value.iLevel + 1].INeedEXP select item.Key).ToList<int>();

        return LevelUpCount.Count;
    }

    public void SetEquipmentUnit(int Index, int Count, bool Equipment)
    {
        for(int i = 0; i < UintList.Count; i++)
        {
            if(UintList[i].iIndex == Index && UintList[i].ICount == Count)
            {
                UintList[i].bEquipped = Equipment;
            }
        }
    }
    public void OnBeforeSerialize()
    {
        ((ISerializationCallbackReceiver)UintList).OnBeforeSerialize();
    }

    public void OnAfterDeserialize()
    {
        ((ISerializationCallbackReceiver)UintList).OnAfterDeserialize();

        for(int i = 0; i < EquipmentUnit.Length; i++)
        {
            if(EquipmentUnit[i] != null)
            {
                if(EquipmentUnit[i].iIndex == 0)
                {
                    EquipmentUnit[i] = null;
                }
            }
        }
    }
}
