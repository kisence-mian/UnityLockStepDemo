using UnityEngine;
using System.Collections;

public class BlowFlyStatus : CharacterBaseStatus
{
    float m_time;

    public override void Init(CharacterBase character)
    {
        base.Init(character);
        m_Status = CharacterStatusEnum.Blowfly;
    }

    public override void OnUpdate()
    {
        m_time -= Time.deltaTime;

        if (m_time <=0)
        {
            m_character.ChangeStatus(CharacterStatusEnum.Move);
        }
    }

    public override void ReceviceCmd(CommandBase cmd)
    {
        if (cmd is BlowFlyCmd)
        {
            BlowFly((BlowFlyCmd)cmd);
        }
    }

    public void BlowFly(BlowFlyCmd cmd)
    {
        ShiftDataGenerate l_shiftInfo = DataGenerateManager<ShiftDataGenerate>.GetData(cmd.m_shiftID);
        m_time = l_shiftInfo.m_Time;

        m_character.transform.position = cmd.m_hurterPos;
        m_character.m_moveComp.Shift(cmd.m_flyerID, cmd.m_attackerID, cmd.m_shiftID, 0, cmd.m_attackerPos);
    }

    public override bool CanSwitchOtherStatus(CharacterStatusEnum otherStatus)
    {

        return false;
    }
}
