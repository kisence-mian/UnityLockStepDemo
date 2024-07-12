using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl 
{
    private CharacterBase character;
    public CharacterBase ControlCharacter
    {
        get
        {
            if (character == null)
            {
                if (CharacterManager.GetCharacterIsExit(m_targetCharacterID))
                {
                    character = CharacterManager.GetCharacter(m_targetCharacterID);
                }
            }

            return character;
        }

        set
        {
            character = value;
        }
    }

    public int m_targetCharacterID;

    private float m_cameraAngle;//摄像机旋转角度（顺时针）

    public void Init()
    {
        InputOperationEventProxy.LoadEventCreater<InputMoveCommand>();
        InputOperationEventProxy.LoadEventCreater<InputAttackCommand>();
        InputOperationEventProxy.LoadEventCreater<InputSkillCommand>();
        InputOperationEventProxy.LoadEventCreater<InputClickScreen>();

        InputManager.AddListener<InputMoveCommand>(OnMove);
        InputManager.AddListener<RotationCommand>(OnRotation);
        //InputManager.AddListener<InputAttackCommand>(OnAttack);
        InputManager.AddListener<InputSkillCommand>(OnUseSkill);
        InputManager.AddListener<InputClickScreen>(OnClickScreen);

        ApplicationManager.s_OnApplicationUpdate += Update;

        //m_GuideArrow = GameObjectManager.GetPoolObject("zhidao_arrow");

        GlobalEvent.AddEvent(UserData.UserDataChangeEvent.PlayerID, RecevicePlayerIDChange);
    }

    public void Dispose()
    {
        InputOperationEventProxy.UnLoadEventCreater<InputMoveCommand>();
        InputOperationEventProxy.UnLoadEventCreater<InputAttackCommand>();
        InputOperationEventProxy.UnLoadEventCreater<InputSkillCommand>();
        InputOperationEventProxy.UnLoadEventCreater<InputClickScreen>();

        InputManager.RemoveListener<InputMoveCommand>(OnMove);
        InputManager.RemoveListener<RotationCommand>(OnRotation);
        //InputManager.RemoveListener<InputAttackCommand>(OnAttack);
        InputManager.RemoveListener<InputSkillCommand>(OnUseSkill);
        InputManager.RemoveListener<InputClickScreen>(OnClickScreen);

        ApplicationManager.s_OnApplicationUpdate -= Update;

        //GameObjectManager.DestroyPoolObject(m_GuideArrow);
        GlobalEvent.RemoveEvent(UserData.UserDataChangeEvent.PlayerID, RecevicePlayerIDChange);

        character = null;
    }

    private float n_cameraAngle;        //摄像机旋转角度（顺时针）
    private Vector3 l_v3_rotatedDir;    //根据摄像机旋转后的移动方向
    float m_SendMoveCommandTimer = 0;
    PoolObject m_GuideArrow;

    void Update()
    {
        m_SendMoveCommandTimer -= Time.deltaTime;

        if (m_SendMoveCommandTimer <0)
        {
            m_SendMoveCommandTimer = 0;
        }

        if (Time.timeSinceLevelLoad - lastFindAimTime > findAimSpace)
        {
            //FindNowEnemy();
            lastFindAimTime = Time.timeSinceLevelLoad;
        }

        GuideLogic();
    }

    void GuideLogic()
    {
        //if (m_character == null)
        //{
        //    return;
        //}

        //aimCharacter = FightLogicService.GetRecentlyEnemy(m_character, m_character.m_camp, m_character.m_isTrueSight);
        //if (aimCharacter == null)
        //{
        //    m_GuideArrow.gameObject.SetActive(false);
        //}
        //else
        //{
        //    m_GuideArrow.gameObject.SetActive(true);
        //    m_GuideArrow.transform.LookAt(aimCharacter.transform);
        //}
    }

    #region 发送消息

    void SendMoveCommand(InputMoveCommand cmd)
    {
        m_cameraAngle = CameraService.Instance.m_mainCameraGo.transform.eulerAngles.y;

        MoveCmd mcmd = HeapObjectPool<MoveCmd>.GetObject();

        Vector3 dir = cmd.m_dir.Vector3RotateInXZ2(m_cameraAngle).normalized;

        mcmd.m_pos = ControlCharacter.m_moveStatus.GetTargetPos(dir);

        mcmd.SetData(m_targetCharacterID, dir);

        CommandRouteService.SendPursueCommand(mcmd);
    }

    void SendRotationCommand(RotationCommand cmd)
    {
        m_cameraAngle = CameraService.Instance.m_mainCameraGo.transform.eulerAngles.y;

        RotationCmd mcmd = HeapObjectPool<RotationCmd>.GetObject();

        Vector3 dir = cmd.m_dir.Vector3RotateInXZ2(m_cameraAngle);

        mcmd.SetData(m_targetCharacterID, dir);
        CommandRouteService.SendPursueCommand(mcmd);
    }

    void SendSkillCommand(string skillID,Vector3 dir)
    {
        SkillCmd scmd = HeapObjectPool<SkillCmd>.GetObject();

        dir = dir.Vector3RotateInXZ2(m_cameraAngle);

        SkillDataGenerate skillInfo = DataGenerateManager<SkillDataGenerate>.GetData(skillID);

        scmd.SetData(m_targetCharacterID, skillID, dir, ControlCharacter.transform.position);

        CommandRouteService.SendPursueCommand(scmd);

        //Debug.Log("SendSkillCommand " + skillID + " dir:" + dir);
    }

    #endregion

    #region 功能函数

    //上次寻找攻击目标的时间
    private float lastFindAimTime = 0;
    //查找间隔
    private float findAimSpace = 1;
    public void FindNowEnemy()
    {
        if (ControlCharacter == null)
        {
            return;
        }

        m_lastEnemy = m_nowEnemy;
        
        //m_nowEnemy = FightLogicService.GetRecentlyEnemy(m_character.GetVisableArea(), m_character.m_camp, m_character.m_isTrueSight);
        if (m_nowEnemy != m_lastEnemy)
        {
            if (m_lastEnemy != null)
            {
                m_lastEnemy.HideAimChircle();
            }

            if (m_nowEnemy != null)
            {
                m_nowEnemy.ShowAimChircle();
            }
            
        }
    }

    [HideInInspector]
    public CharacterBase m_nowEnemy;//当前攻击目标

    private CharacterBase m_lastEnemy;//上一个攻击目标


    public Vector3  GetForward()
    {
        //FindNowEnemy();
        if (m_nowEnemy != null)
        {
            Vector3 forword = m_nowEnemy.transform.position - ControlCharacter.transform.position;
            forword.y = 0;

            return forword;
        }
        else
        {
            return ControlCharacter.transform.forward;
        }
    }

    #endregion

    #region 事件接收

    void RecevicePlayerIDChange(params object[] objs)
    {
        m_targetCharacterID = UserData.PlayerID;
        ControlCharacter = null;
    }

    void OnMove(InputMoveCommand cmd)
    {
        if (ControlCharacter != null)
        {
            if (cmd.m_dir == Vector3.zero)
            {
                //SendMoveCommand(cmd);
            }
            else
            {
                //降低发送命令的频率
                if (m_SendMoveCommandTimer <= 0)
                {
                    m_SendMoveCommandTimer = SyncService.SyncOperaTimeSpace / 2;

                    SendMoveCommand(cmd);
                }
            }
        }
    }

    void OnRotation(RotationCommand cmd)
    {
        if (ControlCharacter != null)
        {
            if (cmd.m_dir != Vector3.zero)
            {
                cmd.m_dir = cmd.m_dir.normalized;

                //降低发送命令的频率
                if (m_SendMoveCommandTimer <= 0)
                {
                    m_SendMoveCommandTimer = SyncService.SyncOperaTimeSpace /2;

                    SendRotationCommand(cmd);
                }

                string skillID = GetSkillName(GameData.ChoiceList);

                skillID = "2000";

                if (ControlCharacter.m_skillStatus.GetSkillCD(skillID))
                {
                    SendSkillCommand(skillID, cmd.m_dir);
                }
            }
        }
    }


    void OnUseSkill(InputSkillCommand cmd)
    {
        //if (ControlCharacter != null)
        //{
        //    SendSkillCommand(cmd);
        //}
    }

    void OnClickScreen(InputClickScreen cmd)
    {
        if (Camera.main != null)
        {

        }
        else
        {
            Debug.LogError("OnClickScreen Error Camera.main is null !");
        }
    }

    DataTable m_comboData;
    string GetSkillName(List<int> elementList)
    {
        if(m_comboData == null)
        {
            m_comboData = DataManager.GetData("CombineData");
        }

        if(elementList.Count == 0)
        {
            return DataGenerateManager<CombineDataGenerate>.GetData(m_comboData.TableIDs[0]).m_key;
        }

        for (int i = 0; i < m_comboData.TableIDs.Count; i++)
        {
            CombineDataGenerate data = DataGenerateManager<CombineDataGenerate>.GetData(m_comboData.TableIDs[i]);
            if (elementList.Count > 1)
            {
                if((data.m_ele_1 == elementList[0] && data.m_ele_2 == elementList[1])
                    || (data.m_ele_2 == elementList[0] && data.m_ele_1 == elementList[1])
                    )
                {
                    return data.m_key;
                }
            }
            else
            {
                if ((data.m_ele_1 == elementList[0] && data.m_ele_2 == 0)
                    || (data.m_ele_2 == elementList[0] && data.m_ele_1 == 0)
                    )
                {
                    return data.m_key;
                }
            }
        }

        //Error!
        throw new System.Exception("Not Find SkillName!");
    }

    #endregion
}
