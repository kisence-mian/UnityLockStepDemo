using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MapGenerateStatus : IApplicationStatus
{
    public const string c_mapDataName = "mapData";
    public const string c_CreatePointDataName = "elementCreatePointData";

    List<Area> list = new List<Area>();
    List<GameObject> list2 = new List<GameObject>();
    public override void OnEnterStatus()
    {

    }

    public override void OnUpdate()
    {
        //CreateMapInfo();
        //ShowInfo();
    }

    public override void OnGUI()
    {
        if(GUILayout.Button("CreateMapInfo"))
        {
            CreateMapInfo();
            CreatePointInfo();
        }

        if (GUILayout.Button("Show"))
        {
            ShowInfo();
        }

        if (GUILayout.Button("TestCollision"))
        {
            TestColl();
        }
    }

    void CreateMapInfo()
    {
        list.Clear();
        GameObject sence = GameObject.Find("Sence");
        MapComponent[] maps = sence.GetComponentsInChildren<MapComponent>();

        string content = "";
        for (int i = 0; i < maps.Length; i++)
        {
            Area tmp = maps[i].GetAreaData();
            list.Add(tmp);
            content += Serializer.Serialize(tmp) + "\n";
        }

        Debug.Log(content);

        ResourceIOTool.WriteStringByFile(Application.dataPath + "/Resources/MapData/" + c_mapDataName+".txt", content);
    }

    void CreatePointInfo()
    {
        GameObject sence = GameObject.Find("Sence");
        ElementCreatePoint[] maps = sence.GetComponentsInChildren<ElementCreatePoint>();

        string content = "";
        for (int i = 0; i < maps.Length; i++)
        {
            ItemCreatePointComponent tmp = maps[i].createPoint;
            tmp.pos.FromVector(maps[i].transform.position);

            content += Serializer.Serialize(tmp) + "\n";
        }

        Debug.Log(content);

        ResourceIOTool.WriteStringByFile(Application.dataPath + "/Resources/MapData/" + c_CreatePointDataName + ".txt", content);
    }

    void ShowInfo()
    {
        for (int i = 0; i < list2.Count; i++)
        {
            GameObjectManager.DestroyGameObjectByPool(list2[i]);
        }
        list2.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            GameObject tip = GameObjectManager.CreateGameObjectByPool("AreaTips");
            CreatMesh RangeTip = tip.GetComponent<CreatMesh>();

            RangeTip.SetArea(list[i],false,null);
            RangeTip.transform.position = list[i].position;
            RangeTip.transform.right = list[i].direction;

            if(list[i].areaType == AreaType.Rectangle)
            {
                Vector3 pos = RangeTip.transform.position;

                pos -= list[i].direction * (list[i].length / 2);

                RangeTip.transform.position = pos;
            }

            list2.Add(tip);
        }
    }

    void TestColl()
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[i].AreaCollideSucceed(list[j]))
                {
                    Debug.Log(i + " " + j + " AreaCollideSucceed");
                }
            }
        }
    }
}
