using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransitionArrowLine : ScriptableObject
{
   

    public List<StateTransition> transitions = new List<StateTransition>();
    public Vector2 UseFormLinePosition;
    public Vector2 GetUseFormLinePosition(int num)
    {
        UseFormLinePosition = FormLinePosition[num];
        return UseFormLinePosition;
    }
    public Vector2[] FormLinePosition
    {
        get
        {
            Vector2[] v2 =new Vector2[2];
            if (transitions.Count > 0)
            {
                v2[0] = StateTransitionGUI.GetMachineStateFromPosition(transitions[0].fromState);
                v2[1] = StateTransitionGUI.GetMachineStateToPosition(transitions[0].fromState);
            }
            return v2;
        }
    }
    public Vector2 UseToLinePosition;
    public Vector2 GetUseToLinePosition(int num)
    {
        UseToLinePosition = ToLinePosition[num];
        return UseToLinePosition;
    }
    public Vector2[] ToLinePosition
    {
        get
        {
            Vector2[] v2 = new Vector2[2];
            if (transitions.Count > 0)
            {
                v2[0] = StateTransitionGUI.GetMachineStateFromPosition(transitions[0].toState);
                v2[1] = StateTransitionGUI.GetMachineStateToPosition(transitions[0].toState);
            }
            return v2;
          
        }
    }
    public MachineStateUIStateEnum uiState = MachineStateUIStateEnum.Normal;

    public bool IsLineSelf {

        get
        {
            if (transitions.Count > 0)
            {
               return transitions[0].fromState == transitions[0].toState;
            }
            return false;
        }
    }
    public Vector2[] LineSelfOneTrianglePoints
    {
        get
        {
           Rect rect = MachineStateGUIDataControl.GetMachineStateGUIRect(transitions[0].fromState);
            List<Vector2> listPoints = new List<Vector2>();
            Vector2 bottomCenterPos = rect.position + new Vector2(rect.width / 2f, rect.height);
            Vector2 temp = new Vector2(bottomCenterPos.x, bottomCenterPos.y + StateTransitionGUI.triangleSize);
            Vector2 pos0 = temp + new Vector2(StateTransitionGUI.triangleSize / 2f, 0);
            Vector2 pos1 = temp - new Vector2(StateTransitionGUI.triangleSize / 2f, 0);
            listPoints.Add(bottomCenterPos);
            listPoints.Add(pos0);
            listPoints.Add(pos1);
           // temp.y = temp.y / 2;//三角形中心点
            listPoints.Add(temp);
            return listPoints.ToArray();
        }
    }

    public Color color
    {
        get
        {
            return StateMachineUtils.GetColorByState(uiState);
        }
    }
    /// <summary>
    /// 分类，相同的分到一个StateTransitionArrowLine里
    /// </summary>
    /// <param name="transitions"></param>
    /// <returns></returns>
    public static StateTransitionArrowLine[] CreateStateTransitionArrowLines(List<StateTransition> transitions)
    {
        List<StateTransition> tempLists = new List<StateTransition>(transitions);
        List<StateTransitionArrowLine> list = new List<StateTransitionArrowLine>();
        StateTransition tempT = null;
        while (tempLists.Count>0)
        {
            StateTransitionArrowLine line = ScriptableObject.CreateInstance<StateTransitionArrowLine>();
            tempT = tempLists[0];
            tempLists.RemoveAt(0);
           
            for (int i = 0; i < tempLists.Count; i++)
            {
                StateTransition t = tempLists[i];
                if(t.fromState == tempT.fromState && t.toState == tempT.toState)
                {
                    line.transitions.Add(t);
                }
            }
            for (int i = 0; i < line.transitions.Count; i++)
            {
                tempLists.Remove(line.transitions[i]);
            }
            line.transitions.Add(tempT);

            list.Add(line);
        }

        return list.ToArray();
    }

   

}
