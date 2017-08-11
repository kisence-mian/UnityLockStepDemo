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
		InputManager.AddListener<InputNetworkMessageEvent>("changecomponentmsg",ReceviceChangeComponentMsg);
		InputManager.AddListener<InputNetworkMessageEvent>("changesingletoncomponentmsg",ReceviceChangeSingletonComponentMsg);
		InputManager.AddListener<InputNetworkMessageEvent>("destroyentitymsg",ReceviceDestroyEntityMsg);
		InputManager.AddListener<InputNetworkMessageEvent>("syncentitymsg",ReceviceSyncEntityMsg);
	}

	public static void Dispose()
	{
		InputManager.RemoveListener<InputNetworkMessageEvent>("waitsynccomponent",ReceviceWaitSyncComponent);
		InputManager.RemoveListener<InputNetworkMessageEvent>("changecomponentmsg",ReceviceChangeComponentMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("changesingletoncomponentmsg",ReceviceChangeSingletonComponentMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("destroyentitymsg",ReceviceDestroyEntityMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("syncentitymsg",ReceviceSyncEntityMsg);
	}
	public static void SendCommand (IProtocolMessageInterface cmd)
	{
		if(cmd is LockStepDemo.GameLogic.Component.WaitSyncComponent )
		{
			SendWaitSyncComponent(cmd);
		}
		else if(cmd is Protocol.ChangeComponentMsg )
		{
			SendChangeComponentMsg(cmd);
		}
		else if(cmd is Protocol.ChangeSingletonComponentMsg )
		{
			SendChangeSingletonComponentMsg(cmd);
		}
		else if(cmd is Protocol.DestroyEntityMsg )
		{
			SendDestroyEntityMsg(cmd);
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
	static void SendChangeComponentMsg(IProtocolMessageInterface msg)
	{
		Protocol.ChangeComponentMsg e = (Protocol.ChangeComponentMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", e.frame);
		data.Add("id", e.id);
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_compname", e.info.m_compName);
				data2.Add("content", e.info.content);
				data.Add("info",data2);
			}
		NetworkManager.SendMessage("changecomponentmsg",data);
	}
	static void SendChangeSingletonComponentMsg(IProtocolMessageInterface msg)
	{
		Protocol.ChangeSingletonComponentMsg e = (Protocol.ChangeSingletonComponentMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", e.frame);
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_compname", e.info.m_compName);
				data2.Add("content", e.info.content);
				data.Add("info",data2);
			}
		NetworkManager.SendMessage("changesingletoncomponentmsg",data);
	}
	static void SendDestroyEntityMsg(IProtocolMessageInterface msg)
	{
		Protocol.DestroyEntityMsg e = (Protocol.DestroyEntityMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", e.frame);
		data.Add("id", e.id);
		NetworkManager.SendMessage("destroyentitymsg",data);
	}
	static void SendSyncEntityMsg(IProtocolMessageInterface msg)
	{
		Protocol.SyncEntityMsg e = (Protocol.SyncEntityMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", e.frame);
		data.Add("id", e.id);
		{
			List<object> list2 = new List<object>();
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
	static void ReceviceChangeComponentMsg(InputNetworkMessageEvent e)
	{
		Protocol.ChangeComponentMsg msg = new Protocol.ChangeComponentMsg();
		msg.frame = (int)e.Data["frame"];
		msg.id = (int)e.Data["id"];
		{
			Dictionary<string, object> data2 = (Dictionary<string, object>)e.Data["info"];
			Protocol.ComponentInfo tmp2 = new Protocol.ComponentInfo();
			tmp2.m_compName = data2["m_compname"].ToString();
			tmp2.content = data2["content"].ToString();
			msg.info = tmp2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void ReceviceChangeSingletonComponentMsg(InputNetworkMessageEvent e)
	{
		Protocol.ChangeSingletonComponentMsg msg = new Protocol.ChangeSingletonComponentMsg();
		msg.frame = (int)e.Data["frame"];
		{
			Dictionary<string, object> data2 = (Dictionary<string, object>)e.Data["info"];
			Protocol.ComponentInfo tmp2 = new Protocol.ComponentInfo();
			tmp2.m_compName = data2["m_compname"].ToString();
			tmp2.content = data2["content"].ToString();
			msg.info = tmp2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void ReceviceDestroyEntityMsg(InputNetworkMessageEvent e)
	{
		Protocol.DestroyEntityMsg msg = new Protocol.DestroyEntityMsg();
		msg.frame = (int)e.Data["frame"];
		msg.id = (int)e.Data["id"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void ReceviceSyncEntityMsg(InputNetworkMessageEvent e)
	{
		Protocol.SyncEntityMsg msg = new Protocol.SyncEntityMsg();
		msg.frame = (int)e.Data["frame"];
		msg.id = (int)e.Data["id"];
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
