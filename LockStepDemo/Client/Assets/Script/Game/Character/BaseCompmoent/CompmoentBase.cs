using UnityEngine;
using System.Collections;

public abstract class CompmoentBase : MonoBehaviour 
{
    protected CharacterBase m_character;

	public virtual void OnCreate()
    {
        m_character = GetComponent<CharacterBase>();
    }

    public virtual void OnInit()
    {
        
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void Destroy()
    {

    }

    public virtual void OnDie()
    {

    }

    public virtual void OnResurgence()
    {

    }

    void Update()
    {
        OnUpdate();
    }
}
