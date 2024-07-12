using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClickScreen : IInputOperationEventBase, IInputOperationEventCreater
{
    public Vector3 m_screenPos;
    public void EventTriggerLogic()
    {
        if (Input.GetMouseButtonUp(0)) 
        {
            InputClickScreen cmd = InputOperationEventProxy.GetEvent<InputClickScreen>("InputClickScreen");
            cmd. m_screenPos = Input.mousePosition;
            InputManager.Dispatch("InputClickScreen",cmd);
        }
    }

	
}
