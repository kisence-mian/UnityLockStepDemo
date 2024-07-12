using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimSystem : SystemBase
{
    public override void Init()
    {
        
    }

    public override void Update(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            AnimLogic(list[i]);
        }
    }

    public override void LateUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            //这里控制玩家的上半身转向
            TurnAnimLogic(list[i]);
        }
    }

    public override Type[] GetFilter()
    {
        return new Type[]{
            typeof(AnimComponent),
            typeof(SkillStatusComponent),
            typeof(MoveComponent),
            typeof(PlayerComponent),
            typeof(LifeComponent),
            typeof(PerfabComponent),
        };
    }

    public void AnimLogic(EntityBase entity)
    {
        LifeComponent lc = entity.GetComp<LifeComponent>();
        AnimComponent ac = entity.GetComp<AnimComponent>();
        MoveComponent mc = entity.GetComp<MoveComponent>();
        PlayerComponent pc = entity.GetComp<PlayerComponent>();
        PerfabComponent pbc = entity.GetComp<PerfabComponent>();

        if (lc.Life <= 0)
        {
            ac.anim.Play("empty", 1);
            ac.anim.Play("die");
            return;
        }

        //移动逻辑
        if (Vector3.Distance( mc.pos.ToVector(), pbc.perfab.transform.position) > .05f)
        {
            Vector3 Dir = mc.dir.ToVector();

            if(Dir == Vector3.zero)
            {
                Dir = mc.pos.ToVector() - pbc.perfab.transform.position;
            }

            TurnStatus status = GetTurnStatus(Dir, pc.faceDir.ToVector());

            //身体移动方向
            SetBodyDir(status, Dir, ac.perfab);
            //播放不同动画
            SetMoveAnim(status, ac.anim);
        }
        else
        {
            Vector3 Dir = pc.faceDir.ToVector();

            ac.anim.Play("wait");
            if(Dir != Vector3.zero)
            {
                ac.perfab.transform.forward = Vector3.Lerp(ac.perfab.transform.forward,Dir,Time.deltaTime * 10f);
            }
        }

        //上层动画
        UpperAnim(entity, ac);
    }

    public void TurnAnimLogic(EntityBase entity)
    {
        AnimComponent ac = entity.GetComp<AnimComponent>();
        PlayerComponent pc = entity.GetComp<PlayerComponent>();
        SkillStatusComponent sc = entity.GetComp<SkillStatusComponent>();
        MoveComponent mc = entity.GetComp<MoveComponent>();

        Vector3 rot = ac.waistNode.transform.eulerAngles;
        Vector3 aimWaistDir = pc.faceDir.ToVector();

        if(aimWaistDir == Vector3.zero)
        {
            aimWaistDir = ac.perfab.transform.forward;
        }

        float euler = Mathf.Atan2(aimWaistDir.x, aimWaistDir.z) * Mathf.Rad2Deg;
        if (aimWaistDir.z == 0)
        {
            euler = 0;
        }

        rot.x = ac.waistNode.transform.eulerAngles.x;
        rot.y = euler - 90;
        rot.z = ac.waistNode.transform.eulerAngles.z;

        ac.waistDir.x = ac.waistNode.transform.eulerAngles.x;
        ac.waistDir.z = ac.waistNode.transform.eulerAngles.z;

        ac.waistDir.y = Mathf.LerpAngle(ac.waistDir.y, rot.y, Time.deltaTime * 10);
        ac.waistNode.transform.eulerAngles = ac.waistDir;

        Debug.DrawRay(ac.waistNode.transform.position, ac.waistNode.transform.forward * 5, Color.red);

        Debug.DrawRay(ac.perfab.transform.position, pc.faceDir.ToVector() * 3, Color.green);

        Debug.DrawRay(ac.perfab.transform.position, ac.perfab.transform.forward * 2, Color.yellow);

        Debug.DrawRay(ac.perfab.transform.position, aimWaistDir * 4, Color.blue);
    }

    public void UpperAnim(EntityBase entity, AnimComponent ac)
    {
        SkillStatusComponent sc = entity.GetComp<SkillStatusComponent>();

        if (sc.m_skillStstus != SkillStatusEnum.Finish
            && sc.m_skillStstus != SkillStatusEnum.None)
        {
            string attackAnimName = "null";

            switch (sc.m_skillStstus)
            {
                case SkillStatusEnum.Before:
                    attackAnimName = sc.m_currentSkillData.BeforeInfo.m_AnimName;
                    break;
                case SkillStatusEnum.Current:
                    attackAnimName = sc.m_currentSkillData.CurrentInfo.m_AnimName;
                    break;
                case SkillStatusEnum.Later:
                    attackAnimName = sc.m_currentSkillData.LaterInfo.m_AnimName;
                    break;
            }
            if (attackAnimName != "null")
            {
                ac.anim.Play(attackAnimName, 1);
            }
            else
            {
                ac.anim.Play("empty", 1);
            }
        }
    }

    public void SetMoveAnim(TurnStatus status, Animator anim)
    {
        string animName = GetMoveAnimName(status);
        anim.Play(animName);
    }

    public void SetBodyDir(TurnStatus status,Vector3 moveDir ,GameObject character)
    {
        if (moveDir == Vector3.zero)
        {
            moveDir = character.transform.forward;
        }

        moveDir.y = 0;

        Vector3 fdir = moveDir;

        switch (status)
        {
            case TurnStatus.Forward:
                fdir =  moveDir;
                break;
            case TurnStatus.Back:
                fdir = -moveDir;
                break;
            case TurnStatus.Right:
                fdir = moveDir.Vector3RotateInXZ(90);
                break;
            case TurnStatus.Left:
                fdir = moveDir.Vector3RotateInXZ2(90);

                break;
        }

        character.transform.forward = Vector3.Lerp(character.transform.forward, fdir, Time.deltaTime * 10f);
    }


    public TurnStatus GetTurnStatus(Vector3 dir, Vector3 faceDir)
    {
        if(faceDir == Vector3.zero)
        {
            return TurnStatus.Forward;
        }

        float angle = Vector3.Angle(dir, faceDir);

        if (angle < 45)
        {
            return TurnStatus.Forward;
        }
        else if (angle > 135)
        {
            return TurnStatus.Back;
        }
        else
        {
            Vector3 tmp = Vector3.Cross(dir, faceDir);
            if (tmp.y > 0)
            {
                return TurnStatus.Left;
            }
            else
            {
                return TurnStatus.Right;
            }
        } 
    }

    string GetMoveAnimName(TurnStatus status)
    {
        switch (status)
        {
            case TurnStatus.Forward: return "move";//m_character.m_Property.m_walkAnimName;
            case TurnStatus.Back: return "move_back";// m_character.m_Property.m_BackWalkAnimName;
            case TurnStatus.Left: return "move_left";// m_character.m_Property.m_LeftWalkAnimName;
            case TurnStatus.Right: return "move_right";// m_character.m_Property.m_RightWalkAnimName;
        }

        return "move";// m_character.m_Property.m_walkAnimName;
    }
}

public enum TurnStatus
{
    Forward,
    Back,
    Left,
    Right,
}
