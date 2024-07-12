using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherUtils  {

    /// <summary>
    /// 根据不同平台返回文件夹名字
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static string GetPlatformFolder(RuntimePlatform target)
    {
        switch (target)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "IOS";
            case RuntimePlatform.WebGLPlayer:
                return "WebGLPlayer";
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            default:
                return null;
        }
    }

    public static string GetWWWLoadPath(string path)
    {
#if UNITY_EDITOR
        path = @"file:///" + path;
#elif UNITY_IOS
        path = @"file://" + path;
#else
        
#endif
        return path;
    }

    public static bool ArrayContains<T>(T[] array,T t)
    {
        for(int i =0;i< array.Length; i++)
        {
            if (array[i].Equals( t))
                return true;
        }
        return false;
    }
    /// <summary>
    /// 创建GameObject并附上脚本
    /// </summary>
    /// <typeparam name="T">脚本</typeparam>
    /// <param name="ObejectName">GameObject名字</param>
    /// <param name="DontDestroyOnLoad">是否切换场景时不摧毁</param>
    /// <param name="isFindFirst">是否先寻找是否存在同名物体</param>
    /// <returns></returns>
    public static T CreateMonoBehaviourBase<T>(string ObejectName="",bool DontDestroyOnLoad =true, bool isFindFirst = false) where T : MonoBehaviour
    {
        if (string.IsNullOrEmpty(ObejectName))
            ObejectName = "[" + typeof(T).Name + "]";
        GameObject obj = null;
        if (isFindFirst)
        {
            obj = GameObject.Find(ObejectName);
        }
        if(obj==null)
          obj = new GameObject(ObejectName);

        if (DontDestroyOnLoad && Application.isPlaying)
            UnityEngine.Object.DontDestroyOnLoad(obj);
        T t = obj.GetComponent<T>();
        if (t == null)
            t = obj.AddComponent<T>();
        return t;
    }

  
  
}
