using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using HDJ.Framework.Tools;

namespace HDJ.Framework.Utils
{
    /// <summary>
    /// Json处理工具类
    /// </summary>
    public static class JsonUtils
    {
        /// <summary>
        /// Json转换为List<T>数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> JsonToList<T>(string json)
        {
            List<T> datas = new List<T>();
            object listData = JsonToList(json, typeof(T));
            if (listData == null)
                return datas;
            Type t = listData.GetType();
            PropertyInfo p = t.GetProperty("Count");
            int count = (int)p.GetValue(listData, null);
            MethodInfo methodInfo = t.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < count; i++)
            {
                object item = methodInfo.Invoke(listData, new object[] { i });
                datas.Add((T)item);
            }
            return datas;
        }
        /// <summary>
        /// Json转换List<T>
        /// </summary>
        /// <param name="json"></param>
        /// <param name="itemType">T的type</param>
        /// <returns></returns>
        public static object JsonToList(string json, Type itemType)
        {
            object obj = SimpleJsonTool.DeserializeObject(json);
            object res = JsonObjectToList(obj, itemType);
            return res;
        }
        private static object JsonObjectToList(object obj, Type itemType)
        {
            IList<object> listData = obj as IList<object>;
            Type listType = typeof(List<>).MakeGenericType(itemType);
            object list = ReflectionUtils.CreateDefultInstance(listType);
            if (listData == null || listData.Count == 0)
                return list;
            bool isSupportBaseValueType = BaseValueUtils.IsSupportJsonToBaseValue(itemType);
           
            // Activator.CreateInstance(listType);
            MethodInfo addMethod = listType.GetMethod("Add");
            for (int i = 0; i < listData.Count; i++)
            {
                object obj0 = listData[i];
                obj0 = ChangeJsonDataByType(itemType, obj0);
                if (obj0 == null)
                    continue;
                addMethod.Invoke(list, new object[] { obj0 });
            }
            return list;
        }
        /// <summary>
        /// List<T>转换为Json
        /// </summary>
        /// <param name="datas">List<T></param>
        /// <returns>json</returns>
        public static string ListToJson(object datas)
        {
            object temp = ListArrayToJsonObject(datas, true);
            return SimpleJsonTool.SerializeObject(temp);

        }
        private static object ListArrayToJsonObject(object datas, bool isList)
        {
            Type type = datas.GetType();
            PropertyInfo pro = null;
            if (isList)
            {
                pro = type.GetProperty("Count");
            }
            else
            {
                pro = type.GetProperty("Length");
            }

            int count = (int)pro.GetValue(datas, null);
            MethodInfo methodInfo = null;
            if (isList)
                methodInfo = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
            else
                methodInfo = type.GetMethod("GetValue", new Type[] { typeof(int) });
            List<object> temp = new List<object>();
            for (int i = 0; i < count; i++)
            {
                //Debug.Log("count :" + count);
                //Debug.Log("methodInfo :" + methodInfo.Name);
                object da = methodInfo.Invoke(datas, new object[] { i });
                da = ChangeObjectToJsonObjec(da);
                temp.Add(da);
            }
            return temp;
        }
        /// <summary>
        /// Json转换为Array
        /// </summary>
        /// <param name="json"></param>
        /// <param name="itemType">数组的类型T[]的T类型</param>
        /// <returns></returns>
        public static object JsonToArray(string json, Type itemType)
        {
            object obj = SimpleJsonTool.DeserializeObject(json);
            return JsonObjectToArray(obj, itemType);
        }
        private static object JsonObjectToArray(object data, Type itemType)
        {
            object result = JsonObjectToList(data, itemType);
            MethodInfo method= result.GetType().GetMethod("ToArray");
            //Debug.Log("JsonObjectToArray : result:" + result.GetType().FullName);
            return method.Invoke(result,null);
        }
        /// <summary>
        /// Array转换为Json
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static string ArrayToJson(object datas)
        {
            object temp = ListArrayToJsonObject(datas, false);
            return SimpleJsonTool.SerializeObject(temp);
        }

        /// <summary>
        /// class或struct转换为json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ClassOrStructToJson(object data)
        {
            object jsonObject = ClassOrStructToJsonObject(data);
            return SimpleJsonTool.SerializeObject(jsonObject);
        }
        private static object ClassOrStructToJsonObject(object data)
        {
            Type type = data.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];
                if (f.IsNotSerialized)
                    continue;
                if (f.IsStatic || f.IsLiteral)
                    continue;
                if (f.IsFamily || f.IsPrivate)
                {
                    object[] atts = f.GetCustomAttributes(typeof(SerializeField), true);
                    if (atts == null || atts.Length == 0)
                        continue;
                }
                object v = f.GetValue(data);
                string name = f.Name;
                if (v == null)
                    continue;
                //Debug.Log("F.name:" + f.Name + " value: " + v);
                v = ChangeObjectToJsonObjec(v);
                dic.Add(name, v);
            }
            return dic;
        }
        private static object ChangeObjectToJsonObjec(object data)
        {
            Type t = data.GetType();
            object value = data;
            if (!BaseValueUtils.IsSupportBaseValueParseJson(t))
            {
                //Debug.Log("ChangeObjectToJsonObjec: type:" + t.FullName + "  value:" + value + " t.IsGenericType:" + t.IsGenericType);
                if (t.IsArray)
                    value = ListArrayToJsonObject(data, false);
                else if (t.IsGenericType)
                {
                    if (typeof(List<>).Name == t.Name)
                        value = ListArrayToJsonObject(data, true);
                    else if (typeof(Dictionary<,>).Name == t.Name)
                        value = DictionaryToJsonObject(data);
                }
                else
                {
                    if (t.IsClass || t.IsValueType)
                        value = ClassOrStructToJsonObject(data);
                }
            }
            return value;
        }

        public static T JsonToClassOrStruct<T>(string json)
        {
            return (T)JsonToClassOrStruct(json, typeof(T));
        }
        /// <summary>
        /// json转换为class或struct
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type">class或struct的type</param>
        /// <returns></returns>
        public static object JsonToClassOrStruct(string json, Type type)
        {
            object obj = SimpleJsonTool.DeserializeObject(json);
            return JsonObjectToClassOrStruct(obj, type);
        }
        private static object JsonObjectToClassOrStruct(object jsonObj, Type type)
        {
            IDictionary<string, object> dic = (IDictionary<string, object>)jsonObj;
            object instance = ReflectionUtils.CreateDefultInstance(type);//  Activator.CreateInstance(type);
            if (dic == null || instance == null)
            {
                //Debug.LogError("Obj:" + jsonObj.GetType().FullName + "  value:" + jsonObj.ToString());
                //Debug.LogError("null : obj:" + (jsonObj == null) + " dic :" + (dic == null) + "  instance:" + (instance == null));
                return null;
            }
            List<string> nameList = new List<string>(dic.Keys);
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            for (int i = 0; i < nameList.Count; i++)
            {
                string key = nameList[i];
                FieldInfo f = type.GetField(key, flags);
                if (f == null)
                    continue;
                object value = dic[key];
                value = ChangeJsonDataByType(f.FieldType, value);
                f.SetValue(instance, value);
            }

            return instance;
        }
        private static object ChangeJsonDataByType(Type type, object data)
        {
            object value = null; 
            if (data == null )
                return value;
            if (type.IsPrimitive || type == typeof(string))
            {
                value = data;
            }
            else if (type.IsEnum)
            {
                value = Enum.ToObject(type, data);
            }
            else if (type.IsArray)
            {
                try
                {
                    object listObj = JsonObjectToList(data, type.GetElementType());
                    MethodInfo methodInfo = listObj.GetType().GetMethod("ToArray", BindingFlags.Instance | BindingFlags.Public);
                    value = methodInfo.Invoke(listObj, null);
                }
                catch (Exception e)
                {
                    Debug.LogError("Array无法转换类型， data：" + data.GetType().FullName + "  type.GetElementType(): " + type.GetElementType().FullName);
                    Debug.LogError(e);
                }
            }
            else if (type.IsGenericType)
            {
                if (typeof(List<>).Name == type.Name)
                {
                    value = JsonObjectToList(data, type.GetGenericArguments()[0]);
                }
                else if (typeof(Dictionary<,>).Name == type.Name)
                {
                    Type[] ts = type.GetGenericArguments();
                    value = JsonObjectToDictionary(data, ts[0], ts[1]);
                }
            }
            else
            {
                if (type.IsClass || type.IsValueType)
                {
                    value = JsonObjectToClassOrStruct(data, type);
                }
            }
            if (value == null)
                return value;
            try
            {
                if (!type.Equals(value.GetType()))
                    value = Convert.ChangeType(value, type);
            }
            catch (Exception e)
            {
                Debug.LogError("无法转换类型， type：" + type.FullName + "  valueType: " + value.GetType().FullName);
                Debug.LogError(e);
            }
            return value;
        }
        /// <summary>
        /// Dictionary<k,v>转换为json 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DictionaryToJson(object data)
        {
            object obj = DictionaryToJsonObject(data);
            return SimpleJsonTool.SerializeObject(obj);
        }
        private static object DictionaryToJsonObject(object data)
        {
            Type type = data.GetType();
            PropertyInfo p = type.GetProperty("Count");
            int count = (int)p.GetValue(data, null);

            p = type.GetProperty("Keys");
            object keys = p.GetValue(data, null);
            List<object> tempList = new List<object>();
          //  MethodInfo addMe = tempList.GetType().GetMethod("Add");
            MethodInfo GetEnumeratorMe = keys.GetType().GetMethod("GetEnumerator");
            PropertyInfo current = GetEnumeratorMe.ReturnParameter.ParameterType.GetProperty("Current");
            MethodInfo moveNext = GetEnumeratorMe.ReturnParameter.ParameterType.GetMethod("MoveNext");

            object enumerator = GetEnumeratorMe.Invoke(keys, null);
            for (int i = 0; i < count; i++)
            {
                moveNext.Invoke(enumerator, null);
                object v = current.GetValue(enumerator, null);
               // addMe.Invoke(tempList, new object[] { v });
                tempList.Add(v);
            }
            keys = ListArrayToJsonObject(tempList, true);

            p = type.GetProperty("Values");
            object values = p.GetValue(data, null);
            tempList = new List<object>();
         //   addMe = tempList.GetType().GetMethod("Add");
            GetEnumeratorMe = values.GetType().GetMethod("GetEnumerator");
            current = GetEnumeratorMe.ReturnParameter.ParameterType.GetProperty("Current");
            moveNext = GetEnumeratorMe.ReturnParameter.ParameterType.GetMethod("MoveNext");
            enumerator = GetEnumeratorMe.Invoke(values, null);
            for (int i = 0; i < count; i++)
            {
                moveNext.Invoke(enumerator, null);
                object v = current.GetValue(enumerator, null);
                // addMe.Invoke(tempList, new object[] { v });
                tempList.Add(v);
            }
            values = ListArrayToJsonObject(tempList, true);

            Dictionary<object, object> dataDic = new Dictionary<object, object>();
            MethodInfo keyMe = keys.GetType().GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo valueMe = values.GetType().GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < count; i++)
            {
                object k = keyMe.Invoke(keys, new object[] { i });
                object v = valueMe.Invoke(values, new object[] { i });
                dataDic.Add(k, v);
            }
            return dataDic;
        }
        /// <summary>
        /// json转换为Dictionary<k,v>
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyType">key的type</param>
        /// <param name="valueType">value的type</param>
        /// <returns></returns>
        public static object JsonToDictionary(string json, Type keyType, Type valueType)
        {
            object obj = SimpleJsonTool.DeserializeObject(json);
            return JsonObjectToDictionary(obj, keyType, valueType);
        }
        public static Dictionary<TKey,TValue> JsonToDictionary<TKey,TValue>(string json )
        {
            object data = SimpleJsonTool.DeserializeObject(json);
            Type keyType = typeof(TKey);
            Type valueType = typeof(TValue);

            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
          
            IList<object> iList = data as IList<object>;
            for (int i = 0; i < iList.Count; i++)
            {
                IDictionary<string, object> iDatasDic = iList[i] as IDictionary<string, object>;
                object key = iDatasDic["Key"];
                object value = iDatasDic["Value"];
                key = ChangeJsonDataByType(keyType, key);
                value = ChangeJsonDataByType(valueType, value);
                //Debug.Log("keyType :" + keyType + "  valueType:" + valueType + "  key:" + key.GetType() + "  value:" + value.GetType());
                dic.Add((TKey)key, (TValue)value);
            }
            return dic;
        }

        private static object JsonObjectToDictionary(object data, Type keyType, Type valueType)
        {
            IList<object> iList = data as IList<object>;
            //Debug.Log(iList.Count);
            Type dicType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            object tempDic = Activator.CreateInstance(dicType);
            MethodInfo addDicMe = dicType.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < iList.Count; i++)
            {
                //Debug.Log(iList[i].GetType().FullName);
                IDictionary<string, object> iDatasDic = iList[i] as IDictionary<string, object>;
                object key = iDatasDic["Key"];
                object value = iDatasDic["Value"];
                key = ChangeJsonDataByType(keyType, key);
                value = ChangeJsonDataByType(valueType, value);
                //Debug.Log("keyType :" + keyType + "  valueType:" + valueType + "  key:" + key.GetType() + "  value:" + value.GetType());
                addDicMe.Invoke(tempDic, new object[] { key,value });            
            }
            return tempDic;
        }
    }
}