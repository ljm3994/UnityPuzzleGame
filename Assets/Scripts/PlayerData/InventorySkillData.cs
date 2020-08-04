using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerSkill
{
    [SerializeField]
    // 스킬 인덱스 번호
    private int _iIndex;
    [SerializeField]
    // 스킬 장착 여부
    private bool _bEquipped;

    [SerializeField]
    private int _iEquipmentIndex;
    /// <summary>
    /// 스킬 인덱스 번호
    /// </summary>
    public int iIndex { get => _iIndex; set => _iIndex = value; }
    /// <summary>
    /// 스킬 장착 여부
    /// </summary>
    public bool BEquipped { get => _bEquipped; set => _bEquipped = value; }
    /// <summary>
    /// 장착 번호
    /// </summary>
    public int IEquipmentIndex { get => _iEquipmentIndex; set => _iEquipmentIndex = value; }

}
[System.Serializable]
public class PlayerSkillData : SerializableDictionary<int, PlayerSkill> { }
[System.Serializable]
public class InventorSkill : ISerializationCallbackReceiver
{
    [SerializeField]
    // 장착 스킬 리스트
    private PlayerSkill[] _playerEquipSkills;
    [SerializeField]
    // 스킬 리스트
    private PlayerSkillData _SkillList;
    /// <summary>
    /// 장착 스킬 리스트(최대 3개)
    /// </summary>
    public PlayerSkill[] playerEquipSkills { get => _playerEquipSkills; set => _playerEquipSkills = value; }
    /// <summary>
    /// 스킬 리스트
    /// </summary>
    public PlayerSkillData SkillList { get => _SkillList; set => _SkillList = value; }

    public InventorSkill()
    {
        playerEquipSkills = new PlayerSkill[3];
        SkillList = new PlayerSkillData();
    }

    public void AddItem(PlayerSkill item)
    {
        SkillList.Add(SkillList.Count, item);
    }

    public void DeleteItem(int Index)
    {
        SkillList.Remove(Index);
    }

    public void DeleteItem(PlayerSkill item)
    {
        int key = -1;
        foreach (var charterItem in SkillList)
        {
            if (charterItem.Value.iIndex == item.iIndex)
            {
                key = charterItem.Key;
                break;
            }
        }

        if (key > -1)
        {
            DeleteItem(key);
        }
    }

    public PlayerSkill FindItem(int Index)
    {
        return SkillList[Index];
    }

    public int FindItem(PlayerSkill item)
    {
        if (item != null)
        {
            foreach (var charterItem in SkillList)
            {
                if (charterItem.Value.iIndex == item.iIndex)
                {
                    return charterItem.Key;
                }
            }
        }

        return -1;
    }

    public bool FindEquipmentItem(int index, out int OutIndex)
    {
        OutIndex = 0;
        for (int i = 0; i < playerEquipSkills.Length; i++)
        {
            if(playerEquipSkills[i] != null)
            {
                if(index == playerEquipSkills[i].iIndex)
                {
                    OutIndex = i;
                    return true;
                }
            }
        }

        return false;
    }

    public void SetSkillEquipment(int Index, bool Equipment)
    {
        for(int i = 0; i < SkillList.Count; i++)
        {
            if(SkillList[i].iIndex == Index)
            {
                SkillList[i].BEquipped = Equipment;
            }
        }
    }
    public void OnBeforeSerialize()
    {
        ((ISerializationCallbackReceiver)SkillList).OnBeforeSerialize();
    }

    public void OnAfterDeserialize()
    {
        ((ISerializationCallbackReceiver)SkillList).OnAfterDeserialize();

        for (int i = 0; i < playerEquipSkills.Length; i++)
        {
            if (playerEquipSkills[i] != null)
            {
                if (playerEquipSkills[i].iIndex == 0)
                {
                    playerEquipSkills[i] = null;
                }
            }
        }
    }
}

