using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MachineStateInputEventController {

    public static void PlayerControlUse()
    {
        MouseBGEmptyRightClikEvent();
        OnMachineStateMouseRightClickMenu();
        OnEndNewTransitionEvent();
        OnBeginDragStateMachine();
        OnEndDragStateMachine();
        KeyboardDeleteItem();
        MouseSelectObject();

        BeginMouseChangeLeftAreaSizeDrag();
        EndMouseChangeLeftAreaSizeDrag();
    }

    private static bool beginMouseChangeLeftAreaSizeDrag = false;
    private static void BeginMouseChangeLeftAreaSizeDrag()
    {
        if (beginMouseChangeLeftAreaSizeDrag)
            return;
        Event e = Event.current;
        if (e.button == 0 && e.type == EventType.MouseDrag)
        {
            Vector2 posTop = new Vector2(StateMachineEditorWindow.Instance.LeftToolAreaRect_with, 0);
            Vector2 posBotton = new Vector2(StateMachineEditorWindow.Instance.LeftToolAreaRect_with, StateMachineEditorWindow.Instance.leftToolAreaRect.height);

            float distance = HandleUtility.DistancePointLine(e.mousePosition, posTop, posBotton);
            if (distance < 15f)
            {
                StateMachineEditorWindow.Instance.wantsMouseMove = true;
                beginMouseChangeLeftAreaSizeDrag = true;
            }
        }

    } 
    private static void EndMouseChangeLeftAreaSizeDrag()
    {
        if (beginMouseChangeLeftAreaSizeDrag)
        {
            Event e = Event.current;

            if (e.button == 0 && e.type == EventType.mouseDrag)
            {
                StateMachineEditorWindow.Instance.LeftToolAreaRect_with =e.mousePosition.x;
                e.Use();
            }
            else
            {
                StateMachineEditorWindow.Instance.wantsMouseMove = false;
                beginMouseChangeLeftAreaSizeDrag = false;
            }
        }
            
    }
   private static void MouseSelectObject()
    {
        Event e = Event.current;
        if (e.button == 0 && e.type == EventType.MouseDown)
        {
            if (StateMachineBGGUI.controlWindowRange.Contains(e.mousePosition))
            {
                bool isSelect = false;
                foreach (var item in MachineStateGUIDataControl.allMachineStateGUI)
                {
                    if (StateMachineUtils.MachineGridRectContainsMousePos(e.mousePosition, item.GUIRect))
                    {
                       
                        isSelect = true;
                        SelectObject.SelectItemObject (item);
                        break;
                    }
                }
                if (!isSelect)
                {
                    StateTransitionArrowLine line = StateTransitionArrowLineDataControl.FindClosestStateTransitionArrowLine(e.mousePosition);
                    if (line)
                    {
                        isSelect = true;
                        SelectObject.SelectItemObject(line);
                    }
                }
                if (isSelect == false)
                {
                    SelectObject. SelectObjectCancel();
                }
                e.Use();
            }
        }
    }


    private static void KeyboardDeleteItem()
    {
        Event e = Event.current;
        if (e.keyCode == KeyCode.Delete && e.type == EventType.KeyDown )
        {
            SelectObject. DeleteSelectObjet();
            e.Use();
        }
    }

    private static bool isDragingStateItem = false;
    private static MachineStateGUI dragItem = null;
    private static void OnBeginDragStateMachine()
    {
        if (isDragingStateItem)
            return;
        Event e = Event.current;

        if (e.button==0 && e.type == EventType.mouseDrag)
        {
            foreach (var item in MachineStateGUIDataControl.allMachineStateGUI)
            {
                if (StateMachineUtils.MachineGridRectContainsMousePos(e.mousePosition, item.GUIRect))
                {
                    dragItem = item;
                    isDragingStateItem = true;
                    StateMachineEditorWindow.Instance.wantsMouseMove = true;
                    break;
                    }
                }
        }
    }
    private static void OnEndDragStateMachine()
    {
        if (isDragingStateItem)
        {
            Event e = Event.current;

            if (e.button == 0 && e.type == EventType.mouseDrag)
            {
                Vector2 pos = StateMachineUtils.MousePos2MachineGridPos(e.mousePosition);
                pos = pos - MachineStateGUI.size / 2f;
                dragItem.position = pos;
                e.Use();
            }
            else
            {
                isDragingStateItem = false;
                dragItem = null;
                StateMachineEditorWindow.Instance.wantsMouseMove = false;
            }

        }
    }

    /// <summary>
    /// 鼠标右键空白区域
    /// </summary>
    public static void MouseBGEmptyRightClikEvent()
    {
        Event e = Event.current;

        if (e.button==1&& e.type == EventType.ContextClick)
        {
            if (StateMachineBGGUI.controlWindowRange.Contains(e.mousePosition))
            {
                bool isInUIPart = false;
                foreach (var item in MachineStateGUIDataControl.allMachineStateGUI)
                {
                    if (StateMachineUtils.MachineGridRectContainsMousePos(e.mousePosition, item.GUIRect))
                    {
                        isInUIPart = true;
                        break;
                    }
                }

                if (!isInUIPart)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("New State"), false, AddNewMachineStateGUIOnMouseMenu, e.mousePosition);
                    menu.AddSeparator("");
                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }
    }
    private static void AddNewMachineStateGUIOnMouseMenu(object mousePos)
    {
        Vector2 mPos = (Vector2)mousePos;
        mPos = StateMachineUtils.MousePos2MachineGridPos(mPos);
        MachineDataController. AddNewMachineStateGUI(mPos);
        Debug.Log("AddNewMachineStateGUIOnMouseMenu :" + mPos);
    }
    private static void NewTransitionStart(object obj)
    {
        StateTransitionGUI.startNewTranstion = true;
        MachineState ms = (MachineState)obj;
        StateTransitionGUI.startPositionMs = ms;
        StateMachineEditorWindow.Instance.wantsMouseMove = true;
    }
    public static void OnMachineStateMouseRightClickMenu()
    {
        if (StateTransitionGUI.startNewTranstion)
            return;
        Event e = Event.current;
        if (e.button ==1&& e.type == EventType.ContextClick)
        {
            foreach (var item in MachineStateGUIDataControl.allMachineStateGUI)
            {
                if (StateMachineUtils.MachineGridRectContainsMousePos(e.mousePosition, item.GUIRect))
                {
                    if (!SelectObject.IsSelectThis(item))
                    {
                        SelectObject.SelectItemObject(item);
                    }
                    else
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("New Transition"), false, NewTransitionStart, item.state);
                        menu.AddItem(new GUIContent("Delete"), false, SelectObject.DeleteSelectObjet);
                        menu.AddSeparator("");
                        menu.ShowAsContext();
                    }
                    e.Use();
                    break;
                }
            }
        }
    }
   private static void OnEndNewTransitionEvent()
    {
        if (StateTransitionGUI.startNewTranstion)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseMove)
            {
                StateTransitionGUI.toPosition = StateMachineUtils.MousePos2MachineGridPos(e.mousePosition);
                e.Use();
            }
            if (e.isMouse)
            {
                if (e.button == 0)
                {
                    foreach (var item in MachineStateGUIDataControl.allMachineStateGUI)
                    {
                        if (StateMachineUtils.MachineGridRectContainsMousePos(e.mousePosition, item.GUIRect))
                        {
                            MachineDataController.AddNewTransitionGUI(StateTransitionGUI.startPositionMs, item.state);
                            break;
                        }
                    }
                    StateTransitionGUI.startNewTranstion = false;
                    e.Use();
                    StateMachineEditorWindow.Instance.wantsMouseMove = false;
                }
                else if (e.button == 1)
                {
                    StateTransitionGUI.startNewTranstion = false;
                    e.Use();
                    StateMachineEditorWindow.Instance.wantsMouseMove = false;
                }
               
            }
        }
    }
  


}
