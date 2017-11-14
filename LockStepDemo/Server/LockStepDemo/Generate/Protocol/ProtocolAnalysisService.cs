#pragma warning disable
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
			case  "affirmmsg":SendAffirmMsg(session , (Protocol.AffirmMsg)msg);break;
			case  "changecomponentmsg":SendChangeComponentMsg(session , (Protocol.ChangeComponentMsg)msg);break;
			case  "changesingletoncomponentmsg":SendChangeSingletonComponentMsg(session , (Protocol.ChangeSingletonComponentMsg)msg);break;
			case  "commandmsg":SendCommandMsg(session , (Protocol.CommandMsg)msg);break;
			case  "debugmsg":SendDebugMsg(session , (Protocol.DebugMsg)msg);break;
			case  "destroyentitymsg":SendDestroyEntityMsg(session , (Protocol.DestroyEntityMsg)msg);break;
			case  "pursuemsg":SendPursueMsg(session , (Protocol.PursueMsg)msg);break;
			case  "querycommand":SendQueryCommand(session , (Protocol.QueryCommand)msg);break;
			case  "startsyncmsg":SendStartSyncMsg(session , (Protocol.StartSyncMsg)msg);break;
			case  "syncentitymsg":SendSyncEntityMsg(session , (Protocol.SyncEntityMsg)msg);break;
			case  "commandcomponent":SendCommandComponent(session , (CommandComponent)msg);break;
			case  "playerbuycharacter_c":SendPlayerBuyCharacter_c(session , (PlayerBuyCharacter_c)msg);break;
			case  "playerloginmsg_c":SendPlayerLoginMsg_c(session , (PlayerLoginMsg_c)msg);break;
			case  "playermatchmsg_c":SendPlayerMatchMsg_c(session , (PlayerMatchMsg_c)msg);break;
			case  "playerrename_c":SendPlayerRename_c(session , (PlayerRename_c)msg);break;
			case  "playerresurgence_c":SendPlayerResurgence_c(session , (PlayerResurgence_c)msg);break;
			case  "playerselectcharacter_c":SendPlayerSelectCharacter_c(session , (PlayerSelectCharacter_c)msg);break;
			case  "playersettlement_c":SendPlayerSettlement_c(session , (PlayerSettlement_c)msg);break;
			default:
			Debug.LogError("SendCommand Exception : 不支持的消息类型!" + key);
				break;
		}
	}
	static void SendAffirmMsg(SyncSession session,Protocol.AffirmMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("index", msg.index);
		data.Add("time", msg.time);
		session.SendMsg("affirmmsg",data);
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
	static void SendCommandMsg(SyncSession session,Protocol.CommandMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("index", msg.index);
		data.Add("servertime", msg.serverTime);
		{
			List<object> list2 = new List<object>();
			for(int i2 = 0;i2 <msg.msg.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("frame", msg.msg[i2].frame);
				data2.Add("id", msg.msg[i2].id);
					{
						Dictionary<string, object> data4 = new Dictionary<string, object>();
						data4.Add("x", msg.msg[i2].moveDir.x);
						data4.Add("y", msg.msg[i2].moveDir.y);
						data4.Add("z", msg.msg[i2].moveDir.z);
						data2.Add("movedir",data4);
					}
					{
						Dictionary<string, object> data4 = new Dictionary<string, object>();
						data4.Add("x", msg.msg[i2].skillDir.x);
						data4.Add("y", msg.msg[i2].skillDir.y);
						data4.Add("z", msg.msg[i2].skillDir.z);
						data2.Add("skilldir",data4);
					}
				data2.Add("element1", msg.msg[i2].element1);
				data2.Add("element2", msg.msg[i2].element2);
				data2.Add("isfire", msg.msg[i2].isFire);
				list2.Add( data2);
			}
			data.Add("msg",list2);
		}
		session.SendMsg("commandmsg",data);
	}
	static void SendDebugMsg(SyncSession session,Protocol.DebugMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		{
			List<object> list2 = new List<object>();
			for(int i2 = 0;i2 <msg.infos.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.infos[i2].id);
				{
					List<object> list4 = new List<object>();
					for(int i4 = 0;i4 <msg.infos[i2].infos.Count ; i4++)
					{
						Dictionary<string, object> data4 = new Dictionary<string, object>();
						data4.Add("m_compname", msg.infos[i2].infos[i4].m_compName);
						data4.Add("content", msg.infos[i2].infos[i4].content);
						list4.Add( data4);
					}
					data2.Add("infos",list4);
				}
				list2.Add( data2);
			}
			data.Add("infos",list2);
		}
		session.SendMsg("debugmsg",data);
	}
	static void SendDestroyEntityMsg(SyncSession session,Protocol.DestroyEntityMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		data.Add("id", msg.id);
		session.SendMsg("destroyentitymsg",data);
	}
	static void SendPursueMsg(SyncSession session,Protocol.PursueMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", msg.id);
		data.Add("recalcframe", msg.recalcFrame);
		data.Add("frame", msg.frame);
		data.Add("advancecount", msg.advanceCount);
		data.Add("servertime", msg.serverTime);
		data.Add("updatespeed", msg.updateSpeed);
		session.SendMsg("pursuemsg",data);
	}
	static void SendQueryCommand(SyncSession session,Protocol.QueryCommand msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		data.Add("id", msg.id);
		session.SendMsg("querycommand",data);
	}
	static void SendStartSyncMsg(SyncSession session,Protocol.StartSyncMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		data.Add("advancecount", msg.advanceCount);
		data.Add("intervaltime", msg.intervalTime);
		data.Add("createentityindex", msg.createEntityIndex);
		data.Add("randomseed", msg.randomSeed);
		data.Add("syncrule", (int)msg.SyncRule);
		session.SendMsg("startsyncmsg",data);
	}
	static void SendSyncEntityMsg(SyncSession session,Protocol.SyncEntityMsg msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("frame", msg.frame);
		{
			List<object> list2 = new List<object>();
			for(int i2 = 0;i2 <msg.infos.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.infos[i2].id);
				{
					List<object> list4 = new List<object>();
					for(int i4 = 0;i4 <msg.infos[i2].infos.Count ; i4++)
					{
						Dictionary<string, object> data4 = new Dictionary<string, object>();
						data4.Add("m_compname", msg.infos[i2].infos[i4].m_compName);
						data4.Add("content", msg.infos[i2].infos[i4].content);
						list4.Add( data4);
					}
					data2.Add("infos",list4);
				}
				list2.Add( data2);
			}
			data.Add("infos",list2);
		}
		{
			List<object> list = new List<object>();
			for(int i = 0;i <msg.destroyList.Count ; i++)
			{
				list.Add( msg.destroyList[i]);
			}
			data.Add("destroylist",list);
		}
		session.SendMsg("syncentitymsg",data);
	}
	static void SendCommandComponent(SyncSession session,CommandComponent msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("x", msg.moveDir.x);
				data2.Add("y", msg.moveDir.y);
				data2.Add("z", msg.moveDir.z);
				data.Add("movedir",data2);
			}
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("x", msg.skillDir.x);
				data2.Add("y", msg.skillDir.y);
				data2.Add("z", msg.skillDir.z);
				data.Add("skilldir",data2);
			}
		data.Add("element1", msg.element1);
		data.Add("element2", msg.element2);
		data.Add("isfire", msg.isFire);
		data.Add("id", msg.id);
		data.Add("frame", msg.frame);
		data.Add("time", msg.time);
		session.SendMsg("commandcomponent",data);
	}
	static void SendPlayerBuyCharacter_c(SyncSession session,PlayerBuyCharacter_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("playerbuycharacter",data);
	}
	static void SendPlayerLoginMsg_c(SyncSession session,PlayerLoginMsg_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("characterid", msg.characterID);
		{
			List<object> list = new List<object>();
			for(int i = 0;i <msg.ownCharacter.Count ; i++)
			{
				list.Add( msg.ownCharacter[i]);
			}
			data.Add("owncharacter",list);
		}
		data.Add("coin", msg.coin);
		data.Add("diamond", msg.diamond);
		session.SendMsg("playerloginmsg",data);
	}
	static void SendPlayerMatchMsg_c(SyncSession session,PlayerMatchMsg_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("predicttime", msg.predictTime);
		data.Add("ismatched", msg.isMatched);
		session.SendMsg("playermatchmsg",data);
	}
	static void SendPlayerRename_c(SyncSession session,PlayerRename_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("newname", msg.newName);
		session.SendMsg("playerrename",data);
	}
	static void SendPlayerResurgence_c(SyncSession session,PlayerResurgence_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		session.SendMsg("playerresurgence",data);
	}
	static void SendPlayerSelectCharacter_c(SyncSession session,PlayerSelectCharacter_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("playerselectcharacter",data);
	}
	static void SendPlayerSettlement_c(SyncSession session,PlayerSettlement_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("rank", msg.rank);
		data.Add("score", msg.score);
		data.Add("historicalhighest", msg.historicalHighest);
		data.Add("diamond", msg.diamond);
		session.SendMsg("playersettlement",data);
	}
	#endregion

	#region 事件接收
	public static void AnalysisAndDispatchMessage (SyncSession session,ProtocolRequestBase cmd)
	{
		switch (cmd.Key)
		{
			case  "affirmmsg":ReceviceAffirmMsg(session , cmd);break;
			case  "changecomponentmsg":ReceviceChangeComponentMsg(session , cmd);break;
			case  "changesingletoncomponentmsg":ReceviceChangeSingletonComponentMsg(session , cmd);break;
			case  "commandmsg":ReceviceCommandMsg(session , cmd);break;
			case  "debugmsg":ReceviceDebugMsg(session , cmd);break;
			case  "destroyentitymsg":ReceviceDestroyEntityMsg(session , cmd);break;
			case  "pursuemsg":RecevicePursueMsg(session , cmd);break;
			case  "querycommand":ReceviceQueryCommand(session , cmd);break;
			case  "startsyncmsg":ReceviceStartSyncMsg(session , cmd);break;
			case  "syncentitymsg":ReceviceSyncEntityMsg(session , cmd);break;
			case  "commandcomponent":ReceviceCommandComponent(session , cmd);break;
			case  "playerbuycharacter":RecevicePlayerBuyCharacter_s(session , cmd);break;
			case  "playerloginmsg":RecevicePlayerLoginMsg_s(session , cmd);break;
			case  "playermatchmsg":RecevicePlayerMatchMsg_s(session , cmd);break;
			case  "playerrename":RecevicePlayerRename_s(session , cmd);break;
			case  "playerresurgence":RecevicePlayerResurgence_s(session , cmd);break;
			case  "playerselectcharacter":RecevicePlayerSelectCharacter_s(session , cmd);break;
			default:
			Debug.LogError("Recevice Exception : 不支持的消息类型!" + cmd.Key);
				break;
		}
	}
	static void ReceviceAffirmMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.AffirmMsg msg = new Protocol.AffirmMsg();
		msg.index = (int)e.m_data["index"];
		msg.time = (int)e.m_data["time"];
		
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
	static void ReceviceCommandMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.CommandMsg msg = new Protocol.CommandMsg();
		msg.index = (int)e.m_data["index"];
		msg.serverTime = (int)e.m_data["servertime"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.m_data["msg"];
			List<Protocol.CommandInfo> list2 = new List<Protocol.CommandInfo>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.CommandInfo tmp2 = new Protocol.CommandInfo();
				tmp2.frame = (int)data2[i2]["frame"];
				tmp2.id = (int)data2[i2]["id"];
				{
					Dictionary<string, object> data4 = (Dictionary<string, object>)data2[i2]["movedir"];
					SyncVector3 tmp4 = new SyncVector3();
					tmp4.x = (int)data4["x"];
					tmp4.y = (int)data4["y"];
					tmp4.z = (int)data4["z"];
					tmp2.moveDir = tmp4;
				}
				{
					Dictionary<string, object> data4 = (Dictionary<string, object>)data2[i2]["skilldir"];
					SyncVector3 tmp4 = new SyncVector3();
					tmp4.x = (int)data4["x"];
					tmp4.y = (int)data4["y"];
					tmp4.z = (int)data4["z"];
					tmp2.skillDir = tmp4;
				}
				tmp2.element1 = (int)data2[i2]["element1"];
				tmp2.element2 = (int)data2[i2]["element2"];
				tmp2.isFire = (bool)data2[i2]["isfire"];
				list2.Add(tmp2);
			}
			msg.msg =  list2;
		}
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceDebugMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.DebugMsg msg = new Protocol.DebugMsg();
		msg.frame = (int)e.m_data["frame"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.m_data["infos"];
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
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceDestroyEntityMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.DestroyEntityMsg msg = new Protocol.DestroyEntityMsg();
		msg.frame = (int)e.m_data["frame"];
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void RecevicePursueMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.PursueMsg msg = new Protocol.PursueMsg();
		msg.id = (int)e.m_data["id"];
		msg.recalcFrame = (int)e.m_data["recalcframe"];
		msg.frame = (int)e.m_data["frame"];
		msg.advanceCount = (int)e.m_data["advancecount"];
		msg.serverTime = (int)e.m_data["servertime"];
		msg.updateSpeed = (float)(double)e.m_data["updatespeed"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceQueryCommand(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.QueryCommand msg = new Protocol.QueryCommand();
		msg.frame = (int)e.m_data["frame"];
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceStartSyncMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.StartSyncMsg msg = new Protocol.StartSyncMsg();
		msg.frame = (int)e.m_data["frame"];
		msg.advanceCount = (int)e.m_data["advancecount"];
		msg.intervalTime = (int)e.m_data["intervaltime"];
		msg.createEntityIndex = (int)e.m_data["createentityindex"];
		msg.randomSeed = (int)e.m_data["randomseed"];
		msg.SyncRule = (SyncRule)e.m_data["syncrule"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceSyncEntityMsg(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.SyncEntityMsg msg = new Protocol.SyncEntityMsg();
		msg.frame = (int)e.m_data["frame"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.m_data["infos"];
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
		msg.destroyList = (List<Int32>)e.m_data["destroylist"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void ReceviceCommandComponent(SyncSession session ,ProtocolRequestBase e)
	{
		CommandComponent msg = new CommandComponent();
		{
			Dictionary<string, object> data2 = (Dictionary<string, object>)e.m_data["movedir"];
			SyncVector3 tmp2 = new SyncVector3();
			tmp2.x = (int)data2["x"];
			tmp2.y = (int)data2["y"];
			tmp2.z = (int)data2["z"];
			msg.moveDir = tmp2;
		}
		{
			Dictionary<string, object> data2 = (Dictionary<string, object>)e.m_data["skilldir"];
			SyncVector3 tmp2 = new SyncVector3();
			tmp2.x = (int)data2["x"];
			tmp2.y = (int)data2["y"];
			tmp2.z = (int)data2["z"];
			msg.skillDir = tmp2;
		}
		msg.element1 = (int)e.m_data["element1"];
		msg.element2 = (int)e.m_data["element2"];
		msg.isFire = (bool)e.m_data["isfire"];
		msg.id = (int)e.m_data["id"];
		msg.frame = (int)e.m_data["frame"];
		msg.time = (int)e.m_data["time"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void RecevicePlayerBuyCharacter_s(SyncSession session ,ProtocolRequestBase e)
	{
		PlayerBuyCharacter_s msg = new PlayerBuyCharacter_s();
		msg.characterID = e.m_data["characterid"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void RecevicePlayerLoginMsg_s(SyncSession session ,ProtocolRequestBase e)
	{
		PlayerLoginMsg_s msg = new PlayerLoginMsg_s();
		msg.playerID = e.m_data["playerid"].ToString();
		msg.nickName = e.m_data["nickname"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void RecevicePlayerMatchMsg_s(SyncSession session ,ProtocolRequestBase e)
	{
		PlayerMatchMsg_s msg = new PlayerMatchMsg_s();
		msg.isCancel = (bool)e.m_data["iscancel"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void RecevicePlayerRename_s(SyncSession session ,ProtocolRequestBase e)
	{
		PlayerRename_s msg = new PlayerRename_s();
		msg.newName = e.m_data["newname"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void RecevicePlayerResurgence_s(SyncSession session ,ProtocolRequestBase e)
	{
		PlayerResurgence_s msg = new PlayerResurgence_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void RecevicePlayerSelectCharacter_s(SyncSession session ,ProtocolRequestBase e)
	{
		PlayerSelectCharacter_s msg = new PlayerSelectCharacter_s();
		msg.characterID = e.m_data["characterid"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	#endregion
}
