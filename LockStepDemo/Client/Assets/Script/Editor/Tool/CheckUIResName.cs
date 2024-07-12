using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HDJ.Framework.Utils;
using System.IO;

public class CheckUIResName  {
    const string UIResPath = "Assets/_Res/UI";
    const string preName = "UI_";

	[MenuItem("Tool/检查UI资源名字前缀")]
    private static void CheckUIName()
    {
      string[] paths=  PathUtils.GetDirectoryFilePath(UIResPath, new string[] { ".png", ".PNG" });
        foreach (var path in paths)
        {
            string tempName = Path.GetFileNameWithoutExtension(path);
            if (tempName.Length > 3)
            {
                string temp0 = tempName.Substring(0, 3);
                if (temp0.Equals(preName)) continue;
            }

            tempName = preName + tempName;
            AssetDatabase.RenameAsset(path, tempName);
        }
        AssetDatabase.Refresh();
    }

}
