using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//初始化系统客户端基类
public class InitSystemBase : SystemBase
{
    public override void Init()
    {
        AddEntityCreaterLisnter();

        AddEntityCompAddLisenter();
    }

    public override void Dispose()
    {
        RemoveEntityCreaterLisnter();
        RemoveEntityCompAddLisenter();
    }

    public override void OnGameStart()
    {
        
    }

    public override void OnEntityCreate(EntityBase entity)
    {
        //服务器这里要改成判断connection组件进来
        if (entity.GetExistComp<SelfComponent>() || entity.GetExistComp<TheirComponent>())
        {
            OnPlayerJoin(entity);
        }
    }

    public override void OnEntityCompAdd(EntityBase entity, string compName, ComponentBase component)
    {
        //Debug.Log("OnEntityCompAdd " + compName);

        if (compName == "SelfComponent" || compName == "TheirComponent")
        {
            OnPlayerJoin(entity);
        }
    }


    public virtual void OnPlayerJoin(EntityBase entity)
    {

    }
}
