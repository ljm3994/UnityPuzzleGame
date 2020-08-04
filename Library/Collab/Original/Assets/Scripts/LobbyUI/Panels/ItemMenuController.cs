using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class ItemMenuController : PanelController
{

    #region inspector
    public TopGSBarController GSBar;

    public Button GoBackBtn;
    public Button SkillMenuBtn;
    public Button UnitMenuBtn;

    public Button ItemETCBtn;
    public Button ItemConsumableBtn;

    public Transform GridSpace;
    public List<GridUnitController> InvenUnits = new List<GridUnitController>();
    #endregion

    int tabType = 0;

    public override void Setup()
    {
        base.Setup();
        UnitMenuBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "UnitPanel"); });
        SkillMenuBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "CharacterPanel"); });
        GoBackBtn.onClick.AddListener(() => { UIManager.instance.SwapPanel(transform, "MainPanel"); });

        ItemETCBtn.onClick.AddListener(
            () => {
                if (this.tabType == 0)
                {
                    Load(1);
                }
            });
        ItemConsumableBtn.onClick.AddListener(
            ()=> {
                if (this.tabType == 1)
                {
                    Load(0);
                }
            });

    }
    void ClearGridSpace()
    {
        for(int i = GridSpace.childCount - 1; i >= 0; --i)
        {
            Destroy(GridSpace.GetChild(i).gameObject);
        }
    }

    public override void Load()
    {
        Load(tabType);
    }
    /// <summary>
    /// 로드
    /// </summary>
    /// <param name="type">type 0: 도구 1: ETC</param>
    public void Load(int type)
    {
        base.Load();
        GSBar.Load();

        tabType = type;
        string name = type == 0 ? "GridUnit_Item_Consume" : "GridUnit_Item_ETC";

        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab(name);
        if(gridUnitPrefab != null)
        {
            PlayerItemData inventory = (type == 0) ? UIDataProcess.GetConsumeItemInventory().ItemList : UIDataProcess.GetETCItemInventory().ItemList;
            
            for (int i = 0; i< inventory.Count; ++i)
            {
                var itemInfo = UIDataProcess.GetItemInfo(inventory[i].iItemIndex);

                if(itemInfo == null)
                {
                    Debug.Log(name + " " + i + " missing!");
                    continue;
                }

                GameObject gridUnit = GameObject.Instantiate(gridUnitPrefab, GridSpace);
                gridUnit.name = name + i;

                var controller = gridUnit.GetComponent<GridUnitController>();
                InvenUnits.Add(controller);
                controller.Setup(inventory[i]);
            }
        }
        else Debug.Log("GridUnitPrefab is Missing! name : " + name);
    }

    public override void ClearGrid()
    {
        foreach (var unit in InvenUnits)
        {
            Destroy(unit.gameObject);
        }
        InvenUnits.Clear();
    }
}
