using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveStatus : CharacterBaseStatus 
{
    //目标位置
    Vector3 m_targetPos;
    Vector3 m_moveDir;

    //移动到目标位置需要的时间
    float m_moveTime;

    bool m_isMove = false;
    bool m_isOnlyTurn = false;
    float m_rotationSpeed = 3;

    public override void Init(CharacterBase character)
    {
        base.Init(character);
        m_Status = CharacterStatusEnum.Move;

        m_character.m_moveComp.m_aimWaistDir = m_character.transform.forward;
    }

    public override void OnUpdate()
    {
        if (m_isMove)
        {
            //m_character.m_moveComp.TunrOn(m_moveDir, m_rotationSpeed * Time.deltaTime);

            //Debug.Log(m_moveDir);

            if (!m_isOnlyTurn)
            {
                m_character.m_moveComp.Move(m_moveDir);
            }

            m_moveTime -= Time.smoothDeltaTime;

            if (m_moveTime < 0)
            {
                m_isMove = false;
            }
        }

        //别的状态控制移动时不播放移动动画
        if (IsCurrentStatus)
        {
            PlayMoveAnim(m_isMove && !m_isOnlyTurn);
        }
    }

    public void Move(MoveCmd cmd)
    {
        //Debug.Log(cmd.GetDir());
        //m_isOnlyTurn = cmd.m_isOnlyTurn;

        //Vector3 dir = cmd.GetPos() - m_character.transform.position;
        //dir.y = 0;

        m_targetPos = CalcTargetPos(cmd);
        m_moveTime = CalcMoveTime(cmd);

        //Debug.Log("m_targetPos " + m_targetPos + " service Pos " + cmd.m_pos + " m_moveTime " + m_moveTime + " " + cmd.GetCreateTime());

        m_moveDir = CalcMoveVector3(m_targetPos, m_moveTime);

        SetBodyDir(m_moveDir);

        m_rotationSpeed = CalcRotaionSpeed(cmd.GetDir());

        //添加重力
        m_moveDir.y = Const.c_gravity;

        //误差限定
        //if (IsInDeviationRange(m_targetPos))
        //{
        //    m_moveTime = SyncService.SyncOperaTimeSpace;
        //    //直接同步过去
        //    m_isMove = true;
        //    m_character.m_moveComp.SyncToAimPos(m_targetPos, SyncService.SyncOperaTimeSpace);
        //}
        //else
        {
            //走过去
            m_isMove = true;
        }
    }

    public void Rotation(RotationCmd cmd)
    {
        //m_character.transform.forward = cmd.m_dir;
        m_character.m_moveComp.m_aimWaistDir = cmd.m_dir;

        SetBodyDir(m_moveDir);
    }

    #region 功能函数

    void TurnOn(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            m_character.transform.forward = dir;
        }
    }

    string moveAnim;
    void PlayMoveAnim(bool isMove)
    {
        if (isMove)
        {
            if (!m_character.GetCloaking())
            {
                moveAnim = GetMoveAnimName();
                m_character.m_animComp.PlayAnim(moveAnim);
            }
            else
            {
                m_character.m_animComp.PlayAnim(m_character.m_Property.m_cloakAnimName);
            }

        }
        else
        {
            m_character.m_animComp.PlayAnim(m_character.m_Property.m_idleAnimName);
        }
    }

    string GetMoveAnimName()
    {
        Vector3 dir = m_moveDir;

        if (dir == Vector3.zero)
        {
            dir = m_character.transform.forward;
        }

        dir.y = 0;

        TurnStatus status = GetTurnStatus(dir, m_character.m_moveComp.m_aimWaistDir);

        switch (status)
        {
            case TurnStatus.Forward: return m_character.m_Property.m_walkAnimName;
            case TurnStatus.Back: return m_character.m_Property.m_BackWalkAnimName;
            case TurnStatus.Left: return m_character.m_Property.m_LeftWalkAnimName;
            case TurnStatus.Right:return m_character.m_Property.m_RightWalkAnimName;
        }
        
        return m_character.m_Property.m_walkAnimName;
    }

    #endregion

    #region 补偿计算

    float CalcMoveTime(MoveCmd cmd)
    {
        //float timeOffset = SyncService.CurrentServiceTime - cmd.GetCreateTime();
        float maxTime = Vector3.Distance(cmd.m_pos, m_character.transform.position) / m_character.m_Property.Speed;

        //if (timeOffset < 0)
        //{
        //    timeOffset = 0;
        //}

        ////发送者还未移动到目标点
        //if (timeOffset < SyncService.SyncOperaTimeSpace)
        //{
        //    t = SyncService.SyncOperaTimeSpace - timeOffset;
        //}
        ////发送者已经到达了目标点
        //else
        //{
        //    t = SyncService.SyncOperaTimeSpace;
        //}

        ////if(t > maxTime)
        ////{
        ////    t = maxTime;
        ////}

        return maxTime;
    }

    //Ray m_ray = new Ray();
    //static Vector3 s_upOffset = new Vector3(0,1,0);
    static int index1 = 0;
    static int index2 = 0;
    public Vector3 CalcTargetPos(MoveCmd cmd)
    {
        //Vector3 result = cmd.m_pos + SyncService.SyncOperaTimeSpace * cmd.GetDir().normalized * m_character.GetSpeed();
        //result = cmd.m_pos;

        return cmd.m_pos;
    }

    public Vector3 GetTargetPos(Vector3 dir)
    {
        Vector3 result = m_character.transform.position + SyncService.SyncOperaTimeSpace * dir.normalized * m_character.GetSpeed();

        return result;
    }


    float CalcRotaionSpeed(Vector3 dir)
    {
        Vector3 forward = m_character.transform.forward;

        float offsetY = Vector3.Angle(forward, dir);

        if (offsetY > 360)
        {
            offsetY -= 360;
        }

        if (offsetY < 0)
        {
            offsetY += 360;
        }

        //float speed = offsetY / SyncService.SyncOperaTimeSpace;
        float speed = offsetY / 0.15f;
        return speed;
    }

    float CalcMoveRadius()
    {
        return SyncService.SyncOperaTimeSpace * m_character.GetSpeed();
    }

    bool IsInDeviationRange(Vector3 aimPos)
    {
        Vector3 currentPos = m_character.transform.position;
        //不考虑y坐标
        currentPos.y = 0;
        aimPos.y = 0;

        if (Vector3.Distance(currentPos, aimPos) >( CalcMoveRadius() +  SyncService.c_DeviationRange))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 CalcMoveVector3(Vector3 targetPos,float moveTime)
    {
        if (moveTime == 0)
        {
            Debug.LogError("moveTime is zero !");
            moveTime = SyncService.SyncOperaTimeSpace;
        }

        Vector3 currentPos = m_character.transform.position;
        Vector3 aimPos = targetPos;

        //不考虑y坐标
        currentPos.y = 0;
        aimPos.y = 0;

        Vector3 result = (aimPos - currentPos) / moveTime;

        return result;
    }

    #endregion

    #region 状态切换

    public override void OnEnterStatus()
    {

        base.OnEnterStatus();
    }

    public override void OnExitStatus()
    {

        m_isMove = false;
        base.OnExitStatus();
    }

    public override bool CanBreakBySkill(string skillID)
    {
        return true;
    }

    public static TurnStatus GetTurnStatus(Vector3 dir,Vector3 faceDir)
    {
        float angle = Vector3.Angle(dir, faceDir);

        if(angle < 45)
        {
            return TurnStatus.Forward;
        }
        else if(angle > 135)
        {
            return TurnStatus.Back;
        }
        else
        {
            Vector3 tmp = Vector3.Cross(dir, faceDir);
            if(tmp.y > 0)
            {
                return TurnStatus.Left;
            }
            else
            {
                return TurnStatus.Right;
            }
        }
    }

    public void SetBodyDir(Vector3 dir)
    {
        if(dir == Vector3.zero)
        {
            dir = m_character.transform.forward;
        }

        dir.y = 0;

        TurnStatus status = GetTurnStatus(dir, m_character.m_moveComp.m_aimWaistDir);

        switch (status)
        {
            case TurnStatus.Forward:
                m_character.transform.forward = dir;
                break;
            case TurnStatus.Back:
                m_character.transform.forward = -dir;
                break;
            case TurnStatus.Right:
                m_character.transform.forward = dir.Vector3RotateInXZ(90);
                break;
            case TurnStatus.Left:
                m_character.transform.forward = dir.Vector3RotateInXZ2(90);
                break;
        }
    }


    #endregion
}


