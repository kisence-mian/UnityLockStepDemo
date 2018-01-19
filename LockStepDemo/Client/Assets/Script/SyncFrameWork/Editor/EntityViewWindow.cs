using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class EntityViewWindow : EditorWindow
{
    [MenuItem("Window/Entity同步监视器", priority = 302)]
    public static void ShowWindow()
    {
        EntityViewWindow win= GetWindow<EntityViewWindow>();
        win.autoRepaintOnSceneChange = true;
        win.wantsMouseMove = true;
    }

    List<WorldBase> worldList;
    List<string> worldNames = new List<string>();
    Dictionary<string, WorldBase> worldDic = new Dictionary<string, WorldBase>();
    private void OnEnable()
    {
        worldList = WorldManager.WorldList;
        worldNames.Clear();
        worldDic.Clear();
        if (worldList == null)
            return;

        foreach (var item in worldList)
        {
            worldNames.Add(item.name);
            worldDic.Add(item.name, item);
        }
    }
    public  int toolbarOption = 0;
     public string[] toolbarTexts = { "显示实体","显示单例组件", "显示Group" };
    private string selectStr;
    private WorldBase selectWorld;
    private void OnGUI()
    {
        GUILayout.Space(5);

        if (GUILayout.Button("刷新"))
        {
            OnEnable();
        }
        GUILayout.Space(5);

        selectStr = EditorDrawGUIUtil.DrawPopup("选择世界:", selectStr, worldNames);
      
            if (worldList == null || worldList.Count == 0 || string.IsNullOrEmpty(selectStr))
            {
                GUILayout.Label("无数据");
                return;
            }

         selectWorld = worldDic[selectStr];
        GUILayout.Space(5);
        toolbarOption = GUILayout.Toolbar(toolbarOption, toolbarTexts);
        switch (toolbarOption)
        {
            case 0:
                ShowEntitysGUI(selectWorld);
                break;
            case 1:

                ShowSingleComponentGUI(selectWorld);
                break;
            case 2:
                ShowGroupsGUI(selectWorld);
                break;
        }
    }



    string serchEntity = "";
    private int selectEntityItem =-1;
    private Vector2 pos0;
    private Vector2 pos1;
    /// <summary>
    /// 显示实体列表
    /// </summary>
    /// <param name="world"></param>
    private void ShowEntitysGUI(WorldBase world)
    {
        List<EntityBase> allEntitys = world.m_entityList;

        GUILayout.BeginHorizontal();
        serchEntity = GUILayout.TextField( serchEntity, "SearchTextField");
        if(GUILayout.Button("", "SearchCancelButton",GUILayout.Width(Screen.width / 3)))
        {
            serchEntity = "";
        }
        GUILayout.EndHorizontal();

        List<string> names = new List<string>();
        foreach (var item in allEntitys)
        {
            string newName = item.ID +" > "+item.name;

            if(serchEntity != "" && !newName.Contains(serchEntity))
            {
                continue;
            }
            names.Add(newName);
        }
        selectEntityItem = ShowLeftPartWindow("Entity",names, selectEntityItem);
        if (selectEntityItem != -1)
            ShowRightPartWindow(() =>
           {
               if (allEntitys.Count == 0)
                   return;
               if(selectEntityItem>= allEntitys.Count)
               {
                   selectEntityItem = allEntitys.Count - 1;
               }
               EntityBase entuty = allEntitys[selectEntityItem]
               ;
               GUILayout.BeginVertical("dockarea");
               EditorGUILayout.LabelField("Name : ", entuty.name);
               GUILayout.EndVertical();
               GUILayout.BeginVertical("dockarea");
               EditorGUILayout.LabelField("ID : ", entuty.ID.ToString());
               GUILayout.EndVertical();
             

               List<ComponentBase> components = new List<ComponentBase>(entuty.comps);
               foreach (var item in components)
               {
                   if (item == null)
                       continue;
                   DrawComponent(item);
               }
           });
    }


    int selectInt1 = -1;
    /// <summary>
    /// 绘制Group
    /// </summary>
    /// <param name="world"></param>
    private void ShowGroupsGUI(WorldBase world)
    {
       ECSGroupManager groupManager = world.group;
        Dictionary<int, ECSGroup> AllGroupDic = groupManager.AllGroupDic;
        List<ECSGroup> groups = new List<ECSGroup>(AllGroupDic.Values);

        List<string> names = new List<string>();
        foreach (var item in groups)
        {
            names.Add(item.Key.ToString());
        }
        selectInt1 = ShowLeftPartWindow("Group ID", names, selectInt1);
        if (selectInt1 != -1)
            ShowRightPartWindow(() =>
            {
                if (groups.Count == 0)
                    return;
                if (selectInt1 >= groups.Count)
                    selectInt1 = groups.Count - 1;
                ECSGroup ecsGroup = groups[selectInt1];
                DrawECSGroup(ecsGroup,groupManager.GroupToEntityDic[ecsGroup]);
            });
    }
    /// <summary>
    /// 绘制单个Group
    /// </summary>
    /// <param name="ecsGroup"></param>
    /// <param name="entitys"></param>
    private void DrawECSGroup(ECSGroup ecsGroup,List<EntityBase> entitys)
    {
        GUILayout.Box("包含的组件名：");
        foreach (var item in ecsGroup.Components)
        {
            GUILayout.Box(item, "WarningOverlay", GUILayout.MaxWidth(Screen.width - 30));
        }

        GUILayout.Space(6);
        GUILayout.Box("包含的实体：");
        foreach (var item in entitys)
        {
            GUILayout.BeginHorizontal("box");
            GUILayout.Box("Entity");
            GUILayout.BeginVertical();
            GUILayout.Label("Name : "+item.name);
            GUILayout.Label("ID : "+item.ID.ToString());
            GUILayout.EndVertical();

            if (GUILayout.Button("查看详细"))
            {
              selectEntityItem =  selectWorld.m_entityList.IndexOf(item);
                toolbarOption = 0;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }
    }
    /// <summary>
    /// 绘制单个组件
    /// </summary>
    /// <param name="component"></param>
    private void DrawComponent(ComponentBase component)
    {
        Type t = component.GetType();
        string typeName = GetTypeName(component);
       
        GUILayout.Box(typeName, "WarningOverlay",GUILayout.MaxWidth(Screen.width-20));
        FieldInfo[] fields= t.GetFields();

        if (fields.Length == 0)
            return;
        GUILayout.BeginVertical("box");
        EditorDrawGUIUtil.IsDrawPropertyInClass = false;
        EditorDrawGUIUtil.CanEdit = false;
        foreach (var item in fields)
        {
            component=(ComponentBase) EditorDrawGUIUtil.DrawBaseValue(component, item, EditorUIOpenState.OpenFirstFold);
        }
        EditorDrawGUIUtil.IsDrawPropertyInClass = true;
        EditorDrawGUIUtil.CanEdit = true;
        GUILayout.EndVertical();
    }

    int selectInt0 = -1;
    /// <summary>
    /// 绘制单例组件
    /// </summary>
    /// <param name="world"></param>
    private void ShowSingleComponentGUI(WorldBase world)
    {
     
        List<SingletonComponent> components=new List<SingletonComponent>(  world.m_singleCompDict.Values);
        List<string> names = new List<string>();
        foreach (var item in components)
        {
            names.Add(GetTypeName(item));
          //  DrawComponent(item);
        }
        selectInt0 = ShowLeftPartWindow("Components", names, selectInt0);
        if (selectInt0 != -1)
            ShowRightPartWindow(() =>
            {
                if (components.Count == 0)
                    return;
                if (selectInt0 >= components.Count)
                    selectInt0 = components.Count - 1;

                SingletonComponent component = components[selectInt0];
                DrawComponent(component);
            });

    }

    int leftRectY = 80;
    /// <summary>
    /// 左边window绘制
    /// </summary>
    /// <param name="ShowName"></param>
    /// <param name="names"></param>
    /// <param name="select"></param>
    /// <returns></returns>
    private int ShowLeftPartWindow(string ShowName, List<string> names, int select)
    {
        int num = select;
        Rect leftRect = new Rect(0, leftRectY + 15, Screen.width / 3, Screen.height - leftRectY );
        GUILayout.BeginArea(leftRect, ShowName, "box");
        GUILayout.Space(20);

        pos0 = GUILayout.BeginScrollView(pos0);
        for (int i = 0; i < names.Count; i++)
        {
            string entity = names[i];
            if (i == select)
            {
                GUI.color = Color.yellow;
            }
            if (GUILayout.Button(entity))
            {
                num = i;

            }
            GUI.color = Color.white;
            GUILayout.Space(4);

        }
        GUILayout.Space(30);
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        return num;
    }
    /// <summary>
    /// 右边window绘制
    /// </summary>
    /// <param name="callBack"></param>
    private void ShowRightPartWindow(CallBack callBack)
    {

        Rect rightRect = new Rect(Screen.width / 3 + 10, leftRectY, Screen.width * 2 / 3 - 10, Screen.height - leftRectY);
        GUILayout.BeginArea(rightRect, "Components", "box");
        GUILayout.Space(25);
        pos1 = GUILayout.BeginScrollView(pos1);
        if (callBack != null)
            callBack();
        GUILayout.Space(20);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private string GetTypeName(object obj)
    {
        Type t = obj.GetType();
        string typeName = t.Name;
        if (t.IsGenericType)
        {
            typeName = typeName.Remove(typeName.IndexOf('`'));
            Type[] gTypes = t.GetGenericArguments();
            string temp = "";
            for (int i = 0; i < gTypes.Length; i++)
            {
                Type tempType = gTypes[i];
                temp += tempType.Name;
                if (i < gTypes.Length - 1)
                {
                    temp += ",";
                }
            }
            typeName = typeName + "<" + temp + ">";
        }

        return typeName;
    }

    
}
