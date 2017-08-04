//#pragma warning disable
//using LockStepDemo.Event;
//using LockStepDemo;
//using LockStepDemo.Service;
//using Protocol;
//using System;
//using System.Collections.Generic;

////指令解析类
////该类自动生成，请勿修改
//public static class ProtocolAnalysisService
//{
//	#region 消息发送
//	public static void SendMsg(this SyncSession session,ComponentBase msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		session.SendMsg("componentbase",data);
//	}
//	public static void SendMsg(this SyncSession session,MomentBase msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		session.SendMsg("momentbase",data);
//	}
//	public static void SendMsg(this SyncSession session,AssetComponent msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		data.Add("m_assetname", msg.m_assetName);
//		session.SendMsg("assetcomponent",data);
//	}
//	public static void SendMsg(this SyncSession session,CommandComponent msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		data.Add("isforward", msg.isForward);
//		data.Add("isback", msg.isBack);
//		data.Add("isright", msg.isRight);
//		data.Add("isleft", msg.isLeft);
//		data.Add("isfire", msg.isFire);
//		session.SendMsg("commandcomponent",data);
//	}
//	public static void SendMsg(this SyncSession session,InputComponent msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		data.Add("m_forward", msg.m_forward);
//		data.Add("m_fire", msg.m_fire);
//		session.SendMsg("inputcomponent",data);
//	}
//	public static void SendMsg(this SyncSession session,MoveComponent msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		data.Add("m_posx", msg.m_posx);
//		data.Add("m_posy", msg.m_posy);
//		data.Add("m_posz", msg.m_posz);
//		data.Add("m_dirx", msg.m_dirx);
//		data.Add("m_diry", msg.m_diry);
//		data.Add("m_dirz", msg.m_dirz);
//		data.Add("m_velocity", msg.m_velocity);
//		session.SendMsg("movecomponent",data);
//	}
//	public static void SendMsg(this SyncSession session,SyncComponent msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		data.Add("currentfixedframe", msg.currentFixedFrame);
//		data.Add("currenttime", msg.currentTime);
//		session.SendMsg("synccomponent",data);
//	}
//	public static void SendMsg(this SyncSession session,ViewComponent msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		session.SendMsg("viewcomponent",data);
//	}
//	public static void SendMsg(this SyncSession session,Protocol.GameSyncModule.playercomponent_c msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		session.SendMsg("playercomponent",data);
//	}
//	public static void SendMsg(this SyncSession session,Protocol.GameSyncModule.perfabcomponent_c msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//		data.Add("perfab", msg.perfab);
//		session.SendMsg("perfabcomponent",data);
//	}
//	public static void SendMsg(this SyncSession session,LockStepDemo.ServiceLogic.ConnectionComponent msg)
//	{
//		Dictionary<string, object> data = new Dictionary<string, object>();
//			{
//				Dictionary<string, object> data2 = new Dictionary<string, object>();
//				data.Add("m_session",data2);
//			}
//		session.SendMsg("connectioncomponent",data);
//	}
//	#endregion

//	#region 事件接收
//	public static void AnalysisAndDispatchMessage (SyncSession session,ProtocolRequestBase cmd)
//	{
//		switch (cmd.Key)
//		{
//			case  "componentbase":ReceviceComponentBase(session , cmd);break;
//			case  "momentbase":ReceviceMomentBase(session , cmd);break;
//			case  "assetcomponent":ReceviceAssetComponent(session , cmd);break;
//			case  "commandcomponent":ReceviceCommandComponent(session , cmd);break;
//			case  "inputcomponent":ReceviceInputComponent(session , cmd);break;
//			case  "movecomponent":ReceviceMoveComponent(session , cmd);break;
//			case  "synccomponent":ReceviceSyncComponent(session , cmd);break;
//			case  "viewcomponent":ReceviceViewComponent(session , cmd);break;
//			case  "playercomponent":Receviceplayercomponent_s(session , cmd);break;
//			case  "perfabcomponent":Receviceperfabcomponent_s(session , cmd);break;
//			case  "connectioncomponent":ReceviceConnectionComponent(session , cmd);break;
//			default:
//			Debug.LogError("SendCommand Exception : 不支持的消息类型!" + cmd.Key);
//				break;
//		}
//	}
//	static void ReceviceComponentBase(SyncSession session ,ProtocolRequestBase e)
//	{
//		ComponentBase msg = new ComponentBase();
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void ReceviceMomentBase(SyncSession session ,ProtocolRequestBase e)
//	{
//		MomentBase msg = new MomentBase();
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void ReceviceAssetComponent(SyncSession session ,ProtocolRequestBase e)
//	{
//		AssetComponent msg = new AssetComponent();
//		msg.m_assetName = e.m_data["m_assetname"].ToString();
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void ReceviceCommandComponent(SyncSession session ,ProtocolRequestBase e)
//	{
//		CommandComponent msg = new CommandComponent();
//		msg.isForward = (bool)e.m_data["isforward"];
//		msg.isBack = (bool)e.m_data["isback"];
//		msg.isRight = (bool)e.m_data["isright"];
//		msg.isLeft = (bool)e.m_data["isleft"];
//		msg.isFire = (bool)e.m_data["isfire"];
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void ReceviceInputComponent(SyncSession session ,ProtocolRequestBase e)
//	{
//		InputComponent msg = new InputComponent();
//		msg.m_forward = (bool)e.m_data["m_forward"];
//		msg.m_fire = (bool)e.m_data["m_fire"];
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void ReceviceMoveComponent(SyncSession session ,ProtocolRequestBase e)
//	{
//		MoveComponent msg = new MoveComponent();
//		msg.m_posx = (int)e.m_data["m_posx"];
//		msg.m_posy = (int)e.m_data["m_posy"];
//		msg.m_posz = (int)e.m_data["m_posz"];
//		msg.m_dirx = (int)e.m_data["m_dirx"];
//		msg.m_diry = (int)e.m_data["m_diry"];
//		msg.m_dirz = (int)e.m_data["m_dirz"];
//		msg.m_velocity = (int)e.m_data["m_velocity"];
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void ReceviceSyncComponent(SyncSession session ,ProtocolRequestBase e)
//	{
//		SyncComponent msg = new SyncComponent();
//		msg.currentFixedFrame = (int)e.m_data["currentfixedframe"];
//		msg.currentTime = (float)(double)e.m_data["currenttime"];
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void ReceviceViewComponent(SyncSession session ,ProtocolRequestBase e)
//	{
//		ViewComponent msg = new ViewComponent();
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void Receviceplayercomponent_s(SyncSession session ,ProtocolRequestBase e)
//	{
//		Protocol.GameSyncModule.playercomponent_s msg = new Protocol.GameSyncModule.playercomponent_s();
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void Receviceperfabcomponent_s(SyncSession session ,ProtocolRequestBase e)
//	{
//		Protocol.GameSyncModule.perfabcomponent_s msg = new Protocol.GameSyncModule.perfabcomponent_s();
//		msg.perfab = e.m_data["perfab"].ToString();
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	static void ReceviceConnectionComponent(SyncSession session ,ProtocolRequestBase e)
//	{
//		LockStepDemo.ServiceLogic.ConnectionComponent msg = new LockStepDemo.ServiceLogic.ConnectionComponent();
//		{
//			LockStepDemo.Service.SyncSession tmp2 = new LockStepDemo.Service.SyncSession();
//			msg.m_session = tmp2;
//		}
		
//		EventService.DispatchTypeEvent(session,msg);
//	}
//	#endregion
//}
