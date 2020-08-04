using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class UnitButtonPopupController : PopupController
{
    #region inspector
    public Button BackGroundBtn;
    [Space]
    public Image UnitImage;
    public Text UnitLvName;
    public GaugeBarController hpGagueBar;
    public Text UnitStatus;
    public Image UnitSkillImg;
    public Text tSkillStatus;
    [Space]
    public List<Image> bufImg;
    public List<Text> bufText;
    public List<Image> debufImg;
    public List<Text> debugText;
    [Space]
    public RectTransform buffSpace;

    #endregion

    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(int))
        {
            var InputData = (int)(object)t;
            if (InputData >= 5)
            {
                SetupMonster(InputData);
            }
            else
            {
                SetupUnit(InputData);
            }
        }

        BackGroundBtn.onClick.AddListener(
            () => {
                BattleUIManager.instance.ClearAllPopup();
            }
            );
    }


    public void SetupUnit(int unitIndex)
    {
        var status = CharacterManager.instance.GetUnit(unitIndex).mStatus;

        var data = GameDataBase.Instance.UnitTable[status.unitID];

        UnitImage.sprite = UICommon.LoadSprite(UIDataProcess.UnitPath + "UnitInven_" + data.StrUnitImage.Replace("[CharacterID]", data.iID.ToString()));

        UnitLvName.text = "Lv." + status.level + " " + data.strName;

        hpGagueBar.Max = status.MAXHP;
        hpGagueBar.Value = status.HP;

        UnitStatus.text = "공격력 : " + status.ATK + " 방어력 : " + status.DEF;

        if (status.SkillID != -1)
        {
            var skillData = UIDataProcess.GetUnitSkillInfo(status.SkillID);
            UnitSkillImg.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", skillData.ISkillId.ToString()));
            tSkillStatus.text = UIDataProcess.GetUnitSkiilDesc(skillData.ISkillId);
        }
        else
        {
            Destroy(tSkillStatus.gameObject);
            Destroy(UnitSkillImg.gameObject);
        }

        {
            List<EFFECT> buffs = status.buffs.Effects;
            buffs.AddRange(status.debuffs.Effects);

            var effectTable = GameDataBase.Instance.EffectTable;

            foreach (var item in buffs)
            {
                var obj = new GameObject("buffImg", typeof(Image));
                obj.transform.SetParent(buffSpace);
                obj.transform.localScale = new Vector3(1, 1, 1);
                var effectData = effectTable[(int)item];
                obj.GetComponent<Image>().sprite = UICommon.LoadSprite(UIDataProcess.EffectImagePath + effectData.strEffectIcon);

            }
        }

    }
    public void SetupMonster(int unitIndex)
    {
        var status = CharacterManager.instance.GetUnit(unitIndex).mStatus;

        var data = GameDataBase.Instance.MonsterTable[status.unitID];

        UnitImage.sprite = UICommon.LoadSprite(UIDataProcess.EnemyImagePath + "Enemy" + data.StrMonsterImage.Replace("[MonsterID]", data.iID.ToString()));
        UnitLvName.text = "Lv." + GameManager.instance.CurentStage.IStageLevel + " " + data.strName;

        hpGagueBar.Max = status.MAXHP;
        hpGagueBar.Value = status.HP;

        UnitStatus.text = "공격력 : " + status.ATK + " 방어력 : " + status.DEF;

        if (status.SkillID != -1)
        {
            var skillData = UIDataProcess.GetUnitSkillInfo(status.SkillID);
            UnitSkillImg.sprite = UICommon.LoadSprite(UIDataProcess.UnitSkillPath + skillData.StrSkillIcon.Replace("[SkillID]", skillData.ISkillId.ToString()));
            tSkillStatus.text = UIDataProcess.GetUnitSkiilDesc(skillData.ISkillId);
        }
        else
        {
            Destroy(tSkillStatus.gameObject);
            Destroy(UnitSkillImg.gameObject);
        }
    }

    public override void Load()
    {
        
    }
}
