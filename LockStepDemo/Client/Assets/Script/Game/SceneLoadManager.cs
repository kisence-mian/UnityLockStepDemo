using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SceneLoadManager  
{
    //static List<ScenceComp> s_sceneList = new List<ScenceComp>();

    static GameObject s_scenceParent;

    public static GameObject ScenceParent
    {
        get {
            if (s_scenceParent == null)
            {
                s_scenceParent = GameObject.Find("Scene");

                if (s_scenceParent == null)
                    s_scenceParent = new GameObject("Scene");

                //MonoBehaviour.DontDestroyOnLoad(s_scenceParent);
            }

            return s_scenceParent; 
        }
        //set { SceneLoadManager.s_scenceParent = value; }
    }

    public static void LoadScence(string sceneName)
    {
        //SceneData data = SceneDataManager.GetData(sceneName);

        //for (int i = 0; i < data.m_sceneList.Count; i++)
        //{
        //    GameObject map = GameObjectManager.CreateGameObjectByPool(data.m_sceneList[i].m_mapName, ScenceParent);
        //    ScenceComp comp = map.GetComponent<ScenceComp>();
        //    comp.Init(data.m_sceneList[i]);
        //    s_sceneList.Add(comp);
        //}

        //if (data.m_style != SceneStyleEnum.Temple)
        //{
        //    //填充边界
        //}
        //GlobalEvent.DispatchEvent(GlobalGameEventEnum.Map_Init);
    }

    public static void CleanScene()
    {
        ////s_sceneList.Clear();
        //for (int i = 0; i < s_sceneList.Count; i++)
        //{
        //    GameObjectManager.DestroyGameObjectByPool(s_sceneList[i].gameObject);
        //}
        //s_sceneList.Clear();
    }

    public static void LoadScenceByEditor(string sceneName)
    {

        //SceneData data = SceneDataManager.GetData(sceneName);

        //for (int i = 0; i < data.m_sceneList.Count; i++)
        //{
        //    GameObject map = GameObjectManager.CreateGameObject(data.m_sceneList[i].m_mapName, ScenceParent);
        //    ScenceComp comp = map.GetComponent<ScenceComp>();
        //    comp.Init(data.m_sceneList[i]);
        //    s_sceneList.Add(comp);
        //}

        //if (data.m_style != SceneStyleEnum.Temple)
        //{
        //    //填充边界
        //}
    }

    public static void CleanSceneByEditor()
    {
        //s_sceneList.Clear();
        //for (int i = 0; i < s_sceneList.Count; i++)
        //{
        //    GameObjectManager.DestroyImmediate(s_sceneList[i].gameObject);
        //}
        //s_sceneList.Clear();
    }

    public static void SetCameraPos(Vector3 pos)
    {
        //pos.y = 0;

        //for (int i = 0; i < s_sceneList.Count; i++)
        //{
        //    if (Vector3.Distance(s_sceneList[i].transform.position,pos) < 200)
        //    {
        //        if(!s_sceneList[i].gameObject.activeSelf)
        //        {
        //            s_sceneList[i].gameObject.SetActive(true);
        //        }
        //    }
        //    else
        //    {
        //        if (s_sceneList[i].gameObject.activeSelf)
        //        {
        //            s_sceneList[i].gameObject.SetActive(false);
        //        }
        //    }
        //}
    }
}
