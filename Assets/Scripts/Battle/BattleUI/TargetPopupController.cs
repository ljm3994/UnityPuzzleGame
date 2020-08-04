using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;
using BattleUnit;
using BattleCommon;

public class TargetPopupController : PopupController
{
    [System.Serializable]
    public class UnitButton
    {
        public GameObject panel;
        public Image iMain;
        public GaugeBarController gaugebar;
        public Button button;

        public void SetActive(bool b)
        {
            iMain.gameObject.SetActive(b);
            gaugebar.gameObject.SetActive(b);
            button.gameObject.SetActive(b);
        }
    }

    #region inspector
    public Image iMain;
    public Text tName;
    public Text tDesc;
    public List<UnitButton> unitUI;

    public Button backBtn;
    #endregion


    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(ItemInfo))
        {
            var inputData = t as ItemInfo;
            Setup(inputData.Target == TARGET.ENEMY_TARGET ? CHARACTER_CAMP.ENEMY : CHARACTER_CAMP.PLAYER, inputData);
        }
        else if(t.GetType() == typeof(PlayerSkillInfo))
        {
            var inputData = t as PlayerSkillInfo;
            Setup(inputData.Target1 == TARGET.ENEMY_TARGET ? CHARACTER_CAMP.ENEMY : CHARACTER_CAMP.PLAYER, inputData);
        }
    }

    void Setup(CHARACTER_CAMP camp, ItemInfo data)
    {

        iMain.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + data.StrIcon.Replace("[ItemID]", data.IItemId.ToString()));
        tName.text = data.StrItemName;
        tDesc.text = data.StrItemDesc;
        var targetIDs = CharacterManager.instance.GetIDs(camp);

        SetUnits(camp,targetIDs, data);
        SetBackBtn();
    }

    void Setup(CHARACTER_CAMP camp, PlayerSkillInfo data)
    {
        int skillLevel = (data.ISkillId % 100 - 1) % 3;
        iMain.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + data.StrSkillIcon.Replace("[SkillID]", (data.ISkillId- skillLevel).ToString()));
        tName.text = data.StrSkillName;
        tDesc.text = data.StrSkillDesc;
        var targetIDs = CharacterManager.instance.GetIDs(camp);

        SetUnits(camp,targetIDs, data);
        SetBackBtn();
    }

    void SetUnits(CHARACTER_CAMP camp, List<int> units, ItemInfo item)
    {
        for(int i = 0; i< units.Count; ++i)
        {
            if(units[i] == -1)
            {
                Destroy(unitUI[i].panel.gameObject);
                continue;
            }
            int index = i;
            if(camp == CHARACTER_CAMP.PLAYER)
            {
                var data =  GameDataBase.Instance.UnitTable[units[i]];
                unitUI[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + data.StrUnitImage.Replace("[CharacterID]", data.iID.ToString()));
                var status = CharacterManager.instance.GetUnit(i).mStatus;
                unitUI[i].gaugebar.Max = status.MAXHP;
                unitUI[i].gaugebar.Value = status.HP;
                unitUI[i].button.onClick.AddListener(
                    () =>
                    {
                        CharacterManager.instance.FirePlayerEffect(item, index);
                        BattleUIManager.instance.ClearAllPopup();
                    });
            }
            else
            {
                var data = GameDataBase.Instance.MonsterTable[units[i]];
                unitUI[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.EnemyImagePath + "Enemy" + data.StrMonsterImage.Replace("[MonsterID]", data.iID.ToString()));
                var status = CharacterManager.instance.GetUnit(i+5).mStatus;
                unitUI[i].gaugebar.Max = status.MAXHP;
                unitUI[i].gaugebar.Value = status.HP;
                unitUI[i].button.onClick.AddListener(
                     () =>
                     {
                         CharacterManager.instance.FirePlayerEffect(item, index + 5);
                         BattleUIManager.instance.ClearAllPopup();
                     });
            }

        }
    }

    void SetUnits(CHARACTER_CAMP camp, List<int> units,  PlayerSkillInfo skill)
    {
        for (int i = 0; i < units.Count; ++i)
        {
            if (units[i] == -1)
            {
                Destroy(unitUI[i].panel.gameObject);
                continue;
            }

            int index = i;
            if (camp == CHARACTER_CAMP.PLAYER)
            {
                var data = GameDataBase.Instance.UnitTable[units[i]];
                unitUI[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + data.StrUnitImage.Replace("[CharacterID]", data.iID.ToString()));
                var status = CharacterManager.instance.GetUnit(i).mStatus;
                unitUI[i].gaugebar.Max = status.MAXHP;
                unitUI[i].gaugebar.Value = status.HP;
                unitUI[i].button.onClick.AddListener(
                    () =>
                    {
                        CharacterManager.instance.FirePlayerEffect(skill, index);
                        BattleUIManager.instance.ClearAllPopup();
                    });
            }
            else
            {
                var data = GameDataBase.Instance.MonsterTable[units[i]];
                unitUI[i].iMain.sprite = UICommon.LoadSprite(UIDataProcess.EnemyImagePath + "Enemy" + data.StrMonsterImage.Replace("[MonsterID]", data.iID.ToString()));
                var status = CharacterManager.instance.GetUnit(i + 5).mStatus;
                unitUI[i].gaugebar.Max = status.MAXHP;
                unitUI[i].gaugebar.Value = status.HP;
                unitUI[i].button.onClick.AddListener(
                     () =>
                     {
                         CharacterManager.instance.FirePlayerEffect(skill, index+5);
                         BattleUIManager.instance.ClearAllPopup();
                     });
            }

        }
    }

    void SetBackBtn()
    {
        backBtn.onClick.AddListener(BattleUIManager.instance.ClearAllPopup);
        backBtn.onClick.AddListener(()=> { BattleUIManager.instance.CentralUI.SetActive(true); });
    }

    public override void Load()
    {
        
    }
}
