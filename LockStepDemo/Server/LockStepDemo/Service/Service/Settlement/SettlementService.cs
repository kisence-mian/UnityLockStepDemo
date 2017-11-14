using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class SettlementService : ServiceBase
{

    public override void OnInit(IServerConfig config)
    {
        EventService.AddEvent(ServiceEventDefine.ServiceEvent.GameFinsih, OnGameFinsih);
    }

    public void OnGameFinsih(params object[] objs)
    {
        WorldBase world = (WorldBase)objs[0];

        world.eventSystem.DispatchEvent(GameUtils.c_scoreChange, null);
        List<PlayerComponent> rankList = world.GetSingletonComp<RankComponent>().rankList;
        int diamond = rankList.Count;

        for (int i = 0; i < rankList.Count; i++)
        {
            ConnectionComponent cc = rankList[i].Entity.GetComp<ConnectionComponent>();

            PlayerSettlement_c msg = new PlayerSettlement_c();
            msg.rank = i + 1;
            msg.diamond = diamond--;
            msg.score = rankList[i].score;
            msg.historicalHighest = rankList[i].score;

            if(cc.m_session != null)
            {
                cc.m_session.player.Diamond += msg.diamond;
                ProtocolAnalysisService.SendMsg(cc.m_session, msg);
            }
            else
            {
                //TODO 将奖励发放给离线玩家
            }
        }
    }
}
