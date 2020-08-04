using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UI_Canvas))]
public class CanvasInspectorEditor : Editor
{
    UI_Canvas canvas;

    private void OnEnable()
    {
        canvas = (UI_Canvas)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("모든 패널 부모 설정"))
        {
             for(int i = 0; i< canvas.transform.childCount; ++i)
            {
                Transform child = canvas.transform.GetChild(i);
                if (child.GetComponent<UI_Element>())
                {
                    if(child.GetComponent<UI_Panel>())
                    {
                        UI_DATA.SetupParent(UI_DATA.UI_PARENT.PANEL, child.gameObject);
                    }
                }
            }
        }

        if(GUILayout.Button("eliminate All UI Element"))
        {
            UI_DATA.eleminateAllUI(canvas.transform);
        }

        /*
        if(GUILayout.Button("모든 로드 관계 재설정"))
        {
            for(int i = 0; i< canvas.transform.childCount; ++i)
            {
                Transform child = canvas.transform.GetChild(i);
                if(child.GetComponent<UI_Element>())
                {
                    Util.BeginLog(child.gameObject);
                    //UI_DATA.SetLoadHierarchy(child.transform, null);
                    Util.PopLog(child.gameObject);
                }
            }
        }
        */

        //if(GUILayout.Button("버튼과 Contents 제외 RaycastTarget 해제"))
        //{
        //    for(int i = 0; i< canvas.transform.childCount; ++i)
        //    {
        //        Transform child = canvas.transform.GetChild(i);
        //        if(child.GetComponent<UI_Element>())
        //        {
        //            if(child.GetComponent<UI_Panel>())
        //            {
        //                UI_DATA.RemoveLayCastTargetWidhOutBtn(child);
        //            }
        //        }
        //    }
        //}

        if (GUI.changed)
            EditorUtility.SetDirty(canvas);
    }

    
}
