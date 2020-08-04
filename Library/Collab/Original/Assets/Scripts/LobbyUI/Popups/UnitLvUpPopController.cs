using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;

public class UnitLvUpPopController : PopupController
{
    #region inspector

    public List<ItemCount> itemCounts;
    public List<Image> plusIcons;
    public Text tCost;

    public Button backGroundBtn;
    public Button lvUpBtn;
    public Button goBackBtn;
    
    #endregion


    public override void Setup<T>(T t)
    {
        /// TODO:
        /// 레벨업 팝업 구성
        /// 
        if (t.GetType() == typeof(PlayerUnit))
        {
            var inputData = t as PlayerUnit;
            var unitData = UIDataProcess.GetUnitInfo(inputData.iIndex);

            int level = inputData.iLevel;
            bool canLvUp = false;

            //3번쨰 재료 아이템 사용 레벨 21 = lv21->lv22
            if(level < unitData.IItemCondition3)
            {
                Destroy(itemCounts[2].iMain.gameObject);
                itemCounts.RemoveAt(2);
                Destroy(plusIcons[1].gameObject);
                plusIcons.RemoveAt(1);

                if(level < unitData.IItemCondition2)
                {
                    Destroy(itemCounts[1].iMain.gameObject);
                    itemCounts.RemoveAt(1);
                    Destroy(plusIcons[0].gameObject);
                    plusIcons.Clear();

                    if (level < unitData.IItemCondition1)
                    {
                        Destroy(itemCounts[0].iMain.gameObject);
                        itemCounts.Clear();
                    }
                    else
                    {
                        var itemID = unitData.IItemID1 + 1000;
                        var itemData = UIDataProcess.GetItemInfo(itemID);
                        itemCounts[0].iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + itemData.StrIcon.Replace("[ItemID]", itemID.ToString()));
                        itemCounts[0].tNum.text = (unitData.IItemNumber1 + unitData.IItemNumberIncrease1 * (level - 1)).ToString();

                        /// TODO : 
                        /// 인벤토리에서 해당 아이템이 존재하는지 확인할 수 있는 함수 구현
                        /// if 존재하면 canLvUp = true;
                        /// 
                        canLvUp = true;
                    }
                }
                else
                {
                    var itemID = unitData.IItemID2 + 1000;
                    var itemData = UIDataProcess.GetItemInfo(itemID);
                    itemCounts[1].iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + itemData.StrIcon.Replace("[ItemID]", itemID.ToString()));
                    itemCounts[1].tNum.text = (unitData.IItemNumber2 + unitData.IItemNumberIncrease2 * (level - unitData.IItemCondition2)).ToString();

                    /// TODO : 
                    /// 인벤토리에서 해당 아이템이 존재하는지 확인할 수 있는 함수 구현
                    canLvUp = true;
                }
            }
            else
            {
                var itemID = unitData.IItemID3 + 1000;
                var itemData = UIDataProcess.GetItemInfo(itemID);
                itemCounts[2].iMain.sprite = UICommon.LoadSprite(UIDataProcess.EtcItemPath + itemData.StrIcon.Replace("[ItemID]", itemID.ToString()));
                itemCounts[2].tNum.text = (unitData.IItemNumber3 + unitData.IItemNumberIncrease3 * (level - unitData.IItemCondition3)).ToString();

                /// TODO : 
                /// 인벤토리에서 해당 아이템이 존재하는지 확인할 수 있는 함수 구현
                canLvUp = true;
            }

            /// ???? 레벨업 시 필요한 재화 데이터
            /// tCost.text = ()

            backGroundBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });
            goBackBtn.onClick.AddListener(() => { UIManager.instance.CloseTopPopup(); });

            if(canLvUp)
            {
                lvUpBtn.onClick.AddListener(
                    () => {
                    /// TODO :
                    /// 유닛 레벨업 구현
                    /// + 이전 패널로 돌아가며 경험치바 애니메이션 실행
                    UIManager.instance.CloseTopPopup();
                    });
            }
            else
            {
                /// 버튼 비활성화 처리
            }

        }
    }
}
