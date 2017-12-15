using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//初始化系统服务器基类
public class InitSystemBase : SystemBase
{
    public override void Init()
    {
        AddEntityCreaterLisnter();
    }

    public override void OnEntityCreate(EntityBase entity)
    {
        //Debug.Log("OnEntityCreate A");

        //服务器这里要改成判断connection组件进来
        if (entity.GetExistComp<ConnectionComponent>() && entity.GetExistComp<PlayerComponent>())
        {
            Debug.Log("OnEntityCreate B");

            ConnectionComponent cc = entity.GetComp<ConnectionComponent>();
            PlayerComponent pc = entity.GetComp<PlayerComponent>();

            cc.m_defaultInput = new CommandComponent();

            //将角色ID传入游戏
            pc.characterID = cc.m_session.player.characterID;
            pc.nickName = cc.m_session.player.nickName;

            OnPlayerJoin(entity);
        }
    }

    public virtual void OnPlayerJoin(EntityBase entity)
    {

    }
}
