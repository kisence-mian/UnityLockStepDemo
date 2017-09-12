using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;

public class PhysicsSystem :SystemBase
{
    World boxWorld = null;

    public override void Init()
    {
        AABB worldAABB = new AABB();
        worldAABB.LowerBound.Set(-100.0f);
        worldAABB.UpperBound.Set(100.0f);

        Vec2 gravity = new Vec2(0.0f, -0.0f);

        boxWorld = new World(worldAABB, gravity, true);
    }

    public override void BeforeFixedUpdate(int deltaTime)
    {
        //boxWorld.Step((float)deltaTime /1000,)
    }
}
