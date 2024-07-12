using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

public enum MachineStateUIStateEnum
{
    Normal,
    select,
    Running,
}
public static class StateTransitionGUI  {
    public static float lineSize = 4f;
    public static float triangleSize = 12f;

    public static void OnGUI()
    { 
        Dictionary<StateTransitionArrowLine, StateTransitionArrowLine> dic = StateTransitionArrowLineDataControl.StateTransitionArrowLinesToGrouping();
        //Debug.Log(dic.Count);
        foreach (var item in dic.Keys)
        {
            if (item.IsLineSelf)
            {
                DrawLine(item, 0);
            }
            else
            {
                DrawLine(item, 0);
                //Debug.Log(" dic:" + item.FormLinePosition[1] + " ||" + item.ToLinePosition[1]);
                if (dic[item] != null)
                {
                    //Debug.LogWarning("   dic[item]:" + dic[item].FormLinePosition[1] + " ||" + dic[item].ToLinePosition[1]);
                    DrawLine(dic[item], 1);
                }
            }

        }


        //Debug.Log(state.showContent.text + ":" + state.stateTransitions.Count);
        //  StateTransitionArrowLine[] lines = StateTransitionArrowLine.CreateStateTransitionArrowLines(state.stateTransitions);
        ////  if (state.stateTransitions.Count > 0)
        //      //Debug.Log(state.showContent.text+"  lines: " + lines.Length);

        //  for (int i = 0; i < lines.Length; i++)
        //  {
        //      StateTransitionArrowLine line = lines[i];
        //      if (line.transitions.Count > 1)
        //      {
        //          DrawArrowCenterThreeTriangle(line.FormLinePosition, line.ToLinePosition, lineSize, triangleSize, line.color);
        //      }
        //      else
        //      {
        //          DrawArrowCenterOneTriangle(line.FormLinePosition, line.ToLinePosition, lineSize, triangleSize, line.color);
        //      }
        //Texture2D tex = (Texture2D)Styles.connectionTexture.image;
        //      //DrawEdge(line.FormLinePosition, line.ToLinePosition, tex, line.color, EdgeGUI.EdgeStyle.Curvy);
        //  }
        //DrawArrowCenter(st.fromState.position, st.toState.position, 12, Color.white);

    }

    private static float lenthBetweenLine = 5;
    public static Vector2 GetMachineStateFromPosition(MachineState state)
    {
        Rect fromStateRect = MachineStateGUIDataControl.GetMachineStateGUIRect(state);
        Vector2 pos = fromStateRect.center;
        pos.x -= lenthBetweenLine;
        pos.y -= lenthBetweenLine;
        return pos;
    }
    public static Vector2 GetMachineStateToPosition(MachineState state)
    {
        Rect fromStateRect = MachineStateGUIDataControl.GetMachineStateGUIRect(state);
        Vector2 pos = fromStateRect.center;
        pos.x += lenthBetweenLine;
        pos.y += lenthBetweenLine;
        return pos;
    }

    public static void DrawLine(StateTransitionArrowLine line,int posNum)
    {
        if (line.IsLineSelf)
        {
            //Debug.Log("line.IsLineSelf:" + line.IsLineSelf + "  " + line.transitions[0].fromState.showContent.text);
            DrawLineSelf(line);
        }
        else
        {
            if (line.transitions.Count > 1)
            {
                DrawArrowCenterThreeTriangle(line.GetUseFormLinePosition(posNum), line.GetUseToLinePosition(posNum), lineSize, triangleSize, line.color);
            }
            else
            {
                DrawArrowCenterOneTriangle(line.GetUseFormLinePosition(posNum), line.GetUseToLinePosition(posNum), lineSize, triangleSize, line.color);
            }
        }
    }

    public static void DrawArrowCenterOneTriangle(Vector2 p0, Vector2 p1,float lineSize, float triangleSize, Color lineColor)
    {
        Handles.color = lineColor;

        Vector2 direction = (p1 - p0).normalized;
        Vector2 verticalDire = MathUtils.VerticalVector2(direction)[0];
        Vector2 center = (p1 - p0) / 2f + p0;
        Vector2 t0 = center + direction * triangleSize/2;
        Vector2 t1 = center + verticalDire * triangleSize / 2;
        Vector2 t2 = center - verticalDire * triangleSize / 2;
        Handles.DrawAAPolyLine(lineSize, p0, p1);
        Handles.DrawAAConvexPolygon(t0, t1, t2);
        StateMachineEditorGUI. HandleColorChangeEnd();
    }
    public static void DrawArrowCenterThreeTriangle(Vector2 p0, Vector2 p1, float lineSize, float triangleSize, Color lineColor)
    {
        Handles.color = lineColor;

        Vector2 direction = (p1 - p0).normalized;
        Vector2 verticalDire = MathUtils.VerticalVector2(direction)[0];
        Vector2 center = (p1 - p0) / 2f + p0;
        Vector2 moveP = direction * triangleSize;
        Vector2 t0 = center + moveP;
        Vector2 t1 = center + verticalDire * triangleSize / 2;
        Vector2 t2 = center - verticalDire * triangleSize / 2;
        Handles.DrawAAPolyLine(lineSize, p0, p1);
        Handles.DrawAAConvexPolygon(t0, t1, t2);
        Handles.DrawAAConvexPolygon(t0 + moveP, t1 + moveP, t2 + moveP);
        Handles.DrawAAConvexPolygon(t0 - moveP, t1 - moveP, t2 - moveP);
        StateMachineEditorGUI.HandleColorChangeEnd();
    }

    public static void DrawLineSelf(StateTransitionArrowLine line)
    {
        if (line.transitions.Count > 1)
        {
            DrawOneTriangleDowm(line.LineSelfOneTrianglePoints, line.color);
            Vector2[] arrPos0 = new Vector2[3];
            arrPos0[0] = line.LineSelfOneTrianglePoints[0] + new Vector2(triangleSize,0);
            arrPos0[1] = line.LineSelfOneTrianglePoints[1] + new Vector2(triangleSize, 0);
            arrPos0[2] = line.LineSelfOneTrianglePoints[2] + new Vector2(triangleSize, 0);
            DrawOneTriangleDowm(arrPos0, line.color);
            Vector2[] arrPos1 = new Vector2[3];
            arrPos1[0] = line.LineSelfOneTrianglePoints[0] - new Vector2(triangleSize, 0);
            arrPos1[1] = line.LineSelfOneTrianglePoints[1] - new Vector2(triangleSize, 0);
            arrPos1[2] = line.LineSelfOneTrianglePoints[2] - new Vector2(triangleSize, 0);
            DrawOneTriangleDowm(arrPos1, line.color);
        }
        else
        {
            DrawOneTriangleDowm(line.LineSelfOneTrianglePoints, line.color);
        }
    }

    private static void DrawOneTriangleDowm(Vector2[] posArr, Color lineColor)
    {
        Handles.color = lineColor;
        Handles.DrawAAConvexPolygon(posArr[0],posArr[1],posArr[2]);
        StateMachineEditorGUI.HandleColorChangeEnd();
    }

    public static bool startNewTranstion = false;
    public static MachineState startPositionMs;
    public static Vector2 toPosition;

    public static void DrawTempArrowTransition()
    {
        if (startNewTranstion)
        {
            Vector2 startPos = GetMachineStateFromPosition(startPositionMs);
            DrawArrowCenterOneTriangle(startPos, toPosition,lineSize, triangleSize, Color.white);
        }
    }

    private static void DrawEdge(Vector2 start, Vector2 end, Texture2D tex, Color color, EdgeGUI.EdgeStyle style)
    {
        if (style != EdgeGUI.EdgeStyle.Angular)
        {
            if (style == EdgeGUI.EdgeStyle.Curvy)
            {
                Vector3[] array;
                Vector3[] array2;
                GetCurvyConnectorValues(start, end, out array, out array2);
                Handles.DrawBezier(array[0], array[1], array2[0], array2[1], color, tex, 3f);
            }
        }
        else
        {
            Vector3[] array;
            Vector3[] array2;
            GetAngularConnectorValues(start, end, out array, out array2);
            DrawRoundedPolyLine(array, array2, tex, color);
        }
    }

    private static void GetCurvyConnectorValues(Vector2 start, Vector2 end, out Vector3[] points, out Vector3[] tangents)
    {
        points = new Vector3[]
        {
                start,
                end
        };
        tangents = new Vector3[2];
        //float arg_57_0 = (start.y >= end.y) ? 0.7f : 0.3f;
        float num = 0.5f;
        float num2 = 1f - num;
        float num3 = 0f;
        if (start.x > end.x)
        {
            num = (num2 = -0.25f);
            float num4 = (start.x - end.x) / (start.y - end.y);
            if (Mathf.Abs(num4) > 0.5f)
            {
                float num5 = (Mathf.Abs(num4) - 0.5f) / 8f;
                num5 = Mathf.Sqrt(num5);
                num3 = Mathf.Min(num5 * 80f, 80f);
                if (start.y > end.y)
                {
                    num3 = -num3;
                }
            }
        }
        float num6 = Mathf.Clamp01(((start - end).magnitude - 10f) / 50f);
        tangents[0] = start + new Vector2((end.x - start.x) * num + 30f, num3) * num6;
        tangents[1] = end + new Vector2((end.x - start.x) * -num2 - 30f, -num3) * num6;
    }

    private static void GetAngularConnectorValues(Vector2 start, Vector2 end, out Vector3[] points, out Vector3[] tangents)
    {
        Vector2 vector = start - end;
        Vector2 vector2 = vector / 2f + end;
        Vector2 vector3 = new Vector2(Mathf.Sign(vector.x), Mathf.Sign(vector.y));
        Vector2 vector4 = new Vector2(Mathf.Min(Mathf.Abs(vector.x / 2f), 5f), Mathf.Min(Mathf.Abs(vector.y / 2f), 5f));
        points = new Vector3[]
        {
                start,
                new Vector3(vector2.x + vector4.x * vector3.x, start.y),
                new Vector3(vector2.x, start.y - vector4.y * vector3.y),
                new Vector3(vector2.x, end.y + vector4.y * vector3.y),
                new Vector3(vector2.x - vector4.x * vector3.x, end.y),
                end
        };
        tangents = new Vector3[]
        {
                (points[1] - points[0]).normalized * vector4.x * 0.6f + points[1],
                (points[2] - points[3]).normalized * vector4.y * 0.6f + points[2],
                (points[3] - points[2]).normalized * vector4.y * 0.6f + points[3],
                (points[4] - points[5]).normalized * vector4.x * 0.6f + points[4]
        };
    }

    private static void DrawRoundedPolyLine(Vector3[] points, Vector3[] tangets, Texture2D tex, Color color)
    {
        Handles.color=(color);
        for (int i = 0; i < points.Length; i += 2)
        {
            Handles.DrawAAPolyLine(tex, 3f, new Vector3[]
            {
                    points[i],
                    points[i + 1]
            });
        }
        for (int j = 0; j < tangets.Length; j += 2)
        {
            Handles.DrawBezier(points[j + 1], points[j + 2], tangets[j], tangets[j + 1], color, tex, 3f);
        }
    }
}
