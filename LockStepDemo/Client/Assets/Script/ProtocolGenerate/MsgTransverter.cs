using Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

//指令解析类
//该类自动生成，请勿修改
public class MsgTransverter
{
	#region 外部调用
	public static void Init()
	{
		GlobalEvent.AddTypeEvent<CreateFlyObjectCmd>(ReceviceCreateFlyObjectCmd);
		GlobalEvent.AddTypeEvent<DestroyFlyObjectCmd>(ReceviceDestroyFlyObjectCmd);
		GlobalEvent.AddTypeEvent<CreateCharacterCmd>(ReceviceCreateCharacterCmd);
		GlobalEvent.AddTypeEvent<RemoveCharacterCmd>(ReceviceRemoveCharacterCmd);
		GlobalEvent.AddTypeEvent<CreateSkillTokenCmd>(ReceviceCreateSkillTokenCmd);
		GlobalEvent.AddTypeEvent<DieCmd>(ReceviceDieCmd);
		GlobalEvent.AddTypeEvent<DamageCmd>(ReceviceDamageCmd);
		GlobalEvent.AddTypeEvent<RecoverCmd>(ReceviceRecoverCmd);
		GlobalEvent.AddTypeEvent<AddBuffCmd>(ReceviceAddBuffCmd);
		GlobalEvent.AddTypeEvent<RemoveBuffCmd>(ReceviceRemoveBuffCmd);
		GlobalEvent.AddTypeEvent<BlowFlyCmd>(ReceviceBlowFlyCmd);
		GlobalEvent.AddTypeEvent<ChangeWeaponCmd>(ReceviceChangeWeaponCmd);
		GlobalEvent.AddTypeEvent<ResurgenceCmd>(ReceviceResurgenceCmd);
		GlobalEvent.AddTypeEvent<AttackCmd>(ReceviceAttackCmd);
		GlobalEvent.AddTypeEvent<MoveCmd>(ReceviceMoveCmd);
		GlobalEvent.AddTypeEvent<SkillCmd>(ReceviceSkillCmd);
		GlobalEvent.AddTypeEvent<RotationCmd>(ReceviceRotationCmd);
		GlobalEvent.AddTypeEvent<CreateTrapCmd>(ReceviceCreateTrapCmd);
		GlobalEvent.AddTypeEvent<TrapTriggerCmd>(ReceviceTrapTriggerCmd);
		GlobalEvent.AddTypeEvent<CreateBloodVialCmd>(ReceviceCreateBloodVialCmd);
		GlobalEvent.AddTypeEvent<PickUpBloodVialCmd>(RecevicePickUpBloodVialCmd);
		GlobalEvent.AddTypeEvent<DestroyBloodVialCmd>(ReceviceDestroyBloodVialCmd);
		GlobalEvent.AddTypeEvent<PickUpItemCmd>(RecevicePickUpItemCmd);
		GlobalEvent.AddTypeEvent<DestroyItemCmd>(ReceviceDestroyItemCmd);

        ApplicationManager.s_OnApplicationOnDrawGizmos += OnDrawGizmos;
	}

	public static void Dispose()
	{
		GlobalEvent.RemoveTypeEvent<CreateFlyObjectCmd>(ReceviceCreateFlyObjectCmd);
		GlobalEvent.RemoveTypeEvent<DestroyFlyObjectCmd>(ReceviceDestroyFlyObjectCmd);
		GlobalEvent.RemoveTypeEvent<CreateCharacterCmd>(ReceviceCreateCharacterCmd);
		GlobalEvent.RemoveTypeEvent<RemoveCharacterCmd>(ReceviceRemoveCharacterCmd);
		GlobalEvent.RemoveTypeEvent<CreateSkillTokenCmd>(ReceviceCreateSkillTokenCmd);
		GlobalEvent.RemoveTypeEvent<DieCmd>(ReceviceDieCmd);
		GlobalEvent.RemoveTypeEvent<DamageCmd>(ReceviceDamageCmd);
		GlobalEvent.RemoveTypeEvent<RecoverCmd>(ReceviceRecoverCmd);
		GlobalEvent.RemoveTypeEvent<AddBuffCmd>(ReceviceAddBuffCmd);
		GlobalEvent.RemoveTypeEvent<RemoveBuffCmd>(ReceviceRemoveBuffCmd);
		GlobalEvent.RemoveTypeEvent<BlowFlyCmd>(ReceviceBlowFlyCmd);
		GlobalEvent.RemoveTypeEvent<ChangeWeaponCmd>(ReceviceChangeWeaponCmd);
		GlobalEvent.RemoveTypeEvent<ResurgenceCmd>(ReceviceResurgenceCmd);
		GlobalEvent.RemoveTypeEvent<AttackCmd>(ReceviceAttackCmd);
		GlobalEvent.RemoveTypeEvent<MoveCmd>(ReceviceMoveCmd);
		GlobalEvent.RemoveTypeEvent<SkillCmd>(ReceviceSkillCmd);
		GlobalEvent.RemoveTypeEvent<RotationCmd>(ReceviceRotationCmd);
		GlobalEvent.RemoveTypeEvent<CreateTrapCmd>(ReceviceCreateTrapCmd);
		GlobalEvent.RemoveTypeEvent<TrapTriggerCmd>(ReceviceTrapTriggerCmd);
		GlobalEvent.RemoveTypeEvent<CreateBloodVialCmd>(ReceviceCreateBloodVialCmd);
		GlobalEvent.RemoveTypeEvent<PickUpBloodVialCmd>(RecevicePickUpBloodVialCmd);
		GlobalEvent.RemoveTypeEvent<DestroyBloodVialCmd>(ReceviceDestroyBloodVialCmd);
		GlobalEvent.RemoveTypeEvent<PickUpItemCmd>(RecevicePickUpItemCmd);
		GlobalEvent.RemoveTypeEvent<DestroyItemCmd>(ReceviceDestroyItemCmd);
	}
	#endregion

	#region 事件接收
	static void ReceviceCreateFlyObjectCmd(CreateFlyObjectCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceDestroyFlyObjectCmd(DestroyFlyObjectCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceCreateCharacterCmd(CreateCharacterCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceRemoveCharacterCmd(RemoveCharacterCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceCreateSkillTokenCmd(CreateSkillTokenCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceDieCmd(DieCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceDamageCmd(DamageCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceRecoverCmd(RecoverCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceAddBuffCmd(AddBuffCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceRemoveBuffCmd(RemoveBuffCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceBlowFlyCmd(BlowFlyCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceChangeWeaponCmd(ChangeWeaponCmd e , params object[] objs)
	{
		CommandRouteService.RecevicePursueCommand(e);
	}
	static void ReceviceResurgenceCmd(ResurgenceCmd e , params object[] objs)
	{
		CommandRouteService.RecevicePursueCommand(e);
	}
	static void ReceviceAttackCmd(AttackCmd e , params object[] objs)
	{
		CommandRouteService.RecevicePursueCommand(e);
	}

    static List<Vector3> list = new List<Vector3>();
	static void ReceviceMoveCmd(MoveCmd e , params object[] objs)
	{
        //Debug.Log("OnDrawGizmos "  + e.m_pos);

        CommandRouteService.RecevicePursueCommand(e);

        list.Add(e.m_pos);

	}
	static void ReceviceSkillCmd(SkillCmd e , params object[] objs)
	{
		CommandRouteService.RecevicePursueCommand(e);
	}
	static void ReceviceRotationCmd(RotationCmd e , params object[] objs)
	{
		CommandRouteService.RecevicePursueCommand(e);
	}
	static void ReceviceCreateTrapCmd(CreateTrapCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceTrapTriggerCmd(TrapTriggerCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceCreateBloodVialCmd(CreateBloodVialCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void RecevicePickUpBloodVialCmd(PickUpBloodVialCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceDestroyBloodVialCmd(DestroyBloodVialCmd e , params object[] objs)
	{
        

		CommandRouteService.ReceviceSyncCommand(e);
	}
	static void RecevicePickUpItemCmd(PickUpItemCmd e , params object[] objs)
	{
        //Debug.Log("RecevicePickUpItemCmd");

        CommandRouteService.ReceviceSyncCommand(e);
	}
	static void ReceviceDestroyItemCmd(DestroyItemCmd e , params object[] objs)
	{
		CommandRouteService.ReceviceSyncCommand(e);
	}
    #endregion

    static void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < list.Count - 1; i++)
        {
            Gizmos.DrawLine(list[i], list[i + 1]);
        }
    }
}
