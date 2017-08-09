using FrameWork;
using LockStepDemo.GameLogic.Component;
using Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : ViewSystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(SelfComponent),
            typeof(CommandComponent)
        };
    }

    public override void BeforeUpdate(int deltaTime)
    {
        Debug.Log("InputSystem BeforeUpdate");

        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            CommandComponent command = list[i].GetComp<CommandComponent>();

            command.isForward = Input.GetKey(KeyCode.W);
            command.isBack    = Input.GetKey(KeyCode.S);
            command.isRight   = Input.GetKey(KeyCode.D);
            command.isLeft    = Input.GetKey(KeyCode.A);

            command.isFire = Input.GetMouseButton(0);
            
            if(!list[i].GetExistComp<WaitSyncComponent>())
            {
                list[i].AddComp<WaitSyncComponent>();

                Debug.Log("InputSystem add");
            }
        }
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            ChangeComponentMsg msg = new ChangeComponentMsg();
            CommandComponent command = list[i].GetComp<CommandComponent>();

            msg.m_id = list[i].ID;
            msg.info.m_compName = "CommandComponent";
            msg.info.content = Serializer.Serialize(command);

            ProtocolAnalysisService.SendCommand(msg);
        }
    }
}
