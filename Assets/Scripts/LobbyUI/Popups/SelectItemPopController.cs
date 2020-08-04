using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class SelectItemPopController : PopupController
{
    #region classes
    [System.Serializable]
    public class EquipItem
    {
        public Image iMain;
        public Button button;
    }
    #endregion

    #region inspector

    GroupToggle groupToggle;
    public List<EquipItem> equipItems;

    public Transform GridSpace;
    public Button goBackBtn;

    #endregion
    List<InvenItemController> InvenUnits = new List<InvenItemController>();


    public override void Setup<T>(T tData)
    {
        /// TODO:
        /// StageEnterence Popup 창에서 도구 클릭시 Item 선택할 수 있는 화면
        /// 
        groupToggle = new GroupToggle();

        string name = "GridUnit_Item_Consume";
        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab(name);
        if(gridUnitPrefab != null)
        {
            PlayerItemData inventory = UIDataProcess.GetConsumeItemInventory().ItemList;
            List<int> EquipmentToggleIndex = new List<int>(); 
            for (int i = 0; i < inventory.Count; ++i)
            {
                var itemInfo = UIDataProcess.GetItemInfo(inventory[i].iItemIndex);

                if (itemInfo == null)
                {
                    Debug.Log(name + " " + i + " missing!");
                    continue;
                }

                GameObject gridUnit = GameObject.Instantiate(gridUnitPrefab, GridSpace);
                gridUnit.name = name + i;

                var controller = gridUnit.GetComponent<InvenItemController>();
                InvenUnits.Add(controller);
                controller.Setup(inventory[i]);

                // Toggle Btn으로 등록
                {
                    controller.button.onClick.RemoveAllListeners();
                    controller.Cover.SetActive(true);

                    UICommons.Toggle toggle = new UICommons.Toggle();

                    toggle.button = controller.button;
                    toggle.image = controller.Cover.GetComponentInChildren<Image>();
                    toggle.image.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);

                    groupToggle.toggles.Add(toggle);

                    for (int j = 0; j < PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList.Length; j++)
                    {
                        if (PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList[j] != null)
                        {
                            if (PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList[j].iItemIndex == inventory[i].iItemIndex)
                            {
                                EquipmentToggleIndex.Add(i);
                                break;
                            }
                        }
                    }
                }
            }
            UICommon.FitGridSize(GridSpace, InvenUnits.Count);

            groupToggle.Setup(3);
            for(int k = 0; k < EquipmentToggleIndex.Count; k++)
            {
                groupToggle.toggles[k].Check = true;
                groupToggle.checkQ.Add(groupToggle.toggles[k].unitIndex);
            }
            groupToggle.Switch = false;
            groupToggle.checkAction += () => { ResetEquipItems(); };
            ResetEquipItems();
        }
        else Debug.Log("GridUnitPrefab is Missing! name : " + name);

        goBackBtn.onClick.AddListener(
            () => {
                /// TODO:
                /// 아이템 장비 적용
                ///  var Inventory = UIDataProcess.GetConsumeItemInventory(); 
                ///  CharterItem itemInfo = Inventory.ItemList[groupToggle.checkQ[0,1,2]]; 을 캐릭터 아이템에 등록
                ///  

                PlayerItemData inventory = UIDataProcess.GetConsumeItemInventory().ItemList;
                goBackBtn.enabled = false;
                for (int i = 0; i < 3; i++)
                {
                    PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList[i] = null;
                    if (groupToggle.checkQ.Count > i)
                    {
                        PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList[i] = inventory[groupToggle.checkQ[i]];
                    }
                }
                
                if(tData.GetType() == typeof(StageEnterPopController))
                {
                    var Data = tData as StageEnterPopController;
                    Data.LoadEquipmentItem();
                }
                PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.ITEM_DATAFILE, (Succed) =>
                {
                    if (Succed)
                    {
                        goBackBtn.enabled = true;
                        UIManager.instance.CloseTopPopup();
                    }
                });
            });
    }

    void ResetEquipItems()
    {
        var Inventory = UIDataProcess.GetConsumeItemInventory();
        for (int i = 0; i < 3; ++i)
        {
            if (i < groupToggle.checkQ.Count)
            {
                int index = i;
                var itemInfo = Inventory.ItemList[groupToggle.checkQ[i]];
                var itemData = UIDataProcess.GetItemInfo(itemInfo.iItemIndex);
                equipItems[i].iMain.sprite
                    = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + itemData.StrIcon.Replace("[ItemID]", itemInfo.iItemIndex.ToString()));

                equipItems[i].button.onClick.RemoveAllListeners();
                equipItems[i].button.onClick.AddListener(
                    () => {
                        groupToggle.ReleaseToggle(groupToggle.checkQ[index]);
                        equipItems[index].iMain.sprite = null;
                        equipItems[index].button.onClick.RemoveAllListeners();
                        ResetEquipItems();
                    });
            }
            else
            {
                equipItems[i].iMain.sprite = null;
                equipItems[i].button.onClick.RemoveAllListeners();
            }
        }
    }

    public override void Load()
    {
        groupToggle = new GroupToggle();

        string name = "GridUnit_Item_Consume";
        GameObject gridUnitPrefab = UIManager.instance.GetGridUnitPrefab(name);
        if (gridUnitPrefab != null)
        {
            PlayerItemData inventory = UIDataProcess.GetConsumeItemInventory().ItemList;
            List<int> EquipmentToggleIndex = new List<int>();
            for (int i = 0; i < inventory.Count; ++i)
            {
                var itemInfo = UIDataProcess.GetItemInfo(inventory[i].iItemIndex);

                if (itemInfo == null)
                {
                    Debug.Log(name + " " + i + " missing!");
                    continue;
                }

                GameObject gridUnit = GameObject.Instantiate(gridUnitPrefab, GridSpace);
                gridUnit.name = name + i;

                var controller = gridUnit.GetComponent<InvenItemController>();
                InvenUnits.Add(controller);
                controller.Setup(inventory[i]);

                // Toggle Btn으로 등록
                {
                    controller.button.onClick.RemoveAllListeners();
                    controller.Cover.SetActive(true);

                    UICommons.Toggle toggle = new UICommons.Toggle();

                    toggle.button = controller.button;
                    toggle.image = controller.Cover.GetComponentInChildren<Image>();
                    toggle.image.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);

                    groupToggle.toggles.Add(toggle);

                    for (int j = 0; j < PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList.Length; j++)
                    {
                        if (PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList[j] != null)
                        {
                            if (PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList[j].iItemIndex == inventory[i].iItemIndex)
                            {
                                EquipmentToggleIndex.Add(i);
                                break;
                            }
                        }
                    }
                }
            }
            UICommon.FitGridSize(GridSpace, InvenUnits.Count);

            groupToggle.Setup(3);
            for (int k = 0; k < EquipmentToggleIndex.Count; k++)
            {
                groupToggle.toggles[k].Check = true;
                groupToggle.checkQ.Add(groupToggle.toggles[k].unitIndex);
            }
            groupToggle.Switch = false;
            groupToggle.checkAction += () => { ResetEquipItems(); };
            ResetEquipItems();
        }
        else Debug.Log("GridUnitPrefab is Missing! name : " + name);
    }
}
