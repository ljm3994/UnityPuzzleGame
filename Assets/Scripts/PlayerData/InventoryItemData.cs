using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharterItem
{
    [SerializeField]
    // 아이템 인덱스 번호
    private int _ItemIndex;
    [SerializeField]
    // 아이템 수량
    private int _iCount;
    [SerializeField]
    // 아이템 수량
    private bool _bEquipment;
    /// <summary>
    /// 아이템 인덱스 번호
    /// </summary>
    public int iItemIndex { get => _ItemIndex; set => _ItemIndex = value; }
    /// <summary>
    /// 아이템 수량
    /// </summary>
    public int iCount { get => _iCount; set => _iCount = value; }
    public bool BEquipment { get => _bEquipment; set => _bEquipment = value; }
}
[System.Serializable]
public class PlayerItemData : SerializableDictionary<int, CharterItem> { }
[System.Serializable]
public class InventoryItem : ISerializationCallbackReceiver
{
    [SerializeField]
    // 아이템 리스트
    private PlayerItemData _ItemList;
    [SerializeField]
    private CharterItem[] _EquipmentItemList;
    /// <summary>
    /// 아이템 리스트
    /// </summary>
    public PlayerItemData ItemList { get => _ItemList; set => _ItemList = value; }
    public CharterItem[] EquipmentItemList { get => _EquipmentItemList; set => _EquipmentItemList = value; }

    public InventoryItem()
    {
        ItemList = new PlayerItemData();
        EquipmentItemList = new CharterItem[3];
    }
    public void AddItem(int Index, int Count)
    {
        foreach (var item in ItemList)
        {
            if(item.Value.iItemIndex == Index)
            {
                item.Value.iCount += Count;
                if (item.Value.iCount > 99)
                {
                    item.Value.iCount = 99;
                }
                return;
            }
        }

        CharterItem Citem = new CharterItem();
        Citem.iCount = Count;
        Citem.iItemIndex = Index;
        ItemList.Add(ItemList.Count, Citem);

    }

    public void DeleteItem(int Index)
    {
        ItemList.Remove(Index);
    }

    public void DeleteItem(CharterItem item)
    {
        int key = -1;
        foreach (var charterItem in ItemList)
        {
            if (charterItem.Value.iItemIndex == item.iItemIndex)
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

    public CharterItem FindItem(int Index)
    {
        return ItemList[Index];
    }

    public int FindItem(CharterItem item)
    {
        foreach (var charterItem in ItemList)
        {
            if (charterItem.Value.iItemIndex == item.iItemIndex)
            {
                return charterItem.Key;
            }
        }

        return -1;
    }

    public int FindItemCount(int Index)
    {
        foreach (var charterItem in ItemList)
        {
            if (charterItem.Value.iItemIndex == Index)
            {
                return charterItem.Value.iCount;
            }
        }

        return 0;
    }

    public void SetEquipmentItem(int Index, bool Equipment)
    {
        for(int i = 0; i < ItemList.Count; i++)
        {
            if(ItemList[i].iItemIndex == Index)
            {
                ItemList[i].BEquipment = Equipment;
            }
        }
    }

    public void OnBeforeSerialize()
    {
        ((ISerializationCallbackReceiver)ItemList).OnBeforeSerialize();
    }

    public void OnAfterDeserialize()
    {
        ((ISerializationCallbackReceiver)ItemList).OnAfterDeserialize();

        for(int i = 0; i < EquipmentItemList.Length; i++)
        {
            if(EquipmentItemList[i] != null)
            {
                if(EquipmentItemList[i].iItemIndex == 0)
                {
                    EquipmentItemList[i] = null;
                }
            }
        }
    }
}
