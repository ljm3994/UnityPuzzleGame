using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class StageEnterPopController : PopupController
{
    #region classes
    public delegate void ResetStageInfo(int num);

    [System.Serializable]
    public class SelectFloorUnit
    {
        public Transform blockSpace;
        public List<Image> iFloor;
        public Image iMark;
        public Text tMark;
        public int curFloor;
        public int limitNum;
        public Button upBtn;
        public Button downBtn;
        public Sprite BlockNotActive;
        public ResetStageInfo ResetStage;
        public Sprite BlockActive;
        public Sprite BlockCurrent;
        public Sprite BlockLimit;

        public void Setup(int limit)
        {
            
            limitNum = limit;
            curFloor = limitNum;
            upBtn.onClick.AddListener( () => { AddNum(1); });
            downBtn.onClick.AddListener(() => { AddNum(-1); });

            for (int i = 0; i < iFloor.Count; ++i)
            {
                Image blockImage = iFloor[i].GetComponent<Image>();
                if (i < limitNum)
                {
                    blockImage.sprite = BlockActive;
                }
                else if (i == limitNum)
                {
                    blockImage.sprite = BlockLimit;
                }
                else
                {
                    blockImage.sprite = BlockNotActive;
                }
            }

            RefreshMark();

        }

        public void AddNum(int val)
        {
            int newNum = Mathf.Clamp(curFloor + val, 0, limitNum);
            if(newNum != curFloor)
            {
                iFloor[curFloor].sprite = (curFloor == limitNum) ? BlockLimit : BlockActive;

                curFloor = newNum;
                RefreshMark();

                ResetStage(curFloor);
            }
        }

        public void RefreshMark()
        {
            iMark.transform.SetParent(iFloor[curFloor].rectTransform);
            float gridSizeX = blockSpace.GetComponent<GridLayoutGroup>().cellSize.x;
            iMark.rectTransform.anchoredPosition = new Vector2((iMark.rectTransform.sizeDelta.x + gridSizeX) /2f, 0);
            iFloor[curFloor].sprite = BlockCurrent;
            tMark.text = (curFloor + 1).ToString() + "F";
        }
    }

    #endregion

    #region inspector

    public Text tStage;

    public SelectFloorUnit floorUnit;
    [Space]
    public List<Image> Monsters;
    [Space]
    public List<Image> Reward_Items;
    [Space]
    public List<Image> Units;
    [Space]
    public List<Image> Skills;
    [Space]
    public List<Image> Items;
    [Space]
    public Button selectItemBtn;
    public Button goStageBtn;
    public Button goBackBtn;
    public Button FormationBtn;
    public Button SkillBtn;
    #endregion
    SelectStageInfo stageInfo;

    public List<StageInfo> StageInfos;
    public override void Setup<T>(T t)
    {
        /// TODO:
        /// 스테이지 정보 로드
        /// 
        if (t.GetType() == typeof(SelectStageInfo))
        {
            stageInfo = t as SelectStageInfo;

            StageInfos = stageInfo.StageInfos;
            tStage.text = stageInfo.StageNum.ToString();
            if (stageInfo.StageNum == PlayerDataManager.PlayerData.Pdata.ICurrentTopNum)
            {
                floorUnit.Setup(PlayerDataManager.PlayerData.Pdata.ICurrentTopFloor - 1);
            }
            else if(stageInfo.StageNum < PlayerDataManager.PlayerData.Pdata.ICurrentTopNum)
            {
                floorUnit.Setup(19);
            }
            else
            {
                floorUnit.Setup(0);
            }
            floorUnit.ResetStage = StageInfoReset;
            StageInfoReset(floorUnit.curFloor);

            var equipUnits = UIDataProcess.GetUnitInventory().EquipmentUnit;
            for (int i = 0; i < equipUnits.Length; ++i)
            {
                if (equipUnits[i] != null && equipUnits[i].iIndex != 0)
                {
                    var unitInfo = UIDataProcess.GetUnitInfo(equipUnits[i].iIndex);
                    Units[i].enabled = true;
                    Units[i].sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitInfo.StrUnitImage.Replace("[CharacterID]", unitInfo.iID.ToString()));
                }
                else
                {
                    Units[i].enabled = false;
                }
            }
            FormationBtn.onClick.AddListener(() => { UIManager.instance.Popup("Popup_SelectUnit", 0); });
            var equipSkills = UIDataProcess.GetSkillInventory().playerEquipSkills;
            for (int i = 0; i < equipSkills.Length; ++i)
            {
                if (equipSkills[i] != null && equipSkills[i].iIndex != 0)
                {
                    var skillInfo = UIDataProcess.GetPlayerSkillInfo(equipSkills[i].iIndex, i);
                    Skills[i].enabled = true;
                    Skills[i].sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillInfo.StrSkillIcon.Replace("[SkillID]", (skillInfo.ISkillId - i).ToString()));
                }
                else
                {
                    Skills[i].enabled = false;
                }
            }

            LoadEquipmentItem();
        }
        SkillBtn.onClick.AddListener(() => { UIManager.instance.Popup("Popup_SelectSkill", 1); });
        selectItemBtn.onClick.AddListener(() => { UIManager.instance.Popup("Popup_SelectItem", this); });
        goStageBtn.onClick.AddListener(() => {
            if (PlayerDataManager.PlayerData.UnitInventory.EquipmentUnitCount() > 0)
            {
                if (SteminaManager.Instance.UseStemina(StageInfos[floorUnit.curFloor].IStageNeedStamina))
                {
                    GameManager.instance.SetBattleScene(StageInfos[floorUnit.curFloor]);
                }
            }
        });
        goBackBtn.onClick.AddListener(() => { UIManager.instance.CloseAllPopup(); });
    }

    public void LoadEquipmentItem()
    {
        var Equipitem = UIDataProcess.GetConsumeItemInventory().EquipmentItemList;

        for (int i = 0; i < Equipitem.Length; i++)
        {
            if (Equipitem[i] != null && Equipitem[i].iItemIndex != 0)
            {
               var ItemInfo = UIDataProcess.GetItemInfo(Equipitem[i].iItemIndex);
               Items[i].enabled = true;
               Items[i].sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + ItemInfo.StrIcon.Replace("[ItemID]", ItemInfo.IItemId.ToString()));
            }
            else
            {
                Items[i].enabled = false;
            }
        }
    }
    public void StageInfoReset(int CurFloor)
    {
        var monsterTable = GameDataBase.Instance.MonsterTable;
        var ItemTable = GameDataBase.Instance.ItemTable;
        var SelectStageInfo = StageInfos[CurFloor];

        for (int i = 0; i < 6; i++)
        {
            string Key = string.Format("_iStageMonsterID{0}", i + 1);
            string ItemKey = string.Format("_iStageDropItemID{0}", i + 1);

            int Value = (int)UIDataProcess.GetStringTypeNameValue(SelectStageInfo, Key);
            int Value2 = (int)UIDataProcess.GetStringTypeNameValue(SelectStageInfo, ItemKey);

            if (monsterTable.ContainsKey(Value))
            {
                Monsters[i].enabled = true;
                Monsters[i].sprite = UICommon.LoadSprite(UIDataProcess.EnemyImagePath + "Enemy" + monsterTable[Value].StrMonsterImage.Replace("[MonsterID]", monsterTable[Value].iID.ToString()));
            }
            else
            {
                Monsters[i].enabled = false;
            }

            if (ItemTable.ContainsKey(Value2))
            {
                Reward_Items[i].enabled = true;
                Reward_Items[i].sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + ItemTable[Value2].StrIcon.Replace("[ItemID]", ItemTable[Value2].IItemId.ToString()));
            }
            else
            {
                Reward_Items[i].enabled = false;
            }
        }
    }

    public override void Load()
    {
        if (stageInfo != null)
        {
            StageInfos = stageInfo.StageInfos;
            StageInfoReset(floorUnit.curFloor);
            var equipUnits = UIDataProcess.GetUnitInventory().EquipmentUnit;
            for (int i = 0; i < equipUnits.Length; ++i)
            {
                if (equipUnits[i] != null && equipUnits[i].iIndex != 0)
                {
                    var unitInfo = UIDataProcess.GetUnitInfo(equipUnits[i].iIndex);
                    Units[i].enabled = true;
                    Units[i].sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + unitInfo.StrUnitImage.Replace("[CharacterID]", unitInfo.iID.ToString()));
                }
                else
                {
                    Units[i].enabled = false;
                }
            }
            var equipSkills = UIDataProcess.GetSkillInventory().playerEquipSkills;
            for (int i = 0; i < equipSkills.Length; ++i)
            {
                if (equipSkills[i] != null && equipSkills[i].iIndex != 0)
                {
                    var skillInfo = UIDataProcess.GetPlayerSkillInfo(equipSkills[i].iIndex, i);
                    Skills[i].enabled = true;
                    Skills[i].sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + skillInfo.StrSkillIcon.Replace("[SkillID]", (skillInfo.ISkillId - i).ToString()));
                }
                else
                {
                    Skills[i].enabled = false;
                }
            }

            LoadEquipmentItem();
        }
    }
}
