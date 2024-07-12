using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using HDJ.Framework.Utils;

public class EditorDrawGUIUtil  {
    public static object DrawBaseValue(string name, object value)
    {
        if (value == null)
            return value;
        Type type = value.GetType();
        object obj = null;
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
           obj=EditorGUILayout.FloatField(new GUIContent(name), (float)value);
        }
        else if (type == typeof(bool))
        {
           obj= EditorGUILayout.Toggle(new GUIContent(name ), (bool)value);
        }
        else if (type.FullName == typeof(string).FullName)
        {
           obj=EditorGUILayout.TextField(new GUIContent(name), value.ToString());
        }
        else if (type.BaseType.FullName == typeof(UnityEngine.Object).FullName || type.BaseType.FullName == typeof(Component).FullName )
        {
           obj= EditorGUILayout.ObjectField(name , (UnityEngine.Object)value,type, true);
        }
        else if (type.BaseType == typeof(Enum))
        {
           obj= EditorGUILayout.EnumPopup(new GUIContent(name), (Enum)Enum.Parse(type, value.ToString()));
        }
        else if (type.FullName == typeof(Vector3).FullName)
        {
           obj= EditorGUILayout.Vector3Field(new GUIContent(name ), (Vector3)value);
        }
        else if (type.FullName == typeof(Vector2).FullName)
        {
            obj = EditorGUILayout.Vector2Field (new GUIContent(name), (Vector2)value);
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
           obj = DrawList(name, value);
        }
        
        else if (type.IsArray)
        {
            obj = DrawArray(name, value);
        }
        else if (type.IsClass)
        {
            GUILayout.Label(name);
            obj = DrawClassData(type.FullName, value);
        }
        else if (type.IsValueType)
        {
            GUILayout.Label(name);
            obj = DrawClassData(type.FullName, value);
        }
        else
        {
            obj = value;
        }

        return obj;
    }
 
    public static object DrawBaseValue(object data, FieldInfo field)
    {
        if (data == null || field == null)
            return null;
        object value = field.GetValue(data);
        object d = DrawBaseValue(field.Name, value);
        field.SetValue(data, d);
        return data;
    }


    public static object DrawClassData(string classFullName,object obj)
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
        return DrawClassData(obj);
    }
    public static object DrawClassData(object obj,List<string> hideFieldNames =null,CallBack<FieldInfo> callAffterDrawField =null)
    {
        if (obj == null)
            return null;
        Type t =obj.GetType();
        FieldInfo[] fs = t.GetFields();
        GUILayout.BeginVertical("box");
        foreach (FieldInfo f in fs)
        {
            if (hideFieldNames != null)
            {
                if (hideFieldNames.Contains(f.Name))
                    continue;
            }
            DrawBaseValue(obj, f);
            if (callAffterDrawField != null)
                callAffterDrawField(f);
        }
        GUILayout.EndVertical();
        return obj;
    }

    public static string DrawPopup(string name, string selectedStr, List<string> displayedOptions,CallBack<string> selectChangeCallBack =null)
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
         selectedIndex = EditorGUILayout.Popup(name, selectedIndex, displayedOptions.ToArray(),style);

        if (displayedOptions.Count == 0)
            selectedStr= "";
        else
            selectedStr= displayedOptions[selectedIndex];
        if (selectedIndex != recode)
        {
            if (selectChangeCallBack != null)
                selectChangeCallBack(displayedOptions[selectedIndex]);
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
            da = DrawPopup("", da.ToString(),displayedOptions);
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
    /// <param name="obj"></param>
    /// <returns></returns>
    public static object DrawList(string name, object obj,CallBackR<object> addCustomData=null, CallBack<object> addItemCallBack=null,CallBack<object> removeItemCallBack =null)
    {
        if (obj == null)
            return null;
        Type type = obj.GetType();
        Type t = type.GetGenericArguments()[0];

        MethodInfo methodInfo = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);        
        MethodInfo methodInfo1 = type.GetMethod("set_Item", BindingFlags.Instance | BindingFlags.Public);
        MethodInfo methodInfo2 = type.GetMethod("RemoveAt", BindingFlags.Instance | BindingFlags.Public);
        MethodInfo methodInfo3 = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Label(name);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.Width(50)))
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
            methodInfo3.Invoke(obj, new object[] { temp });
            if (addItemCallBack != null)
                addItemCallBack(temp);
        }
        GUILayout.EndHorizontal();
        PropertyInfo pro = type.GetProperty("Count");
        int cout = (int)pro.GetValue(obj, null);
        GUILayout.BeginVertical("box");
        for (int i = 0; i < cout; i++)
        {
            object da = methodInfo.Invoke(obj, new object[] { i });
            GUILayout.BeginHorizontal();
              da =  DrawBaseValue("", da);
             methodInfo1.Invoke(obj, new object[] { i,da });

             if (GUILayout.Button("-", GUILayout.Width(50)))
             {
                 methodInfo2.Invoke(obj, new object[] { i });
                if (removeItemCallBack != null)
                    removeItemCallBack(da);
                 break;
             }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        return obj;
    }
    /// <summary>
    /// 绘制数组
    /// </summary> 
    /// <param name="name"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static object DrawArray(string name, object obj, CallBackR<object> addCustomData = null, CallBack<object> addItemCallBack = null, CallBack<object> removeItemCallBack = null)
    {
        if (obj == null)
            return null;
        Type type = obj.GetType();
        string ts = type.FullName.Replace("[]", "");
        if (!ts.Contains("System."))
        {
            Assembly tmp = Assembly.Load("Assembly-CSharp");
            type = tmp.GetType(ts);
        }
        else
        {
            type = Type.GetType(ts);
        }
        var typeList = typeof(List<>);
        Type typeDataList = typeList.MakeGenericType(type);
        object instance = Activator.CreateInstance(typeDataList);

      MethodInfo AddRange =  typeDataList.GetMethod("AddRange");
      AddRange.Invoke(instance, new object[] { obj });
      instance = DrawList(name, instance, addCustomData,addItemCallBack, removeItemCallBack);

      MethodInfo ToArray = typeDataList.GetMethod("ToArray");
      return   ToArray.Invoke (instance,null);
        
    }

  
}
