using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ShopGSMenuController : PanelController
{
    #region inspector

    public TopGSBarController GSBar;
    public Button GoBackBtn;

    public Button ShopUIBtn;

    public Transform GoldUnitSpace;
    public List<GridUnitController> ShopGolds = new List<GridUnitController>();
    public Transform SteminaUnitSpace;
    public List<GridUnitController> ShopSteminas = new List<GridUnitController>();
    #endregion
    

    public override void Setup() 
    {
        base.Setup();
        GoBackBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "MainPanel"); });
        ShopUIBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "Shop_UnitItem_Panel"); });

    }

    public override void Load()
    {
        base.Load();
        GSBar.Load();

        string goldPrefabName = "GridUnit_ShopGold";
        string steminaPrefabName = "GridUnit_ShopStemina";

        GameObject goldUnitPrefab = UIManager.instance.GetGridUnitPrefab(goldPrefabName);
        if (goldUnitPrefab != null)
        {
            var shop = TestLoadDatas.instance.ShopGoldIndex;

            for(int i = 0; i< shop.Length; ++i)
            {
                var shopInfo = UIDataProcess.GetShopInfo(shop[i]);

                if(shopInfo == null)
                {
                    Debug.Log(goldPrefabName + " " + i + " missing!");
                    continue;
                }

                GameObject gridUnit = GameObject.Instantiate(goldUnitPrefab, GoldUnitSpace);
                gridUnit.name = goldPrefabName + i;

                var controller = gridUnit.GetComponent<GridUnitController>();
                ShopGolds.Add(controller);
                controller.Setup(shopInfo);
            }
        }
        else Debug.Log("GridUnitPrefab is Missing! name : " + goldPrefabName);

        GameObject steminaUnitPrefab = UIManager.instance.GetGridUnitPrefab(steminaPrefabName);
        if (steminaUnitPrefab != null)
        {
            var shop = TestLoadDatas.instance.ShopSteminaIndex;

            for (int i = 0; i < shop.Length; ++i)
            {
                var shopInfo = UIDataProcess.GetShopInfo(shop[i]);

                if (shopInfo == null)
                {
                    Debug.Log(steminaPrefabName + " " + i + " missing!");
                    continue;
                }

                GameObject gridUnit = GameObject.Instantiate(steminaUnitPrefab, GoldUnitSpace);
                gridUnit.name = steminaPrefabName + i;

                var controller = gridUnit.GetComponent<GridUnitController>();
                ShopSteminas.Add(controller);
                controller.Setup(shopInfo);
            }
        }
        else Debug.Log("GridUnitPrefab is Missing! name : " + steminaPrefabName);
    }

    public override void ClearGrid()
    {
        foreach (var unit in ShopGolds)
        {
            Destroy(unit.gameObject);
        }
        ShopGolds.Clear();

        foreach (var unit in ShopSteminas)
        {
            Destroy(unit.gameObject);
        }
        ShopSteminas.Clear();
    }
}
