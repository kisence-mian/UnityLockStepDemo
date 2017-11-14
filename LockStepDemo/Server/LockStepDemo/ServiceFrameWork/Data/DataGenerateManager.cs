using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DataGenerateManager<T> where T : DataGenerateBase, new()
{
    static Dictionary<string, T> s_dict = new Dictionary<string, T>();

    public static T GetData(string key) 
    {
        if (key == null)
        {
            throw new Exception("DataGenerateManager<" + typeof(T).Name + "> GetData key is Null !");
        }

        if (s_dict.ContainsKey(key))
        {
            return s_dict[key];
        }
        else
        {
            T data = new T();
            data.LoadData(key);
            s_dict.Add(key,data);
            return data;
        }
    }

    /// <summary>
    /// 全查表
    /// </summary>
    public static void PreLoad()
    {
        string dataName = typeof(T).Name.Replace("Generate","");

        DataTable data = DataManager.GetData(dataName);
        for (int i = 0; i < data.TableIDs.Count; i++)
        {
            GetData(data.TableIDs[i]);
        }
    }

    public static void CleanCache(params object[] objs)
    {
        s_dict.Clear();
    }
}


public abstract class DataGenerateBase
{
    public virtual void LoadData(string key)
    {

    }
}

