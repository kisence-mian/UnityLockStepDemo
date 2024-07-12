using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class StateTransitionArrowLineDataControl  {

    public static List<StateTransitionArrowLine> allStateTransitionArrowLineList = new List<StateTransitionArrowLine>();

    public static StateTransitionArrowLine AddStateTransitionToCreateArrowLine(StateTransition trans)
    {
        foreach (var item in allStateTransitionArrowLineList)
        {
            if (item.transitions[0].toState == trans.toState && item.transitions[0].fromState == trans.fromState)
            {
                item.transitions.Add(trans);
                return item;
            }
        }
        StateTransitionArrowLine line = ScriptableObject.CreateInstance<StateTransitionArrowLine>();
        line.transitions.Add(trans);
        allStateTransitionArrowLineList.Add(line);
        return line;

    }
    public static bool RemoveStateTransitionToDeleteArrowLine(StateTransition trans)
    {
        foreach (var item in allStateTransitionArrowLineList)
        {
            foreach (var t in item.transitions)
            {
                if (t == trans)
                {
                    if (item.transitions.Count == 1)
                    {
                        allStateTransitionArrowLineList.Remove(item);
                        Object. DestroyImmediate(item);
                        return true;
                    }
                    else
                    {
                        item.transitions.Remove(trans);
                        return true;
                    }
                }
            }

        }
        return false;
    }
    public static bool DeleteStateTransitionArrowLine(StateTransitionArrowLine line)
    {
        if (allStateTransitionArrowLineList.Contains(line))
        {
            foreach (var item in line.transitions)
            {
                StateTransitionDataControl.DeleteStateTransition(item);
            }
            allStateTransitionArrowLineList.Remove(line);
            Object.DestroyImmediate(line);
            return true;
        }
        return false;
    }

    public static StateTransitionArrowLine[] GetMachineStateToOtherStateArrowLine(MachineState state)
    {
        List<StateTransitionArrowLine> list = new List<StateTransitionArrowLine>();
        foreach (var item in allStateTransitionArrowLineList)
        {
            if (item.transitions[0].fromState == state)
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }
    public static StateTransitionArrowLine[] GetMachineStateFromOtherStateArrowLine(MachineState state)
    {
        List<StateTransitionArrowLine> list = new List<StateTransitionArrowLine>();
        foreach (var item in allStateTransitionArrowLineList)
        {
            if (item.transitions[0].toState == state)
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public static Dictionary<StateTransitionArrowLine, StateTransitionArrowLine> StateTransitionArrowLinesToGrouping()
    {
        Dictionary<StateTransitionArrowLine, StateTransitionArrowLine> dic = new Dictionary<StateTransitionArrowLine, StateTransitionArrowLine>();
        List<StateTransitionArrowLine> linesList = new List<StateTransitionArrowLine>(allStateTransitionArrowLineList);

        StateTransitionArrowLine tempOne = null;
        StateTransitionArrowLine tempTwo = null;

        //dic.Add(temp,null);
        while (linesList.Count > 0)
        {
            tempOne = linesList[0];
            linesList.RemoveAt(0);
            tempTwo = null;
            for (int i = 0; i < linesList.Count; i++)
            {
                StateTransitionArrowLine s = linesList[i];
                if (s.transitions[0].fromState == tempOne.transitions[0].toState && s.transitions[0].toState == tempOne.transitions[0].fromState)
                {
                    tempTwo = s;
                    break;
                }
            }
            if (tempTwo != null)
                linesList.Remove(tempTwo);
            dic.Add(tempOne, tempTwo);
        }

        return dic;
    }

    public static StateTransitionArrowLine FindClosestStateTransitionArrowLine(Vector2 mousePosition)
    {
       Vector2 v2 =  StateMachineUtils.MousePos2MachineGridPos(mousePosition);

        StateTransitionArrowLine result = null;
        float lenth = float.PositiveInfinity;
        foreach (var item in allStateTransitionArrowLineList)
        {
            float temoLenth;
            if (item.IsLineSelf)
            {
                temoLenth= Vector2.Distance(item.LineSelfOneTrianglePoints[3], v2);
            }
            else
            {
                temoLenth = HandleUtility.DistancePointLine(v2, item.UseFormLinePosition, item.UseToLinePosition);
            }

            if(temoLenth<lenth && temoLenth < 10)
            {
                lenth = temoLenth;
                result = item;
            }
        }

        return result;
    }
}
