#define USE_MAP
#define NEW_VERSION

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(UI_Button))]
public class ButtonInspectorEditor : Editor
{
    UI_Button button;
    public int NumParam1;
    public int NumParam2;
    string name;
    public void Awake()
    {
       


    }

    public void OnEnable()
    {
        button = (UI_Button)target;
        if (button.mListParameters == null) button.mListParameters = new List<UI_Button.ButtonParameter>();
        if (button.mListHoldActionParameters == null) button.mListHoldActionParameters = new List<UI_Button.ButtonParameter>();
        if (button.mListConditionParameter == null) button.mListConditionParameter = new List<UI_Button.ButtonParameter>();

        if (button.mListParameters.Count == 0)
        {
            button.mListParameters.Add(new UI_Button.ButtonParameter());
            button.mListParameters.Add(new UI_Button.ButtonParameter());
            button.mListParameters.Add(new UI_Button.ButtonParameter());
            button.mListParameters.Add(new UI_Button.ButtonParameter());
            button.mListParameters.Add(new UI_Button.ButtonParameter());
            button.mListParameters.Add(new UI_Button.ButtonParameter());
        }

        if (button.mListHoldActionParameters.Count == 0)
        {
            button.mListHoldActionParameters.Add(new UI_Button.ButtonParameter());
            button.mListHoldActionParameters.Add(new UI_Button.ButtonParameter());
            button.mListHoldActionParameters.Add(new UI_Button.ButtonParameter());
            button.mListHoldActionParameters.Add(new UI_Button.ButtonParameter());
            button.mListHoldActionParameters.Add(new UI_Button.ButtonParameter());
            button.mListHoldActionParameters.Add(new UI_Button.ButtonParameter());
            button.mListHoldActionParameters.Add(new UI_Button.ButtonParameter());
        }

        if(button.mListConditionParameter.Count == 0)
        {
            button.mListConditionParameter.Add(new UI_Button.ButtonParameter());
            button.mListConditionParameter.Add(new UI_Button.ButtonParameter());
            button.mListConditionParameter.Add(new UI_Button.ButtonParameter());
            button.mListConditionParameter.Add(new UI_Button.ButtonParameter());
            button.mListConditionParameter.Add(new UI_Button.ButtonParameter());
            button.mListConditionParameter.Add(new UI_Button.ButtonParameter());
        }
    }

    public static bool bButtonType = false;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Label("버튼 활성화 조건 설정----------------------------------------------");
        ShowActiveCondition();

        GUILayout.Label("버튼 유형 설정----------------------------------------------");
        bButtonType = EditorGUILayout.Foldout(bButtonType,"버튼 유형 설정");
        if(bButtonType)
        {
            button.mButtonType = (UI_Button.BUTTON_TYPE)EditorGUILayout.EnumPopup("버튼 유형", (UI_Button.BUTTON_TYPE)button.mButtonType);

            switch (button.mButtonType)
            {
                case UI_Button.BUTTON_TYPE.BASIC: ShowBasicButtonSetting(); break;
                case UI_Button.BUTTON_TYPE.TOGGLE: ShowToggleButtonSetting(); break;
                case UI_Button.BUTTON_TYPE.HOLDABLE: ShowHoldableButtonSetting(); break;
                default: break;
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(button);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
        if (GUILayout.Button("설정된 정보로 버튼 적용"))
        {
            switch (button.mButtonType)
            {
                case UI_Button.BUTTON_TYPE.BASIC: SetBasicButton(); break;
                case UI_Button.BUTTON_TYPE.TOGGLE: SetToggleButton(); break;
                case UI_Button.BUTTON_TYPE.HOLDABLE: SetHoldableButton(); break;
                default: break;
            }
        }
    }


    void ShowBasicButtonSetting()
    {
        int pBtnAction = button.mButtonActionType;
        button.mButtonActionType = (int)(Button_Basic.BUTTON_ACTION_BASIC)EditorGUILayout.EnumPopup("버튼 기능 선택", (Button_Basic.BUTTON_ACTION_BASIC)button.mButtonActionType);
        if (pBtnAction != button.mButtonActionType)
        {
            foreach (var parameter in button.mListParameters)
            {
                parameter.@int = -1;
                parameter.@object = null;
            }
        }

        switch ((Button_Basic.BUTTON_ACTION_BASIC)button.mButtonActionType)
        {
            case Button_Basic.BUTTON_ACTION_BASIC.SHOW_PANEL: //패널
                {                                           // 매개변수 1개
                    button.mListParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("이동할 패널", button.mListParameters[0].@object, typeof(GameObject), true);
                    NumParam1 = 1;
                }
                break;

            case Button_Basic.BUTTON_ACTION_BASIC.OPEN_POPUP: //팝업 생성
                {                                           // 매개변수 1개
                    button.mListParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("띄울 팝업(프리팹)", button.mListParameters[0].@object, typeof(GameObject), false);

                    button.mListParameters[1].@int = EditorGUILayout.Toggle("데이터 전달 여부", (button.mListParameters[1].@int > 0 ? true : false)) ? 1 : -1;

                    if (button.mListParameters[1].@int > 0)
                    {
                        button.mListParameters[2].@object = (GameObject)EditorGUILayout.ObjectField("전달할 LoadData(UI_Space)", button.mListParameters[2].@object, typeof(GameObject), true);
                        NumParam1 = 3;
                    }
                }
                break;

            case Button_Basic.BUTTON_ACTION_BASIC.GO_BACK:
                {
                    button.mListParameters[0].@int = (int)((Button_Basic.GO_BACK_TYPE)EditorGUILayout.EnumPopup("돌아가기 유형", (Button_Basic.GO_BACK_TYPE)button.mListParameters[0].@int));
                    Mathf.Clamp(button.mListParameters[0].@int, (int)Button_Basic.GO_BACK_TYPE.GO_BACK_LATEST, (int)Button_Basic.GO_BACK_TYPE.END - 1);
                    NumParam1 = 1;
                }
               break;
            case Button_Basic.BUTTON_ACTION_BASIC.APPLY:
                {
                    button.mListParameters[0].@int = (int)((Button_Basic.BUTTON_APPLY_TYPE)EditorGUILayout.EnumPopup("적용 유형", (Button_Basic.BUTTON_APPLY_TYPE)button.mListParameters[0].@int));
                    switch ((Button_Basic.BUTTON_APPLY_TYPE)button.mListParameters[0].@int)
                    {
                        case Button_Basic.BUTTON_APPLY_TYPE.EQUIP_INVEN_SWAP:
                            {
                                button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("선택된 대상(Space or ToggleGroup)", button.mListParameters[1].@object, typeof(GameObject), true);
                                if (button.mListParameters[1].@object)
                                {
                                    Toggle_Group tg = button.mListParameters[1].@object.GetComponent<Toggle_Group>();
                                    UI_Space space = button.mListParameters[1].@object.GetComponent<UI_Space>();
                                    UI_Popup received = button.mListParameters[1].@object.GetComponent<UI_Popup>();
                                    if (tg)
                                    {
                                        for (int i = 0; i < tg.mNumMaxSelect; ++i)
                                        {
                                            button.mListParameters[2 + i].@object = (GameObject)EditorGUILayout.ObjectField("교체할 대상" + i + "(Space or ToggleGroup)", button.mListParameters[2 + i].@object, typeof(GameObject), true);
                                        }
                                        NumParam1 = 1 + tg.mNumMaxSelect;
                                    }
                                    else if (space)
                                    {
                                        button.mListParameters[2].@object = (GameObject)EditorGUILayout.ObjectField("교체할 대상(Space)", button.mListParameters[2].@object, typeof(GameObject), true);
                                        NumParam1 = 3;
                                    }
                                    else if (received)
                                    {
                                        button.mListParameters[2].@object = (GameObject)EditorGUILayout.ObjectField("교체할 대상(Popup Received Data)", button.mListParameters[2].@object, typeof(GameObject), true);
                                        NumParam1 = 3;
                                    }
                                }
                            }
                            break;
                        case Button_Basic.BUTTON_APPLY_TYPE.RELEASE:
                            {
                                button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("해제할 대상(Popup Received Data)", button.mListParameters[1].@object, typeof(GameObject), true);
                                NumParam1 = 2;
                            }
                            break;
                        case Button_Basic.BUTTON_APPLY_TYPE.PURCHASE:
                            {
                                button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("구매할 대상(Space or ToggleGroup)", button.mListParameters[1].@object, typeof(GameObject), true);
                                button.mListParameters[2].@int = EditorGUILayout.Toggle("개수 선택 여부", button.mListParameters[2].@int > 0 ? true : false) ? 1 : 0;
                                NumParam1 = 4;
                                if(button.mListParameters[2].@int == 1)
                                {
                                    button.mListParameters[3].@object = (GameObject)EditorGUILayout.ObjectField("개수가 저장된 대상(UI_TEXT)", button.mListParameters[3].@object, typeof(GameObject), true);
                                    NumParam1 = 4;
                                }
                            }
                            break;
                        case Button_Basic.BUTTON_APPLY_TYPE.LVUP:
                            {
                                button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("레벨업할 대상(Popup(ReceivedData))", button.mListParameters[1].@object, typeof(GameObject), true);
                                NumParam1 = 2;
                            }
                            break;
                        case Button_Basic.BUTTON_APPLY_TYPE.NONE:
                        default:
                            break;
                    }
                }
                break;
            case Button_Basic.BUTTON_ACTION_BASIC.STAGE_IN:
                {
                    button.mListParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("스테이지 로드 정보(UI_Space)", button.mListParameters[0].@object, typeof(GameObject), true);
                    button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("스테이지 층", button.mListParameters[1].@object, typeof(GameObject), true);
                    NumParam1 = 2;
                }
                break;
            case Button_Basic.BUTTON_ACTION_BASIC.ETC:
                {
                    button.mListParameters[0].@int = (int)((Button_Basic.BUTTON_ETC_TYPE)EditorGUILayout.EnumPopup("동작 TYPE", (Button_Basic.BUTTON_ETC_TYPE)button.mListParameters[0].@int));
                    switch ((Button_Basic.BUTTON_ETC_TYPE)button.mListParameters[0].@int)
                    {
                        case Button_Basic.BUTTON_ETC_TYPE.NUMUP:
                            button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("연결된 text(숫자)", button.mListParameters[1].@object, typeof(GameObject), true);
                            button.mListParameters[2].@int = (int)EditorGUILayout.IntField("최대값", button.mListParameters[2].@int);
                            NumParam1 = 3;
                            break;
                        case Button_Basic.BUTTON_ETC_TYPE.NUMDOWN:
                            button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("연결된 text(숫자)", button.mListParameters[1].@object, typeof(GameObject), true);
                            button.mListParameters[2].@int = (int)EditorGUILayout.IntField("최소값", button.mListParameters[2].@int);
                            NumParam1 = 3;
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

    void ShowToggleButtonSetting()
    {
        // In GameScene
        if (button.mRoot)
        {
            UI_BackGround ui_background = button.mRoot.GetComponent<UI_BackGround>();

            List<string> groupKeys = new List<string>();
            groupKeys.Add("NONE");
            for (int i = 0, count = 0; i < ui_background.transform.childCount; ++i)
            {
                Toggle_Group tg = ui_background.transform.GetChild(i).GetComponent<Toggle_Group>();
                if (tg)
                {
                    string newKey = new string(tg.name.ToCharArray());
                    newKey = newKey.Substring(3);
                    groupKeys.Add(newKey);
                    count++;
                }
            }

            int pGroupKey = groupKeys.IndexOf(button.mToggleGroupKey);
            if (pGroupKey == -1)
            {
                button.mToggleGroupKey = "NONE";
            }
            button.mToggleGroupKey = groupKeys[EditorGUILayout.Popup("GroupKey", groupKeys.IndexOf(button.mToggleGroupKey), groupKeys.ToArray())];

        }
        else // In Prefab Mode
        {
            GUILayout.Label("설정된 key는 플레이 시 생성 됩니다.");
            button.mToggleGroupKey = EditorGUILayout.TextField("그룹 key", button.mToggleGroupKey);
        }

        button.mToggleBackGroundImg = (GameObject)EditorGUILayout.ObjectField("토글 이미지(체크시 생성)", button.mToggleBackGroundImg, typeof(GameObject), true);






        GUILayout.Space(10);
        GUILayout.Label("-----------------------------------------------------------");
        int pBtnAction = button.mButtonActionType;
        button.mButtonActionType = (int)(Button_Toggle.BUTTON_ACTION_TOGGLE)EditorGUILayout.EnumPopup("버튼 기능 선택", (Button_Toggle.BUTTON_ACTION_TOGGLE)button.mButtonActionType);
        if (pBtnAction != button.mButtonActionType)
        {
            foreach (var parameter in button.mListParameters)
            {
                parameter.@int = -1;
                parameter.@object = null;
            }
        }

        switch ((Button_Toggle.BUTTON_ACTION_TOGGLE)button.mButtonActionType)
        {
            case Button_Toggle.BUTTON_ACTION_TOGGLE.COPY_LOADDATA: // 매개변수 source_space, target_space
                {
                    GUILayout.Space(10);
                    GUILayout.Label("토글 시 Source(Space)의 로드 정보를 ToggleGroup에서 설정된 Target(Space)에 복사합니다. ");
                    GUILayout.Space(10);
                    GUILayout.Label("1.Source Space : 토글 시 불러올 Load Data가 있는 Space");
                    button.mListParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("1.Source Space", button.mListParameters[0].@object, typeof(GameObject), true);
                    NumParam1 = 1;
                }
                break;
            default:
                break;
        }

        GUILayout.Label("-----------------------------------------------------------");

    }

    public static bool bFoldMainAction = false;
    public static bool bFoldHoldAction = false;
    void ShowHoldableButtonSetting()
    {
        bFoldMainAction = EditorGUILayout.Foldout(bFoldMainAction, "클릭 액션 설정");

        if (bFoldMainAction)
        {
            int pBtnAction = button.mButtonActionType;
            button.mButtonActionType = (int)(Button_Basic.BUTTON_ACTION_BASIC)EditorGUILayout.EnumPopup("버튼 기능 선택", (Button_Basic.BUTTON_ACTION_BASIC)button.mButtonActionType);
            if (pBtnAction != button.mButtonActionType)
            {
                foreach (var parameter in button.mListParameters)
                {
                    parameter.@int = -1;
                    parameter.@object = null;
                }
            }

            switch ((Button_Basic.BUTTON_ACTION_BASIC)button.mButtonActionType)
            {
                case Button_Basic.BUTTON_ACTION_BASIC.SHOW_PANEL: //패널
                                                               // 매개변수 1개
                    button.mListParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("이동할 패널", button.mListParameters[0].@object, typeof(GameObject), true);
                    NumParam1 = 1;
                    break;

                case Button_Basic.BUTTON_ACTION_BASIC.OPEN_POPUP: //팝업 생성
                                                               // 매개변수 1개
                    button.mListParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("띄울 팝업(프리팹)", button.mListParameters[0].@object, typeof(GameObject), false);
                    
                    button.mListParameters[1].@int = EditorGUILayout.Toggle("데이터 전달 여부", (button.mListParameters[1].@int > 0 ? true : false)) ? 1 : -1;
                    NumParam1 = 2;
                    if (button.mListParameters[1].@int > 0)
                    {
                        button.mListParameters[2].@object = (GameObject)EditorGUILayout.ObjectField("전달할 LoadData(UI_Space)", button.mListParameters[2].@object, typeof(GameObject), true);
                        NumParam1 = 3;
                    }
                    break;

                case Button_Basic.BUTTON_ACTION_BASIC.GO_BACK:
                    button.mListParameters[0].@int = (int)((Button_Basic.GO_BACK_TYPE)EditorGUILayout.EnumPopup("돌아가기 유형", (Button_Basic.GO_BACK_TYPE)button.mListParameters[0].@int));
                    Mathf.Clamp(button.mListParameters[0].@int, (int)Button_Basic.GO_BACK_TYPE.GO_BACK_LATEST, (int)Button_Basic.GO_BACK_TYPE.END - 1);
                    NumParam1 = 1;
                    break;
                case Button_Basic.BUTTON_ACTION_BASIC.APPLY:
                    {
                        button.mListParameters[0].@int = (int)((Button_Basic.BUTTON_APPLY_TYPE)EditorGUILayout.EnumPopup("적용 유형", (Button_Basic.BUTTON_APPLY_TYPE)button.mListParameters[0].@int));
                        NumParam1 = 1;
                        switch ((Button_Basic.BUTTON_APPLY_TYPE)button.mListParameters[0].@int)
                        {
                            case Button_Basic.BUTTON_APPLY_TYPE.EQUIP_INVEN_SWAP:
                                {
                                    button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("선택된 대상(Space or ToggleGroup)", button.mListParameters[1].@object, typeof(GameObject), true);
                                    NumParam1 = 2;
                                    if (button.mListParameters[1].@object)
                                    {
                                        Toggle_Group tg = button.mListParameters[1].@object.GetComponent<Toggle_Group>();
                                        UI_Space space = button.mListParameters[1].@object.GetComponent<UI_Space>();
                                        UI_Popup received = button.mListParameters[1].@object.GetComponent<UI_Popup>();
                                        if (tg)
                                        {
                                            for (int i = 0; i < tg.mNumMaxSelect; ++i)
                                            {
                                                button.mListParameters[2 + i].@object = (GameObject)EditorGUILayout.ObjectField("교체할 대상" + i + "(Space or ToggleGroup)", button.mListParameters[2 + i].@object, typeof(GameObject), true);
                                            }
                                            NumParam1 = tg.mNumMaxSelect + 1;
                                        }
                                        else if (space)
                                        {
                                            button.mListParameters[2].@object = (GameObject)EditorGUILayout.ObjectField("교체할 대상(Space)", button.mListParameters[2].@object, typeof(GameObject), true);
                                            NumParam1 = 3;
                                        }
                                        else if (received)
                                        {
                                            button.mListParameters[2].@object = (GameObject)EditorGUILayout.ObjectField("교체할 대상(Popup Received Data)", button.mListParameters[2].@object, typeof(GameObject), true);
                                            NumParam1 = 3;
                                        }
                                    }
                                }
                                break;
                            case Button_Basic.BUTTON_APPLY_TYPE.RELEASE:
                                {
                                    button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("해제할 대상(Popup Received Data)", button.mListParameters[1].@object, typeof(GameObject), true);
                                    NumParam1 = 2;
                                }
                                break;
                            case Button_Basic.BUTTON_APPLY_TYPE.PURCHASE:
                                {
                                    button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("구매할 대상(Space or ToggleGroup)", button.mListParameters[1].@object, typeof(GameObject), true);
                                    NumParam1 = 2;
                                }
                                break;
                            case Button_Basic.BUTTON_APPLY_TYPE.LVUP:
                                {
                                    button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("레벨업할 대상(Popup(ReceivedData))", button.mListParameters[1].@object, typeof(GameObject), true);
                                    NumParam1 = 2;
                                }
                                break;
                            case Button_Basic.BUTTON_APPLY_TYPE.NONE:
                            default:
                                break;
                        }
                    }
                    break;
                case Button_Basic.BUTTON_ACTION_BASIC.ETC:
                    {
                        button.mListParameters[0].@int = (int)((Button_Basic.BUTTON_ETC_TYPE)EditorGUILayout.EnumPopup("동작 TYPE", (Button_Basic.BUTTON_ETC_TYPE)button.mListParameters[0].@int));
                        switch ((Button_Basic.BUTTON_ETC_TYPE)button.mListParameters[0].@int)
                        {
                            case Button_Basic.BUTTON_ETC_TYPE.NUMUP:
                                button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("연결된 text(숫자)", button.mListParameters[1].@object, typeof(GameObject), true);
                                button.mListParameters[2].@int = (int)EditorGUILayout.IntField("최대값", button.mListParameters[2].@int);
                                NumParam1 = 3;
                                break;
                            case Button_Basic.BUTTON_ETC_TYPE.NUMDOWN:
                                button.mListParameters[1].@object = (GameObject)EditorGUILayout.ObjectField("연결된 text(숫자)", button.mListParameters[1].@object, typeof(GameObject), true);
                                button.mListParameters[2].@int = (int)EditorGUILayout.IntField("최소값", button.mListParameters[2].@int);
                                NumParam1 = 3;
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

        bFoldHoldAction = EditorGUILayout.Foldout(bFoldHoldAction, "홀드 액션 설정");

        if (bFoldHoldAction)
        {
            Button_Holdable.BUTTON_ACTION_HOLD pBtnAction = button.mHoldActionType;
            button.mHoldActionType = (Button_Holdable.BUTTON_ACTION_HOLD)EditorGUILayout.EnumPopup("홀드 기능 선택", (Button_Holdable.BUTTON_ACTION_HOLD)button.mHoldActionType);
            if (pBtnAction != button.mHoldActionType)
            {
                foreach (var parameter in button.mListHoldActionParameters)
                {
                    parameter.@int = -1;
                    parameter.@object = null;
                }
            }

            button.mMinHoldTime = EditorGUILayout.FloatField("최소 홀딩 시간(초)", button.mMinHoldTime);

            switch (button.mHoldActionType)
            {
                case Button_Holdable.BUTTON_ACTION_HOLD.SHOW_MINIPOPUP:
                    {
                        button.mListHoldActionParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("띄울 팝업(프리팹)", button.mListHoldActionParameters[0].@object, typeof(GameObject), false);

                        button.mListHoldActionParameters[1].@int = EditorGUILayout.Toggle("데이터 전달 여부", (button.mListHoldActionParameters[1].@int > 0 ? true : false)) ? 1 : -1;
                        NumParam2 = 2;
                        if (button.mListHoldActionParameters[1].@int > 0)
                        {
                            button.mListHoldActionParameters[2].@object = (GameObject)EditorGUILayout.ObjectField("전달할 LoadData(UI_Space)", button.mListHoldActionParameters[2].@object, typeof(GameObject), true);
                            NumParam2 = 3;
                        }
                    }
                    break;
                case Button_Holdable.BUTTON_ACTION_HOLD.NUMUP:
                    {
                        button.mListParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("연결된 text(숫자)", button.mListParameters[0].@object, typeof(GameObject), true);
                        button.mListParameters[1].@int = (int)EditorGUILayout.IntField("최대값", button.mListParameters[1].@int);
                        NumParam1 = 2;
                    }
                    break;
                case Button_Holdable.BUTTON_ACTION_HOLD.NUMDOWN:
                    {
                        button.mListParameters[0].@object = (GameObject)EditorGUILayout.ObjectField("연결된 text(숫자)", button.mListParameters[0].@object, typeof(GameObject), true);
                        button.mListParameters[1].@int = (int)EditorGUILayout.IntField("최소값", button.mListParameters[1].@int);
                        NumParam1 = 2;
                    }
                    break;
                default:
                    break;
            }


        }
    }

    // 버튼 활성화 조건 설정
    public static bool bSettingActive = false;
    int numConditionParameters = 0;
    void ShowActiveCondition()
    {
        bSettingActive = EditorGUILayout.Foldout(bSettingActive, "버튼 활성화 조건 설정");

        if(bSettingActive)
        {
            button.mActiveCondition = (IButton.ActiveCondition)EditorGUILayout.EnumPopup("버튼 활성화 유형", (IButton.ActiveCondition)button.mActiveCondition);

            switch (button.mActiveCondition)
            {
                case IButton.ActiveCondition.IS_IN_INVENTORY:
                    {
                        button.mListConditionParameter[0].@int = (int)((IButton.INVEN_CONDITION_DETAIL)EditorGUILayout.EnumPopup("INVEN_CONDITION_DETAIL", (IButton.INVEN_CONDITION_DETAIL)button.mListConditionParameter[0].@int));

                        button.mListConditionParameter[1].@object = (GameObject)EditorGUILayout.ObjectField("Loadable Target", button.mListConditionParameter[1].@object, typeof(LoadElement), true);

                        if(button.mListConditionParameter[1].@object != null)
                        {
                            LoadElement param1LoadElement = button.mListConditionParameter[0].@object.GetComponent<LoadElement>();
                        }
                        numConditionParameters = 2;
                    }
                    break;
                case IButton.ActiveCondition.IS_DATA_MATCH:
                    {
                        //button.mListConditionParameter[0].@int = (int)((IButton.MATCH_CONDITION_DETAIL)EditorGUILayout.EnumPopup("MATCH_CONDITION_DETAIL", (IButton.MATCH_CONDITION_DETAIL)button.mListConditionParameter[0].@int));

                        button.mListConditionParameter[0].@object = (GameObject)EditorGUILayout.ObjectField("Compare A(LoadElement)", button.mListConditionParameter[0].@object , typeof(GameObject), true);
                        button.mListConditionParameter[1].@object = (GameObject)EditorGUILayout.ObjectField("Compare B(LoadElement)", button.mListConditionParameter[1].@object , typeof(GameObject), true);

                        LoadElement loadElement = null;
                        if (button.mListConditionParameter[0].@object != null && button.mListConditionParameter[1].@object != null)
                            loadElement = button.mListConditionParameter[0].@object.GetComponent<LoadElement>();

                        if (loadElement)
                        {
                            switch (loadElement.mLoadDetail)
                            {
                                case UI_DATA.LOAD_DETAIL.UNIT:
                                    button.mListConditionParameter[2].@int = (int)((UI_DATA.LOAD_ELEMENT_UNIT)EditorGUILayout.EnumPopup("비교할 데이터 ", (UI_DATA.LOAD_ELEMENT_UNIT)button.mListConditionParameter[2].@int));
                                    break;
                            }
                        }

                        numConditionParameters = 3;
                    }
                    break;
                case IButton.ActiveCondition.NONE: default: break;
            }

        }
    }

    void SetBasicElement(IButton ibutton)
    {
        ibutton.myType = button.myType;
        ibutton.mParentType = button.mParentType;
        ibutton.mParent = button.mParent;
        ibutton.mRootType = button.mRootType;
        ibutton.mRoot = button.mRoot;
    }

    void SetBasicButton()
    {
        Button_Basic button_basic = button.gameObject.AddComponent<Button_Basic>();

        SetBasicElement(button_basic);


        button_basic.mButtonActionType = (Button_Basic.BUTTON_ACTION_BASIC)button.mButtonActionType;
        if (button_basic.mListParameters != null) button_basic.mListParameters.Clear();

        for(int i = 0; i< NumParam1; ++i)
        {
            if (button_basic.mListParameters == null) button_basic.mListParameters = new List<IButton.ButtonParameter>();


            IButton.ButtonParameter param = new IButton.ButtonParameter();
            param.@int = button.mListParameters[i].@int;
            param.@object = button.mListParameters[i].@object;

            button_basic.mListParameters.Add(param);
        }

        SetActiveCondition();
        DestroyImmediate(button);
    }

    void SetToggleButton()
    {
        Button_Toggle button_toggle = button.gameObject.AddComponent<Button_Toggle>();
        SetBasicElement(button_toggle);

        button_toggle.mToggleGroupKey = button.mToggleGroupKey;
        button_toggle.mToggleBackGroundImg = button.mToggleBackGroundImg;
        button_toggle.mToggleActionType = button.mToggleActionType;

        if (button_toggle.mListParameters != null) button_toggle.mListParameters.Clear();

        for(int i = 0; i< NumParam1; ++i)
        {
            if (button_toggle.mListParameters == null) button_toggle.mListParameters = new List<IButton.ButtonParameter>();

            IButton.ButtonParameter param = new IButton.ButtonParameter();
            param.@int = button.mListParameters[i].@int;
            param.@object = button.mListParameters[i].@object;

            button_toggle.mListParameters.Add(param);
        }

        SetActiveCondition();
        DestroyImmediate(button);
    }

    void SetHoldableButton()
    {
        Button_Holdable button_holdable = button.gameObject.AddComponent<Button_Holdable>();
        SetBasicElement(button_holdable);

        button_holdable.mButtonActionType = (Button_Basic.BUTTON_ACTION_BASIC)button.mButtonActionType;

        if (button_holdable.mListParameters != null) button_holdable.mListParameters.Clear();
        //

        button_holdable.mHoldActionType = button.mHoldActionType;
        button_holdable.mMinHoldTime = button.mMinHoldTime;


        if (button_holdable.mListHoldActionParameters != null) button_holdable.mListHoldActionParameters.Clear();

        for (int i = 0; i < NumParam1; ++i)
        {
            if (button_holdable.mListParameters == null) button_holdable.mListParameters = new List<IButton.ButtonParameter>();

            IButton.ButtonParameter param = new IButton.ButtonParameter();
            param.@int = button.mListParameters[i].@int;
            param.@object = button.mListParameters[i].@object;

            button_holdable.mListParameters.Add(param);
        }

        for (int i = 0; i< NumParam2; ++i)
        {
            if (button_holdable.mListHoldActionParameters == null) button_holdable.mListHoldActionParameters = new List<IButton.ButtonParameter>();

            IButton.ButtonParameter param = new IButton.ButtonParameter();
            param.@int = button.mListHoldActionParameters[i].@int;
            param.@object = button.mListHoldActionParameters[i].@object;

            button_holdable.mListHoldActionParameters.Add(param);
        }

        SetActiveCondition();

        DestroyImmediate(button);
    }

    void SetActiveCondition()
    {
        IButton btn = button.gameObject.GetComponent<IButton>();

        btn.mActiveCondition = button.mActiveCondition;

        if (btn.mListConditionParameter != null) btn.mListConditionParameter.Clear();


        for (int i = 0; i < numConditionParameters; ++i)
        {
            if (btn.mListConditionParameter == null) btn.mListConditionParameter = new List<IButton.ButtonParameter>();

            IButton.ButtonParameter param = new IButton.ButtonParameter();
            param.@int = button.mListConditionParameter[i].@int;
            param.@object = button.mListConditionParameter[i].@object;

            btn.mListConditionParameter.Add(param);
        }
    }
}
