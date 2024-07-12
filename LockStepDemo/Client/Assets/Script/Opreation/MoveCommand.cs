using UnityEngine;
using System.Collections;

public class InputMoveCommand : IInputOperationEventBase, IInputOperationEventCreater
{
    public Vector3 m_dir;

    public Vector3 m_aimPos;

    bool m_isInput = false;
    public void EventTriggerLogic()
    {
        m_isInput = false;

        Vector3 dic = Vector3.zero;
        bool w, s, a, d, wa, wd, sa, sd;
        w = Input.GetKey(KeyCode.W);
        s = Input.GetKey(KeyCode.S);
        a = Input.GetKey(KeyCode.A);
        d = Input.GetKey(KeyCode.D);
        wa = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A);
        wd = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D);
        sa = Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A);
        sd = Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D);
        if (wa || wd || sa || sd) w = s = a = d = false;
        if (w || a || d || s || wa || wd || sa || sd)
        {
            //m_isInput = true;
            if (w) dic = Vector3.forward;
            if (s) dic = Vector3.back;
            if (a) dic = Vector3.left;
            if (d) dic = Vector3.right;

            if (wa) dic = Vector3.forward + Vector3.left;
            if (wd) dic = Vector3.forward + Vector3.right;
            if (sa) dic = Vector3.back + Vector3.left;
            if (sd) dic = Vector3.back + Vector3.right;
            InputMoveCommand cmd = InputOperationEventProxy.GetEvent<InputMoveCommand>("InputMoveCommand");
            cmd.m_dir = dic;
            InputManager.Dispatch("InputMoveCommand", cmd);
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 500, LayerMask.GetMask("floor")))
            {
                InputMoveCommand cmd = InputOperationEventProxy.GetEvent<InputMoveCommand>("InputMoveCommand");
                cmd.m_aimPos = hitInfo.point;
                InputManager.Dispatch("InputMoveCommand", cmd);

            }
        }

    }
}
