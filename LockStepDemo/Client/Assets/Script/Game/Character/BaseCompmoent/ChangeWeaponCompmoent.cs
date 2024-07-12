using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeaponCompmoent : CompmoentBase  
{
    GameObject m_weapon01;
    GameObject m_weapon02;

    public void ChangeWeapon(string[] modelIDs)
    {
        ClearWeapon();

        if(modelIDs == null || modelIDs.Length == 0)
        {
            return;
        }

        m_weapon01 = LoadWeapon(HardPointEnum.Weapon_01, modelIDs[0]);

        if(modelIDs.Length > 1)
        {
            m_weapon02 = LoadWeapon(HardPointEnum.Weapon_02, modelIDs[1]);
        }

        SetLayout(0);
    }

    public void ChangeWeaponByUI(string[] modelIDs)
    {
        ClearWeapon();

        if (modelIDs == null || modelIDs.Length == 0)
        {
            return;
        }

        m_weapon01 = LoadWeapon(HardPointEnum.Weapon_01, modelIDs[0]);

        if (modelIDs.Length > 1)
        {
            m_weapon02 = LoadWeapon(HardPointEnum.Weapon_02, modelIDs[1]);
        }

        SetLayout(5);
    }

    public override void OnCreate()
    {
        base.OnCreate();
    }

    void SetLayout(int layer)
    {
        if (m_weapon01 != null)
        {
            m_weapon01.layer = layer;
        }

        if (m_weapon02 != null)
        {
            m_weapon02.layer = layer;
        }

        //m_character.gameObject.layer = layout;

        Transform[] list =  m_character.GetComponentsInChildren<Transform>();

        for (int i = 0; i < list.Length; i++)
        {
            list[i].gameObject.layer = layer;
        }
    }

    void Destory()
    {

    }

    GameObject LoadWeapon(HardPointEnum hardPoint,string modelID)
    {
        if(modelID == "null")
        {
            return null;
        }

        GameObject weapon = GameObjectManager.CreateGameObjectByPool(modelID, m_character.m_hardPoint.GetHardPoint(hardPoint).gameObject);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localEulerAngles = Vector3.zero;
        weapon.transform.localScale = Vector3.one;

        return weapon;
    }

    void ClearWeapon()
    {
        if(m_weapon01 != null)
        {
            GameObjectManager.DestroyGameObjectByPool(m_weapon01);
            m_weapon01 = null;
        }

        if (m_weapon02 != null)
        {
            GameObjectManager.DestroyGameObjectByPool(m_weapon02);
            m_weapon01 = null;
        }
    }
}
