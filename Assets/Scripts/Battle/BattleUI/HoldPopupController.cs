using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class HoldPopupController : PopupController
{

    #region inspector
    public Image image;
    public Text Name;
    public Text Desc;
    #endregion

    void Setup(ItemInfo data)
    {
        image.sprite = UICommon.LoadSprite(UIDataProcess.ConsumptionItemPath + data.StrIcon.Replace("[ItemID]", data.IItemId.ToString()));
        Name.text = data.StrItemName;
        Desc.text = UIDataProcess.GetConsumptionItemDesc(data.IItemId);
    }

    void Setup(PlayerSkillInfo data)
    {
        int skillLevel = (data.ISkillId % 100 - 1)%3;
        image.sprite = UICommon.LoadSprite(UIDataProcess.PlayerSkillPath + data.StrSkillIcon.Replace("[SkillID]", (data.ISkillId - skillLevel).ToString()));
        Name.text = data.StrSkillName;
        Desc.text = UIDataProcess.GetPlayerSkillDesc(data.ISkillId);
    }

    public override void Setup<T>(T t)
    {
        if(t.GetType() == typeof(ItemInfo))
        {
            var InputData = t as ItemInfo;
            Setup(InputData);
        }
        else if(t.GetType() == typeof(PlayerSkillInfo))
        {
            var InputData = t as PlayerSkillInfo;
            Setup(InputData);
        }
    }

    public override void Load()
    {
        throw new System.NotImplementedException();
    }
}
