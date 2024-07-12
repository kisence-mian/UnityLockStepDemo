using FrameWork;
using Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : CommandSyncSystem<CommandComponent>
{
    public static Vector3 moveDirCache = Vector3.zero;
    public static Vector3 skillDirCache = Vector3.zero;
    bool isFireCache = false;

    int element1Cache = GameUtils.c_element_default;
    int element2Cache = GameUtils.c_element_default;

    public override void Init()
    {
        base.Init();

        InputManager.AddListener<InputMoveCommand>(ReceviceMove);
        InputManager.AddListener<RotationCommand>(ReceviceRotation);
    }

    public override void Update(int deltaTime)
    {
        if(GameData.ChoiceList.Count > 0)
        {
            element1Cache = GameData.ChoiceList[0];
        }
        else
        {
            element1Cache = GameUtils.c_element_default;
        }
        
        if(GameData.ChoiceList.Count > 1)
        {
            element2Cache = GameData.ChoiceList[1];
        }
        else
        {
            element1Cache = GameUtils.c_element_default;
        }
        
        Vector3 keyCache = Vector3.zero;

        bool isChange = false;

        if(Input.GetKeyDown(KeyCode.A))
        {
            keyCache += new Vector3(-1, 0, 0);
            isChange = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            keyCache += new Vector3(1, 0, 0);
            isChange = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            keyCache += new Vector3(0, 0, 1);
            isChange = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            keyCache += new Vector3(0, 0, -1);
            isChange = true;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            isChange = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            isChange = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            isChange = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            isChange = true;
        }

        if (isChange)
        {
            moveDirCache = keyCache.normalized;
        }
    }

    public override void BuildCommand(CommandComponent command)
    {
        command.moveDir.FromVector( moveDirCache);
        command.skillDir.FromVector(skillDirCache);
        command.isFire = isFireCache;
        command.element1 = element1Cache;
        command.element2 = element2Cache;

        moveDirCache = Vector3.zero;
        skillDirCache = Vector3.zero;
    }

    public void ReceviceMove(InputMoveCommand cmd)
    {
        float m_cameraAngle = CameraService.Instance.m_mainCameraGo.transform.eulerAngles.y;

        moveDirCache = cmd.m_dir.Vector3RotateInXZ2(m_cameraAngle).normalized;
    }

    public void ReceviceRotation(RotationCommand cmd)
    {
        float m_cameraAngle = CameraService.Instance.m_mainCameraGo.transform.eulerAngles.y;
        if(cmd.m_dir != Vector3.zero)
        {
            skillDirCache = cmd.m_dir.Vector3RotateInXZ2(m_cameraAngle).normalized;
            isFireCache = true;
        }
        else
        {
            isFireCache = false;
        }
    }
}
