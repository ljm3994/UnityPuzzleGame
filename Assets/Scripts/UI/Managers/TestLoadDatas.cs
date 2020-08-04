using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class TestLoadDatas : MonoBehaviour
{
    public static TestLoadDatas instance;
    public bool debug = false;
    private int[] _ShopItemIndex;
    private int[] _ShopCharterIndex;
    private int[] _ShopGoldIndex;
    private int[] _ShopSteminaIndex;

    public int[] ShopItemIndex { get => _ShopItemIndex; set => _ShopItemIndex = value; }
    public int[] ShopCharterIndex { get => _ShopCharterIndex; set => _ShopCharterIndex = value; }
    public int[] ShopGoldIndex { get => _ShopGoldIndex; set => _ShopGoldIndex = value; }
    public int[] ShopSteminaIndex { get => _ShopSteminaIndex; set => _ShopSteminaIndex = value; }

    private void Awake()
    {
        instance = this;
        ShopItemIndex = (from item in GameDataBase.Instance.ShopTable where item.Value.ItemType == SHOPITEM_TYPE.EXPENDABLES_TYPE select item.Key).ToArray();
        ShopCharterIndex = (from item in GameDataBase.Instance.ShopTable where item.Value.ItemType == SHOPITEM_TYPE.CHARTER_TYPE select item.Key).ToArray();
        ShopGoldIndex = (from item in GameDataBase.Instance.ShopTable where item.Value.ItemType == SHOPITEM_TYPE.GOLD_TYPE select item.Key).ToArray();
        ShopSteminaIndex = (from item in GameDataBase.Instance.ShopTable where item.Value.ItemType == SHOPITEM_TYPE.STEMINA_TYPE select item.Key).ToArray();
    }
}
