using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;


[CustomEditor(typeof(UI_Space))]
[RequireComponent(typeof(UI_Space))]
public class SpaceInspectorEditor : LoadableInspectorEditor
{
    UI_Space space;
    private static bool foldOut = false;

    string newName;

    UI_DATA.UI_ELEMENT_TYPE ui_element;


    public void Awake()
    {
        space = (UI_Space)target;
    }

    private void OnEnable()
    {
        space = (UI_Space)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        foldOut = EditorGUILayout.Foldout(foldOut, "SpaceType");

        bool testFieldOn = false;
        bool isContents = (space.gameObject.name == "Content");
        if (foldOut)
        {
            GridLayoutGroup spaceGLG = space.GetComponent<GridLayoutGroup>();
            ScrollRect spaceSR = space.GetComponent<ScrollRect>();


            UI_Space_Test spaceTest = space.GetComponent<UI_Space_Test>();
            if (!spaceTest)
            {
                spaceTest = space.gameObject.AddComponent<UI_Space_Test>();
            }


            if (isContents)
            {
                GUILayout.Label("This Space is Contents(Scrable Rect in parent space)");
                GUILayout.Label("contents는 scroll을 선택할 수 없습니다.");
            }
            GUILayout.Space(10);


            UI_Space.SPACE_TYPE pSpaceType = space.mSpaceType; 
            space.mSpaceType = (UI_Space.SPACE_TYPE)EditorGUILayout.EnumPopup("Space Type :", (UI_Space.SPACE_TYPE)space.mSpaceType);
            if(pSpaceType != space.mSpaceType)
            {
                switch (space.mSpaceType)
                {
                    case UI_Space.SPACE_TYPE.GRID:
                        {
                            if (!spaceGLG) space.gameObject.AddComponent<GridLayoutGroup>();
                            if(spaceSR)
                            {
                                DestroyImmediate(spaceSR.viewport.gameObject);
                                DestroyImmediate(spaceSR);
                            }
                        }
                        break;
                    case UI_Space.SPACE_TYPE.SCROLL:
                        {
                            if (!isContents)
                            {
                                DestroyImmediate(spaceGLG);
                                if(!spaceSR)
                                {
                                    spaceSR = space.gameObject.AddComponent<ScrollRect>();
                                    spaceSR.viewport = ((GameObject)Instantiate(Resources.Load("Prefabs/UI_Basic/ScrollViewport"), space.transform)).GetComponent<RectTransform>();
                                    
                                    spaceSR.viewport.sizeDelta = new Vector2(0,0); // RECT Transform 의 위 2개
                                    spaceSR.viewport.anchoredPosition = new Vector2(0,0); // RECTTransform의 아래 2개
                                    spaceSR.content = spaceSR.viewport.Find("Content").GetComponent<RectTransform>();
                                    Util.BeginLog(spaceSR.content.gameObject);
                                    UI_DATA.SetupParent(UI_DATA.UI_PARENT.SPACE, spaceSR.content.transform, space);
                                    Util.PopLog(spaceSR.content.gameObject);
                                }
                            }
                            else space.mSpaceType = pSpaceType;

                        }
                        break;
                    default:
                        {
                            if (!spaceGLG) DestroyImmediate(spaceGLG);
                            if (spaceSR)
                            {
                                if(spaceSR.viewport) DestroyImmediate(spaceSR.viewport.gameObject);
                                DestroyImmediate(spaceSR);
                            }
                        }
                        break;
                }
            }


            spaceGLG = space.GetComponent<GridLayoutGroup>();

            switch (space.mSpaceType)
            {
                case UI_Space.SPACE_TYPE.GRID:
                    {
                        space.mNumUnitX = (int)EditorGUILayout.IntField("화면에 보일 요소 수 X", space.mNumUnitX); if (space.mNumUnitX < 1) space.mNumUnitX = 1;
                        space.mNumUnitY = (int)EditorGUILayout.IntField("화면에 보일 요소 수 Y", space.mNumUnitY); if (space.mNumUnitY < 1) space.mNumUnitY = 1;

                        {
                            RectTransform rect;
                            if (isContents) rect = space.transform.parent.parent.GetComponent<RectTransform>();
                            else            rect = space.transform.GetComponent<RectTransform>(); 
                            float unitSizeX = (rect.sizeDelta.x - spaceGLG.padding.left - spaceGLG.padding.right - spaceGLG.spacing.x * (space.mNumUnitX - 1)) / space.mNumUnitX;
                            float unitSizeY = (rect.sizeDelta.y - spaceGLG.padding.top - spaceGLG.padding.bottom - spaceGLG.spacing.y * (space.mNumUnitY - 1)) / space.mNumUnitY;
                            spaceGLG.cellSize = new Vector2(unitSizeX, unitSizeY);
                        }

                        if (isContents)
                        {
                            testFieldOn = true;
                            GUILayout.Label("TEST---------------------------------------------------------");
                            string ofText = isContents ? "그리드를 채울 프리팹" : "그리드에 생성할 오브젝트";
                            string numText = isContents ? "테스트 수" : "생성할 수";
                            space.mUnitPrefab = (GameObject)EditorGUILayout.ObjectField(ofText, space.mUnitPrefab, typeof(GameObject), isContents ? false : true);

                            space.mEmptyUnitPrefab = (GameObject)EditorGUILayout.ObjectField("빈 그리드 유닛의 프리팹", space.mEmptyUnitPrefab, typeof(GameObject), false);

                            GUILayout.Label("다수일 시 번호가 붙습니다.");
                            newName = EditorGUILayout.TextField("생성할 이름", newName);



                            spaceTest.testNumLoad = (int)EditorGUILayout.IntField(numText, spaceTest.testNumLoad);
                            {
                                EditorGUILayout.BeginHorizontal();
                                string btnText1 = isContents ? "테스트" : "생성";
                                if (GUILayout.Button(btnText1))
                                {
                                    foreach (var obj in spaceTest.mTestList)
                                        GameObject.DestroyImmediate(obj);
                                    spaceTest.mTestList.Clear();

                                    for (int i = 0; i < spaceTest.testNumLoad; ++i)
                                    {
                                        UI_Element element = UI_DATA.GenerateUIPrefab(newName + i.ToString(), space.mUnitPrefab, UI_DATA.UI_PARENT.SPACE, space.gameObject);

                                        if(space.loadElement)
                                        {

                                            if(space.loadElement.mLoadIndexMode == UI_DATA.LOAD_IDENTIFY.INDEXING)
                                            {
                                                //UI_DATA.SetLoadHierarchy(element.transform, space, i);
                                                LoadElement.SetIndexAtHierarchy(element.transform, i);
                                            }

                                        }
                                        spaceTest.mTestList.Add(element.gameObject);

                                    }
                                }

                                string btnText2 = isContents ? "테스트 삭제" : "모두 삭제";
                                if (GUILayout.Button(btnText2))
                                {
                                    foreach (var obj in spaceTest.mTestList)
                                        GameObject.DestroyImmediate(obj);
                                    spaceTest.mTestList.Clear();

                                }

                                EditorGUILayout.EndHorizontal();
                            }

                            RectTransform rect = space.transform.parent.parent.GetComponent<RectTransform>();
                            ScrollRect scrollParent = rect.GetComponent<ScrollRect>();
                            RectTransform myRectT = space.gameObject.GetComponent<RectTransform>();
                            if (spaceTest.testNumLoad < space.mNumUnitX * space.mNumUnitY)
                            {
                                myRectT.sizeDelta = rect.sizeDelta;
                            }
                            else
                            {
                                if (scrollParent)
                                {
                                    if (scrollParent.vertical && scrollParent.horizontal)
                                    {

                                    }
                                    else if (scrollParent.vertical)
                                    {
                                        //int numLoad = (space.mNumUnitX == 1) ? spaceTest.testNumLoad : spaceTest.testNumLoad + space.mNumUnitX;
                                        int numLoad = (spaceTest.testNumLoad % space.mNumUnitX == 0) ? spaceTest.testNumLoad :  spaceTest.testNumLoad + space.mNumUnitX - spaceTest.testNumLoad % space.mNumUnitX; 
                                        myRectT.sizeDelta
                                            = new Vector2(rect.sizeDelta.x, spaceGLG.cellSize.y * (numLoad / space.mNumUnitX) + spaceGLG.spacing.y * ((numLoad / space.mNumUnitX) - 1) + spaceGLG.padding.top + spaceGLG.padding.bottom);
                                    }
                                    else // scrollPrent.horizontal
                                    {
                                        //int numLoad = (space.mNumUnitY == 1) ? spaceTest.testNumLoad : spaceTest.testNumLoad + space.mNumUnitY;
                                        int numLoad = (spaceTest.testNumLoad % space.mNumUnitY == 0) ? spaceTest.testNumLoad : spaceTest.testNumLoad + space.mNumUnitY - spaceTest.testNumLoad % space.mNumUnitY;
                                        myRectT.sizeDelta
                                            = new Vector2(spaceGLG.cellSize.x * (numLoad / space.mNumUnitY) + spaceGLG.spacing.x * ((numLoad / space.mNumUnitY) - 1) + spaceGLG.padding.left + spaceGLG.padding.right, rect.sizeDelta.y);
                                    }
                                }

                            }
                        }

                    }
                    break;
                case UI_Space.SPACE_TYPE.SCROLL:
                    {
                        GUILayout.Space(10);
                        GUILayout.Label("SPACE_TYPE = SCROLL 일 때, \n" +
                            "표시될 화면은 하위 오브젝트c Contents에서 작업해야합니다.");
                        GUILayout.Label("-------------------------------------------------------------");
                        GUILayout.Label("스크롤 옵션");
                        ScrollRect sr = space.GetComponent<ScrollRect>();
                        sr.horizontal = EditorGUILayout.Toggle("수평이동", sr.horizontal);
                        if (sr.horizontal) sr.vertical = false;
                        sr.vertical = EditorGUILayout.Toggle("수직이동", sr.vertical);
                        if (sr.vertical) sr.horizontal = false;

                        GridLayoutGroup contentGrid = sr.content.GetComponent<GridLayoutGroup>();
                        if(contentGrid)
                        {
                            if (sr.horizontal) contentGrid.startAxis = GridLayoutGroup.Axis.Vertical;
                            if (sr.vertical) contentGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
                        }

                        sr.movementType = (ScrollRect.MovementType)EditorGUILayout.EnumPopup("이동 타입", sr.movementType);

                    }
                    break;
                case UI_Space.SPACE_TYPE.NONE: break;
                default: break;
            }
                


            
        }
        // Space 안에서 UI 요소 생성


        {
            if (!testFieldOn)
            {
                GUILayout.Label("-------------------------------------------------------------");
                ui_element = (UI_DATA.UI_ELEMENT_TYPE)EditorGUILayout.EnumPopup("생성할 UI 선택", ui_element);

                if (ui_element == UI_DATA.UI_ELEMENT_TYPE.PREFAB)
                {
                    space.mUnitPrefab = (GameObject)EditorGUILayout.ObjectField("생성할 프리팹", space.mUnitPrefab, typeof(GameObject), false);
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("이름");
                newName = GUILayout.TextField(newName);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("생성"))
                {
                    switch (ui_element)
                    {
                        case UI_DATA.UI_ELEMENT_TYPE.NONE:
                            break;
                        case UI_DATA.UI_ELEMENT_TYPE.BUTTON:
                            UI_DATA.GenerateUI<UI_Button>(newName, UI_DATA.UI_PARENT.SPACE, space.gameObject);
                            break;
                        case UI_DATA.UI_ELEMENT_TYPE.SPACE:
                            UI_DATA.GenerateUI<UI_Space>(newName, UI_DATA.UI_PARENT.SPACE, space.gameObject);
                            break;
                        case UI_DATA.UI_ELEMENT_TYPE.IMAGE:
                            UI_DATA.GenerateUI<UI_Image>(newName, UI_DATA.UI_PARENT.SPACE, space.gameObject);
                            break;
                        case UI_DATA.UI_ELEMENT_TYPE.TEXT:
                            UI_DATA.GenerateUI<UI_Text>(newName, UI_DATA.UI_PARENT.SPACE, space.gameObject);
                            break;
                        case UI_DATA.UI_ELEMENT_TYPE.PREFAB:
                            UI_DATA.GenerateUIPrefab(newName, space.mUnitPrefab, UI_DATA.UI_PARENT.SPACE, space.gameObject);
                            break;
                    }
                }
            }

        }


        if (GUI.changed) { 

            EditorUtility.SetDirty(space);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

}

}
