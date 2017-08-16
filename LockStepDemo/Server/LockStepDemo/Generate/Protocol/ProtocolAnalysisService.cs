#pragma warning disable
using LockStepDemo.Event;
using LockStepDemo;
using LockStepDemo.Service;
using Protocol;
using System;
using System.Collections.Generic;

//指令解析类
//该类自动生成，请勿修改
public static class ProtocolAnalysisService
{
	#region 消息发送
	public static void SendMsg (SyncSession session,IProtocolMessageInterface msg)
	{
		string key = msg.GetType().Name.ToLower();
		switch (key)
		{
			case  "waitsynccomponent":SendWaitSyncComponent(session , (LockStepDemo.GameLogic.Component.WaitSyncComponent)msg);break;
			case  "changecomponentmsg":SendChangeComponentMsg(session , (Protocol.ChangeComponentMsg)msg);break;
			case  "changesingletoncomponentmsg":SendChangeSingletonComponentMsg(session , (Protocol.ChangeSingletonComponentMsg)msg);break;
			case  "destroyentitymsg":SendDestroyEntityMsg(session , (Protocol.DestroyEntityMsg)msg);break;
			case  "startsyncmsg":SendStartSyncMsg(session , (Protocol.StartSyncMsg)msg);break;
			case  "syncentitymsg":SendSyncEntityMsg(session , (Protocol.SyncEntityMsg)msg);break;
			case  "commandcomponent":SendCommandComponent(session , (CommandComponent)msg);break;
			default:
			Debug.LogError("SendCommand Exception : 不支持的消息类型!" + key);
				break;
		}
	}
	static void SendWaitSyncComponent(SyncSession session,LockStepDemo.GameLogic.Component.WaitSyncComponent msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		session.SendMsg("waitsynccomponent",data);
	}
	static void SendChangeComponentMsg(SyncSession session,Protocol.ChangeComponentMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		data.Add("id", msg.id);
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_compname", msg.info.m_compName);
				data2.Add("content", msg.info.content);
				data.Add("info",data2);
			}
		session.SendMsg("changecomponentmsg",data);
	}
	static void SendChangeSingletonComponentMsg(SyncSession session,Protocol.ChangeSingletonComponentMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_compname", msg.info.m_compName);
				data2.Add("content", msg.info.content);
				data.Add("info",data2);
			}
		session.SendMsg("changesingletoncomponentmsg",data);
	}
	static void SendDestroyEntityMsg(SyncSession session,Protocol.DestroyEntityMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		data.Add("id", msg.id);
		session.SendMsg("destroyentitymsg",data);
	}
	static void SendStartSyncMsg(SyncSession session,Protocol.StartSyncMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		data.Add("intervaltime", msg.intervalTime);
		session.SendMsg("startsyncmsg",data);
	}
	static void SendSyncEntityMsg(SyncSession session,Protocol.SyncEntityMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		data.Add("id", msg.id);
		{
			List<object> list2 = new List<object>();
			for(int i2 = 0;i2 <msg.infos.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_compname", msg.infos[i2].m_compName);
				data2.Add("content", msg.infos[i2].content);
				list2.Add( data2);
			}
			data.Add("infos",list2);
		}
		session.SendMsg("syncentitymsg",data);
	}
	static void SendCommandComponent(SyncSession session,CommandComponent msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("isforward", msg.isForward);
		data.Add("isback", msg.isBack);
		data.Add("isright", msg.isRight);
		data.Add("isleft", msg.isLeft);
		data.Add("isfire", msg.isFire);
		data.Add("id", msg.id);
		data.Add("frame", msg.frame);
		session.SendMsg("commandcomponent",data);
	}
	#endregion

	#region 事件接收
	public static void AnalysisAndDispatchMessage (SyncSession session,ProtocolRequestBase cmd)
	{
		switch (cmd.Key)
		{
			case  "waitsynccomponent":ReceviceWaitSyncComponent(session , cmd);break;
			case  "changecomponentmsg":ReceviceChangeComponentMsg(session , cmd);break;
			case  "changesingletoncomponentmsg":ReceviceChangeSingletonComponentMsg(session , cmd);break;
			case  "destroyentitymsg":ReceviceDestroyEntityMsg(session , cmd);break;
			case  "startsyncmsg":ReceviceStartSyncMsg(session , cmd);break;
			case  "syncentitymsg":ReceviceSyncEntityMsg(session , cmd);break;
			case  "commandcomponent":ReceviceCommandComponent(session , cmd);break;
			default:
			Debug.LogError("Recevice Exception : 不支持的消息类型!" + cmd.Key);
				break;
		}
	}
	static void ReceviceWaitSyncComponent(SyncSession session ,ProtocolRequestBase e)
	{
		LockStepDemo.GameLogic.Component.WaitSyncComponent msg = new LockStepDemo.GameLogic.Component.WaitSyncComponent();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceChangeComponentMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.ChangeComponentMsg msg = new Protocol.ChangeComponentMsg();
		msg.frame = (int)e.m_data["frame"];
		msg.id = (int)e.m_data["id"];
		{
			Dictionary<string, object> data2 = (Dictionary<string, object>)e.m_data["info"];
			Protocol.ComponentInfo tmp2 = new Protocol.ComponentInfo();
			tmp2.m_compName = data2["m_compname"].ToString();
			tmp2.content = data2["content"].ToString();
			msg.info = tmp2;
		}
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceChangeSingletonComponentMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.ChangeSingletonComponentMsg msg = new Protocol.ChangeSingletonComponentMsg();
		msg.frame = (int)e.m_data["frame"];
		{
			Dictionary<string, object> data2 = (Dictionary<string, object>)e.m_data["info"];
			Protocol.ComponentInfo tmp2 = new Protocol.ComponentInfo();
			tmp2.m_compName = data2["m_compname"].ToString();
			tmp2.content = data2["content"].ToString();
			msg.info = tmp2;
		}
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceDestroyEntityMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.DestroyEntityMsg msg = new Protocol.DestroyEntityMsg();
		msg.frame = (int)e.m_data["frame"];
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceStartSyncMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.StartSyncMsg msg = new Protocol.StartSyncMsg();
		msg.frame = (int)e.m_data["frame"];
		msg.intervalTime = (int)e.m_data["intervaltime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceSyncEntityMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.SyncEntityMsg msg = new Protocol.SyncEntityMsg();
		msg.frame = (int)e.m_data["frame"];
		msg.id = (int)e.m_data["id"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.m_data["infos"];
			List<Protocol.ComponentInfo> list2 = new List<Protocol.ComponentInfo>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.ComponentInfo tmp2 = new Protocol.ComponentInfo();
				tmp2.m_compName = data2[i2]["m_compname"].ToString();
				tmp2.content = data2[i2]["content"].ToString();
				list2.Add(tmp2);
			}
			msg.infos =  list2;
		}
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceCommandComponent(SyncSession session ,ProtocolRequestBase e)
	{
		CommandComponent msg = new CommandComponent();
		msg.isForward = (bool)e.m_data["isforward"];
		msg.isBack = (bool)e.m_data["isback"];
		msg.isRight = (bool)e.m_data["isright"];
		msg.isLeft = (bool)e.m_data["isleft"];
		msg.isFire = (bool)e.m_data["isfire"];
		msg.id = (int)e.m_data["id"];
		msg.frame = (int)e.m_data["frame"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	#endregion
}
