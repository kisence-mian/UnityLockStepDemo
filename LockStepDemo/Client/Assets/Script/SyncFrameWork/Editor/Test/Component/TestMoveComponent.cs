using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveComponent : MomentComponentBase
{
    public SyncVector3 pos = new SyncVector3();

    public SyncVector3 dir = new SyncVector3();

    public int m_velocity; //速度

    public bool isCollision = false;

    public override MomentComponentBase DeepCopy()
    {
        TestMoveComponent mc = new TestMoveComponent();

        mc.ID = ID;
        mc.Frame = Frame;

        mc.pos = pos.DeepCopy();
        mc.dir = dir.DeepCopy();

        mc.m_velocity = m_velocity;
        mc.isCollision = isCollision;
        return mc;
    }

    //public override int ToHash()
    //{
    //    return Serializer.Serialize(this).ToHash();
    //}
}