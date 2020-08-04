using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class InventoryETCItemData
{
    [SerializeField]
    // 아이템 리스트
    private PlayerItemData _ItemList;
    /// <summary>
    /// 아이템 리스트
    /// </summary>
    public PlayerItemData ItemList { get => _ItemList; set => _ItemList = value; }

    public InventoryETCItemData()
    {
        ItemList = new PlayerItemData();
    }
    public void AddItem(int index, int Count)
    {
        foreach(var item in ItemList)
        {
            if (item.Value.iItemIndex == index)
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
        Citem.iItemIndex = index;
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

    public bool ItemUse(int Index, int Count)
    {
        foreach (var item in ItemList)
        {
            if (item.Value.iItemIndex == Index)
            {
                if(item.Value.iCount >= Count)
                {
                    item.Value.iCount -= Count;
                    if(item.Value.iCount <= 0)
                    {
                        DeleteItem(item.Key);
                    }
                    return true;
                }
            }
        }

        return false;
    }
    public CharterItem FindItem(int Index)
    {
        return ItemList[Index];
    }

    public int FindItemIndexSelectCount(int Index)
    {
        return (from item in ItemList where item.Value.iItemIndex == Index select item.Value.iCount).FirstOrDefault();
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
}
