using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class ImportTool : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        for (int i = 0; i < importedAssets.Length; i++)
        {
            string expandName = FileTool.GetExpandName(importedAssets[i]);

            if (expandName.ToUpper() == "FBX")
            {
                OnImportFBX(importedAssets[i]);
            }
        }
    }

    #region FBX

    const char c_FBXspltChar = '@';
    const char c_FBXsubSpltChar = '_';

    void OnPostprocessModel(GameObject g)
    {
        //ModelImporter mi = import
    }

    static void OnImportFBX(string assetPath)
    {
        try
        {
            string fileName = FileTool.GetFileNameByPath(assetPath);

            string[] fileSplitTmp = fileName.Split(c_FBXspltChar);
            if (fileSplitTmp.Length > 1 && fileSplitTmp[0] == "FBX")
            {
                //动作
                if (fileSplitTmp.Length >= 3)
                {
                    Debug.Log("FBXImportTool :检测到导入了 " + fileName);

                    //建立文件夹
                    string animPath = "Assets/_Res/Anim/";
                    string modelName = fileSplitTmp[1];
                    string animName = modelName + "_" + FileTool.RemoveExpandName(fileSplitTmp[2]);
                    int animLayer = 0;

                    if (fileSplitTmp.Length > 3)
                    {
                        animLayer = int.Parse(FileTool.RemoveExpandName(fileSplitTmp[3]));
                    }

                    string[] tmp = modelName.Split(c_FBXsubSpltChar);

                    if (tmp.Length > 1)
                    {
                        string subPath = "";
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            subPath += tmp[i] + "/";
                        }

                        animPath += subPath;
                    }

                    animPath += "CTR_" + modelName + ".controller";
                    AnimatorController ac;

                    //创建AnimControl文件
                    if (!File.Exists(animPath))
                    {
                        FileTool.CreatFilePath(animPath);
                        ac = AnimatorController.CreateAnimatorControllerAtPath(animPath);
                    }
                    else
                    {
                        ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(animPath);
                    }

                    while (ac.layers.Length <= animLayer)
                    {
                        ac.AddLayer("Layer " + ac.layers.Length);
                    }

                    //自动绑定动画
                    AnimatorControllerLayer layer = ac.layers[animLayer];
                    AddStateTransition(animName, assetPath, layer);

                }
                //模型
                else
                {

                }
            }
            else
            {
                Debug.LogWarning("FBXImportTool: " + assetPath + " 资源不满足 FBX" + c_FBXspltChar + "模型名(下划线分级)" + c_FBXspltChar + "动作名" + c_FBXspltChar + "分层数(可选)  的格式，不能被自动导入");
            }
        }
        catch(Exception e)
        {
            Debug.LogError("FBXImportTool 导入资源出错 ->" + assetPath + "<- " + e.ToString());
        }
    }

    private static void AddStateTransition(string animName, string path, AnimatorControllerLayer layer)
    {
        AnimatorStateMachine sm = layer.stateMachine;
        //根据动画文件读取它的AnimationClip对象
        AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
        AnimatorState state = GetState(sm, animName);

        if(state == null)
        {
            state = sm.AddState(animName);
        }

        //取出动画名子 添加到state里面
        state.motion = newClip;
        newClip.name = animName;
    }

    static AnimatorState GetState(AnimatorStateMachine sm, string animName)
    {
        for (int i = 0; i < sm.states.Length; i++)
        {
            if (sm.states[i].state.name == animName)
            {
                return sm.states[i].state;
            }
        }

        return null;
    }

    #endregion
}
