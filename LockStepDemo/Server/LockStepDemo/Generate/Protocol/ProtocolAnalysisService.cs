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
	public static void SendMsg(this SyncSession session,CommandComponent msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("isforward", msg.isForward);
		data.Add("isback", msg.isBack);
		data.Add("isright", msg.isRight);
		data.Add("isleft", msg.isLeft);
		data.Add("isfire", msg.isFire);
		session.SendMsg("commandcomponent",data);
	}
	public static void SendMsg(this SyncSession session,LockStepDemo.GameLogic.Component.WaitSyncComponent msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		session.SendMsg("waitsynccomponent",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.SyncEntityMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_id", msg.m_id);
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
	public static void SendMsg(this SyncSession session,Protocol.ChangeComponentMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_id", msg.m_id);
		data.Add("m_operation", (int)msg.m_operation);
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_compname", msg.info.m_compName);
				data2.Add("content", msg.info.content);
				data.Add("info",data2);
			}
		session.SendMsg("changecomponentmsg",data);
	}
	#endregion

	#region 事件接收
	public static void AnalysisAndDispatchMessage (SyncSession session,ProtocolRequestBase cmd)
	{
		switch (cmd.Key)
		{
			case  "commandcomponent":ReceviceCommandComponent(session , cmd);break;
			case  "waitsynccomponent":ReceviceWaitSyncComponent(session , cmd);break;
			case  "syncentitymsg":ReceviceSyncEntityMsg(session , cmd);break;
			case  "changecomponentmsg":ReceviceChangeComponentMsg(session , cmd);break;
			default:
			Debug.LogError("SendCommand Exception : 不支持的消息类型!" + cmd.Key);
				break;
		}
	}
	static void ReceviceCommandComponent(SyncSession session ,ProtocolRequestBase e)
	{
		CommandComponent msg = new CommandComponent();
		msg.isForward = (bool)e.m_data["isforward"];
		msg.isBack = (bool)e.m_data["isback"];
		msg.isRight = (bool)e.m_data["isright"];
		msg.isLeft = (bool)e.m_data["isleft"];
		msg.isFire = (bool)e.m_data["isfire"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceWaitSyncComponent(SyncSession session ,ProtocolRequestBase e)
	{
		LockStepDemo.GameLogic.Component.WaitSyncComponent msg = new LockStepDemo.GameLogic.Component.WaitSyncComponent();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceSyncEntityMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.SyncEntityMsg msg = new Protocol.SyncEntityMsg();
		msg.m_id = (int)e.m_data["m_id"];
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
	static void ReceviceChangeComponentMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.ChangeComponentMsg msg = new Protocol.ChangeComponentMsg();
		msg.m_id = (int)e.m_data["m_id"];
		msg.m_operation = (Protocol.ChangeStatus)e.m_data["m_operation"];
		{
			Protocol.ComponentInfo tmp2 = new Protocol.ComponentInfo();
			tmp2.m_compName = e.m_data["m_compname"].ToString();
			tmp2.content = e.m_data["content"].ToString();
			msg.info = tmp2;
		}
		
		EventService.DispatchTypeEvent(session,msg);
	}
	#endregion
}
