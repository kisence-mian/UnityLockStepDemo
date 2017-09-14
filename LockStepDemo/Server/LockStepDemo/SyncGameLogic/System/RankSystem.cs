using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RankSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {

            typeof(PlayerComponent),
        };
    }

    public override void Init()
    {
        RankComponent rank = m_world.GetSingletonComp<RankComponent>();

        m_world.eventSystem.AddListener(GameUtils.c_scoreChange, ReCalcRank);
    }

    //计算排行榜不重计算
    public void ReCalcRank(EntityBase entity, params object[] pbjs)
    {
        RankComponent rank = m_world.GetSingletonComp<RankComponent>();
        List<EntityBase> list = GetEntityList();

        list.Sort(sort);
        rank.rankList.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            rank.rankList.Add(list[i].GetComp<PlayerComponent>());
        }
    }

    static int sort(EntityBase x, EntityBase y)
    {
        PlayerComponent a = x.GetComp<PlayerComponent>();
        PlayerComponent b = y.GetComp<PlayerComponent>();

        if (a.score > b.score)
            return -1;

        if (a.score < b.score)
            return 1;

        return 0;
    }
}
