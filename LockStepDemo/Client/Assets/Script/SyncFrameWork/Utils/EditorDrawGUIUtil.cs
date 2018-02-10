using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class EditorDrawGUIUtil
{
    /// <summary>
    /// 是否绘制类和结构体的属性
    /// </summary>
    public static bool IsDrawPropertyInClass = true;
    /// <summary>
    /// 是否绘制能编辑GUI
    /// </summary>
    public static bool CanEdit = true;
    public static object DrawBaseValue(string name, object value, EditorUIOpenState openState = EditorUIOpenState.NoOpen, UIStateData uIStateData = null)
    {
        if (value == null)
            return value;
        Type type = value.GetType();
        object obj = null;
        if (CanEdit)
        {
            if (type == typeof(int))
            {
                obj = EditorGUILayout.IntField(new GUIContent(name), (int)value);
            }
            else if (type == typeof(short))
            {
                obj = EditorGUILayout.IntField(new GUIContent(name), (short)value);
            }
            else if (type == typeof(long))
            {
                obj = EditorGUILayout.LongField(new GUIContent(name), (long)value);
            }
            else if (type == typeof(double))
            {
                obj = EditorGUILayout.DoubleField(new GUIContent(name), (double)value);
            }
            else if (type == typeof(float))
            {
                obj = EditorGUILayout.FloatField(new GUIContent(name), (float)value);
            }
            else if (type == typeof(bool))
            {
                obj = EditorGUILayout.Toggle(new GUIContent(name), (bool)value);
            }
            else if (type.FullName == typeof(string).FullName)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(name);
                obj = GUILayout.TextArea(value.ToString());
                GUILayout.EndHorizontal();
            }
            else if (type.BaseType.FullName == typeof(UnityEngine.Object).FullName || type.BaseType.FullName == typeof(Component).FullName|| type.BaseType.FullName==typeof(MonoBehaviour).FullName)
            {
                obj = EditorGUILayout.ObjectField(name, (UnityEngine.Object)value, type, true);
            }
            else if (type.BaseType == typeof(Enum))
            {
                obj = EditorGUILayout.EnumPopup(new GUIContent(name), (Enum)Enum.Parse(type, value.ToString()));
            }
            else if (type.FullName == typeof(Vector3).FullName)
            {
                obj = EditorGUILayout.Vector3Field(new GUIContent(name), (Vector3)value);
            }
            else if (type.FullName == typeof(Vector2).FullName)
            {
                obj = EditorGUILayout.Vector2Field(new GUIContent(name), (Vector2)value);
            }
            else if (type.FullName == typeof(Vector4).FullName)
            {
                obj = EditorGUILayout.Vector4Field(new GUIContent(name), (Vector4)value);
            }
            else if (type.FullName == typeof(UnityEngine.Color).FullName)
            {
                obj = EditorGUILayout.ColorField(new GUIContent(name), (Color)value);
            }
            else if (type.Name == typeof(List<>).Name)
            {
                obj = DrawList(name, value, null, null, openState, uIStateData);
            }
            else if (type.Name == typeof(Dictionary<,>).Name)
            {
                obj = DrawDictionary(name, value, openState, uIStateData);
            }
            else if (type.IsArray)
            {
                obj = DrawArray(name, value, null, null, openState, uIStateData);
            }
            else if (type.IsClass && type != typeof(string))
            {
                obj = DrawClassData(name, type.FullName, value, openState, uIStateData);
            }
            else if (type.IsValueType)
            {
                obj = DrawClassData(name, type.FullName, value, openState, uIStateData);
            }
        }
        else
        {
            if(type.IsPrimitive || type == typeof(string) || type.IsEnum || type==typeof(Vector2)
               || type == typeof(Vector3)
               || type == typeof(Vector4)
               || type.BaseType.FullName == typeof(UnityEngine.Object).FullName || type.BaseType.FullName == typeof(Component).FullName
               || type.FullName == typeof(UnityEngine.Color).FullName
               || type.BaseType.FullName == typeof(MonoBehaviour).FullName)
            {
                string showStr = value.ToString();
                if (type.BaseType.FullName == typeof(UnityEngine.Object).FullName || type.BaseType.FullName == typeof(Component).FullName|| type.BaseType.FullName == typeof(MonoBehaviour).FullName)
                {
                    obj = EditorGUILayout.ObjectField(name, (UnityEngine.Object)value, type, true);
                }
                else
                {
                    GUILayout.BeginVertical("dockarea");
                    EditorGUILayout.LabelField(name, showStr);
                    GUILayout.EndVertical();
                }
                obj = value;
            }
           else if (type.Name == typeof(List<>).Name)
            {
                obj = DrawList(name, value, null, null, openState, uIStateData);
            }
            else if (type.Name == typeof(Dictionary<,>).Name)
            {
                obj = DrawDictionary(name, value, openState, uIStateData);
            }
            else if (type.IsArray)
            {
                obj = DrawArray(name, value, null, null, openState, uIStateData);
            }
            else if (type.IsClass && type != typeof(string))
            {
                obj = DrawClassData(name, type.FullName, value, openState, uIStateData);
            }
            else if (type.IsValueType)
            {
                obj = DrawClassData(name, type.FullName, value, openState, uIStateData);
            }
            else
            {
               
                obj = value;
            }

        }
      

        return obj;
    }

    public static object DrawBaseValue(object data, FieldInfo field, EditorUIOpenState openState = EditorUIOpenState.NoOpen, UIStateData uIStateData = null)
    {
        if (data == null || field == null)
            return null;
        object value = field.GetValue(data);
        if (value == null)
        {
            value = ReflectionUtils.CreateDefultInstance(field.FieldType);
        }
        object d = DrawBaseValue(field.Name, value,openState, uIStateData);
        field.SetValue(data, d);
        return data;
    }
    public static object DrawBaseValue(object data, PropertyInfo property, EditorUIOpenState openState = EditorUIOpenState.NoOpen, UIStateData uIStateData = null)
    {
        if (data == null || property == null)
            return null;
        if (property.CanRead)
        {
            object value = property.GetValue(data, null);
            if (value == null)
            { 
                value = ReflectionUtils.CreateDefultInstance(property.PropertyType);
            }
            object d = DrawBaseValue(property.Name, value,openState, uIStateData);
            if (property.CanWrite)
            {
                property.SetValue(data, d, null);
            }
        }

        return data;
    }


    public static object DrawClassData(string name, string classFullName, object obj, EditorUIOpenState openState = EditorUIOpenState.NoOpen, UIStateData uIStateData = null)
    {

        Type t = ReflectionUtils.GetTypeByTypeFullName(classFullName);

        if (obj == null)
            obj = Activator.CreateInstance(t);
        else
        {
            if (t.FullName != obj.GetType().FullName)
            {
                obj = Activator.CreateInstance(t);
            }
        }
        return DrawClassData(name, obj, null, null,openState, uIStateData);
    }
    /// <summary>
    /// 绘制类
    /// </summary>
    /// <param name="data"></param>
    /// <param name="hideFieldPropertyNames">隐藏某些字段不绘制</param>
    /// <param name="callAffterDrawField">每绘制一个字段后调用</param>
    /// <returns></returns>
    public static object DrawClassData(string name, object data, List<string> hideFieldPropertyNames = null, CallBack<MemberInfo> callAffterDrawField = null, EditorUIOpenState openState = EditorUIOpenState.NoOpen, UIStateData uIStateData = null)
    {
        if (data == null)
        {
            Debug.LogError("data is null");
            return null;
        }
        if (uIStateData == null)
            uIStateData = EditorUIStateData.GetRootUIState(data.GetHashCode(), openState);

        Type t = data.GetType();
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


        FieldInfo[] fs = t.GetFields();
        PropertyInfo[] propertys = t.GetProperties();

        if (!string.IsNullOrEmpty(name))
            name = name + "(" + t.Name + ")";
        else
            name = "";

        bool isFold = true;
        bool useStateData = false;
        if (uIStateData != null)
        {
            useStateData = true;
            isFold = EditorGUILayout.Foldout(uIStateData.isFold, name);
        }
        else
            GUILayout.Label(name);

        if (fs.Length > 0 || (propertys.Length > 0 && IsDrawPropertyInClass))
        {
           
            if (isFold)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Box("FieldInfo:");
            }
            foreach (var f in fs)
            {
                if (hideFieldPropertyNames != null)
                {
                    if (hideFieldPropertyNames.Contains(f.Name))
                        continue;
                }
                object[] attrs = f.GetCustomAttributes(false);
                bool isShow = true;
                foreach (var att in attrs)
                {
                    if (att.GetType() == ReflectionUtils.GetTypeByTypeFullName("NoShowInEditorAttribute"))
                    {
                        isShow = false;
                        break;
                    }
                }

                if (!isShow)
                    continue;
                UIStateData uIStateData0 = null;
                if (uIStateData != null)
                {
                    uIStateData0 = uIStateData.editorUIStateData.GetNextStateData();
                }
                if (isFold)
                {
                    DrawBaseValue(data, f, openState, uIStateData0);
                    if (callAffterDrawField != null)
                        callAffterDrawField(f);
                }
            }
            if (propertys.Length > 0 && IsDrawPropertyInClass)
            {
                if (isFold)
                {
                    GUILayout.Space(2);
                    GUILayout.Box("PropertyInfo:");
                }
                foreach (var f in propertys)
                {
                    if (hideFieldPropertyNames != null)
                    {
                        if (hideFieldPropertyNames.Contains(f.Name))
                            continue;
                    }

                    UIStateData uIStateData0 = null;
                    if (uIStateData != null)
                    {
                        uIStateData0 = uIStateData.editorUIStateData.GetNextStateData();
                    }
                    if (isFold)
                    {
                        DrawBaseValue(data, f, openState, uIStateData0);
                        if (callAffterDrawField != null)
                            callAffterDrawField(f);
                    }
                }
            }
            if (isFold)
                GUILayout.EndVertical();
        }
        if (useStateData)
        {
            uIStateData.isFold = isFold;
        }
        return data;
    }

    public static string DrawPopup(string name, string selectedStr, List<string> displayedOptions, CallBack<string> selectChangeCallBack = null)
    {
        int selectedIndex = -1;
        if (displayedOptions.Contains(selectedStr))
        {
            selectedIndex = displayedOptions.IndexOf(selectedStr);
        }
        int recode = selectedIndex;
        if (selectedIndex == -1)
            selectedIndex = 0;
        GUIStyle style = new GUIStyle("Popup");
        style.richText = true;
        selectedIndex = EditorGUILayout.Popup(name, selectedIndex, displayedOptions.ToArray(), style);

        if (displayedOptions.Count == 0)
            selectedStr = "";
        else
            selectedStr = displayedOptions[selectedIndex];
        if (selectedIndex != recode)
        {
            if (selectChangeCallBack != null)
                selectChangeCallBack(selectedStr);
        }
        return selectedStr;
    }
    /// <summary>
    /// List的 弹出菜单
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    /// <param name="displayedOptions"></param>
    /// <returns></returns>
    public static object DrawStringPopupList(string name, object obj, List<string> displayedOptions)
    {
        if (obj == null)
            return null;
        Type type = obj.GetType();

        MethodInfo methodInfo = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
        MethodInfo methodInfo1 = type.GetMethod("Item", BindingFlags.Instance | BindingFlags.Public);
        MethodInfo methodInfo2 = type.GetMethod("RemoveAt", BindingFlags.Instance | BindingFlags.Public);
        MethodInfo methodInfo3 = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
        GUILayout.BeginHorizontal();
        GUILayout.Label(name);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.Width(50)))
        {
            object temp = "";
            methodInfo3.Invoke(obj, new object[] { temp });
        }
        GUILayout.EndHorizontal();
        PropertyInfo pro = type.GetProperty("Count");
        int cout = (int)pro.GetValue(obj, null);
        GUILayout.BeginVertical("box");
        for (int i = 0; i < cout; i++)
        {
            object da = methodInfo.Invoke(obj, new object[] { i });
            GUILayout.BeginHorizontal();
            da = DrawPopup("", da.ToString(), displayedOptions);
            methodInfo1.Invoke(obj, new object[] { i, da });

            if (GUILayout.Button("-", GUILayout.Width(50)))
            {
                methodInfo2.Invoke(obj, new object[] { i });
                break;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        return obj;
    }
    /// <summary>
    /// 绘制List泛型
    /// </summary>
    /// <param name="name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static object DrawList(string name, object data, CallBackR<object> addCustomData = null, CallBack<bool, object> ItemChnageCallBack = null, EditorUIOpenState openState = EditorUIOpenState.NoOpen, UIStateData uIStateData = null)
    {
        if (data == null)
        {
            Debug.LogError("data is null");
            return null;
        }
        if (uIStateData == null)
            uIStateData = EditorUIStateData.GetRootUIState(data.GetHashCode(), openState);

        Type type = data.GetType();
        Type t = type.GetGenericArguments()[0];

        int count = (int)type.GetProperty("Count").GetValue(data, null);

        if (!string.IsNullOrEmpty(name))
            name = name + "(List<" + t.Name + ">)  Count : "+ count;
        else
            name = "";

        bool isFold = true;
        bool useStateData = false;
        if (uIStateData != null)
        {
            useStateData = true;
            isFold = EditorGUILayout.Foldout(uIStateData.isFold, name);
        }
        else
            GUILayout.Label(name);

        MethodInfo methodInfo = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
        MethodInfo methodInfo1 = type.GetMethod("set_Item", BindingFlags.Instance | BindingFlags.Public);
        MethodInfo methodInfo2 = type.GetMethod("RemoveAt", BindingFlags.Instance | BindingFlags.Public);
        MethodInfo methodInfo3 = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
      

        if (isFold)
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (CanEdit && GUILayout.Button("+", GUILayout.Width(50)))
            {
                object temp = null;
                if (addCustomData != null)
                {
                    object tempVV = addCustomData();
                    if (tempVV == null || tempVV.GetType().FullName != t.FullName)
                        temp = ReflectionUtils.CreateDefultInstance(t);
                    else
                        temp = tempVV;
                }
                else
                    temp = ReflectionUtils.CreateDefultInstance(t);
                methodInfo3.Invoke(data, new object[] { temp });
                if (ItemChnageCallBack != null)
                    ItemChnageCallBack(true, temp);
            }
            GUILayout.EndHorizontal();
        }

        if (count > 0)
        {
            if (isFold)
                GUILayout.BeginVertical("box");

            for (int i = 0; i < count; i++)
            {
                UIStateData uIStateData0 = null;
                if (uIStateData != null)
                {
                    uIStateData0 = uIStateData.editorUIStateData.GetNextStateData();
                }
                if (isFold)
                {
                    object da = methodInfo.Invoke(data, new object[] { i });
                    GUILayout.BeginHorizontal();

                    da = DrawBaseValue("" + i, da, openState, uIStateData0);

                    methodInfo1.Invoke(data, new object[] { i, da });

                    if (CanEdit && GUILayout.Button("-", GUILayout.Width(50)))
                    {
                        methodInfo2.Invoke(data, new object[] { i });
                        if (ItemChnageCallBack != null)
                            ItemChnageCallBack(false, da);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }
            }

            if (isFold)
                GUILayout.EndVertical();
        }

        if (isFold)
        {
            GUILayout.EndVertical();
        }

        if (useStateData)
        {
            uIStateData.isFold = isFold;
        }
        return data;
    }
    /// <summary>
    /// 绘制数组
    /// </summary> 
    /// <param name="name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static object DrawArray(string name, object data, CallBackR<object> addCustomData = null, CallBack<bool, object> ItemChnageCallBack = null, EditorUIOpenState openState = EditorUIOpenState.NoOpen, UIStateData uIStateData = null)
    {
        if (data == null)
        {
            Debug.LogError("data is null");
            return null;
        }
        if (uIStateData == null)
            uIStateData = EditorUIStateData.GetRootUIState(data.GetHashCode(), openState);

        Type type = data.GetType();
        string ts = type.FullName.Replace("[]", "");

        type = ReflectionUtils.GetTypeByTypeFullName(ts);
        var typeList = typeof(List<>);
        Type typeDataList = typeList.MakeGenericType(type);
        object instance = Activator.CreateInstance(typeDataList);

        MethodInfo AddRange = typeDataList.GetMethod("AddRange");
        AddRange.Invoke(instance, new object[] { data });
        instance = DrawList(name, instance, addCustomData, ItemChnageCallBack,openState, uIStateData);

        MethodInfo ToArray = typeDataList.GetMethod("ToArray");
        return ToArray.Invoke(instance, null);

    }

    public static object DrawDictionary(string name, object data, EditorUIOpenState openState= EditorUIOpenState.NoOpen, UIStateData uIStateData = null)
    {

        if (data == null)
        {
            Debug.LogError("data is null");
            return null;
        }
        if (uIStateData == null)
            uIStateData = EditorUIStateData.GetRootUIState(data.GetHashCode(), openState);

        Type type = data.GetType();
        Type[] tArr = type.GetGenericArguments();
        PropertyInfo p = type.GetProperty("Count");
        int count = (int)p.GetValue(data, null);

        if (!string.IsNullOrEmpty(name))
            name = name + "(Dictionary<" + tArr[0].FullName + "," + tArr[1].FullName + ">)  Count : "+ count;
        else
            name = "";

        bool isFold = true;
        bool useStateData = false;
        if (uIStateData != null)
        {
            useStateData = true;
            isFold = EditorGUILayout.Foldout(uIStateData.isFold, name);
        }
        else
            GUILayout.Label(name);

        

        p = type.GetProperty("Keys");
        object keys = p.GetValue(data, null);
        List<object> tempListKeys = new List<object>();
        MethodInfo GetEnumeratorMe = keys.GetType().GetMethod("GetEnumerator");
        PropertyInfo current = GetEnumeratorMe.ReturnParameter.ParameterType.GetProperty("Current");
        MethodInfo moveNext = GetEnumeratorMe.ReturnParameter.ParameterType.GetMethod("MoveNext");

        object enumerator = GetEnumeratorMe.Invoke(keys, null);
        for (int i = 0; i < count; i++)
        {
            moveNext.Invoke(enumerator, null);
            object v = current.GetValue(enumerator, null);
            tempListKeys.Add(v);
        }

        p = type.GetProperty("Values");
        object values = p.GetValue(data, null);
        List<object> tempListValues = new List<object>();
        GetEnumeratorMe = values.GetType().GetMethod("GetEnumerator");
        current = GetEnumeratorMe.ReturnParameter.ParameterType.GetProperty("Current");
        moveNext = GetEnumeratorMe.ReturnParameter.ParameterType.GetMethod("MoveNext");
        enumerator = GetEnumeratorMe.Invoke(values, null);
        for (int i = 0; i < count; i++)
        {
            moveNext.Invoke(enumerator, null);
            object v = current.GetValue(enumerator, null);
            // addMe.Invoke(tempList, new object[] { v });
            tempListValues.Add(v);
        }

        object tempDic = Activator.CreateInstance(type);

        MethodInfo addDicMe = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

        if (tempListKeys.Count > 0)
        {
            if (isFold)
                GUILayout.BeginVertical("box");
            for (int i = 0; i < tempListKeys.Count; i++)
            {
                UIStateData uIStateData0 = null;
                UIStateData uIStateData1 = null;
                if (useStateData)
                {
                    uIStateData0 = uIStateData.editorUIStateData.GetNextStateData();
                }
                if (useStateData)
                {
                    uIStateData1 = uIStateData.editorUIStateData.GetNextStateData();
                }

                if (isFold)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (CanEdit && GUILayout.Button("-", GUILayout.Width(40)))
                    {
                        tempListKeys.RemoveAt(i);
                        tempListValues.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("box");
                    GUILayout.BeginVertical("box");


                    tempListKeys[i] = DrawBaseValue(i + ": key", tempListKeys[i], openState, uIStateData0);
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("box");


                    tempListValues[i] = DrawBaseValue("value", tempListValues[i], openState, uIStateData1);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();


                    GUILayout.Space(2);
                }
            }
            if (isFold)
                GUILayout.EndVertical();
            for (int i = 0; i < tempListKeys.Count; i++)
            {
                addDicMe.Invoke(tempDic, new object[] { tempListKeys[i], tempListValues[i] });
            }
        }

        if (useStateData)
        {
            uIStateData.isFold = isFold;
        }
        return tempDic;
    }

}

public enum EditorUIOpenState
{
    /// <summary>
    /// 不使用状态保存
    /// </summary>
    NoOpen,
    /// <summary>
    /// 使用状态保存,第一次打开全部折叠
    /// </summary>
    OpenFirstFold,
    /// <summary>
    /// 使用状态保存,第一次打开全部不折叠
    /// </summary>
    OpenFirstNoFold,
}
/// <summary>
/// 记录UI状态
/// </summary>
public class EditorUIStateData
{

    private static Dictionary<int, EditorUIStateData> allStateDataDic = new Dictionary<int, EditorUIStateData>();
    private const int CacheUIStateMaxNumber = 2000;
    private static EditorUIStateData GetUIStateData(int hashCode,out bool isFirst)
    {
       
        if (allStateDataDic.ContainsKey(hashCode))
        {
            isFirst = false;
            return allStateDataDic[hashCode];
        }
        if (allStateDataDic.Count >= CacheUIStateMaxNumber)
            allStateDataDic.Remove(new List<int>(allStateDataDic.Keys)[0]);

       
        EditorUIStateData d = new EditorUIStateData();
        allStateDataDic.Add(hashCode, d);
        isFirst = true;
        return d;
    }

    public static UIStateData GetRootUIState(int hashCode, EditorUIOpenState openState)
    {
        bool isFirst = false;
        EditorUIStateData d;
        switch (openState)
        {
            case EditorUIOpenState.NoOpen:
                return null;
            case EditorUIOpenState.OpenFirstFold:

                d = GetUIStateData(hashCode, out isFirst);
                if (isFirst)
                    d.isFoldFirstOpen = false;
                return d.GetRootStateData();
            case EditorUIOpenState.OpenFirstNoFold:

                d = GetUIStateData(hashCode, out isFirst);
                if (isFirst)
                    d.isFoldFirstOpen = true;
                return d.GetRootStateData();
        }
        return null;
    }

    public bool isFoldFirstOpen = true;

   

    private List<UIStateData> allStateData = new List<UIStateData>();

    private int index = 0;

    public UIStateData GetRootStateData()
    {
        index = 0;
        
        return GetNextStateData();
    }

    public UIStateData GetNextStateData()
    {
        UIStateData uIState = null;
        if (allStateData.Count <= index)
        {
            uIState = new UIStateData(this);
            uIState.isFold = isFoldFirstOpen;
            allStateData.Add(uIState);
        }
        if (uIState == null)
        {
            uIState = allStateData[index];
        }
        index++;
        return uIState;
    }
}

public class UIStateData
{
    /// <summary>
    /// 是否折叠
    /// </summary>
    public bool isFold = false;

    public Vector2 scrollPos = Vector2.zero;

    public EditorUIStateData editorUIStateData;
    public UIStateData(EditorUIStateData editorUIStateData)
    {
        this.editorUIStateData = editorUIStateData;
    }
}
