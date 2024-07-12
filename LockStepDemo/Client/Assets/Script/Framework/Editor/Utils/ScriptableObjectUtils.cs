using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectUtils  {

	public static T LoadCreateScriptableObject<T>(string assetPath) where T: ScriptableObject
    {
      T  msData = AssetDatabase.LoadAssetAtPath<T>(assetPath);

        if (msData == null)
        {
            msData = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(msData, assetPath);
            AssetDatabase.Refresh();
        }

        return msData;
    }


}
