#pragma warning disable
using Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

//指令解析类
//该类自动生成，请勿修改
public class ProtocolAnalysisService
{
	#region 外部调用
	public static void Init()
	{
		InputManager.AddListener<InputNetworkMessageEvent>("waitsynccomponent",ReceviceWaitSyncComponent);
		InputManager.AddListener<InputNetworkMessageEvent>("commandcomponent",ReceviceCommandComponent);
		InputManager.AddListener<InputNetworkMessageEvent>("changecomponentmsg",ReceviceChangeComponentMsg);
		InputManager.AddListener<InputNetworkMessageEvent>("syncentitymsg",ReceviceSyncEntityMsg);
	}

	public static void Dispose()
	{
		InputManager.RemoveListener<InputNetworkMessageEvent>("waitsynccomponent",ReceviceWaitSyncComponent);
		InputManager.RemoveListener<InputNetworkMessageEvent>("commandcomponent",ReceviceCommandComponent);
		InputManager.RemoveListener<InputNetworkMessageEvent>("changecomponentmsg",ReceviceChangeComponentMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("syncentitymsg",ReceviceSyncEntityMsg);
	}
	public static void SendCommand (IProtocolMessageInterface cmd)
	{
		if(cmd is LockStepDemo.GameLogic.Component.WaitSyncComponent )
		{
			SendWaitSyncComponent(cmd);
		}
		else if(cmd is CommandComponent )
		{
			SendCommandComponent(cmd);
		}
		else if(cmd is Protocol.ChangeComponentMsg )
		{
			SendChangeComponentMsg(cmd);
		}
		else if(cmd is Protocol.SyncEntityMsg )
		{
			SendSyncEntityMsg(cmd);
		}
		else
		{
			throw new Exception("SendCommand Exception : 不支持的消息类型!" + cmd.GetType());
		}
	}
	static void SendWaitSyncComponent(IProtocolMessageInterface msg)
	{
		LockStepDemo.GameLogic.Component.WaitSyncComponent e = (LockStepDemo.GameLogic.Component.WaitSyncComponent)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("waitsynccomponent",data);
	}
	static void SendCommandComponent(IProtocolMessageInterface msg)
	{
		CommandComponent e = (CommandComponent)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("isforward", e.isForward);
		data.Add("isback", e.isBack);
		data.Add("isright", e.isRight);
		data.Add("isleft", e.isLeft);
		data.Add("isfire", e.isFire);
		NetworkManager.SendMessage("commandcomponent",data);
	}
	static void SendChangeComponentMsg(IProtocolMessageInterface msg)
	{
		Protocol.ChangeComponentMsg e = (Protocol.ChangeComponentMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_id", e.m_id);
		data.Add("m_operation", (int)e.m_operation);
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_compname", e.info.m_compName);
				data2.Add("content", e.info.content);
				data.Add("info",data2);
			}
		NetworkManager.SendMessage("changecomponentmsg",data);
	}
	static void SendSyncEntityMsg(IProtocolMessageInterface msg)
	{
		Protocol.SyncEntityMsg e = (Protocol.SyncEntityMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_id", e.m_id);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <e.infos.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_compname", e.infos[i2].m_compName);
				data2.Add("content", e.infos[i2].content);
				list2.Add( data2);
			}
			data.Add("infos",list2);
		}
		NetworkManager.SendMessage("syncentitymsg",data);
	}
	#endregion

	#region 事件接收
	static void ReceviceWaitSyncComponent(InputNetworkMessageEvent e)
	{
		LockStepDemo.GameLogic.Component.WaitSyncComponent msg = new LockStepDemo.GameLogic.Component.WaitSyncComponent();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void ReceviceCommandComponent(InputNetworkMessageEvent e)
	{
		CommandComponent msg = new CommandComponent();
		msg.isForward = (bool)e.Data["isforward"];
		msg.isBack = (bool)e.Data["isback"];
		msg.isRight = (bool)e.Data["isright"];
		msg.isLeft = (bool)e.Data["isleft"];
		msg.isFire = (bool)e.Data["isfire"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void ReceviceChangeComponentMsg(InputNetworkMessageEvent e)
	{
		Protocol.ChangeComponentMsg msg = new Protocol.ChangeComponentMsg();
		msg.m_id = (int)e.Data["m_id"];
		msg.m_operation = (ChangeStatus)e.Data["m_operation"];
		{
			Protocol.ComponentInfo tmp2 = new Protocol.ComponentInfo();
			tmp2.m_compName = e.Data["m_compname"].ToString();
			tmp2.content = e.Data["content"].ToString();
			msg.info = tmp2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void ReceviceSyncEntityMsg(InputNetworkMessageEvent e)
	{
		Protocol.SyncEntityMsg msg = new Protocol.SyncEntityMsg();
		msg.m_id = (int)e.Data["m_id"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["infos"];
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
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	#endregion
}
