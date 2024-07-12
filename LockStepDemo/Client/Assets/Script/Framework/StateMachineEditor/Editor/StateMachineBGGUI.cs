using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

public static class StateMachineBGGUI  {
    public static Rect bgRect;
    public const float extraLenth = 400f;
    public static float widthMin;
    public static float heightMin;
    public static float widthNow;
    public static float heightNow;

    public static float xMin = 0;
    public static float xMax =0;
    public static float yMin =0;
    public static float yMax =0;
    public static Rect controlWindowRange;
    private static Vector2 pos;
    public static void BeginGUI(Rect controlRange, Rect nodeMaxRange)
    {
        controlWindowRange = controlRange;
        ComputebgRect( nodeMaxRange);
        GUILayout.BeginArea(controlRange, Styles.graphBackground);
        //Rect res = new Rect(0, 0, controlRange.width, controlRange.height);
        GUILayout.BeginArea(bgRect, Styles.graphBackground);
     //   pos= GUILayout.BeginScrollView(pos, true, true);
        DrawBackGroundGrid();
    }
    public static void EndGUI()
    {
       // GUILayout.EndScrollView();
        GUILayout.EndArea();
        GUILayout.EndArea();
    }

    private static void ComputebgRect(Rect nodeMaxRange)
    {
        // widthMin = controlWindowRange.width + extraLenth;
        // heightMin = controlWindowRange.height + extraLenth;
        //widthNow =Mathf.Max(nodeMaxRange.width + extraLenth, widthMin);
        //heightNow =Mathf.Max( nodeMaxRange.height + extraLenth , heightMin)  ;
        //if (bgRect == null)
        //{
        //    float x = controlWindowRange.x +(widthNow - controlWindowRange.width) / -2f;
        //    float y = controlWindowRange.y+ (heightNow - controlWindowRange.height) / -2f;
        //    Vector2 v2 = new Vector2(x, y);
        //    bgRect = new Rect(v2, new Vector2(widthNow, heightNow));
        //}
        //else
        //{
        //    Vector2 v2 = bgRect.position + Control();
        //    float xC = (widthNow - controlWindowRange.width) / 2f;
        //    float yC = (heightNow - controlWindowRange.height) / 2f;
        //    v2.x = Mathf.Clamp(v2.x, -xC, 0);
        //    v2.y = Mathf.Clamp(v2.y, -yC, 0);
        //    bgRect = new Rect(v2, new Vector2(widthNow, heightNow));
        //}
        xMin = bgRect.xMin;
        xMax = bgRect.xMax;
        yMin = bgRect.yMin;
        yMax = bgRect.yMax;
        float tempxMin = -extraLenth / 2;
        xMin = Mathf.Min(xMin, Mathf.Min(tempxMin, nodeMaxRange.xMin - extraLenth));
        float tempxMax = xMin + controlWindowRange.width + extraLenth ; ;
        xMax = Mathf.Max(xMax, Mathf.Max(tempxMax, nodeMaxRange.xMax+ extraLenth));
        float tempyMin = -extraLenth / 2;
        yMin = Mathf.Min(yMin, Mathf.Min(tempyMin, nodeMaxRange.yMin - extraLenth));
        float tempyMax = yMin + controlWindowRange.height + extraLenth; ;
        yMax = Mathf.Max(yMax, Mathf.Max(tempyMax, nodeMaxRange.yMax+ extraLenth));
        bgRect.xMin = xMin;
        bgRect.xMax = xMax;
        bgRect.yMin = yMin;
        bgRect.yMax = yMax;

        Vector2 v2 = bgRect.position + Control();
        float xC = (bgRect.width - controlWindowRange.width) /2;
        float yC = (bgRect.height - controlWindowRange.height) /2;
        v2.x = Mathf.Clamp(v2.x, -xC, 0);
        v2.y = Mathf.Clamp(v2.y, -yC, 0);
        bgRect.position = v2;
    }
    private static Vector2 Control()
    {
        Vector2 v2 = Vector2.zero;
        Event e = Event.current;
        if (e.isMouse)
        {
            if(e.button ==2 && e.type == EventType.mouseDrag && controlWindowRange.Contains(e.mousePosition))
            {
                v2 = e.delta;
                e.Use();
            }
        }
        return v2;
    }
    public static void DrawBackGroundGrid()
    {
        DrawGridLines(bgRect, 10f, new Color(0.4f, 0.4f, 0.4f, 1));
        DrawGridLines(bgRect, 100f, Color.black);

        StateMachineEditorGUI.HandleColorChangeEnd();
    }

    public static void DrawGridLines(Rect range, float gridSize, Color gridColor)
    {
        Handles.color = gridColor;
        float xMin = range.x;
        float xMax = range.x+range.width;
        float yMin = range.y;
        float yMax = range.y+range.height;
        int wNum = (int)(range.width / gridSize);
        int hNum = (int)(range.height / gridSize);
        for (int i = 0; i < wNum; i++)
        {
            Handles.DrawLine(new Vector2(i* gridSize, yMin), new Vector2(i*gridSize, yMax));
        }
        for (int i = 0; i < hNum; i++)
        {
            Handles.DrawLine(new Vector2(xMin, i*gridSize), new Vector2(xMax, i*gridSize));
        }
       
    }

  
}
