using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using NUnit.Framework;
using Protocol;

namespace LockStepFrameWork
{
    public class LockStepFrameUnitTest
    {
        #region 数值回滚测试

        [Test(Description = "数值回滚测试")]
        public void ValueRollbackTest()
        {
            ResourcesConfigManager.Initialize();
            WorldManager.IntervalTime = 100;

            LockStepEntityTestWorld world = (LockStepEntityTestWorld)WorldManager.CreateWorld<LockStepEntityTestWorld>();
            world.IsClient = true;
            world.IsStart = true;
            world.IsLocal = true;

            ConnectStatusComponent csc = world.GetSingletonComp<ConnectStatusComponent>();
            csc.confirmFrame = 0; //从目标帧之后开始计算

            SelfComponent sc   = new SelfComponent();

            EntityBase c1 =  world.CreateEntityImmediately(1, sc);

            LockStepInputSystem.commandCache.moveDir.x = 1000;

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            LockStepInputSystem.commandCache.moveDir.x = 0000;

            TestMoveComponent mc = c1.GetComp<TestMoveComponent>();

            //Debug.Log("mc.pos.x " + mc.pos.x + " frame " + world.FrameCount);

            Assert.AreEqual( 400, mc.pos.x);

            TestCommandComponent cmd = new TestCommandComponent();
            cmd.frame = 1;
            cmd.id = 1;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            mc = c1.GetComp<TestMoveComponent>();
            //Debug.Log("mc.pos.x " + mc.pos.x + " frame " + world.FrameCount);

            for (int i = 0; i < 10; i++)
            {
                world.CallRecalc();
                world.FixedLoop(WorldManager.IntervalTime);
                mc = c1.GetComp<TestMoveComponent>();
                //Debug.Log("mc.pos.x " + mc.pos.x + " frame " + world.FrameCount);
            }

            Assert.AreEqual(0, mc.pos.x);
        }

        [Test(Description = "单例组件数值回滚测试")]
        public void SingleComponentValueRollbackTest()
        {
            ResourcesConfigManager.Initialize();
            WorldManager.IntervalTime = 100;

            LockStepEntityTestWorld world = (LockStepEntityTestWorld)WorldManager.CreateWorld<LockStepEntityTestWorld>();
            world.IsClient = true;
            world.IsStart = true;
            world.IsLocal = true;

            ConnectStatusComponent csc = world.GetSingletonComp<ConnectStatusComponent>();
            csc.confirmFrame = 0; //从目标帧之后开始计算

            SelfComponent sc = new SelfComponent();

            /*EntityBase c1 =*/ world.CreateEntityImmediately(1, sc);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            TestSingleComponent tc = world.GetSingletonComp<TestSingleComponent>();
            Assert.AreEqual(0, tc.testValue);

            TestCommandComponent cmd = new TestCommandComponent();
            cmd.frame = 1;
            cmd.id = 1;
            cmd.isFire = true;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            tc = world.GetSingletonComp<TestSingleComponent>();
            Assert.AreEqual(1, tc.testValue);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            tc = world.GetSingletonComp<TestSingleComponent>();
            Assert.AreEqual(1, tc.testValue);

            cmd = new TestCommandComponent();
            cmd.frame = 2;
            cmd.id = 1;
            cmd.isFire = true;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            tc = world.GetSingletonComp<TestSingleComponent>();
            Assert.AreEqual(2, tc.testValue);
        }

        #endregion

        #region 实体回滚测试

        [Test(Description = "实体回滚测试 1")]
        public void EnityRollBackTest_1()
        {
            WorldManager.IntervalTime = 100;

            LockStepEntityTestWorld world = (LockStepEntityTestWorld)WorldManager.CreateWorld<LockStepEntityTestWorld>();
            world.IsClient = true;
            world.IsStart = true;
            world.IsLocal = true;

            ConnectStatusComponent csc = world.GetSingletonComp<ConnectStatusComponent>();
            csc.confirmFrame = 0; //从目标帧之后开始计算

            PlayerComponent pc = new PlayerComponent();
            SelfComponent sc = new SelfComponent();

            /*EntityBase c1 =*/ world.CreateEntityImmediately(1, pc, sc);

            int createFrame = -1;
            int destroyFrame = -1;

            world.OnEntityOptimizeCreated += (entity) =>
            {
                //Debug.Log("OnEntityCreate " + entity.ID + " frame " + world.FrameCount);
                createFrame = world.FrameCount;
            };

            world.OnEntityOptimizeDestroyed += (entity) =>
            {
                //Debug.Log("OnEntityDestroyed " + entity.ID + " frame " + world.FrameCount);
                destroyFrame = world.FrameCount;
            };

            string tmp = (1 + "FireObject" + 1);
            int id = tmp.ToHash();

            LockStepInputSystem.commandCache.isFire = true;

            Assert.AreEqual(false, world.GetEntityIsExist(id)); //没执行前不存在这个对象

            world.CallRecalc();
            world.FixedLoop(100);

            LockStepInputSystem.commandCache.isFire = false;

            Assert.AreEqual(true, world.GetEntityIsExist(id)); //执行完存在这个对象

            TestCommandComponent cmd = new TestCommandComponent();
            cmd.frame = 1;
            cmd.id = 1;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            Assert.AreEqual(false, world.GetEntityIsExist(id)); //重计算后对象消失

            Assert.AreEqual(1, createFrame); //应该立即创建处这个对象
            Assert.AreEqual(1, destroyFrame); //在第一帧销毁这个对象
        }


        [Test(Description = "实体回滚测试 2")]
        public void EnityRollBackTest_2()
        {
            WorldManager.IntervalTime = 100;

            LockStepEntityTestWorld world = (LockStepEntityTestWorld)WorldManager.CreateWorld<LockStepEntityTestWorld>();
            world.IsClient = true;
            world.IsStart = true;
            world.IsLocal = true;

            ConnectStatusComponent csc = world.GetSingletonComp<ConnectStatusComponent>();
            csc.confirmFrame = 0; //从目标帧之后开始计算

            PlayerComponent pc = new PlayerComponent();
            SelfComponent sc = new SelfComponent();

            /*EntityBase c1 =*/ world.CreateEntityImmediately(1, pc, sc);

            int createFrame = -1;
            int destroyFrame = -1;

            world.OnEntityOptimizeCreated += (entity) =>
            {
                //Debug.Log("OnEntityCreate " + entity.ID + " frame " + world.FrameCount);

                createFrame = world.FrameCount;
            };

            world.OnEntityOptimizeDestroyed += (entity) =>
            {
                //Debug.Log("OnEntityDestroyed " + entity.ID + " frame " + world.FrameCount);
                destroyFrame = world.FrameCount;
            };

            string tmp = (1 + "FireObject" + 1);
            int id = tmp.ToHash();

            Assert.AreEqual(false, world.GetEntityIsExist(id)); //没执行前不存在这个对象

            LockStepInputSystem.commandCache.isFire = false;

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            Assert.AreEqual(false, world.GetEntityIsExist(id)); //执行完也不存在这个对象

            TestCommandComponent cmd = new TestCommandComponent();
            cmd.frame = 1;
            cmd.id = 1;
            cmd.isFire = true;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            Assert.AreEqual(true, world.GetEntityIsExist(id)); //重计算后对象出现

            Assert.AreEqual(1, createFrame);  //应该立即创建处这个对象
            Assert.AreEqual(-1, destroyFrame); //这个对象不销毁
        }

        [Test(Description = "实体回滚测试 3")]
        public void EnityRollBackTest_3()
        {
            WorldManager.IntervalTime = 100;

            ResourcesConfigManager.Initialize();

            LockStepEntityTestWorld world = (LockStepEntityTestWorld)WorldManager.CreateWorld<LockStepEntityTestWorld>();
            world.IsClient = true;
            world.IsStart = true;
            world.IsLocal = true;

            ConnectStatusComponent csc = world.GetSingletonComp<ConnectStatusComponent>();
            csc.confirmFrame = 0; //从目标帧之后开始计算

            PlayerComponent pc = new PlayerComponent();
            SelfComponent sc = new SelfComponent();

            /*EntityBase c1 =*/ world.CreateEntityImmediately(1, pc, sc);

            int createFrame = -1;
            int destroyFrame = -1;

            string tmp = (1 + "FireObject" + 1);
            int id = tmp.ToHash();

            string tmp2 = (2 + "FireObject" + 1);
            int id2 = tmp2.ToHash();

            world.OnEntityOptimizeCreated += (entity) =>
            {
                //Debug.Log("OnEntityCreate " + entity.ID + " frame " + world.FrameCount);

                if(entity.ID == id2)
                {
                    createFrame = world.FrameCount;
                }

            };

            world.OnEntityOptimizeDestroyed += (entity) =>
            {
                if (entity.ID == id2)
                {
                    destroyFrame = world.FrameCount;
                }
            };

            Assert.AreEqual(false, world.GetEntityIsExist(id)); //没执行前不存在这个对象
            Assert.AreEqual(false, world.GetEntityIsExist(id2)); //没执行前不存在这个对象

            LockStepInputSystem.commandCache.isFire = true;

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            //id1 存在 id2 不存在
            Assert.AreEqual(true, world.GetEntityIsExist(id));
            Assert.AreEqual(false, world.GetEntityIsExist(id2));

            LockStepInputSystem.commandCache.isFire = false;

            for (int i = 0; i < 3; i++)
            {
                world.CallRecalc();
                world.FixedLoop(WorldManager.IntervalTime);
            }

            TestCommandComponent cmd = new TestCommandComponent();
            cmd.frame = 1;
            cmd.id = 1;
            cmd.isFire = false;
            GlobalEvent.DispatchTypeEvent(cmd);

            cmd = new TestCommandComponent();
            cmd.frame = 2;
            cmd.id = 1;
            cmd.isFire = true;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();

            //id2 存在 id1 不存在
            Assert.AreEqual(true, world.GetEntityIsExist(id2));
            Assert.AreEqual(false, world.GetEntityIsExist(id));

            Assert.AreEqual(world.FrameCount, createFrame);  //应该立即创建处这个对象
            Assert.AreEqual(-1, destroyFrame); //这个对象不销毁
        }

        [Test(Description = "实体回滚测试 4")]
        public void EnityRollBackTest_4()
        {
            WorldManager.IntervalTime = 100;

            ResourcesConfigManager.Initialize();

            LockStepEntityTestWorld world = (LockStepEntityTestWorld)WorldManager.CreateWorld<LockStepEntityTestWorld>();
            world.IsClient = true;
            world.IsStart = true;
            world.IsLocal = true;

            ConnectStatusComponent csc = world.GetSingletonComp<ConnectStatusComponent>();
            csc.confirmFrame = 0; //从目标帧之后开始计算

            PlayerComponent pc = new PlayerComponent();
            SelfComponent sc = new SelfComponent();

            /*EntityBase c1 = */ world.CreateEntityImmediately(1, pc, sc);

            int createFrame = -1;
            int destroyFrame = -1;

            string tmp = (1 + "FireObject" + 1);
            int id = tmp.ToHash();

            string tmp2 = (2 + "FireObject" + 1);
            int id2 = tmp2.ToHash();

            world.OnEntityOptimizeCreated += (entity) =>
            {
                //Debug.Log("OnEntityCreate " + entity.ID + " frame " + world.FrameCount);

                if (entity.ID == id2)
                {
                    createFrame = world.FrameCount;
                }

            };

            world.OnEntityOptimizeDestroyed += (entity) =>
            {
                if (entity.ID == id2)
                {
                    destroyFrame = world.FrameCount;
                }
            };

            Assert.AreEqual(false, world.GetEntityIsExist(id)); //没执行前不存在这个对象
            Assert.AreEqual(false, world.GetEntityIsExist(id2)); //没执行前不存在这个对象

            LockStepInputSystem.commandCache.isFire = true;

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            //id1 存在 id2 不存在
            Assert.AreEqual(true, world.GetEntityIsExist(id));
            Assert.AreEqual(false, world.GetEntityIsExist(id2));

            LockStepInputSystem.commandCache.isFire = false;

            for (int i = 0; i < 10; i++)
            {
                world.CallRecalc();
                world.FixedLoop(WorldManager.IntervalTime);
            }

            TestCommandComponent cmd = new TestCommandComponent();
            cmd.frame = 1;
            cmd.id = 1;
            cmd.isFire = false;
            GlobalEvent.DispatchTypeEvent(cmd);

            cmd = new TestCommandComponent();
            cmd.frame = 2;
            cmd.id = 1;
            cmd.isFire = true;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();

            //id2 不存在 id1 不存在
            Assert.AreEqual(false, world.GetEntityIsExist(id2));
            Assert.AreEqual(false, world.GetEntityIsExist(id));

            Assert.AreEqual(-1, createFrame);  //应该不派发
            Assert.AreEqual(-1, destroyFrame); //不派发
        }

        #endregion

        #region Same消息测试

        [Test(Description = "Same消息测试 1")]
        public void SameMsgTest_1()
        {
            ResourcesConfigManager.Initialize();
            WorldManager.IntervalTime = 100;

            LockStepEntityTestWorld world = (LockStepEntityTestWorld)WorldManager.CreateWorld<LockStepEntityTestWorld>();
            world.IsClient = true;
            world.IsStart = true;
            world.IsLocal = true;

            ConnectStatusComponent csc = world.GetSingletonComp<ConnectStatusComponent>();
            csc.confirmFrame = 0; //从目标帧之后开始计算

            SelfComponent sc = new SelfComponent();

            EntityBase c1 = world.CreateEntityImmediately(1, sc);

            LockStepInputSystem.commandCache.moveDir.x = 0;

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);
            TestMoveComponent mc = c1.GetComp<TestMoveComponent>();

            Assert.AreEqual(0, mc.pos.x);

            LockStepInputSystem.commandCache.moveDir.x = 1000;

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            mc = c1.GetComp<TestMoveComponent>();

            Assert.AreEqual(1000, mc.pos.x);

            TestCommandComponent cmd = new TestCommandComponent();
            cmd.frame = 1;
            cmd.id = 1;
            cmd.moveDir.x = 1000;
            GlobalEvent.DispatchTypeEvent(cmd);

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);

            mc = c1.GetComp<TestMoveComponent>();

            Assert.AreEqual(1000, mc.pos.x);

            SameCommand sameMsg = new SameCommand();
            sameMsg.id = 1;
            sameMsg.frame = 2;

            GlobalEvent.DispatchTypeEvent(sameMsg);

            mc = c1.GetComp<TestMoveComponent>();

            world.CallRecalc();
            world.FixedLoop(WorldManager.IntervalTime);
            mc = c1.GetComp<TestMoveComponent>();


            Assert.AreEqual(0, mc.pos.x);
        }

        #endregion

        #region 断线重连测试

        [Test(Description = "断线重连测试")]
        public void OffLineReconnectTest()
        {

        }

        #endregion
    }
}
