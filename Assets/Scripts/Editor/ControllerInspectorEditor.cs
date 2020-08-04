using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UICommons;

#region PanelController


[CustomEditor(typeof(PanelController))]
public class PanelControllerEditor : Editor
{
    public static string prefabPath = "Prefabs/UI_GridUnit/";
    PanelController panelController;
    public static int numGenerate = 0;
    private void OnEnable()
    {
        panelController = (PanelController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Option---------------------------------------------");
        numGenerate = EditorGUILayout.IntField("생성할 수", numGenerate);
        if(GUILayout.Button("Generate Grid Unit"))
        {
            for(int i = 0; i<numGenerate; ++i)
            {
                GenerateGridUnit(panelController);
            }
        }
        if(GUILayout.Button("Clear Grid Unit"))
        {
            ClearGridUnit(panelController);
        }
        if(GUILayout.Button("Fit Grid Size"))
        {
            FitGridSize(panelController);
        }


        if (GUI.changed)
            EditorUtility.SetDirty(panelController);
    }


    public virtual void GenerateGridUnit(PanelController panelController) { }
    public virtual void ClearGridUnit(PanelController panelController) { }

    public virtual void FitGridSize(PanelController panelController) { }
}

[CustomEditor(typeof(CharacterMenuController))]
public class CMenuControllerEditor : PanelControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    public override void GenerateGridUnit(PanelController panelController) {
        //GridUnit_InvenSkill
        var controller = panelController as CharacterMenuController;

        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(prefabPath+ "GridUnit_InvenSkill"), controller.GridSpace);

        if (controller.InvenSkills == null) controller.InvenSkills = new List<GridUnitController>();

        controller.InvenSkills.Add(newObj.GetComponent<GridUnitController>());
        UICommon.FitGridSize(controller.GridSpace, controller.InvenSkills.Count);
    }

    public override void FitGridSize(PanelController panelController)
    {
        var controller = panelController as CharacterMenuController;
        UICommon.FitGridSize(controller.GridSpace, controller.InvenSkills.Count);
    }
    public override void ClearGridUnit(PanelController panelController)
    {
        var controller = panelController as CharacterMenuController;
        foreach (var unit in controller.InvenSkills)
        {
            DestroyImmediate(unit.gameObject);
        }
        controller.InvenSkills.Clear();
    }
}

[CustomEditor(typeof(ItemMenuController))]
public class ItemMenuControllerEditor : PanelControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    public override void GenerateGridUnit(PanelController panelController) {

        //"GridUnit_Item_Consume" : "GridUnit_Item_ETC";
        var controller = panelController as ItemMenuController;

        if (controller.InvenUnits == null) controller.InvenUnits = new List<GridUnitController>();
        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(prefabPath + "GridUnit_Item_Consume"), controller.GridSpace);
        GameObject newObj2 = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(prefabPath + "GridUnit_Item_ETC"), controller.GridSpace);
        controller.InvenUnits.Add(newObj.GetComponent<GridUnitController>());
        controller.InvenUnits.Add(newObj2.GetComponent<GridUnitController>());
        UICommon.FitGridSize(controller.GridSpace, controller.InvenUnits.Count);
    }
    public override void FitGridSize(PanelController panelController)
    {
        var controller = panelController as ItemMenuController;
        UICommon.FitGridSize(controller.GridSpace, controller.InvenUnits.Count);
    }
    public override void ClearGridUnit(PanelController panelController)
    {
        var controller = panelController as ItemMenuController;
        foreach (var unit in controller.InvenUnits)
        {
            DestroyImmediate(unit.gameObject);
        }
        controller.InvenUnits.Clear();
    }
}

[CustomEditor(typeof(ShopGSMenuController))]
public class ShopGSMenuControllerEditor : PanelControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    public override void GenerateGridUnit(PanelController panelController) {

        var controller = panelController as ShopGSMenuController;
        if (controller.ShopSteminas == null) controller.ShopSteminas = new List<GridUnitController>();
        if (controller.ShopGolds == null) controller.ShopGolds = new List<GridUnitController>();

        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(prefabPath + "GridUnit_ShopStemina"), controller.SteminaUnitSpace);
        GameObject newObj2 = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(prefabPath + "GridUnit_ShopGold"), controller.GoldUnitSpace);
        controller.ShopSteminas.Add(newObj.GetComponent<GridUnitController>());
        UICommon.FitGridSize(controller.SteminaUnitSpace, controller.ShopSteminas.Count);
        controller.ShopGolds.Add(newObj2.GetComponent<GridUnitController>());
        UICommon.FitGridSize(controller.GoldUnitSpace, controller.ShopGolds.Count);

    }
    public override void FitGridSize(PanelController panelController)
    {
        var controller = panelController as ShopGSMenuController;
        UICommon.FitGridSize(controller.SteminaUnitSpace, controller.ShopSteminas.Count);
        UICommon.FitGridSize(controller.GoldUnitSpace, controller.ShopGolds.Count);
    }
    public override void ClearGridUnit(PanelController panelController)
    {
        var controller = panelController as ShopGSMenuController;
        foreach (var unit in controller.ShopSteminas)
        {
            DestroyImmediate(unit.gameObject);
        }
        controller.ShopSteminas.Clear();

        foreach (var unit in controller.ShopGolds)
        {
            DestroyImmediate(unit.gameObject);
        }
        controller.ShopGolds.Clear();
    }
}

[CustomEditor(typeof(ShopUIMenuController))]
public class ShopUIMenuControllerEditor : PanelControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    public override void GenerateGridUnit(PanelController panelController) {

        var controller = panelController as ShopUIMenuController;

        if (controller.ShopUnits == null) controller.ShopUnits = new List<GridUnitController>();
        if (controller.ShopItems == null) controller.ShopItems = new List<GridUnitController>();

        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(prefabPath + "GridUnit_ShopUnit"), controller.unitUnitSpace);
        GameObject newObj2 = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(prefabPath + "GridUnit_ShopItem"), controller.itemUnitSpace);
        controller.ShopUnits.Add(newObj.GetComponent<GridUnitController>());
        UICommon.FitGridSize(controller.unitUnitSpace, controller.ShopUnits.Count);
        controller.ShopItems.Add(newObj2.GetComponent<GridUnitController>());
        UICommon.FitGridSize(controller.itemUnitSpace, controller.ShopItems.Count);
    }
    public override void FitGridSize(PanelController panelController)
    {
        var controller = panelController as ShopUIMenuController;
        UICommon.FitGridSize(controller.unitUnitSpace, controller.ShopUnits.Count);
        UICommon.FitGridSize(controller.itemUnitSpace, controller.ShopItems.Count);
    }
    public override void ClearGridUnit(PanelController panelController)
    {
        var controller = panelController as ShopUIMenuController;
        foreach (var unit in controller.ShopUnits)
        {
            DestroyImmediate(unit.gameObject);
        }
        controller.ShopUnits.Clear();

        foreach (var unit in controller.ShopItems)
        {
            DestroyImmediate(unit.gameObject);
        }
        controller.ShopItems.Clear();
    }
}

[CustomEditor(typeof(UnitMenuController))]
public class UnitMenuControllerEditor : PanelControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    public override void GenerateGridUnit(PanelController panelController) {
        //GridUnit_InvenUnit
        var controller = panelController as UnitMenuController;
        if (controller.InvenUnits == null) controller.InvenUnits = new List<GridUnitController>();

        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(prefabPath + "GridUnit_InvenUnit"), controller.GridSpace);
        controller.InvenUnits.Add(newObj.GetComponent<GridUnitController>());
        UICommon.FitGridSize(controller.GridSpace, controller.InvenUnits.Count);
    }
    public override void FitGridSize(PanelController panelController)
    {
        var controller = panelController as UnitMenuController;
        UICommon.FitGridSize(controller.GridSpace, controller.InvenUnits.Count);
    }
    public override void ClearGridUnit(PanelController panelController)
    {
        var controller = panelController as UnitMenuController;
        foreach (var unit in controller.InvenUnits)
        {
            DestroyImmediate(unit.gameObject);
        }
        controller.InvenUnits.Clear();
    }
}


#endregion

#region GridUnitController

[CustomEditor(typeof(GridUnitController))]
public class GridUnitControllerEditor : Editor
{
    GridUnitController controller;
    private void OnEnable()
    {
        controller = (GridUnitController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("------------------------------------------------------");
        controller.isCovered = EditorGUILayout.Toggle("Cover", controller.isCovered);


        if (GUI.changed)
        {
            EditorUtility.SetDirty(controller);

            if(controller.Cover != null)
            {
                controller.Cover.SetActive(controller.isCovered);
            }
        }
    }
}

[CustomEditor(typeof(EquipUnitController))]
public class EquipUnitControllerEditor : GridUnitControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
[CustomEditor(typeof(InvenItemController))]
public class InvenItemControllerEditor : GridUnitControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(InvenSkillController))]
public class InvenSkillControllerEditor : GridUnitControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
[CustomEditor(typeof(InvenUnitController))]
public class InvenUnitControllerEditor : GridUnitControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
[CustomEditor(typeof(ShopGoldController))]
public class ShopGoldControllerEditor : GridUnitControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
[CustomEditor(typeof(ShopItemController))]
public class ShopItemControllerEditor : GridUnitControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
[CustomEditor(typeof(ShopSteminaController))]
public class ShopSteminaControllerEditor : GridUnitControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
[CustomEditor(typeof(ShopUnitController))]
public class ShopUnitControllerEditor : GridUnitControllerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endregion
 
[CustomEditor(typeof(GridOption))]
public class GridEditor : Editor
{
    GridOption m;
    private void OnEnable()
    {
        m = (GridOption)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridLayoutGroup grid = m.grid;

        int nX = EditorGUILayout.IntField("화면에 보일 수 X", m.numX);
        int nY = EditorGUILayout.IntField("화면에 보일 수 Y", m.numY);
        if (nX > 0) m.numX = nX;
        if (nY > 0) m.numY = nY;
        if (m.scroll != null)
        {

            RectTransform window = m.scroll.GetComponent<RectTransform>();

            if (m.scroll.horizontal && !m.scroll.vertical)
            {
                grid.cellSize = new Vector2(
                    (window.rect.width - grid.padding.left - grid.spacing.y - (m.numX - 1) * grid.spacing.x) / (float)m.numX,
                    (window.rect.height - grid.padding.top - grid.padding.bottom - (m.numY - 1) * grid.spacing.y) / (float)m.numY);
            }
            else if (m.scroll.vertical && !m.scroll.horizontal)
            {
                grid.cellSize = new Vector2(
                    (window.rect.width - grid.padding.left - grid.padding.right - (m.numX - 1) * grid.spacing.x) / (float)m.numX,
                    (window.rect.height - grid.padding.top - grid.spacing.y - (m.numY - 1) * grid.spacing.y) / (float)m.numY);
            }


            UICommon.FitGridSize(m.transform, m.transform.childCount);
        }
        else
        {
            RectTransform window = m.GetComponent<RectTransform>();
            grid.cellSize = new Vector2(
                (window.rect.width - grid.padding.left - grid.padding.right - (m.numX - 1)* grid.spacing.x)/(float)m.numX,
                (window.rect.height - grid.padding.top - grid.padding.bottom - (m.numY-1)*grid.spacing.y)/(float)m.numY
                );
        }


        if (GUI.changed)
        {
            EditorUtility.SetDirty(m);
        }
    }
}