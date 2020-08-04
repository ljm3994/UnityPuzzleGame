using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ShopUIMenuController : PanelController
{
    #region inspector

    public TopGSBarController GSBar;
    public Button GoBackBtn;

    public Button ShopGSBtn;


    public Transform unitUnitSpace;
    public List<GridUnitController> ShopUnits = new List<GridUnitController>();
    public Transform itemUnitSpace;
    public List<GridUnitController> ShopItems = new List<GridUnitController>();
    #endregion

    public override void Setup()
    {
        base.Setup();
        GoBackBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "MainPanel"); });
        ShopGSBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "Shop_GoldStemina_Panel"); });

    }

    public override void Load()
    {
        base.Load();
        GSBar.Load();

        string unitPrefabName = "GridUnit_ShopUnit";
        string itemPrefabName = "GridUnit_ShopItem";

        GameObject unitUnitPrefab = UIManager.instance.GetGridUnitPrefab(unitPrefabName);
        if (unitUnitPrefab != null)
        {
            var shop = TestLoadDatas.instance.ShopCharterIndex;

            for(int i = 0; i< shop.Length; ++i)
            {
                var shopInfo = UIDataProcess.GetShopInfo(shop[i]);

                if(shopInfo == null)
                {
                    Debug.Log(unitPrefabName + " " + i + " missing!");
                    continue;
                }
                
                GameObject gridUnit = GameObject.Instantiate(unitUnitPrefab, unitUnitSpace);
                gridUnit.name = unitPrefabName + i;

                var controller = gridUnit.GetComponent<GridUnitController>();
                ShopUnits.Add(controller);
                controller.Setup(shopInfo);
            }
            UICommon.FitGridSize(unitUnitSpace, ShopUnits.Count);
        }
        else Debug.Log("GridUnitPrefab is Missing! name : " + unitPrefabName);

        GameObject itemUnitPrefab = UIManager.instance.GetGridUnitPrefab(itemPrefabName);
        if (itemUnitPrefab != null)
        {
            var shop = TestLoadDatas.instance.ShopItemIndex;

            for (int i = 0; i < shop.Length; ++i)
            {
                var shopInfo = UIDataProcess.GetShopInfo(shop[i]);

                if (shopInfo == null)
                {
                    Debug.Log(itemPrefabName + " " + i + " missing!");
                    continue;
                }

                GameObject gridUnit = GameObject.Instantiate(itemUnitPrefab, itemUnitSpace);
                gridUnit.name = itemPrefabName + i;

                var controller = gridUnit.GetComponent<GridUnitController>();
                ShopItems.Add(controller);
                controller.Setup(shopInfo);
            }
            UICommon.FitGridSize(itemUnitSpace, ShopItems.Count);
        }
        else Debug.Log("GridUnitPrefab is Missing! name : " + itemPrefabName);

    }

    public override void ClearGrid()
    {
        foreach (var unit in ShopUnits)
        {
            Destroy(unit.gameObject);
        }
        ShopUnits.Clear();

        foreach (var unit in ShopItems)
        {
            Destroy(unit.gameObject);
        }
        ShopItems.Clear();
    }
}
