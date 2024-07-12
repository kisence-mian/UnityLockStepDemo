using UnityEngine;
using System.Collections;

public class DieStatus : CharacterBaseStatus
{

    public override void Init(CharacterBase character)
    {
        base.Init(character);
        m_Status = CharacterStatusEnum.Die;
    }
    public override void OnEnterStatus()
    {
        //CharacterManager.Dispatch(m_logic.m_characterID, CharacterEventType.Die, m_character);

        m_character.IsAlive = false;
        
        Timer.DelayCallBack(1.5f, (o) =>
            {
                if (IsCurrentStatus == true)
                {
                    m_character.Dissolve();
                }
            }
        );

    }

    public override void OnUpdate()
    {
        m_character.m_animComp.PlayAnim(m_character.m_Property.m_dieAnimName);
    }

    public override bool CanSwitchOtherStatus(CharacterStatusEnum otherStatus)
    {
        return false;
    }
}
