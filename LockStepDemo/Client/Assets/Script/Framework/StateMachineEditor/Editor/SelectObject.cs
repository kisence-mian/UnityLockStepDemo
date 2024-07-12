using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SelectObject  {

    private static object selectObject;
    public static bool IsSelectThis(object item)
    {
        if (selectObject == null)
            return false;
        return item.Equals(selectObject);
    }

    public static void SelectItemObject(object item)
    {
        SelectObjectCancel();
        selectObject = item;
        if (selectObject is MachineStateGUI)
        {
            ((MachineStateGUI)selectObject).uiState = MachineStateUIStateEnum.select;
            Selection.activeObject = ((MachineStateGUI)selectObject);
        }
        else if (selectObject is StateTransitionArrowLine)
        {
            ((StateTransitionArrowLine)selectObject).uiState = MachineStateUIStateEnum.select;
            Selection.activeObject = ((StateTransitionArrowLine)selectObject);
        }
    }
    public static void SelectObjectCancel()
    {
        if (selectObject != null)
        {
            if (selectObject is MachineStateGUI)
            {
                ((MachineStateGUI)selectObject).uiState = MachineStateUIStateEnum.Normal;
            }
            else if (selectObject is StateTransitionArrowLine)
            {
                ((StateTransitionArrowLine)selectObject).uiState = MachineStateUIStateEnum.Normal;
            }
        }
        selectObject = null;
    }

    public static void DeleteSelectObjet()
    {
        if (selectObject != null)
        {
            if (selectObject is MachineStateGUI)
            {
                MachineDataController.DeleteMachineStateGUI(((MachineStateGUI)selectObject));
            }
            else if (selectObject is StateTransitionArrowLine)
            {
                MachineDataController.DeleteStateTransitionArrowLine(((StateTransitionArrowLine)selectObject));
            }
        }
    }

}
