using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using NUnit.Framework;

namespace LockStepFrameWork
{
    public class LockStepFrameUnitTest
    {
        [Test(Description = "数值回滚测试")]
        public void ValueRollbackTest()
        {
            LockStepTestWorld world = (LockStepTestWorld)WorldManager.CreateWorld<LockStepTestWorld>();
            world.m_isLocal = true;
            world.IsStart = true;

            ConnectStatusComponent csc = world.GetSingletonComp<ConnectStatusComponent>();
            csc.confirmFrame = 0; //从目标帧之后开始计算

            PlayerComponent pc = new PlayerComponent();
            SelfComponent sc   = new SelfComponent();

            EntityBase c1 =  world.CreateEntityImmediately(1,pc, sc);

            world.CallRecalc();
            world.FixedLoop(1000);

            MoveComponent mc = c1.GetComp<MoveComponent>();

            Debug.Log("mc.pos.x " + mc.pos.x + " frame " + world.FrameCount);

            Assert.AreEqual( 19000, mc.pos.x);

            CommandComponent cmd = new CommandComponent();
            cmd.frame = 1;
            cmd.id = 1;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();
            world.FixedLoop(1000);

            mc = c1.GetComp<MoveComponent>();
            Debug.Log("mc.pos.x " + mc.pos.x + " frame " + world.FrameCount);

            for (int i = 0; i < 10; i++)
            {
                world.CallRecalc();
                world.FixedLoop(1000);
                mc = c1.GetComp<MoveComponent>();
                Debug.Log("mc.pos.x " + mc.pos.x + " frame " + world.FrameCount);
            }

            Assert.AreEqual(15000, mc.pos.x);

        }

        [Test(Description = "实体回滚测试")]
        public void EnityRollBackTest()
        {

        }


        [Test(Description = "Same消息测试")]
        public void SameMsgTestTest()
        {

        }

        [Test(Description = "断线重连测试")]
        public void OffLineReconnectTest()
        {

        }
    }
}
