using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class CharacterHardPoint
{
    public Transform m_head;
    public Transform m_hand_R;
    public Transform m_hand_L;
    public Transform m_chest;
    public Transform m_pos;
    public Transform m_weapon01;
    public Transform m_weapon02;
    public Transform m_enemy;
    public Transform m_headTop;

    public Transform GetHardPoint(HardPointEnum pointEnum)
    {
        Transform result = null;

        switch (pointEnum)
        {
            case HardPointEnum.head: result = m_head; break;
            case HardPointEnum.chest: result = m_chest; break;
            case HardPointEnum.hand_R: result = m_hand_R; break;
            case HardPointEnum.hand_L: result = m_hand_L; break;
            case HardPointEnum.position: result = m_pos; break;
            case HardPointEnum.enemy: result = m_enemy; break;
            case HardPointEnum.Weapon_01: result = m_weapon01; break;
            case HardPointEnum.Weapon_02: result = m_weapon02; break;
            case HardPointEnum.headTop: result = m_headTop; break;
            default: throw new Exception("ERROR HardPointEnum " + pointEnum);
        }

        if (result == null)
        {
            throw new Exception("ERROR HardPoint is null ! " + pointEnum);
        }

        return result;
    }
}
