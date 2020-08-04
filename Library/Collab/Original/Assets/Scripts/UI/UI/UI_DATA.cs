//#define USE_UI_LOADABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class UI_DATA : ScriptableObject
{
    private static bool debug = true;

    #region ElementTypes

    [System.Serializable]
    public enum UI_ELEMENT_TYPE
    {
        NONE = 0,
        BUTTON,
        SPACE,
        IMAGE,
        TEXT,
        PREFAB,
        END,
    }

    [System.Serializable]
    public enum UI_PARENT
    {
        PANEL,
        POPUP,
        SPACE,
        END,
    }
    #endregion

    #region Enum Loadable Units
    public enum LOAD_TYPE
    {
        NONE,
        LOAD,
        CONNECT,
    }
    public enum LOAD_STAGE
    {
        NONE,
        MAIN = 0,
        IDENTIFY,
        DETAIL,
        ELEMENT,
    }

    // -- space
    [System.Serializable]
    public enum LOAD_MAIN
    {
        NONE,
        EQUIP,
        INVENTORY,
        SHOP,
        STAGE,
        FROM_PARENT,
    }

    public enum LOAD_IDENTIFY
    {
        NONE,
        INDEXING,
        FROM_PARENT,
    }

    [System.Serializable]
    public enum LOAD_DETAIL
    {
        NONE,
        GOLD,
        STEMINA,
        USER_SKILL,
        USER,
        UNIT,
        ITEM_CONSUMABLE,
        ITEM_ETC,
        STAGE,
        STAGE_ENEMY,
        STAGE_ITEM,
    }


    //-- img, txt
    [System.Serializable]
    public enum LOAD_ELEMENT_BASIC
    {
        NONE,
        IMAGE,
        TEXT,
        TIMERTEXT,
    }
    [System.Serializable]
    public enum LOAD_ELEMENT_USER_SKILL
    {
        NONE,
        NAME,
        IMAGE,
        DESC,
        EQUIPTEXT,
        EQUIP,
    }
    [System.Serializable]
    public enum LOAD_ELEMENT_USER
    {
        NONE,
        NAME,
        IMAGE3D,
        LEVEL,
        EXP,
    }
    [System.Serializable]
    public enum LOAD_ELEMENT_UNIT
    {
        NONE,
        NAME,
        IMAGE,
        STATUS_DESC,
        LEVEL,
        EXP,
        IMG_POSITION,
        SKILL_NAME,
        SKILL_IMAGE,
        SKILL_DESC,
        ATTACK_NAME,
        ATTACK_IMAGE,
        ATTACK_DESC,
        LV_UP_GOLD,
        LV_UP_ITEM1_IMAGE,
        LV_UP_ITEM1_NUM,
        LV_UP_ITEM2_IMAGE,
        LV_UP_ITEM2_NUM,
        LV_UP_ITEM3_IMAGE,
        LV_UP_ITEM3_NUM,
    }
    [System.Serializable]
    public enum LOAD_ELEMENT_ITEM
    {
        NONE,
        NAME,
        IMAGE,
        DESC,
        NUM,
    }

    [System.Serializable]
    public enum LOAD_ELEMENT_STAGE
    {
        STAGE_NUM_TEXT,
        STAGE_FLOOR,
    }

    [System.Serializable]
    public enum LOAD_ELEMENT_SHOP_ETC
    {
        NONE,
        NAME,
        IMAGE,
        DESC,
        PRICE,
    }

    #endregion
    [System.Serializable]
    public enum LOAD_ELEMENT_SHOP_UNIT
    {
        NONE,
        NAME,
        IMAGE,
        STATUS_DESC,
        LEVEL,
        IMG_POSITION,
        PRICE,
        PRICETYPE,
    }
    [System.Serializable]
    public enum LOAD_ELEMENT_SHOP_ITEM
    {
        NONE,
        NAME,
        IMAGE,
        DESC,
        PRICE,
        PRICETYPE,
        NAMEANDCOUNT,
    }
    public enum LOAD_ELEMENT_SHOP_GOLD
    {
        NONE,
        NAME,
        IMAGE,
        PRICE,
        PRICETYPE,
        DESC,
    }
    public enum LOAD_ELEMENT_SHOP_STEMINA
    {
        NONE,
        NAME,
        IMAGE,
        PRICE,
        PRICETYPE,
        DESC,
    }
    public static UI_Element GenerateUI<T>(string objName, UI_PARENT uiFrom, GameObject objFrom)
    {
        Util.BeginLog();
        Util.Log("GenerateUI : " + typeof(T).ToString() + "\n\t" + "argument : " + objName + ", " + uiFrom.ToString() + ", " + "objFrom : " + objFrom.name + "\n\t");

        GameObject newObj = Instantiate((GameObject)Resources.Load(GetPrefabPath<T>()), objFrom.transform);
        //GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(GetPrefabPath<T>()), objFrom.transform);
        newObj.name = objName;
        RectTransform rectT = newObj.GetComponent<RectTransform>();
        rectT.anchoredPosition = new Vector3(0f, 0f, 0f);

        SetupParent(uiFrom, newObj.transform, objFrom.GetComponent<UI_Element>());


        Util.PopLog();
        return newObj.GetComponent<UI_Element>();
    }

    public static UI_Element GenerateUIPrefab(string objName, GameObject prefab, UI_PARENT uiFrom, GameObject objFrom)
    {
        GameObject newObject =  Instantiate(prefab, objFrom.transform);
        //GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab, objFrom.transform);
        newObject.name = objName;
        RectTransform rectT = newObject.GetComponent<RectTransform>();
        rectT.anchoredPosition = new Vector3(0, 0f, 0f);
        SetupParent(uiFrom,newObject.transform,objFrom.GetComponent<UI_Element>());


        return newObject.GetComponent<UI_Element>();
    }


    public static string GetPrefabPath<T>()
    {
        System.Type type = typeof(T);

        if (type == typeof(UI_Button)) return "Prefabs/UI_Basic/Button_prefab";
        if (type == typeof(UI_Image)) return "Prefabs/UI_Basic/Image_prefab";
        if (type == typeof(UI_Space)) return "Prefabs/UI_Basic/Space_prefab";
        if (type == typeof(UI_Text)) return "Prefabs/UI_Basic/Text_prefab";
        return "";
    }

    public static UI_ELEMENT_TYPE GetMyUIType(Transform obj)
    {
        if (obj.GetComponent<IButton>()) return UI_ELEMENT_TYPE.BUTTON;
        if (obj.GetComponent<UI_Image>()) return UI_ELEMENT_TYPE.IMAGE;
        if (obj.GetComponent<UI_Space>()) return UI_ELEMENT_TYPE.SPACE;
        if (obj.GetComponent<UI_Text>()) return UI_ELEMENT_TYPE.TEXT;

        return UI_ELEMENT_TYPE.NONE;
    }


    public static void SetupParent(UI_PARENT uiFrom, Transform targetElement, UI_Element parentElement)
    {
        UI_Element myElement = targetElement.GetComponent<UI_Element>();


        if (myElement)
        {
            Util.BeginLog();
            Util.Log("SetupParent()\n");
            myElement.myType = GetMyUIType(myElement.transform);

            myElement.mRootType = parentElement.mRootType;
            myElement.mRoot = parentElement.mRoot;

            if(parentElement.myType == UI_ELEMENT_TYPE.SPACE)
            {
                myElement.mParentType = UI_PARENT.SPACE;
                myElement.mParent = parentElement.gameObject;
            }
            else
            {
                myElement.mParentType = uiFrom;
                myElement.mParent = parentElement.mParent;
            }

            if(myElement.myType == UI_ELEMENT_TYPE.BUTTON)
            {
                IButton ui_button = myElement.GetComponent<IButton>();
                if (ui_button) ui_button.ButtonSetup();
            }

            Util.Log_Info_Element(myElement);
            Util.PopLog();
        }


        for (int i = 0; i < targetElement.transform.childCount; ++i)
        {
            Transform child = targetElement.transform.GetChild(i);

            if (myElement)
            {
                if(myElement.myType == UI_ELEMENT_TYPE.SPACE)
                {
                    SetupParent(UI_PARENT.SPACE, child.transform, myElement);
                }
                else
                {
                    SetupParent(uiFrom, child.transform, parentElement);
                }
            }
            else
            {
                SetupParent(uiFrom, child, parentElement);
            }
        }


    }

    // panel과 popuppanel 에서만 실행
    public static void SetupParent(UI_PARENT parent, GameObject rootObject)
    {
        Util.BeginLog();
        Util.Log("SetupParent()\n");

        UI_Element rootElement = rootObject.GetComponent<UI_Element>();

        rootElement.mRootType = parent;
        rootElement.mRoot = rootElement.gameObject;
        rootElement.mParentType = (parent == UI_PARENT.PANEL) ? UI_PARENT.PANEL : UI_PARENT.POPUP;
        rootElement.mParent = rootElement.gameObject;

        Util.Log_Info_Element(rootElement);

        Util.endLine();
        Util.PopLog();


        for (int i = 0; i< rootObject.transform.childCount; ++i)
        {
            Transform child = rootObject.transform.GetChild(i);

            SetupParent(parent, child, rootObject.GetComponent<UI_Element>());
        }

    }

    public static void StartLoadHierarchy(Transform target)
    {
        UI_Loadable tLoadable = target.GetComponent<UI_Loadable>();
        if(tLoadable)
        {
            if (tLoadable.loadElement)
            {
                Util.BeginLog();
                Util.Log(target.name + " Load Start!\n");
                Util.Log_Info_Loadable(tLoadable.GetComponent< LoadElement>());

                tLoadable.LoadUIData();

                Util.PopLog();
            }
        }


        for (int i = 0; i < target.childCount; ++i)
        {
            StartLoadHierarchy(target.GetChild(i));
        }
    }

    public static void SetHierarchy(Transform target, UI_Element parentElement, UI_Loadable parent, int index)
    {
        UI_Element myElement = target.GetComponent<UI_Element>();
        UI_Loadable tLoadable = target.GetComponent<UI_Loadable>();

        if (myElement)
        {
            Util.BeginLog();
            Util.Log("SetupParent()\n");
            myElement.myType = GetMyUIType(myElement.transform);

            myElement.mRootType = parentElement.mRootType;
            myElement.mRoot = parentElement.mRoot;

            if (parentElement.myType == UI_ELEMENT_TYPE.SPACE)
            {
                myElement.mParentType = UI_PARENT.SPACE;
                myElement.mParent = parentElement.gameObject;
            }
            else
            {
                myElement.mParentType = parentElement.mParentType;
                myElement.mParent = parentElement.mParent;
            }

            
            if (myElement.myType == UI_ELEMENT_TYPE.BUTTON)
            {
                IButton ui_button = myElement.GetComponent<IButton>();
                if (ui_button) ui_button.ButtonSetup();
            }
            

            Util.Log_Info_Element(myElement);
            Util.PopLog();
        }

        for (int i = 0; i < target.childCount; ++i)
        {
            Transform child = target.GetChild(i);

            if (myElement)
            {
                if (myElement.myType == UI_ELEMENT_TYPE.SPACE)
                {
                    SetHierarchy(child.transform, myElement, (tLoadable) ? tLoadable : parent, index);
                }
                else
                {
                    SetHierarchy(child.transform, parentElement, (tLoadable) ? tLoadable : parent, index);
                }
            }
            else
            {
                SetHierarchy(child, parentElement, (tLoadable) ? tLoadable : parent, index);
            }
        }

    }

    public static void CopyLoadData(Transform source, Transform dest)
    {
        
        LoadElement sourceL = source.GetComponent<LoadElement>();
        LoadElement destL = dest.GetComponent<LoadElement>();

        if(sourceL && destL)
        {
            Util.BeginLog();
            Util.Log("Source(" + sourceL.name + ")" + " Dest(" + destL.name + ")\n");
            Util.Log("Copy LoadMain :"+destL.mLoadMain + " = " + sourceL.mLoadMain + "\n");
            destL.mLoadMain = sourceL.mLoadMain;

            Util.Log("Copy LoadIndex :" + destL.mLoadIndexMode + "(" + destL.mLoadIndex + ")" +  " = " + sourceL.mLoadIndexMode + "(" + sourceL.mLoadIndex +")" + "\n");
            destL.mLoadIndexMode = sourceL.mLoadIndexMode;
            destL.mLoadIndex = sourceL.mLoadIndex;

            Util.Log("Copy LoadDetail :" + destL.mLoadDetail + " = " + sourceL.mLoadDetail + "\n");
            destL.mLoadDetail = sourceL.mLoadDetail;

            destL.ResetHierarchy();
            Util.PopLog();
        }
    }

    public static void RemoveLayCastTargetWidhOutBtn(Transform target)
    {

        if (!target.GetComponent<UI_Button>() ||
            target.GetComponent<UI_Panel>() ||
            target.GetComponent<UI_Popup>())
        {
            if (target.name != "Contents")
            {
                Image img = target.GetComponent<Image>();
                if (img) { img.raycastTarget = false;  }
                Text txt = target.GetComponent<Text>();
                if (txt) { txt.raycastTarget = false; }
                RawImage rImg = target.GetComponent<RawImage>();
                if (rImg){rImg.raycastTarget = false;  }

            }
            else
            {
                Image img = target.GetComponent<Image>();
            }
        }

        for(int i = 0; i< target.childCount; ++i)
        {
            RemoveLayCastTargetWidhOutBtn(target.transform.GetChild(i));
        }
    }

}

