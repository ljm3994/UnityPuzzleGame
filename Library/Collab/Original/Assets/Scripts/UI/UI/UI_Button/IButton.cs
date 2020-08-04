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


                        if (leA.mLoadDetail == leB.mLoadDetail && leA.mLoadElement == leB.mLoadElement)
                        {
                            // TODO: load된 데이터와 비교
                            isMatch = true;
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
