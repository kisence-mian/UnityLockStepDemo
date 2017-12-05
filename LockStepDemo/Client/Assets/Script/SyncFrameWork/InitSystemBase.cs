using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//初始化系统客户端基类
public class InitSystemBase : SystemBase
{
    public override void Init()
    {
        AddEntityOptimizeCreaterLisnter();
    }

    public override void Dispose()
    {
        RemoveEntityOptimizeCreaterLisnter();
    }

    public override void OnGameStart()
    {
        
    }

    public override void OnEntityOptimizeCreate(EntityBase entity)
    {
        //服务器这里要改成判断connection组件进来
        if (entity.GetExistComp<SelfComponent>() || entity.GetExistComp<TheirComponent>())
        {
            OnPlayerJoin(entity);
        }
    }


    public virtual void OnPlayerJoin(EntityBase entity)
    {

    }
}
