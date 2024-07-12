using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimCompmoent : CompmoentBase 
{
    public Animator m_animator;

    string m_currentAnimName = "";
    public override void OnCreate()
    {
        base.OnCreate();
        m_animator = GetComponent<Animator>();
    }

    public virtual void ChangeAnim(string animName, float fadeTime = 0.1f)
    {
        //Debug.Log(animName);
        //m_animator.Play(animName);
        m_animator.CrossFade(animName, fadeTime);
        m_currentAnimName = animName;

        m_animator.Play("empty", 1);
    }

    public virtual void PlayAnim(string animName)
    {
        if (m_currentAnimName == animName)
        {
            //Debug.Log(m_currentAnimName);
            //m_animator.Play(animName);
        }
        else
        {
           //Debug.Log("PlayAnim " + animName + " m_currentAnimName: " + m_currentAnimName);
            //Debug.Log(m_currentAnimName);
            ChangeAnim(animName);
        }
    }

    public virtual void ResetPlayAnim(string animName, float RaiseTime = 0)
    {
        if (animName == m_currentAnimName)
        {
            m_animator.Play(animName, 1, RaiseTime);
        }
        else
        {
            m_animator.CrossFade(animName, 0.1f, 1, RaiseTime);
        }

        m_currentAnimName = animName;
    }

    public virtual void StopAnim()
    {
        m_animator.speed = 0;
    }

    public virtual void RunAnim()
    {
        m_animator.speed = 1;
    }
}
