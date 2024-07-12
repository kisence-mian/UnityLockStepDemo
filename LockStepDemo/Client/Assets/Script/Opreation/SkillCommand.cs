using UnityEngine;
using System.Collections;

public class InputSkillCommand:IInputOperationEventBase,IInputOperationEventCreater
{
    public int m_skillIndex = 0;

    public void EventTriggerLogic()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            InputSkillCommand cmd = InputOperationEventProxy.GetEvent<InputSkillCommand>("InputSkillCommand");
            cmd.m_skillIndex = 1;

            InputManager.Dispatch("InputSkillCommand",cmd);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            InputSkillCommand cmd = InputOperationEventProxy.GetEvent<InputSkillCommand>("InputSkillCommand");
            cmd.m_skillIndex = 2;

            InputManager.Dispatch("InputSkillCommand",cmd);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            InputSkillCommand cmd = InputOperationEventProxy.GetEvent<InputSkillCommand>("InputSkillCommand");
            cmd.m_skillIndex = 3;

            InputManager.Dispatch("InputSkillCommand",cmd);
        }
    }
}
