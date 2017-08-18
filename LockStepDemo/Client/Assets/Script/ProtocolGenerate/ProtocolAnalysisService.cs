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
		InputManager.AddListener<InputNetworkMessageEvent>("debugmsg",ReceviceDebugMsg);
		InputManager.AddListener<InputNetworkMessageEvent>("destroyentitymsg",ReceviceDestroyEntityMsg);
		InputManager.AddListener<InputNetworkMessageEvent>("startsyncmsg",ReceviceStartSyncMsg);
		InputManager.AddListener<InputNetworkMessageEvent>("syncentitymsg",ReceviceSyncEntityMsg);
		InputManager.AddListener<InputNetworkMessageEvent>("commandcomponent",ReceviceCommandComponent);
	}

	public static void Dispose()
	{
		InputManager.RemoveListener<InputNetworkMessageEvent>("waitsynccomponent",ReceviceWaitSyncComponent);
		InputManager.RemoveListener<InputNetworkMessageEvent>("changecomponentmsg",ReceviceChangeComponentMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("changesingletoncomponentmsg",ReceviceChangeSingletonComponentMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("debugmsg",ReceviceDebugMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("destroyentitymsg",ReceviceDestroyEntityMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("startsyncmsg",ReceviceStartSyncMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("syncentitymsg",ReceviceSyncEntityMsg);
		InputManager.RemoveListener<InputNetworkMessageEvent>("commandcomponent",ReceviceCommandComponent);
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
		else if(cmd is Protocol.DebugMsg )
		{
			SendDebugMsg(cmd);
		}
		else if(cmd is Protocol.DestroyEntityMsg )
		{
			SendDestroyEntityMsg(cmd);
		}
		else if(cmd is Protocol.StartSyncMsg )
		{
			SendStartSyncMsg(cmd);
		}
		else if(cmd is Protocol.SyncEntityMsg )
		{
			SendSyncEntityMsg(cmd);
		}
		else if(cmd is CommandComponent )
		{
			SendCommandComponent(cmd);
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
	static void SendDebugMsg(IProtocolMessageInterface msg)
	{
		Protocol.DebugMsg e = (Protocol.DebugMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", e.frame);
		{
			List<object> list2 = new List<object>();
			for(int i2 = 0;i2 <e.infos.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", e.infos[i2].id);
				{
					List<object> list4 = new List<object>();
					for(int i4 = 0;i4 <e.infos[i2].infos.Count ; i4++)
					{
						Dictionary<string, object> data4 = new Dictionary<string, object>();
						data4.Add("m_compname", e.infos[i2].infos[i4].m_compName);
						data4.Add("content", e.infos[i2].infos[i4].content);
						list4.Add( data4);
					}
					data2.Add("infos",list4);
				}
				list2.Add( data2);
			}
			data.Add("infos",list2);
		}
		NetworkManager.SendMessage("debugmsg",data);
	}
	static void SendDestroyEntityMsg(IProtocolMessageInterface msg)
	{
		Protocol.DestroyEntityMsg e = (Protocol.DestroyEntityMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", e.frame);
		data.Add("id", e.id);
		NetworkManager.SendMessage("destroyentitymsg",data);
	}
	static void SendStartSyncMsg(IProtocolMessageInterface msg)
	{
		Protocol.StartSyncMsg e = (Protocol.StartSyncMsg)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", e.frame);
		data.Add("intervaltime", e.intervalTime);
		data.Add("createentityindex", e.createEntityIndex);
		data.Add("syncrule", (int)e.SyncRule);
		NetworkManager.SendMessage("startsyncmsg",data);
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
	static void SendCommandComponent(IProtocolMessageInterface msg)
	{
		CommandComponent e = (CommandComponent)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("isforward", e.isForward);
		data.Add("isback", e.isBack);
		data.Add("isright", e.isRight);
		data.Add("isleft", e.isLeft);
		data.Add("isfire", e.isFire);
		data.Add("id", e.id);
		data.Add("frame", e.frame);
		NetworkManager.SendMessage("commandcomponent",data);
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
	static void ReceviceDebugMsg(InputNetworkMessageEvent e)
	{
		Protocol.DebugMsg msg = new Protocol.DebugMsg();
		msg.frame = (int)e.Data["frame"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["infos"];
			List<Protocol.EntityInfo> list2 = new List<Protocol.EntityInfo>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.EntityInfo tmp2 = new Protocol.EntityInfo();
				tmp2.id = (int)data2[i2]["id"];
				{
					List<Dictionary<string, object>> data4 = (List<Dictionary<string, object>>)data2[i2]["infos"];
					List<Protocol.ComponentInfo> list4 = new List<Protocol.ComponentInfo>();
					for (int i4 = 0; i4 < data4.Count; i4++)
					{
						Protocol.ComponentInfo tmp4 = new Protocol.ComponentInfo();
						tmp4.m_compName = data4[i4]["m_compname"].ToString();
						tmp4.content = data4[i4]["content"].ToString();
						list4.Add(tmp4);
					}
					tmp2.infos =  list4;
				}
				list2.Add(tmp2);
			}
			msg.infos =  list2;
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
	static void ReceviceStartSyncMsg(InputNetworkMessageEvent e)
	{
		Protocol.StartSyncMsg msg = new Protocol.StartSyncMsg();
		msg.frame = (int)e.Data["frame"];
		msg.intervalTime = (int)e.Data["intervaltime"];
		msg.createEntityIndex = (int)e.Data["createentityindex"];
		msg.SyncRule = (SyncRule)e.Data["syncrule"];
		
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
	static void ReceviceCommandComponent(InputNetworkMessageEvent e)
	{
		CommandComponent msg = new CommandComponent();
		msg.isForward = (bool)e.Data["isforward"];
		msg.isBack = (bool)e.Data["isback"];
		msg.isRight = (bool)e.Data["isright"];
		msg.isLeft = (bool)e.Data["isleft"];
		msg.isFire = (bool)e.Data["isfire"];
		msg.id = (int)e.Data["id"];
		msg.frame = (int)e.Data["frame"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	#endregion
}
