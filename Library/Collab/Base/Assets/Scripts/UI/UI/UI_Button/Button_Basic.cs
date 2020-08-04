using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Basic : IButton
{
    [System.Serializable]
    public enum BUTTON_ACTION_BASIC
    {
        SHOW_PANEL,
        OPEN_POPUP,
        GO_BACK,
        TOOGLE_CHECK_GROUP,
        APPLY,
        STAGE_IN,
        ETC,
        SHOW_NEWPOPUP,
        NONE
    }

    [System.Serializable]
    public enum BUTTON_APPLY_TYPE
    {
        NONE,
        EQUIP_INVEN_SWAP,
        RELEASE,
        PURCHASE,
        LVUP,
        EQUIP_SKILLINVEN_APPLY,
        EQUIP_SKILLINVEN_SWAP,
        EQUIP_UNITINVENT_APPLY,
    }

    [System.Serializable]
    public enum GO_BACK_TYPE
    {
        GO_BACK_LATEST,
        GO_BACK_PANEL,
        GO_BACK_MAIN,
        UNCHECK_TOGGLE,
        END,
        GO_BACK_CANCEL,
    }

    [System.Serializable]
    public enum BUTTON_ETC_TYPE
    {
        NUMUP,
        NUMDOWN,
    }

    [HideInInspector]
    public BUTTON_ACTION_BASIC mButtonActionType;

    public List<string> PopupButtonText;

    public override void ButtonSetup()
    {
        base.ButtonSetup();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ClickAction);
    }

    public override void ClickAction()
    {
        if (!isActive) return;
        switch ((BUTTON_ACTION_BASIC)mButtonActionType)
        {
            case BUTTON_ACTION_BASIC.SHOW_PANEL: SwapPanel((GameObject)mListParameters[0].@object); break;
            case BUTTON_ACTION_BASIC.OPEN_POPUP: OpenPop((GameObject)mListParameters[0].@object); break;
            case BUTTON_ACTION_BASIC.GO_BACK: ClosePop((GO_BACK_TYPE)mListParameters[0].@int); break;
            case BUTTON_ACTION_BASIC.STAGE_IN: EnterStage((GameObject)mListParameters[0].@object); break;
            case BUTTON_ACTION_BASIC.APPLY: Apply(); break;
            case BUTTON_ACTION_BASIC.ETC: ActionETC(); break;
            default: break;
        }
    }

    public void EnterStage(GameObject LoadStageInfo)
    {
        LoadElement stageElement = LoadStageInfo.GetComponent<LoadElement>();
        UI_ProgressBlock floorElement = mListParameters[1].@object.GetComponent<UI_ProgressBlock>();

        int stageNum = stageElement.mLoadIndex;
        int stageFloor = floorElement.CurNum;
        
        // 임시
        //GameManager.instance.SetBattleScene();
    }
    
    public void SwapPanel(GameObject target)
    {
        // In Screen
        // Panel A => Panel B
        UI_Panel panelA = mRoot.GetComponent<UI_Panel>();
        panelA.panelBack = null;
        UI_Panel panelB = target.GetComponent<UI_Panel>();
        
        Vector3 temp_pos;
        temp_pos = panelA.transform.localPosition;
        panelA.transform.localPosition = panelB.transform.localPosition;
        panelB.transform.localPosition = temp_pos;
        panelB.panelBack = panelA;

        UI_DATA.StartLoadHierarchy(panelB.transform);
    }

    public void OpenPop(GameObject targetPopupPrefab)
    {
        Util.BeginLog(gameObject);
        Util.Log("Popup() !\n");
        GameObject targetPopObj = (GameObject)GameObject.Instantiate(targetPopupPrefab, mRoot.transform);
        if(targetPopObj.name == "StageEnter_Popup(Clone)")
        {
            SteminaManager.Instance.UseStemina(5);
        }
        UI_DATA.SetupParent(UI_DATA.UI_PARENT.POPUP, targetPopObj);

        RectTransform calleeRT = targetPopObj.GetComponent<RectTransform>();
        UI_Popup callee = targetPopObj.GetComponent<UI_Popup>();
        Vector3 callerPosition = mRoot.transform.position;
        calleeRT.position = callerPosition;

        callee.mPopupFrom = this.mRoot;
        callee.mTypePopupFrom = mRootType;

        if (mListParameters[1].@int > 0)
        {
            Util.Log("CopyLoadData()!\n");
            callee.mReceivedData = mListParameters[2].@object.GetComponent<UI_Space>();
            UI_DATA.CopyLoadData(callee.mReceivedData.transform, callee.mRecieveTarget.transform);
        }


        UI_DATA.StartLoadHierarchy(callee.transform);

        Util.PopLog(gameObject);
    }

    public void ClosePop(GO_BACK_TYPE goBackType)
    {
        switch (mRootType)
        {
            case UI_DATA.UI_PARENT.PANEL:
                switch (goBackType)
                {
                    case GO_BACK_TYPE.GO_BACK_LATEST:
                    case GO_BACK_TYPE.GO_BACK_PANEL:
                        SwapPanel((mRoot.GetComponent<UI_Panel>()).panelBack.gameObject);
                        break;
                    case GO_BACK_TYPE.GO_BACK_MAIN:
                        SwapPanel(GameObject.Find("MainPanel"));
                        break;
                    case GO_BACK_TYPE.GO_BACK_CANCEL:
                        {
                            LoadElement element = mListParameters[0].@object.GetComponent<LoadElement>();
                            if (element != null)
                            {
                                switch (element.mLoadDetail)
                                {
                                    case UI_DATA.LOAD_DETAIL.USER_SKILL:
                                        PlayerDataManager.PlayerData.SkillInventory.SkillList[element.mLoadIndex].IEquipmentIndex = 0;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            SwapPanel((mRoot.GetComponent<UI_Panel>()).panelBack.gameObject);
                        }
                        break;
                    default:
                        break;
                }


                if (mRoot)
                {
                    if (mRoot.GetComponent<UI_Panel>().panelBack)
                        UI_DATA.StartLoadHierarchy(mRoot.GetComponent<UI_Panel>().panelBack.transform);
                }
                break;
            case UI_DATA.UI_PARENT.POPUP:
                switch (goBackType)
                {
                    case GO_BACK_TYPE.GO_BACK_LATEST:
                        GameObject.Destroy(mRoot);
                        break;
                    case GO_BACK_TYPE.GO_BACK_PANEL:
                        mRoot.GetComponent<UI_Popup>().CloseRootPopup();
                        break;
                    case GO_BACK_TYPE.GO_BACK_MAIN:
                        mRoot.GetComponent<UI_Popup>().CloseRootPopup();
                        SwapPanel(GameObject.Find("MainPanel"));
                        break;
                    case GO_BACK_TYPE.GO_BACK_CANCEL:
                        {
                            LoadElement element = mListParameters[0].@object.GetComponent<LoadElement>();
                            if (element != null)
                            {
                                switch (element.mLoadDetail)
                                {
                                    case UI_DATA.LOAD_DETAIL.USER_SKILL:
                                        int EqpIndex = 0;
                                        int i = 0;
                                        foreach (var item in PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills)
                                        {
                                            if (item != null)
                                            {
                                                if (item.iIndex == PlayerDataManager.PlayerData.SkillInventory.SkillList[element.mLoadIndex].iIndex)
                                                {
                                                    EqpIndex = i;
                                                }
                                            }
                                            i++;
                                        }
                                        PlayerDataManager.PlayerData.SkillInventory.SkillList[element.mLoadIndex].IEquipmentIndex = EqpIndex;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            mRoot.GetComponent<UI_Popup>().CloseRootPopup();
                        }
                        break;
                    default:
                        break;
                }

                if(mRoot)
                {
                    if(mRoot.GetComponent<UI_Popup>().mPopupFrom)
                        UI_DATA.StartLoadHierarchy(mRoot.GetComponent<UI_Popup>().mPopupFrom.transform);
                }
                break;

            default:
                break;
        }

        if(goBackType == GO_BACK_TYPE.UNCHECK_TOGGLE)
        {
            Toggle_Group tg = mListParameters[1].@object.GetComponent<Toggle_Group>();

            tg.OnTargetSourceBtnCheck(this);

            Destroy(gameObject);
        }
    }

    public void Apply()
    {
        bool bClose = true;
        switch ((BUTTON_APPLY_TYPE)mListParameters[0].@int)
        {
            case BUTTON_APPLY_TYPE.EQUIP_INVEN_SWAP:
                {
                    Util.BeginLog(gameObject);
                    Toggle_Group toggleGroup = mListParameters[1].@object.GetComponent<Toggle_Group>();
                    UI_Space space = mListParameters[1].@object.GetComponent<UI_Space>();
                   
                    if (toggleGroup)
                    {
                        List<UI_Space> selectedTarget1 = toggleGroup.GetChecked();
                        if (selectedTarget1 != null)
                        {
                            for (int i = 0; i < selectedTarget1.Count; ++i)
                            {
                                if (selectedTarget1[i])
                                {
                                    if (mListParameters[2].@object)
                                    {
                                        LoadElement element1 = mListParameters[2].@object.GetComponent<LoadElement>();
                                        if(element1)
                                        {
                                            
                                            int EquipIndex = selectedTarget1[i].loadElement.mLoadIndex;
                                            if(PlayerDataManager.PlayerData.UnitInventory.EquipmentUnit[EquipIndex] != null)
                                            {
                                                for(int idx = 0; idx < PlayerDataManager.PlayerData.UnitInventory.UintList.Count; idx++)
                                                {
                                                    if(PlayerDataManager.PlayerData.UnitInventory.UintList[idx].iIndex == PlayerDataManager.PlayerData.UnitInventory.EquipmentUnit[EquipIndex].iIndex)
                                                    {
                                                        PlayerDataManager.PlayerData.UnitInventory.UintList[idx].bEquipped = false;
                                                        break;
                                                    }
                                                }

                                            }
                                            PlayerDataManager.PlayerData.UnitInventory.UintList[element1.mLoadIndex].bEquipped = true;
                                            PlayerDataManager.PlayerData.UnitInventory.EquipmentUnit[EquipIndex] = PlayerDataManager.PlayerData.UnitInventory.UintList[element1.mLoadIndex];
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (space)
                    {
                        if (mListParameters[2].@object)
                        {
                            // TODO:
                            // SWAP Function
                            
                        }
                        else Util.Log("Equip : (" + mListParameters[1].@object.name + ")\n");
                        Util.Log_Info_Loadable(mListParameters[1].@object.GetComponent<LoadElement>());
                    }
                    Util.PopLog(gameObject);
                }
                break;
            case BUTTON_APPLY_TYPE.RELEASE:
                {
                    Util.BeginLog(gameObject);
                    UI_Space space = mListParameters[1].@object.GetComponent<UI_Space>();
                    if (space)
                    {
                        Util.Log("Release : (" + space.name + ")\n");
                        // TODO:
                        // RELEASE Func
                        Util.Log_Info_Loadable(space.GetComponent< LoadElement>());
                    }
                    Util.PopLog(gameObject);
                }
                break;
            case BUTTON_APPLY_TYPE.PURCHASE:
                {
                    UI_Space uI_Space = mListParameters[1].@object.GetComponent<UI_Space>();
                    int Max = 0;
                    switch (uI_Space.loadElement.mLoadDetail)
                    {
                        case UI_DATA.LOAD_DETAIL.GOLD:
                            Max = TestLoadDatas.instance.ShopGoldIndex.Length;
                            if (uI_Space.loadElement.mLoadIndex < Max && uI_Space.loadElement.mLoadIndex >= 0)
                            {
                                UIDataProcess.ShopGoldPurchase(TestLoadDatas.instance.ShopGoldIndex[uI_Space.loadElement.mLoadIndex]);
                            }
                            PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.USER_DATAFILE);
                            break;
                        case UI_DATA.LOAD_DETAIL.STEMINA:
                            Max = TestLoadDatas.instance.ShopSteminaIndex.Length;
                            if (uI_Space.loadElement.mLoadIndex < Max && uI_Space.loadElement.mLoadIndex >= 0)
                            {
                                UIDataProcess.ShopSteminaPurchase(TestLoadDatas.instance.ShopSteminaIndex[uI_Space.loadElement.mLoadIndex]);
                            }
                            PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.USER_DATAFILE);
                            break;
                        case UI_DATA.LOAD_DETAIL.UNIT:
                            Max = TestLoadDatas.instance.ShopCharterIndex.Length;
                            if (uI_Space.loadElement.mLoadIndex < Max && uI_Space.loadElement.mLoadIndex >= 0)
                            {
                                UIDataProcess.ShopUnitPurchase(TestLoadDatas.instance.ShopCharterIndex[uI_Space.loadElement.mLoadIndex]);
                            }
                            PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.UNIT_DATAFILE);
                            break;
                        case UI_DATA.LOAD_DETAIL.ITEM_CONSUMABLE:
                            Max = TestLoadDatas.instance.ShopItemIndex.Length;
                            if (uI_Space.loadElement.mLoadIndex < Max && uI_Space.loadElement.mLoadIndex >= 0)
                            {
                                int Count = 0;
                                if(mListParameters.Count > 1)
                                {
                                    Text Temp = mListParameters[2].@object.GetComponent<Text>();
                                    if (Temp != null)
                                    {
                                        Count = DataProcess.stringToint(Temp.text);
                                    }
                                }
                                UIDataProcess.ShopItemPurchase(TestLoadDatas.instance.ShopItemIndex[uI_Space.loadElement.mLoadIndex], Count);
                            }
                            PlayerDataManager.PlayerData.PlayerDataSave(PLAYERDATAFILE.ITEM_DATAFILE);
                            break;
                        default:
                            break;
                    }
                }
                break;
            case BUTTON_APPLY_TYPE.LVUP:
                {

                }
                break;
            case BUTTON_APPLY_TYPE.EQUIP_SKILLINVEN_APPLY:
                LoadElement element = mListParameters[2].@object.GetComponent<LoadElement>();
                if(element != null)
                {
                    bool bEquip = true;
                    if(element.mLoadDetail == UI_DATA.LOAD_DETAIL.USER_SKILL)
                    {
                        bEquip = PlayerDataManager.PlayerData.SkillInventory.SkillList[element.mLoadIndex].BEquipped;

                        if(!bEquip)
                        {
                            OpenPop(mListParameters[0].@object);
                            bClose = false;
                        }
                        else
                        {
                            PlayerDataManager.PlayerData.SkillInventory.SkillList[element.mLoadIndex].BEquipped = false;
                            int i = 0;
                            foreach (var item in PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills)
                            {
                                if (item != null)
                                {
                                    if (item.iIndex == PlayerDataManager.PlayerData.SkillInventory.SkillList[element.mLoadIndex].iIndex)
                                    {
                                        PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills[i] = null;
                                        break;
                                    }
                                }
                                i++;
                            }
                        }
                    }

                }
                break;
            case BUTTON_APPLY_TYPE.EQUIP_UNITINVENT_APPLY:
                {
                    LoadElement Unitelement = mListParameters[2].@object.GetComponent<LoadElement>();
                    if(Unitelement)
                    {
                        bool bEquip = true;
                        if (Unitelement.mLoadDetail == UI_DATA.LOAD_DETAIL.UNIT)
                        {
                            bEquip = PlayerDataManager.PlayerData.UnitInventory.UintList[Unitelement.mLoadIndex].bEquipped;
                            if (!bEquip)
                            {
                                OpenPop(mListParameters[0].@object);
                                bClose = false;
                            }
                            else
                            {
                                PlayerDataManager.PlayerData.UnitInventory.UintList[Unitelement.mLoadIndex].bEquipped = false;
                                // 장착 유닛 검사
                                int idx = 0;
                                foreach (var item in PlayerDataManager.PlayerData.UnitInventory.EquipmentUnit)
                                {
                                    if (item != null)
                                    {
                                        if (item.iIndex == PlayerDataManager.PlayerData.UnitInventory.UintList[Unitelement.mLoadIndex].iIndex)
                                        {
                                            PlayerDataManager.PlayerData.UnitInventory.EquipmentUnit[idx] = null;
                                            break;
                                        }
                                    }
                                    idx++;
                                }
                            }
                        }
                    }
                }
                break;
            case BUTTON_APPLY_TYPE.EQUIP_SKILLINVEN_SWAP:
                Toggle_Group group = mListParameters[0].@object.GetComponent<Toggle_Group>();
                LoadElement element2 = mListParameters[1].@object.GetComponent<LoadElement>();
                if (group != null)
                {
                    List<UI_Space> spaces = group.GetChecked();
                    if (spaces.Count > 0)
                    {
                        int EqIndex = PlayerDataManager.PlayerData.SkillInventory.SkillList[element2.mLoadIndex].IEquipmentIndex;
                        LoadElement element1 = spaces[0].loadElement;
                        foreach (var item in PlayerDataManager.PlayerData.SkillInventory.SkillList)
                        {
                            if (PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills[EqIndex] != null)
                            {
                                if (PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills[EqIndex].iIndex == item.Value.iIndex)
                                {
                                    item.Value.BEquipped = false;
                                    break;
                                }
                            }
                        }
                        PlayerDataManager.PlayerData.SkillInventory.SkillList[element2.mLoadIndex].BEquipped = true;
                        PlayerDataManager.PlayerData.SkillInventory.playerEquipSkills[EqIndex] = PlayerDataManager.PlayerData.SkillInventory.SkillList[element2.mLoadIndex];
                        
                    }
                }
                break;
            case BUTTON_APPLY_TYPE.NONE:
            default:
                break;
        }

        if (bClose)
        {
            ClosePop(GO_BACK_TYPE.GO_BACK_PANEL);
        }
    }

    public void ActionETC()
    {
        switch ((BUTTON_ETC_TYPE)mListParameters[0].@int)
        {
            case BUTTON_ETC_TYPE.NUMUP:
                {
                    UI_Text tNum = mListParameters[1].@object.GetComponent<UI_Text>();
                    tNum.AddNum(1);
                }
            break;
            case BUTTON_ETC_TYPE.NUMDOWN:
                {
                    UI_Text tNum = mListParameters[1].@object.GetComponent<UI_Text>();
                    tNum.AddNum(-1);
                }
                break;
            default:
                break;
        }
    }
}
