using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq.Expressions;
using System;

namespace UICommons
{
    #region classes

    [System.Serializable]
    public class GroupToggle
    {
        public List<Toggle> toggles;
        public int toggleMax = 1;

        public List<int> checkQ;
        public delegate void CheckAction();
        public CheckAction checkAction;

        public bool Switch;

        public GroupToggle()
        {
            toggles = new List<Toggle>();
        }

        public void Setup(int max)
        {
            Switch = true;
            checkQ = new List<int>();
            toggleMax = max;
            for (int i = 0; i < toggles.Count; i++)
            {
                toggles[i].isActive = true;
                toggles[i].unitIndex = i;
                toggles[i].Check = false;

                int index = i;
                toggles[i].button.onClick.AddListener(
                    () => {
                        if(ToggleAction(toggles[index]))
                        {
                            if (checkAction != null)
                                checkAction.Invoke();
                        }
                    });
            }
        }

        public bool ToggleAction(Toggle toggle)
        {
            if(toggle.isActive)
            {
                if (toggle.Check)
                {
                    toggle.Check = false;
                    checkQ.Remove(toggle.unitIndex);
                    return true;
                }
                else
                {
                    if (checkQ.Count < toggleMax)
                    {
                        toggle.Check = true;
                        checkQ.Add(toggle.unitIndex);
                        return true;
                    }
                    else
                    {
                        if (Switch)
                        {
                            toggles[checkQ[0]].Check = false;
                            checkQ.RemoveAt(0);

                            toggle.Check = true;
                            checkQ.Add(toggle.unitIndex);

                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        public void ReleaseToggle(int index)
        {
            for(int i = 0; i< checkQ.Count; ++i)
            {
                if(checkQ[i] == index)
                {
                    checkQ.RemoveAt(i);
                    break;
                }
            }

            toggles[index].Check = false;
        }
    }

    [System.Serializable]
    public class Toggle
    {
        public Button button;
        public Image image;
        public bool isActive;
        bool check;
        public bool Check {
            get => check;
            set {
                check = value;
                if(image != null)
                {
                    if (check) image.gameObject.SetActive(true);
                    else image.gameObject.SetActive(false);
                }
                
            }
        }
        public int unitIndex;

    }

    [System.Serializable]
    public class ItemCount
    {
        public Image iMain;
        public Text tNum;
    }

    [System.Serializable]
    public class EquipSkill
    {
        public Button button;
        public Image image;
    }

    [System.Serializable]
    public class EquipUnit
    {
        public Button button;
        public Image iMain;
        public Image iPosition;
        public Text tLv;
    }

    [System.Serializable]
    public class SwitchSkillUnit
    {
        public Image iMain;
        public Text tName;
        public Text tDesc;
    }

    [System.Serializable]
    public class SwitchUnitUnit
    {
        public Image iMain;
        public Text tName;
        public Text tLv;
        public Text tStatus;
        public Text SkillDesc;
        public Image iSkill;
        public Image iPosition;
    }
    #endregion

    #region Functions

    public class UICommon
    {
        public static string PopupPath = "Prefabs/UI_Popup/";
        public static string GridUnitPath = "Prefabs/UI_GridUnit/";
        public static string BattlePopupPath = "Prefabs/BattleUI/Popup/";

        public static Sprite LoadSprite(string path)
        {
            return (Sprite)Resources.Load(path, typeof(Sprite));
        }

        public static void FitGridSize(Transform gridSpace, int numUnit)
        {
            GridOption option = gridSpace.GetComponent<GridOption>();
            RectTransform window = option.scroll.GetComponent<RectTransform>();
            RectTransform gridRT = gridSpace.GetComponent<RectTransform>();
            if (option)
            {
                // horizontal 세로 고정 
                //1 3 5 7 9
                //2 4 6 8 ..
                if (option.scroll.horizontal && !option.scroll.vertical)
                {
                    float height = window.rect.height;
                    float width;
                    int numX = (numUnit) / option.numY + ((option.numY != 1) ? 1 : 0);
                    if (numX <= option.numX)
                    {
                        width = window.rect.width;
                    }
                    else width = option.grid.padding.left + option.grid.padding.right +
                            numX * option.grid.cellSize.x + (numX - 1) * option.grid.spacing.x;

                    gridRT.sizeDelta = new Vector2(width, height);
                }
                // vertical 가로 고정
                // 1 2 3 4
                // 5 6 7 ..
                else if (option.scroll.vertical && !option.scroll.horizontal)
                {
                    float width = window.rect.width;
                    float height;
                    int numY = (numUnit) / option.numX + ((option.numX != 1) ? 1 : 0);
                    if (numY <= option.numY)
                    {
                        height = window.rect.height;
                    }
                    else height = option.grid.padding.top + option.grid.padding.bottom +
                            numY * option.grid.cellSize.y + (numY - 1) * option.grid.spacing.y;

                    gridRT.sizeDelta = new Vector2(width, height);
                }
            }
        }
    }
    #endregion

}

