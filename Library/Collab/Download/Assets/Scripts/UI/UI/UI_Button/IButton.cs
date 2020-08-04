using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class IButton : UI_Element
{
    [System.Serializable]
    public class ButtonParameter
    {
        public ButtonParameter() { @object = null; @int = -1; }
        public GameObject @object = null;
        public int @int = -1;
    }


    public enum ActiveCondition
    {
        NONE,
        IS_IN_INVENTORY,
        IS_DATA_MATCH,
    }

    public enum INVEN_CONDITION_DETAIL
    {
        NONE,
        HAS_LEVELUP_DATAS,
    }

    public enum MATCH_CONDITION_DETAIL
    {

        NONE,
    }

    public ActiveCondition mActiveCondition;
    public List<ButtonParameter> mListConditionParameter;
    public bool isActive;

    public List<ButtonParameter> mListParameters;

    private void Awake()
    {
    }

    private void Start()
    {
        Util.BeginLog(gameObject);
        Util.Log(name + "Button SetUP()\n");
        ButtonSetup();
        Util.PopLog(gameObject);
    }

    public virtual void ButtonSetup()
    {
        isActive = true;
        Util.BeginLog();

        if(mActiveCondition != ActiveCondition.NONE)
        {
            Util.Log("Active Condition :" + mActiveCondition + "\n");

            Button btn = GetComponent<Button>();
            bool isMatch = false;

            // TODO : 비교
            switch (mActiveCondition)
            {
                case ActiveCondition.IS_IN_INVENTORY:
                    {
                        switch ((INVEN_CONDITION_DETAIL)mListConditionParameter[0].@int)
                        {
                            case INVEN_CONDITION_DETAIL.HAS_LEVELUP_DATAS:
                                {

                                }
                            break;
                            default: case INVEN_CONDITION_DETAIL.NONE: break;
                        }
                    }
                break;
                case ActiveCondition.IS_DATA_MATCH:
                    {
                        LoadElement leA = mListConditionParameter[0].@object.GetComponent<LoadElement>();
                        LoadElement leB = mListConditionParameter[1].@object.GetComponent<LoadElement>();
                        Util.Log("Compare A B :\n");
                        Util.Log("A\n");
                        Util.Log("A Detail : " + leA.mLoadDetail + "B Detail : " + leB.mLoadDetail + "\n");
                        Util.Log("A Element : " + leA.mLoadElement + "B Element : " + leB.mLoadElement + "\n");

                        switch (leA.mLoadDetail)
                        {
                            case UI_DATA.LOAD_DETAIL.UNIT:
                                {
                                    switch ((UI_DATA.LOAD_ELEMENT_UNIT)mListConditionParameter[2].@int)
                                    {
                                        case UI_DATA.LOAD_ELEMENT_UNIT.POSITION:
                                        {
                                                string typeB = TestLoadDatas.instance.getData(leB.mLoadMain, leB.mLoadIndexMode, leB.mLoadIndex, leB.mLoadDetail, (int)UI_DATA.LOAD_ELEMENT_UNIT.POSITION);
                                                int indexA = leA.mLoadIndex;
                                                string typeA = indexA < 2 ? UNITPOSITION.SUPPORTER_POSITION.ToString() : indexA < 4 ? UNITPOSITION.DEALER_POSITION.ToString() : UNITPOSITION.TANKER_POSITION.ToString();
                                                if(typeB == typeA)
                                                {
                                                    isMatch = true;
                                                }
                                        }
                                        break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                break;
                default:
                    break;
            }

            if(!isMatch)
            {
                isActive = false;
            }
        }

        Util.PopLog();
    }

    public abstract void ClickAction();

}
