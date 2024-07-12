using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : IApplicationStatus
{

    PlayerControl playerControl = new PlayerControl();
    public override void OnEnterStatus()
    {
        WorldManager.Init(200);
        WorldBase world =  WorldManager.CreateWorld<DemoWorld>();
        world.IsStart = true;
        world.SyncRule = SyncRule.Frame;
        world.m_isLocal = true;

        SelfComponent self = new SelfComponent();

        world.CreateEntity("1",self);


        TheirComponent their = new TheirComponent();
        MoveComponent mc = new MoveComponent();
        mc.pos.FromVector(new Vector3(20, 0, 5));

        CommandComponent cc = new CommandComponent();

        world.CreateEntity("2",their, mc);
    }

    public override void OnGUI()
    {

    }
}
