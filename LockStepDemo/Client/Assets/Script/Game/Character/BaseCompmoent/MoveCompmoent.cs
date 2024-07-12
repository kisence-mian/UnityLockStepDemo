using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(CharacterController))]
public class MoveCompmoent : CompmoentBase 
{
    CharacterController m_characterControl;

    Vector3 m_dir;
    //float m_distance = 0;
    float m_height = 0;
    float m_time = 0;

    float m_gravity = 0;

    Vector3 m_MoveDir; //当前飞行的重力

    private Ghost m_myGhost;//残影组件

    public Vector3 m_aimWaistDir = Vector3.zero;


    public override void OnCreate()
    {
        base.OnCreate();
        m_characterControl = GetComponent<CharacterController>();
        m_myGhost = GetComponent<Ghost>();
        if (m_myGhost == null)
        {
            m_myGhost = gameObject.AddComponent<Ghost>();
            m_myGhost.enabled = false;
        }

        this.enabled = false;
        
    }
    /// <summary>
    /// 在LateUpdate里去修改骨骼动画
    /// </summary>
    private void LateUpdate()
    {
        if (m_character.m_waistNode != null)
        {
            Vector3 rot = m_character.m_waistNode.eulerAngles;

            m_aimWaistDir = m_aimWaistDir.normalized;

            Debug.DrawRay(m_character.transform.position, m_aimWaistDir,Color.blue,5);

            float euler = Mathf.Atan2(m_aimWaistDir.x, m_aimWaistDir.z) * Mathf.Rad2Deg;

            //Debug.Log(euler);

            if (m_aimWaistDir.z == 0)
            {
                euler = 0;
            }

            rot.x = 0;
            rot.y = euler - 105;

            m_character.m_waistNode.eulerAngles = rot;
        }
    }

    public override void OnInit()
    {
        SetBlock(true);
        this.enabled = true;
    }

    public void Move(Vector3 dir)
    {
        if (m_characterControl != null && m_characterControl.enabled)
        {
            dir = dir * Time.deltaTime;
            m_characterControl.Move(dir);
        }

    }
     Vector3 m_gravitySpeed = new Vector3(0, -10, 0);
    public void OnlyGravity()
    {
        if (m_characterControl != null && m_characterControl.enabled)
        {
            m_characterControl.Move(m_gravitySpeed * Time.deltaTime);
        }
    }

    public void TunrOn(Vector3 dir,float ratotionSpeed)
    {
        dir.y = 0 ;
        dir = dir.normalized;

        Quaternion formTmp = m_character.transform.rotation;
        Quaternion to = Quaternion.LookRotation(dir,Vector3.up);

        m_character.transform.rotation = Quaternion.RotateTowards(formTmp, to, ratotionSpeed);
    }

    public bool IsFloor()
    {
        return m_characterControl.isGrounded;
    }

    public void Shift(int flyerID, int attackerID, string shiftID, float offsetTime, Vector3 attackerPos = new Vector3())
    {
        //if (shiftID == "null")
        //{
        //    return;
        //}
        //CharacterBase flyer = CharacterManager.GetCharacter(flyerID);
        //CharacterBase attacker = CharacterManager.GetCharacter(attackerID);
        //ShiftDataGenerate l_shiftInfo = DataGenerateManager<ShiftDataGenerate>.GetData(shiftID);

        //if (attackerPos == new Vector3())
        //{
        //    attackerPos = attacker.transform.position;
        //}

        //if (l_shiftInfo.m_Time <= 0)
        //{
        //    return;
        //}

        //m_dir = Vector3.zero;
        //Vector3 l_dir;

        //switch (l_shiftInfo.m_Direction)
        //{

        //    case DirectionEnum.Forward:
        //        m_dir = attacker.transform.forward;
        //        break;
        //    case DirectionEnum.Backward:
        //        m_dir = attacker.transform.forward * (-1);
        //        break;
        //    case DirectionEnum.Close:
        //        l_dir = (attackerPos - flyer.transform.position);
        //        l_dir.y = 0;
        //        m_dir = l_dir.normalized;
        //        break;
        //    case DirectionEnum.Leave:
        //        l_dir = (flyer.transform.position - attackerPos);
        //        l_dir.y = 0;
        //        m_dir = l_dir.normalized;
        //        break;
        //}

        //m_distance = l_shiftInfo.m_Distance;
        //m_height = l_shiftInfo.m_Height;
        //m_time = l_shiftInfo.m_Time - offsetTime;

        //if (m_time < 0.05f)
        //{
        //    m_time = 0.05f;
        //}

        //m_gravity = -1 *(2 * m_height) / (3 * m_time * m_time)*17; //17 为一个大约的数

        //float speed =  m_distance / m_time;

        //m_MoveDir = m_dir * speed;

        //if (speed > 10 && flyerID == attackerID)
        //{
        //    m_myGhost.SetColor(l_shiftInfo.m_GhostColor);
        //    m_myGhost.enabled = true;
            
        //}

        //if (m_height > 0)
        //{
        //    m_MoveDir.y = -0.5f * m_gravity * m_time;
        //}
        //else
        //{
        //    m_MoveDir.y = -9.8f;
        //}
    }

    public override void OnUpdate()
    {
        if (m_time > 0)
        {
            m_time -= Time.deltaTime;

            if (m_height > 0)
            {
                m_MoveDir.y += m_gravity * Time.deltaTime;
            }

            Move(m_MoveDir);
        }
        else
        {
            if (m_myGhost != null&& m_myGhost.enabled == true)
            {
                m_myGhost.enabled = false;
            }

            OnlyGravity();
        }

    }

    /// <summary>
    /// 误差修正
    /// </summary>
    /// <param name="pos"></param>
    public bool DeviationRangeLimit(Vector3 pos)
    {
        Vector3 aimPos = pos;
        aimPos.y = 0;

        if (IsInDeviationRange(aimPos))
        {
            SyncToAimPos(aimPos, SyncService.SyncOperaTimeSpace);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsInDeviationRange(Vector3 aimPos)
    {
        Vector3 currentPos = m_character.transform.position;
        //不考虑y坐标
        currentPos.y = 0;

        if (Vector3.Distance(currentPos, aimPos) > SyncService.c_DeviationRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SyncToAimPos(Vector3 aimPos,float syncTime)
    {
        aimPos.y = m_character.gameObject.transform.position.y;

        AnimSystem.Move(m_character.gameObject, null, aimPos, time: syncTime);
    }

    public void SetBlock(bool isBlock)
    {
        if (m_characterControl != null)
        {
            m_characterControl.enabled = isBlock;
        }
    }

    public override void OnDie()
    {

		if (m_characterControl != null) 
		{
			if (!IsFloor ()) {
				m_characterControl.Move (m_gravitySpeed);
			}
		}

        SetBlock(false);
    }

    public override void OnResurgence()
    {
        base.OnResurgence();
        m_character.m_moveComp.SetBlock(true);
    }
}


