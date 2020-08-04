using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICommons;
using BattleUnit;
public class CenteralUIController : MonoBehaviour
{
    #region classes
    [System.Serializable]
    public class HoldableCover
    {
        public BattleHoldableButton button;
        public Image image;
        public GameObject Cover;
        bool active;
        public bool Active { get => active; set { active = value; Cover.SetActive(!active); button.ClickActive = active; } }
    }

    [System.Serializable]
    public class ImageCover
    {
        public Image image;
        public GameObject Cover;
        bool active;
        public bool Active { get => active; set { active = value; Cover.SetActive(!active); } }

    }
    #endregion

    #region inspector
    public List<HoldableCover> SkillButtons;
    public List<HoldableCover> ItemButtons;
    public List<ImageCover> ManaBubbles;


    public GaugeBarController ManaGaugeBar;
    #endregion


    private void Start()
    {
        // 1. Skill Button Load ( Click, Hold, Holdout Action & Image)
        for(int i = 0; i< 3; ++i)
        {
            var SkillInven = UIDataProcess.GetSkillInventory();
            if (SkillInven.playerEquipSkills[i] != null)
            {
                var Skill = UIDataProcess.GetPlayerSkillInfo(SkillInven.playerEquipSkills[i].iIndex, i);
                string IconPath = UIDataProcess.PlayerSkillPath + Skill.StrSkillIcon.Replace("[SkillID]", (Skill.ISkillId - i).ToString());
                SkillButtons[i].image.sprite = UICommon.LoadSprite(IconPath);
                SkillButtons[i].Active = true;
                SkillButtons[i].button.HoldActive = true;
                SkillButtons[i].button.holdAction +=
                    () => {
                        BattleUIManager.instance.Popup("HoldPopup", Skill);
                    };
                SkillButtons[i].button.holdOutAction +=
                    () => {
                        BattleUIManager.instance.ClearAllPopup();
                    };

                /// 하나 선택
                if(Skill.TargetObj1 == TARGETOBJECT.SELECT_TARGETOBJ)
                {
                    if (Skill.Target1 == TARGET.FRIENDLY_TARGET)
                    {
                        SkillButtons[i].button.clickAction +=
                            () =>
                            {
                                if (PuzzleManager.instance.State == PuzzleManager.PUZZLE_STATE.MATCH)
                                {
                                    SetActive(false);
                                    BattleManager.instance.ManaBubble -= i + 1;
                                    BattleUIManager.instance.Popup("TargetUnitPopup", Skill);
                                }
                            };
                    }
                    else if (Skill.Target1 == TARGET.ENEMY_TARGET)
                    {
                        SkillButtons[i].button.clickAction +=
                            () =>
                            {
                                if (PuzzleManager.instance.State == PuzzleManager.PUZZLE_STATE.MATCH)
                                {
                                    SetActive(false);
                                    BattleManager.instance.ManaBubble -= i + 1;
                                    BattleUIManager.instance.Popup("TargetEnemyPopup", Skill);
                                }
                            };
                    }
                }
                else /// 선택이 아닌 스킬
                {
                    SkillButtons[i].button.clickAction +=
                        () =>
                        {
                            if (PuzzleManager.instance.State == PuzzleManager.PUZZLE_STATE.MATCH)
                            {
                                SetActive(false);
                                BattleManager.instance.ManaBubble -= i + 1;
                                if (Skill.ISkillEffectID1 == (int)EFFECT.LINKNUMBERINCREASE_EFFECT)
                                {

                                }
                                else
                                {
                                    StartCoroutine(CharacterManager.instance.FirePlayerEffect(Skill));
                                }
                            }
                        };
                }

                /// 활성화 조건 추가
                int index = i;
                BattleManager.instance.manaBubble.AddNoti(
                    (int val) =>
                    {
                        if(index < val) // 활성화
                        {
                            SkillButtons[index].Active = true;
                        }
                        else
                        {
                            SkillButtons[index].Active = false;
                        }
                    });
            }
            else
            {
                SkillButtons[i].image.enabled = false;
                SkillButtons[i].Active = false;
            }
        }

        // 2. Item Button Image Load ( Click, Hold, Holdout Action & Image)
        for(int i = 0; i< 3; ++i)
        {
            if (PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList[i] != null)
            {
                var Item = UIDataProcess.GetItemInfo(PlayerDataManager.PlayerData.PlayerItem.EquipmentItemList[i].iItemIndex);

                string IconPath = (Item != null) ? UIDataProcess.ConsumptionItemPath + Item.StrIcon.Replace("[ItemID]", Item.IItemId.ToString()) : "";
                ItemButtons[i].image.sprite = UICommon.LoadSprite(IconPath);
                ItemButtons[i].Active = true;
                ItemButtons[i].button.HoldActive = true;
                ItemButtons[i].button.holdAction +=
                    () =>
                    {
                        BattleUIManager.instance.Popup("HoldPopup", Item);
                    };
                ItemButtons[i].button.holdOutAction +=
                    () =>
                    {
                        BattleUIManager.instance.ClearAllPopup();
                    };

                /// 하나 선택
                if (Item.TargetSelect == TARGETOBJECT.SELECT_TARGETOBJ)
                {
                    if(Item.IEffectID == 21) // Puzzle 없애기
                    {
                        ItemButtons[i].button.clickAction +=
                            () =>
                            {
                                /// TODO : 퍼즐 선택 Popup창
                                if (PuzzleManager.instance.State == PuzzleManager.PUZZLE_STATE.MATCH)
                                {
                                    BattleUIManager.instance.Popup("TargetPuzzlePopup", Item);
                                }
                            };
                    }
                    else if (Item.Target == TARGET.FRIENDLY_TARGET)
                    {
                        ItemButtons[i].button.clickAction +=
                            () =>
                            {
                                if (PuzzleManager.instance.State == PuzzleManager.PUZZLE_STATE.MATCH)
                                {
                                    BattleUIManager.instance.Popup("TargetUnitPopup", Item);
                                }
                            };
                    }
                    else if (Item.Target == TARGET.ENEMY_TARGET)
                    {
                        ItemButtons[i].button.clickAction +=
                            () =>
                            {
                                if (PuzzleManager.instance.State == PuzzleManager.PUZZLE_STATE.MATCH)
                                {
                                    BattleUIManager.instance.Popup("TargetEnemyPopup", Item);
                                }
                            };
                    }
                }
                else /// 선택이 아닌 아이템들
                {
                    ItemButtons[i].button.clickAction +=
                        () =>
                        {
                            if (PuzzleManager.instance.State == PuzzleManager.PUZZLE_STATE.MATCH)
                            {
                                SetActive(false);
                                StartCoroutine(CharacterManager.instance.FirePlayerEffect(Item));
                            }
                        };
                }
            }
            else
            {
                ItemButtons[i].image.enabled = false;
                ItemButtons[i].Active = false;
            }
        }

        // 3. ManaMable Setting
        BattleManager.instance.manaBubble.AddNoti(
            (int val) => {
                for(int i = 0; i< 3; ++i)
                {
                    if( i < val)
                    {
                        /// TODO:
                        /// ManaBubbles[i] 활성화 이미지
                        /// 
                        ManaBubbles[i].Active = true;
                    }
                    else
                    {
                        /// TODO:
                        /// ManaBubbles[i] 비활성화 이미지
                        ///
                        ManaBubbles[i].Active = false;
                    }
                }
            });

        // 4. Engage GaugeBar with value
        ManaGaugeBar.Max = 100;
        BattleManager.instance.mana.AddNoti(
            (int val) => {
                if(val >= ManaGaugeBar.Max)
                {
                    if (BattleManager.instance.ManaBubble == BattleManager.instance.MANABUBBLE_MAX)
                    {
                        val = (int)ManaGaugeBar.Max;
                    }
                    else
                    {
                        BattleManager.instance.ManaBubble++;
                        BattleManager.instance.Mana = val - 100;
                    }
                }
                else
                {
                    ManaGaugeBar.Value = val;
                }
            });
    }


    public void SetActive(bool active)
    {
        for(int i = 0; i< SkillButtons.Count; ++i)
        {
            if(active)
            {
                if (i + 1 <= BattleManager.instance.ManaBubble)
                {
                    if(SkillButtons[i].image.enabled)
                        SkillButtons[i].Active = true;
                }
                else
                {
                    SkillButtons[i].Active = false;
                }
            }
            else
            {
                SkillButtons[i].Active = false;
            }
        }

        foreach (var item in ItemButtons)
        {
            if(item.image.enabled)
                item.Active = active;
        }
    }

}
