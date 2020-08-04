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

        public void Setup(int max)
        {
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
                        ToggleAction(toggles[index]);
                        if (checkAction != null)
                            checkAction.Invoke();
                    });
            }
        }

        public void ToggleAction(Toggle toggle)
        {
            if(toggle.isActive)
            {
                if (toggle.Check)
                {
                    toggle.Check = false;
                    checkQ.Remove(toggle.unitIndex);
                }
                else
                {
                    if (checkQ.Count < toggleMax)
                    {
                        toggle.Check = true;
                        checkQ.Add(toggle.unitIndex);
                    }
                    else
                    {
                        toggles[checkQ[0]].Check = false;
                        checkQ.RemoveAt(0);

                        toggle.Check = true;
                        checkQ.Add(toggle.unitIndex);
                    }
                }
            }
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

        public Image iSkill;
        public Image iPosition;
    }
    #endregion

    #region Functions

    public class UICommon
    {
        public static string PopupPath = "Prefabs/UI_Popup/";
        public static string GridUnitPath = "Prefabs/UI_GridUnit/";

        public static Sprite LoadSprite(string path)
        {
            return (Sprite)Resources.Load(path, typeof(Sprite));
        }


    }

    #endregion

}

