using UnityEngine;
using System.Collections;

public class InputAttackCommand : IInputOperationEventBase,IInputOperationEventCreater
{
   
    public void EventTriggerLogic()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InputAttackCommand cmd = new InputAttackCommand();
            
            InputManager.Dispatch("InputAttackCommand",cmd);
        }
    }
}
