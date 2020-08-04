//#define OLD_VERSION


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(UI_BackGround))]
public class BackgroundInspectorEditor : Editor
{

    UI_BackGround mBG;
    private static bool bBGFold = false;
    public string groupKey;

    public Toggle_Group.TOGGLE_GROUP_ACTION ToggleGroupAction;
    public int NumMaxSelect = 1;
    public bool CanSwitchOff;
    public bool MakeTargetResetBtn;

    [SerializeField]
    public List<UI_Space> listTargetSpace = new List<UI_Space>();

    private void OnEnable()
    {
        mBG = (UI_BackGround)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        mBG = (UI_BackGround)target;


        if (GUILayout.Button("eliminate All UI Element"))
        {
            UI_DATA.eleminateAllUI(mBG.transform);
        }

        if (GUILayout.Button("LoadElement 모두 삭제"))
        {
            DestroyLoadElement(mBG.transform);
        }


        bBGFold = EditorGUILayout.Foldout(bBGFold,"ToggleGroup 세팅");
        if(bBGFold)
        {
            groupKey = EditorGUILayout.TextField("생성할 토글 그룹 키", groupKey);

            for (int i = 0; i < mBG.transform.childCount; ++i)
            {

                Toggle_Group tg = mBG.transform.GetChild(i).GetComponent<Toggle_Group>();
                if (tg)
                {
                    if (tg.name != "_tg" + groupKey) continue;

                    groupKey = "";
                    break;
                }
            }

            if (groupKey != "")
            {
                if (GUILayout.Button("ToggleGroup 생성"))
                {

                    GameObject tg = new GameObject();
                    tg.transform.SetParent(mBG.transform);
                    Toggle_Group togglegroup = tg.AddComponent<Toggle_Group>();
                    togglegroup.mCanSwitchOff = false;
                    togglegroup.mNumMaxSelect = 1;
                    togglegroup.mToggleGroupAction = Toggle_Group.TOGGLE_GROUP_ACTION.BASIC;
                    togglegroup.mMakeTargetResetBtn = false;
                    tg.name = "tg_" + groupKey;
                }
            }


        }
    }

    

    public void DestroyLoadElement(Transform tr)
    {
        for(int i = 0; i < tr.childCount; i++)
        {
            DestroyLoadElement(tr.GetChild(i));

            LoadElement le = tr.GetChild(i).GetComponent<LoadElement>();
            if(le)
            {
                DestroyImmediate(le);
            }
        }
    }
}
