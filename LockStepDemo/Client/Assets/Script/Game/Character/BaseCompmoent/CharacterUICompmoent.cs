using UnityEngine;
using System.Collections;

public class CharacterUICompmoent : CompmoentBase
{
    public FightBehaveWindow m_bloodUI;

    public override void OnCreate()
    {
        base.OnCreate();
        if (m_bloodUI == null)
        {
            m_bloodUI = UIManager.OpenUIWindow<FightBehaveWindow>();
            m_bloodUI.SetCharacterID(m_character.m_characterID);
        }
        else
        {
            Debug.LogError("重复创建 UI !");
        }
    }

    public override void OnDie()
    {
        DestroyUI();
    }

    public override void OnResurgence()
    {
        OnCreate();
    }

    public override void Destroy()
    {
        DestroyUI();
    }

    void DestroyUI()
    {
        if (m_bloodUI != null)
        {
            UIManager.CloseUIWindow(m_bloodUI, false);
            m_bloodUI = null;
        }
    }
}
