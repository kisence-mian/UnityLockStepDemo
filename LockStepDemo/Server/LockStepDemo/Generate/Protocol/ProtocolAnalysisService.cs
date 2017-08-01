#pragma warning disable
using LockStepDemo.Event;
using LockStepDemo;
using LockStepDemo.Service;
using Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;

//指令解析类
//该类自动生成，请勿修改
public static class ProtocolAnalysisService
{
	#region 消息发送
	public static void SendMsg(this SyncSession session,Protocol.user_heartbeat_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		session.SendMsg("user_heartbeat",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.roleModule.role_kick_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("role_kick",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.roleModule.role_attr_change_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("role_id", msg.role_id);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.changes.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("key", msg.changes[i2].key);
				data2.Add("value", msg.changes[i2].value);
				list2.Add( data2);
			}
			data.Add("changes",list2);
		}
		session.SendMsg("role_attr_change",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.roleModule.role_attr_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("role_id", msg.role_id);
		data.Add("gold", msg.gold);
		data.Add("diamond", msg.diamond);
		data.Add("lv", msg.lv);
		data.Add("exp", msg.exp);
		data.Add("expneed", msg.expneed);
		data.Add("phy", msg.phy);
		data.Add("renown", msg.renown);
		data.Add("power", msg.power);
		data.Add("att", msg.att);
		data.Add("def", msg.def);
		data.Add("hp", msg.hp);
		data.Add("hprecover", msg.hprecover);
		data.Add("crit", msg.crit);
		data.Add("critdamage", msg.critdamage);
		data.Add("ignoredef", msg.ignoredef);
		data.Add("hpabsorb", msg.hpabsorb);
		data.Add("movespeed", msg.movespeed);
		data.Add("tough", msg.tough);
		data.Add("model_id", msg.model_id);
		data.Add("oid", msg.oid);
		data.Add("nick", msg.nick);
		data.Add("head", msg.head);
		data.Add("sex", msg.sex);
		data.Add("weapon", msg.weapon);
		data.Add("hero", msg.hero);
		session.SendMsg("role_attr",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.roleModule.role_create_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("role_create",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.roleModule.role_login_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("create", msg.create);
		session.SendMsg("role_login",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.roleModule.role_money_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("gold", msg.gold);
		data.Add("diamond", msg.diamond);
		session.SendMsg("role_money",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.roleModule.role_auth_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("key", msg.key);
		session.SendMsg("role_auth",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.bagModule.bag_diamondnum_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("diamondnum", msg.diamondnum);
		session.SendMsg("bag_diamondnum",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.bagModule.bag_add_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.bag.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.bag[i2].id);
				data2.Add("num", msg.bag[i2].num);
				list2.Add( data2);
			}
			data.Add("bag",list2);
		}
		session.SendMsg("bag_add",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.bagModule.bag_changedgoods_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.del.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.del[i2].id);
				data2.Add("num", msg.del[i2].num);
				list2.Add( data2);
			}
			data.Add("del",list2);
		}
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.changed.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.changed[i2].id);
				data2.Add("num", msg.changed[i2].num);
				list2.Add( data2);
			}
			data.Add("changed",list2);
		}
		session.SendMsg("bag_changedgoods",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.bagModule.bag_sell_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("result", msg.result);
		session.SendMsg("bag_sell",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.bagModule.bag_use_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("result", msg.result);
		session.SendMsg("bag_use",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.bagModule.bag_info_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.list[i2].id);
				data2.Add("num", msg.list[i2].num);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("bag_info",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.mailModule.mail_list_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.list[i2].id);
				data2.Add("title", msg.list[i2].title);
				data2.Add("content", msg.list[i2].content);
				data2.Add("isread", msg.list[i2].isread);
				{
					List<Dictionary<string, object>> list4 = new List<Dictionary<string, object>>();
					for(int i4 = 0;i4 <msg.list[i2].items.Count ; i4++)
					{
						Dictionary<string, object> data4 = new Dictionary<string, object>();
						data4.Add("id", msg.list[i2].items[i4].id);
						data4.Add("num", msg.list[i2].items[i4].num);
						list4.Add( data4);
					}
					data2.Add("items",list4);
				}
				data2.Add("time", msg.list[i2].time);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("mail_list",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.mailModule.mail_read_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("mail_read",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.mailModule.mail_new_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("type", msg.type);
		data.Add("num", msg.num);
		session.SendMsg("mail_new",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.mailModule.mail_attach_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("mail_attach",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.mailModule.mail_del_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<System.Int32> list = new List<System.Int32>();
			for(int i = 0;i <msg.ids.Count ; i++)
			{
				list.Add( msg.ids[i]);
			}
			data.Add("ids",list);
		}
		session.SendMsg("mail_del",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.taskModule.task_ach_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("type", msg.type);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.data.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.data[i2].id);
				data2.Add("num", msg.data[i2].num);
				data2.Add("max", msg.data[i2].max);
				data2.Add("status", msg.data[i2].status);
				list2.Add( data2);
			}
			data.Add("data",list2);
		}
		session.SendMsg("task_ach",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.taskModule.task_cat_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.data.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.data[i2].id);
				data2.Add("point", msg.data[i2].point);
				data2.Add("num", msg.data[i2].num);
				list2.Add( data2);
			}
			data.Add("data",list2);
		}
		session.SendMsg("task_cat",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.taskModule.task_list_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.data.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.data[i2].id);
				data2.Add("num", msg.data[i2].num);
				data2.Add("max", msg.data[i2].max);
				data2.Add("status", msg.data[i2].status);
				list2.Add( data2);
			}
			data.Add("data",list2);
		}
		session.SendMsg("task_list",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.taskModule.task_award_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("id", msg.id);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.items.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.items[i2].id);
				data2.Add("num", msg.items[i2].num);
				list2.Add( data2);
			}
			data.Add("items",list2);
		}
		session.SendMsg("task_award",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_fuse_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("gold", msg.gold);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.items.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.items[i2].id);
				data2.Add("num", msg.items[i2].num);
				list2.Add( data2);
			}
			data.Add("items",list2);
		}
		session.SendMsg("equip_fuse",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_strength_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("lv", msg.lv);
		session.SendMsg("equip_strength",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_list_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("pos", msg.list[i2].pos);
				data2.Add("id", msg.list[i2].id);
				data2.Add("item_id", msg.list[i2].item_id);
				{
					List<System.Int32> list = new List<System.Int32>();
					for(int i = 0;i <msg.list[i2].gem.Count ; i++)
					{
						list.Add( msg.list[i2].gem[i]);
					}
					data2.Add("gem",list);
				}
				{
					List<Dictionary<string, object>> list4 = new List<Dictionary<string, object>>();
					for(int i4 = 0;i4 <msg.list[i2].ench.Count ; i4++)
					{
						Dictionary<string, object> data4 = new Dictionary<string, object>();
						data4.Add("name", msg.list[i2].ench[i4].name);
						data4.Add("value", msg.list[i2].ench[i4].value);
						list4.Add( data4);
					}
					data2.Add("ench",list4);
				}
				data2.Add("lv", msg.list[i2].lv);
				{
					List<System.String> list = new List<System.String>();
					for(int i = 0;i <msg.list[i2].skills.Count ; i++)
					{
						list.Add( msg.list[i2].skills[i]);
					}
					data2.Add("skills",list);
				}
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("equip_list",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_skill_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("id", msg.id);
		data.Add("skill_id", msg.skill_id);
		session.SendMsg("equip_skill",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_bag_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.list[i2].id);
				data2.Add("item_id", msg.list[i2].item_id);
				data2.Add("lv", msg.list[i2].lv);
				{
					List<System.Int32> list = new List<System.Int32>();
					for(int i = 0;i <msg.list[i2].gem.Count ; i++)
					{
						list.Add( msg.list[i2].gem[i]);
					}
					data2.Add("gem",list);
				}
				{
					List<Dictionary<string, object>> list4 = new List<Dictionary<string, object>>();
					for(int i4 = 0;i4 <msg.list[i2].ench.Count ; i4++)
					{
						Dictionary<string, object> data4 = new Dictionary<string, object>();
						data4.Add("name", msg.list[i2].ench[i4].name);
						data4.Add("value", msg.list[i2].ench[i4].value);
						list4.Add( data4);
					}
					data2.Add("ench",list4);
				}
				{
					List<System.String> list = new List<System.String>();
					for(int i = 0;i <msg.list[i2].skills.Count ; i++)
					{
						list.Add( msg.list[i2].skills[i]);
					}
					data2.Add("skills",list);
				}
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("equip_bag",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_inlay_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("item_id", msg.item_id);
		data.Add("diamondid", msg.diamondid);
		data.Add("diamond_pos", msg.diamond_pos);
		session.SendMsg("equip_inlay",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_enchanting_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("item_id", msg.item_id);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.attrs.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("name", msg.attrs[i2].name);
				data2.Add("value", msg.attrs[i2].value);
				list2.Add( data2);
			}
			data.Add("attrs",list2);
		}
		session.SendMsg("equip_enchanting",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_wear_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("pos", msg.pos);
		data.Add("id", msg.id);
		data.Add("item_id", msg.item_id);
		session.SendMsg("equip_wear",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.equipModule.equip_syn_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("id", msg.id);
		data.Add("new_id", msg.new_id);
		data.Add("new_num", msg.new_num);
		session.SendMsg("equip_syn",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.shopModule.shop_buy_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("id", msg.id);
		session.SendMsg("shop_buy",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.shopModule.shop_list_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("type", msg.type);
		{
			List<System.Int32> list = new List<System.Int32>();
			for(int i = 0;i <msg.list.Count ; i++)
			{
				list.Add( msg.list[i]);
			}
			data.Add("list",list);
		}
		session.SendMsg("shop_list",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.friendModule.friend_info_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("role_id", msg.list[i2].role_id);
				data2.Add("nick", msg.list[i2].nick);
				data2.Add("head", msg.list[i2].head);
				data2.Add("lv", msg.list[i2].lv);
				data2.Add("power", msg.list[i2].power);
				data2.Add("time", msg.list[i2].time);
				data2.Add("online", msg.list[i2].online);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("friend_info",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.friendModule.friend_op_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("role_id", msg.role_id);
		data.Add("type", msg.type);
		data.Add("code", msg.code);
		session.SendMsg("friend_op",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.friendModule.friend_new_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		session.SendMsg("friend_new",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.friendModule.friend_request_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("role_id", msg.list[i2].role_id);
				data2.Add("nick", msg.list[i2].nick);
				data2.Add("head", msg.list[i2].head);
				data2.Add("lv", msg.list[i2].lv);
				data2.Add("power", msg.list[i2].power);
				data2.Add("time", msg.list[i2].time);
				data2.Add("online", msg.list[i2].online);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("friend_request",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.friendModule.friend_search_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("role_id", msg.list[i2].role_id);
				data2.Add("nick", msg.list[i2].nick);
				data2.Add("head", msg.list[i2].head);
				data2.Add("lv", msg.list[i2].lv);
				data2.Add("power", msg.list[i2].power);
				data2.Add("time", msg.list[i2].time);
				data2.Add("online", msg.list[i2].online);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("friend_search",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.chatModule.chat_msg_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("from_id", msg.from_id);
		data.Add("from_name", msg.from_name);
		data.Add("channel", msg.channel);
		data.Add("msg", msg.msg);
		session.SendMsg("chat_msg",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.guideModule.guide_list_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("value", msg.value);
		session.SendMsg("guide_list",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.guideModule.guide_add_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("id", msg.id);
		session.SendMsg("guide_add",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.signinModule.signin_add_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("num", msg.num);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.items.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.items[i2].id);
				data2.Add("num", msg.items[i2].num);
				list2.Add( data2);
			}
			data.Add("items",list2);
		}
		session.SendMsg("signin_add",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.signinModule.signin_rep_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("day", msg.day);
		session.SendMsg("signin_rep",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.signinModule.signin_award_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.items.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.items[i2].id);
				data2.Add("num", msg.items[i2].num);
				list2.Add( data2);
			}
			data.Add("items",list2);
		}
		session.SendMsg("signin_award",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.signinModule.signin_online_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("id", msg.id);
		session.SendMsg("signin_online",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.signinModule.signin_get_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("time", msg.time);
		data.Add("id", msg.id);
		session.SendMsg("signin_get",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.signinModule.signin_info_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("num", msg.num);
		data.Add("ok", msg.ok);
		session.SendMsg("signin_info",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.livenessModule.liveness_list_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.task.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.task[i2].id);
				data2.Add("num", msg.task[i2].num);
				data2.Add("status", msg.task[i2].status);
				list2.Add( data2);
			}
			data.Add("task",list2);
		}
		{
			List<System.Int32> list = new List<System.Int32>();
			for(int i = 0;i <msg.box.Count ; i++)
			{
				list.Add( msg.box[i]);
			}
			data.Add("box",list);
		}
		data.Add("score", msg.score);
		session.SendMsg("liveness_list",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.livenessModule.liveness_award_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.items.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.items[i2].id);
				data2.Add("num", msg.items[i2].num);
				list2.Add( data2);
			}
			data.Add("items",list2);
		}
		data.Add("pos", msg.pos);
		session.SendMsg("liveness_award",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.timeModule.time_resync_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		session.SendMsg("time_resync",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.timeModule.time_syncreturn_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("sendtime", msg.sendtime);
		data.Add("servicetime", msg.servicetime);
		session.SendMsg("time_syncreturn",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_task_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("num", msg.num);
		session.SendMsg("room2_task",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_pool_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("type", msg.list[i2].type);
				data2.Add("id", msg.list[i2].id);
				data2.Add("start", msg.list[i2].start);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		data.Add("time", msg.time);
		session.SendMsg("room2_pool",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_level_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("op", msg.op);
		data.Add("val", msg.val);
		session.SendMsg("room2_level",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_reflashp_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("room2_reflashp",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_join_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("room2_join",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_cityadd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("nick", msg.list[i2].nick);
				data2.Add("role_id", msg.list[i2].role_id);
				data2.Add("model_id", msg.list[i2].model_id);
				data2.Add("weapon", msg.list[i2].weapon);
				data2.Add("x", msg.list[i2].x);
				data2.Add("z", msg.list[i2].z);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("room2_cityadd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_exit_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("room2_exit",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_timesync_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("client_time", msg.client_time);
		data.Add("server_time", msg.server_time);
		session.SendMsg("room2_timesync",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_citymove_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("role_id", msg.role_id);
		data.Add("x", msg.x);
		data.Add("z", msg.z);
		data.Add("time", msg.time);
		session.SendMsg("room2_citymove",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_changeserver_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("server", msg.server);
		session.SendMsg("room2_changeserver",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_citydel_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<System.Int32> list = new List<System.Int32>();
			for(int i = 0;i <msg.list.Count ; i++)
			{
				list.Add( msg.list[i]);
			}
			data.Add("list",list);
		}
		session.SendMsg("room2_citydel",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_info_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<System.String> list = new List<System.String>();
			for(int i = 0;i <msg.roominfo.Count ; i++)
			{
				list.Add( msg.roominfo[i]);
			}
			data.Add("roominfo",list);
		}
		data.Add("gameplay", msg.gameplay);
		session.SendMsg("room2_info",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.room2Module.room2_users_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("nick", msg.list[i2].nick);
				data2.Add("head", msg.list[i2].head);
				data2.Add("lv", msg.list[i2].lv);
				data2.Add("role_id", msg.list[i2].role_id);
				data2.Add("model_id", msg.list[i2].model_id);
				data2.Add("weapon", msg.list[i2].weapon);
				data2.Add("camp", msg.list[i2].camp);
				data2.Add("att", msg.list[i2].att);
				data2.Add("def", msg.list[i2].def);
				data2.Add("hp", msg.list[i2].hp);
				data2.Add("hprecover", msg.list[i2].hprecover);
				data2.Add("crit", msg.list[i2].crit);
				data2.Add("critdamage", msg.list[i2].critdamage);
				data2.Add("ignoredef", msg.list[i2].ignoredef);
				data2.Add("hpabsorb", msg.list[i2].hpabsorb);
				data2.Add("movespeed", msg.list[i2].movespeed);
				data2.Add("tough", msg.list[i2].tough);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("room2_users",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.removecharactercmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("removecharactercmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.createtrapcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_trapid", msg.m_trapid);
		data.Add("m_trapname", msg.m_trapname);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_dirx", msg.m_dirx);
		data.Add("m_diry", msg.m_diry);
		data.Add("m_dirz", msg.m_dirz);
		data.Add("m_camp", msg.m_camp);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("createtrapcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_end_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		session.SendMsg("fight_end",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.destroyflyobjectcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_flyid", msg.m_flyid);
		data.Add("m_isshowhiteffect", msg.m_isshowhiteffect);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("destroyflyobjectcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.rotationcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_dirx", msg.m_dirx);
		data.Add("m_diry", msg.m_diry);
		data.Add("m_dirz", msg.m_dirz);
		data.Add("m_creatcomandtime", msg.m_creatcomandtime);
		data.Add("m_characterid", msg.m_characterid);
		session.SendMsg("rotationcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.movecmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_dirx", msg.m_dirx);
		data.Add("m_dirz", msg.m_dirz);
		data.Add("m_test", msg.m_test);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_isonlyturn", msg.m_isonlyturn);
		data.Add("m_creatcomandtime", msg.m_creatcomandtime);
		data.Add("m_characterid", msg.m_characterid);
		session.SendMsg("movecmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.skillcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_skillid", msg.m_skillid);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_skilldirx", msg.m_skilldirx);
		data.Add("m_skilldiry", msg.m_skilldiry);
		data.Add("m_skilldirz", msg.m_skilldirz);
		data.Add("m_creatcomandtime", msg.m_creatcomandtime);
		data.Add("m_characterid", msg.m_characterid);
		session.SendMsg("skillcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.traptriggercmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_trapid", msg.m_trapid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("traptriggercmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_setrole_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("fight_setrole",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_rank_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.lists.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("nick", msg.lists[i2].nick);
				data2.Add("score", msg.lists[i2].score);
				list2.Add( data2);
			}
			data.Add("lists",list2);
		}
		session.SendMsg("fight_rank",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_item_num_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", msg.id);
		data.Add("num", msg.num);
		session.SendMsg("fight_item_num",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.createitemcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("m_itemid", msg.list[i2].m_itemid);
				data2.Add("m_itemname", msg.list[i2].m_itemname);
				data2.Add("m_posx", msg.list[i2].m_posx);
				data2.Add("m_posy", msg.list[i2].m_posy);
				data2.Add("m_posz", msg.list[i2].m_posz);
				data2.Add("num", msg.list[i2].num);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("createitemcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_cancel_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("fight_cancel",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_element_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.list.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.list[i2].id);
				data2.Add("num", msg.list[i2].num);
				list2.Add( data2);
			}
			data.Add("list",list2);
		}
		session.SendMsg("fight_element",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.createflyobjectcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_flyname", msg.m_flyname);
		data.Add("m_flyid", msg.m_flyid);
		data.Add("m_skillid", msg.m_skillid);
		data.Add("m_createrid", msg.m_createrid);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_dirx", msg.m_dirx);
		data.Add("m_diry", msg.m_diry);
		data.Add("m_dirz", msg.m_dirz);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("createflyobjectcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_match_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("fight_match",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_loading_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		session.SendMsg("fight_loading",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_setelement_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("fight_setelement",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.pickupitemcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_itemid", msg.m_itemid);
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("pickupitemcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.destroyitemcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_itemid", msg.m_itemid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("destroyitemcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.changeweaponcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_weaponid", msg.m_weaponid);
		data.Add("m_creatcomandtime", msg.m_creatcomandtime);
		data.Add("m_characterid", msg.m_characterid);
		session.SendMsg("changeweaponcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.removebuffcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_buffid", msg.m_buffid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("removebuffcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.blowflycmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_flyerid", msg.m_flyerid);
		data.Add("m_attackerid", msg.m_attackerid);
		data.Add("m_shiftid", msg.m_shiftid);
		data.Add("m_attackerposx", msg.m_attackerposx);
		data.Add("m_attackerposy", msg.m_attackerposy);
		data.Add("m_attackerposz", msg.m_attackerposz);
		data.Add("m_hurterposx", msg.m_hurterposx);
		data.Add("m_hurterposy", msg.m_hurterposy);
		data.Add("m_hurterposz", msg.m_hurterposz);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("blowflycmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_item_list_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.lists.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.lists[i2].id);
				data2.Add("num", msg.lists[i2].num);
				list2.Add( data2);
			}
			data.Add("lists",list2);
		}
		session.SendMsg("fight_item_list",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_use_item_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("id", msg.id);
		session.SendMsg("fight_use_item",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.fight_relive_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		session.SendMsg("fight_relive",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.resurgencecmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_creatcomandtime", msg.m_creatcomandtime);
		data.Add("m_characterid", msg.m_characterid);
		session.SendMsg("resurgencecmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.attackcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_dirx", msg.m_dirx);
		data.Add("m_diry", msg.m_diry);
		data.Add("m_dirz", msg.m_dirz);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_creatcomandtime", msg.m_creatcomandtime);
		data.Add("m_characterid", msg.m_characterid);
		session.SendMsg("attackcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.diecmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_killerid", msg.m_killerid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("diecmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.destroybloodvialcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_bloodvialid", msg.m_bloodvialid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("destroybloodvialcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.createbloodvialcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_bloodvialid", msg.m_bloodvialid);
		data.Add("m_camp", msg.m_camp);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("createbloodvialcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.pickupbloodvialcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_bloodvialid", msg.m_bloodvialid);
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("pickupbloodvialcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.addbuffcmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_attackerid", msg.m_attackerid);
		data.Add("m_buffid", msg.m_buffid);
		data.Add("m_skillid", msg.m_skillid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("addbuffcmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.damagecmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_attackerid", msg.m_attackerid);
		data.Add("m_damagenumber", msg.m_damagenumber);
		data.Add("m_skillid", msg.m_skillid);
		data.Add("m_buffid", msg.m_buffid);
		data.Add("m_crit", msg.m_crit);
		data.Add("m_disrupting", msg.m_disrupting);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("damagecmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.createskilltokencmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_skillid", msg.m_skillid);
		data.Add("m_createrid", msg.m_createrid);
		data.Add("m_camp", msg.m_camp);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_dirx", msg.m_dirx);
		data.Add("m_diry", msg.m_diry);
		data.Add("m_dirz", msg.m_dirz);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("createskilltokencmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.recovercmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_attackerid", msg.m_attackerid);
		data.Add("m_recovernumber", msg.m_recovernumber);
		data.Add("m_isautorecover", msg.m_isautorecover);
		data.Add("m_skillid", msg.m_skillid);
		data.Add("m_buffid", msg.m_buffid);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("recovercmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.fightModule.createcharactercmd_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_charactertype", msg.m_charactertype);
		data.Add("m_charactername", msg.m_charactername);
		data.Add("m_characterid", msg.m_characterid);
		data.Add("m_camp", msg.m_camp);
		data.Add("m_posx", msg.m_posx);
		data.Add("m_posy", msg.m_posy);
		data.Add("m_posz", msg.m_posz);
		data.Add("m_dirx", msg.m_dirx);
		data.Add("m_diry", msg.m_diry);
		data.Add("m_dirz", msg.m_dirz);
		data.Add("m_amplification", msg.m_amplification);
		data.Add("m_executetime", msg.m_executetime);
		session.SendMsg("createcharactercmd",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.chestModule.chest_open_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("code", msg.code);
		data.Add("id", msg.id);
		data.Add("time", msg.time);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <msg.items.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", msg.items[i2].id);
				data2.Add("num", msg.items[i2].num);
				list2.Add( data2);
			}
			data.Add("items",list2);
		}
		session.SendMsg("chest_open",data);
	}
	public static void SendMsg(this SyncSession session,Protocol.chestModule.chest_init_c msg)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", msg.id);
		data.Add("time", msg.time);
		session.SendMsg("chest_init",data);
	}
	#endregion

	#region 事件接收
	public static void AnalysisAndDispatchMessage (SyncSession session,ProtocolRequestBase cmd)
	{
		switch (cmd.Key)
		{
			case  "user_heartbeat":Receviceuser_heartbeat_s(session , cmd);break;
			case  "role_create":Recevicerole_create_s(session , cmd);break;
			case  "role_login":Recevicerole_login_s(session , cmd);break;
			case  "role_money":Recevicerole_money_s(session , cmd);break;
			case  "role_auth":Recevicerole_auth_s(session , cmd);break;
			case  "bag_diamondnum":Recevicebag_diamondnum_s(session , cmd);break;
			case  "bag_changedgoods":Recevicebag_changedgoods_s(session , cmd);break;
			case  "bag_sell":Recevicebag_sell_s(session , cmd);break;
			case  "bag_use":Recevicebag_use_s(session , cmd);break;
			case  "bag_info":Recevicebag_info_s(session , cmd);break;
			case  "mail_read":Recevicemail_read_s(session , cmd);break;
			case  "mail_list":Recevicemail_list_s(session , cmd);break;
			case  "mail_attach":Recevicemail_attach_s(session , cmd);break;
			case  "mail_del":Recevicemail_del_s(session , cmd);break;
			case  "mail_new":Recevicemail_new_s(session , cmd);break;
			case  "task_ach":Recevicetask_ach_s(session , cmd);break;
			case  "task_cat":Recevicetask_cat_s(session , cmd);break;
			case  "task_list":Recevicetask_list_s(session , cmd);break;
			case  "task_award":Recevicetask_award_s(session , cmd);break;
			case  "equip_strength":Receviceequip_strength_s(session , cmd);break;
			case  "equip_bag":Receviceequip_bag_s(session , cmd);break;
			case  "equip_list":Receviceequip_list_s(session , cmd);break;
			case  "equip_skill":Receviceequip_skill_s(session , cmd);break;
			case  "equip_fuse":Receviceequip_fuse_s(session , cmd);break;
			case  "equip_enchanting":Receviceequip_enchanting_s(session , cmd);break;
			case  "equip_inlay":Receviceequip_inlay_s(session , cmd);break;
			case  "equip_wear":Receviceequip_wear_s(session , cmd);break;
			case  "equip_syn":Receviceequip_syn_s(session , cmd);break;
			case  "shop_buy":Receviceshop_buy_s(session , cmd);break;
			case  "shop_list":Receviceshop_list_s(session , cmd);break;
			case  "friend_op":Recevicefriend_op_s(session , cmd);break;
			case  "friend_info":Recevicefriend_info_s(session , cmd);break;
			case  "friend_search":Recevicefriend_search_s(session , cmd);break;
			case  "friend_request":Recevicefriend_request_s(session , cmd);break;
			case  "chat_msg":Recevicechat_msg_s(session , cmd);break;
			case  "guide_list":Receviceguide_list_s(session , cmd);break;
			case  "guide_msg":Receviceguide_msg_s(session , cmd);break;
			case  "guide_add":Receviceguide_add_s(session , cmd);break;
			case  "signin_info":Recevicesignin_info_s(session , cmd);break;
			case  "signin_add":Recevicesignin_add_s(session , cmd);break;
			case  "signin_rep":Recevicesignin_rep_s(session , cmd);break;
			case  "signin_get":Recevicesignin_get_s(session , cmd);break;
			case  "signin_online":Recevicesignin_online_s(session , cmd);break;
			case  "signin_award":Recevicesignin_award_s(session , cmd);break;
			case  "liveness_award":Receviceliveness_award_s(session , cmd);break;
			case  "liveness_list":Receviceliveness_list_s(session , cmd);break;
			case  "time_sync":Recevicetime_sync_s(session , cmd);break;
			case  "time_resync":Recevicetime_resync_s(session , cmd);break;
			case  "room2_cityexit":Receviceroom2_cityexit_s(session , cmd);break;
			case  "room2_level":Receviceroom2_level_s(session , cmd);break;
			case  "room2_task":Receviceroom2_task_s(session , cmd);break;
			case  "room2_pool":Receviceroom2_pool_s(session , cmd);break;
			case  "room2_reflashp":Receviceroom2_reflashp_s(session , cmd);break;
			case  "room2_join":Receviceroom2_join_s(session , cmd);break;
			case  "room2_exit":Receviceroom2_exit_s(session , cmd);break;
			case  "room2_report":Receviceroom2_report_s(session , cmd);break;
			case  "room2_citymove":Receviceroom2_citymove_s(session , cmd);break;
			case  "room2_cityadd":Receviceroom2_cityadd_s(session , cmd);break;
			case  "room2_info":Receviceroom2_info_s(session , cmd);break;
			case  "room2_timesync":Receviceroom2_timesync_s(session , cmd);break;
			case  "createtrapcmd":Recevicecreatetrapcmd_s(session , cmd);break;
			case  "removecharactercmd":Receviceremovecharactercmd_s(session , cmd);break;
			case  "fight_end":Recevicefight_end_s(session , cmd);break;
			case  "traptriggercmd":Recevicetraptriggercmd_s(session , cmd);break;
			case  "rotationcmd":Recevicerotationcmd_s(session , cmd);break;
			case  "movecmd":Recevicemovecmd_s(session , cmd);break;
			case  "fight_setrole":Recevicefight_setrole_s(session , cmd);break;
			case  "createitemcmd":Recevicecreateitemcmd_s(session , cmd);break;
			case  "fight_cancel":Recevicefight_cancel_s(session , cmd);break;
			case  "skillcmd":Receviceskillcmd_s(session , cmd);break;
			case  "createcharactercmd":Recevicecreatecharactercmd_s(session , cmd);break;
			case  "fight_match":Recevicefight_match_s(session , cmd);break;
			case  "fight_loading":Recevicefight_loading_s(session , cmd);break;
			case  "fight_setelement":Recevicefight_setelement_s(session , cmd);break;
			case  "createflyobjectcmd":Recevicecreateflyobjectcmd_s(session , cmd);break;
			case  "recovercmd":Recevicerecovercmd_s(session , cmd);break;
			case  "pickupitemcmd":Recevicepickupitemcmd_s(session , cmd);break;
			case  "attackcmd":Receviceattackcmd_s(session , cmd);break;
			case  "changeweaponcmd":Recevicechangeweaponcmd_s(session , cmd);break;
			case  "removebuffcmd":Receviceremovebuffcmd_s(session , cmd);break;
			case  "resurgencecmd":Receviceresurgencecmd_s(session , cmd);break;
			case  "fight_use_item":Recevicefight_use_item_s(session , cmd);break;
			case  "fight_relive":Recevicefight_relive_s(session , cmd);break;
			case  "destroyflyobjectcmd":Recevicedestroyflyobjectcmd_s(session , cmd);break;
			case  "blowflycmd":Receviceblowflycmd_s(session , cmd);break;
			case  "destroyitemcmd":Recevicedestroyitemcmd_s(session , cmd);break;
			case  "destroybloodvialcmd":Recevicedestroybloodvialcmd_s(session , cmd);break;
			case  "diecmd":Recevicediecmd_s(session , cmd);break;
			case  "pickupbloodvialcmd":Recevicepickupbloodvialcmd_s(session , cmd);break;
			case  "createbloodvialcmd":Recevicecreatebloodvialcmd_s(session , cmd);break;
			case  "addbuffcmd":Receviceaddbuffcmd_s(session , cmd);break;
			case  "damagecmd":Recevicedamagecmd_s(session , cmd);break;
			case  "createskilltokencmd":Recevicecreateskilltokencmd_s(session , cmd);break;
			case  "chest_open":Recevicechest_open_s(session , cmd);break;
			case  "chest_init":Recevicechest_init_s(session , cmd);break;
			default:
			Console.WriteLine("SendCommand Exception : 不支持的消息类型!" + cmd.Key);
				break;
		}
	}
	static void Receviceuser_heartbeat_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.user_heartbeat_s msg = new Protocol.user_heartbeat_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicerole_create_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.roleModule.role_create_s msg = new Protocol.roleModule.role_create_s();
		msg.nick = e.m_data["nick"].ToString();
		msg.sex = (int)e.m_data["sex"];
		msg.model_id = e.m_data["model_id"].ToString();
		msg.head = e.m_data["head"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicerole_login_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.roleModule.role_login_s msg = new Protocol.roleModule.role_login_s();
		msg.account = e.m_data["account"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicerole_money_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.roleModule.role_money_s msg = new Protocol.roleModule.role_money_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicerole_auth_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.roleModule.role_auth_s msg = new Protocol.roleModule.role_auth_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicebag_diamondnum_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.bagModule.bag_diamondnum_s msg = new Protocol.bagModule.bag_diamondnum_s();
		msg.gold = (int)e.m_data["gold"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.m_data["goods"];
			List<Protocol.goods1> list2 = new List<Protocol.goods1>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.goods1 tmp2 = new Protocol.goods1();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.goods =  list2;
		}
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicebag_changedgoods_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.bagModule.bag_changedgoods_s msg = new Protocol.bagModule.bag_changedgoods_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicebag_sell_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.bagModule.bag_sell_s msg = new Protocol.bagModule.bag_sell_s();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.m_data["sellgoods"];
			List<Protocol.goods1> list2 = new List<Protocol.goods1>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.goods1 tmp2 = new Protocol.goods1();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.sellgoods =  list2;
		}
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicebag_use_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.bagModule.bag_use_s msg = new Protocol.bagModule.bag_use_s();
		msg.id = (int)e.m_data["id"];
		msg.num = (int)e.m_data["num"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicebag_info_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.bagModule.bag_info_s msg = new Protocol.bagModule.bag_info_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicemail_read_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.mailModule.mail_read_s msg = new Protocol.mailModule.mail_read_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicemail_list_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.mailModule.mail_list_s msg = new Protocol.mailModule.mail_list_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicemail_attach_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.mailModule.mail_attach_s msg = new Protocol.mailModule.mail_attach_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicemail_del_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.mailModule.mail_del_s msg = new Protocol.mailModule.mail_del_s();
		msg.ids = (List<Int32>)e.m_data["ids"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicemail_new_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.mailModule.mail_new_s msg = new Protocol.mailModule.mail_new_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicetask_ach_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.taskModule.task_ach_s msg = new Protocol.taskModule.task_ach_s();
		msg.type = (int)e.m_data["type"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicetask_cat_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.taskModule.task_cat_s msg = new Protocol.taskModule.task_cat_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicetask_list_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.taskModule.task_list_s msg = new Protocol.taskModule.task_list_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicetask_award_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.taskModule.task_award_s msg = new Protocol.taskModule.task_award_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_strength_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_strength_s msg = new Protocol.equipModule.equip_strength_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_bag_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_bag_s msg = new Protocol.equipModule.equip_bag_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_list_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_list_s msg = new Protocol.equipModule.equip_list_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_skill_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_skill_s msg = new Protocol.equipModule.equip_skill_s();
		msg.id = (int)e.m_data["id"];
		msg.skill_id = e.m_data["skill_id"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_fuse_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_fuse_s msg = new Protocol.equipModule.equip_fuse_s();
		msg.ids = (List<Int32>)e.m_data["ids"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_enchanting_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_enchanting_s msg = new Protocol.equipModule.equip_enchanting_s();
		msg.item_id = (int)e.m_data["item_id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_inlay_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_inlay_s msg = new Protocol.equipModule.equip_inlay_s();
		msg.item_id = (int)e.m_data["item_id"];
		msg.diamondid = (int)e.m_data["diamondid"];
		msg.diamond_pos = (int)e.m_data["diamond_pos"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_wear_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_wear_s msg = new Protocol.equipModule.equip_wear_s();
		msg.pos = (int)e.m_data["pos"];
		msg.id = (int)e.m_data["id"];
		msg.item_id = e.m_data["item_id"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceequip_syn_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.equipModule.equip_syn_s msg = new Protocol.equipModule.equip_syn_s();
		msg.id = (int)e.m_data["id"];
		msg.num = (int)e.m_data["num"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceshop_buy_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.shopModule.shop_buy_s msg = new Protocol.shopModule.shop_buy_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceshop_list_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.shopModule.shop_list_s msg = new Protocol.shopModule.shop_list_s();
		msg.type = (int)e.m_data["type"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefriend_op_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.friendModule.friend_op_s msg = new Protocol.friendModule.friend_op_s();
		msg.role_id = (int)e.m_data["role_id"];
		msg.type = (int)e.m_data["type"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefriend_info_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.friendModule.friend_info_s msg = new Protocol.friendModule.friend_info_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefriend_search_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.friendModule.friend_search_s msg = new Protocol.friendModule.friend_search_s();
		msg.name = e.m_data["name"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefriend_request_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.friendModule.friend_request_s msg = new Protocol.friendModule.friend_request_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicechat_msg_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.chatModule.chat_msg_s msg = new Protocol.chatModule.chat_msg_s();
		msg.to = (int)e.m_data["to"];
		msg.channel = (int)e.m_data["channel"];
		msg.msg = e.m_data["msg"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceguide_list_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.guideModule.guide_list_s msg = new Protocol.guideModule.guide_list_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceguide_msg_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.guideModule.guide_msg_s msg = new Protocol.guideModule.guide_msg_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceguide_add_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.guideModule.guide_add_s msg = new Protocol.guideModule.guide_add_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicesignin_info_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.signinModule.signin_info_s msg = new Protocol.signinModule.signin_info_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicesignin_add_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.signinModule.signin_add_s msg = new Protocol.signinModule.signin_add_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicesignin_rep_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.signinModule.signin_rep_s msg = new Protocol.signinModule.signin_rep_s();
		msg.day = (int)e.m_data["day"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicesignin_get_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.signinModule.signin_get_s msg = new Protocol.signinModule.signin_get_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicesignin_online_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.signinModule.signin_online_s msg = new Protocol.signinModule.signin_online_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicesignin_award_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.signinModule.signin_award_s msg = new Protocol.signinModule.signin_award_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceliveness_award_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.livenessModule.liveness_award_s msg = new Protocol.livenessModule.liveness_award_s();
		msg.pos = (int)e.m_data["pos"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceliveness_list_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.livenessModule.liveness_list_s msg = new Protocol.livenessModule.liveness_list_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicetime_sync_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.timeModule.time_sync_s msg = new Protocol.timeModule.time_sync_s();
		msg.sendtime = (float)(double)e.m_data["sendtime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicetime_resync_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.timeModule.time_resync_s msg = new Protocol.timeModule.time_resync_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_cityexit_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_cityexit_s msg = new Protocol.room2Module.room2_cityexit_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_level_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_level_s msg = new Protocol.room2Module.room2_level_s();
		msg.op = (int)e.m_data["op"];
		msg.val = (int)e.m_data["val"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_task_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_task_s msg = new Protocol.room2Module.room2_task_s();
		msg.id = e.m_data["id"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_pool_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_pool_s msg = new Protocol.room2Module.room2_pool_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_reflashp_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_reflashp_s msg = new Protocol.room2Module.room2_reflashp_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_join_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_join_s msg = new Protocol.room2Module.room2_join_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_exit_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_exit_s msg = new Protocol.room2Module.room2_exit_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_report_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_report_s msg = new Protocol.room2Module.room2_report_s();
		msg.time = (float)(double)e.m_data["time"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_citymove_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_citymove_s msg = new Protocol.room2Module.room2_citymove_s();
		msg.role_id = (int)e.m_data["role_id"];
		msg.x = (int)e.m_data["x"];
		msg.z = (int)e.m_data["z"];
		msg.time = (int)e.m_data["time"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_cityadd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_cityadd_s msg = new Protocol.room2Module.room2_cityadd_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_info_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_info_s msg = new Protocol.room2Module.room2_info_s();
		msg.roominfo = (List<String>)e.m_data["roominfo"];
		msg.gameplay = (int)e.m_data["gameplay"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceroom2_timesync_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.room2Module.room2_timesync_s msg = new Protocol.room2Module.room2_timesync_s();
		msg.client_time = (float)(double)e.m_data["client_time"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicecreatetrapcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.createtrapcmd_s msg = new Protocol.fightModule.createtrapcmd_s();
		msg.m_trapid = (int)e.m_data["m_trapid"];
		msg.m_trapname = e.m_data["m_trapname"].ToString();
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_dirx = (float)(double)e.m_data["m_dirx"];
		msg.m_diry = (float)(double)e.m_data["m_diry"];
		msg.m_dirz = (float)(double)e.m_data["m_dirz"];
		msg.m_camp = (int)e.m_data["m_camp"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceremovecharactercmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.removecharactercmd_s msg = new Protocol.fightModule.removecharactercmd_s();
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefight_end_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.fight_end_s msg = new Protocol.fightModule.fight_end_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicetraptriggercmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.traptriggercmd_s msg = new Protocol.fightModule.traptriggercmd_s();
		msg.m_trapid = (int)e.m_data["m_trapid"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicerotationcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.rotationcmd_s msg = new Protocol.fightModule.rotationcmd_s();
		msg.m_dirx = (float)(double)e.m_data["m_dirx"];
		msg.m_diry = (float)(double)e.m_data["m_diry"];
		msg.m_dirz = (float)(double)e.m_data["m_dirz"];
		msg.m_creatcomandtime = (int)e.m_data["m_creatcomandtime"];
		msg.m_characterid = (int)e.m_data["m_characterid"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicemovecmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.movecmd_s msg = new Protocol.fightModule.movecmd_s();
		msg.m_dirx = (int)e.m_data["m_dirx"];
		msg.m_dirz = (int)e.m_data["m_dirz"];
		msg.m_test = (int)e.m_data["m_test"];
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_isonlyturn = (bool)e.m_data["m_isonlyturn"];
		msg.m_creatcomandtime = (int)e.m_data["m_creatcomandtime"];
		msg.m_characterid = (int)e.m_data["m_characterid"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefight_setrole_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.fight_setrole_s msg = new Protocol.fightModule.fight_setrole_s();
		msg.id = e.m_data["id"].ToString();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicecreateitemcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.createitemcmd_s msg = new Protocol.fightModule.createitemcmd_s();
		msg.m_itemid = (int)e.m_data["m_itemid"];
		msg.m_itemname = e.m_data["m_itemname"].ToString();
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefight_cancel_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.fight_cancel_s msg = new Protocol.fightModule.fight_cancel_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceskillcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.skillcmd_s msg = new Protocol.fightModule.skillcmd_s();
		msg.m_skillid = e.m_data["m_skillid"].ToString();
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_skilldirx = (float)(double)e.m_data["m_skilldirx"];
		msg.m_skilldiry = (float)(double)e.m_data["m_skilldiry"];
		msg.m_skilldirz = (float)(double)e.m_data["m_skilldirz"];
		msg.m_creatcomandtime = (int)e.m_data["m_creatcomandtime"];
		msg.m_characterid = (int)e.m_data["m_characterid"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicecreatecharactercmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.createcharactercmd_s msg = new Protocol.fightModule.createcharactercmd_s();
		msg.m_charactertype = (int)e.m_data["m_charactertype"];
		msg.m_charactername = e.m_data["m_charactername"].ToString();
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_camp = (int)e.m_data["m_camp"];
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_dirx = (float)(double)e.m_data["m_dirx"];
		msg.m_diry = (float)(double)e.m_data["m_diry"];
		msg.m_dirz = (float)(double)e.m_data["m_dirz"];
		msg.m_amplification = (float)(double)e.m_data["m_amplification"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefight_match_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.fight_match_s msg = new Protocol.fightModule.fight_match_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefight_loading_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.fight_loading_s msg = new Protocol.fightModule.fight_loading_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefight_setelement_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.fight_setelement_s msg = new Protocol.fightModule.fight_setelement_s();
		msg.item1 = (int)e.m_data["item1"];
		msg.item2 = (int)e.m_data["item2"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicecreateflyobjectcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.createflyobjectcmd_s msg = new Protocol.fightModule.createflyobjectcmd_s();
		msg.m_flyname = e.m_data["m_flyname"].ToString();
		msg.m_flyid = (int)e.m_data["m_flyid"];
		msg.m_skillid = e.m_data["m_skillid"].ToString();
		msg.m_createrid = (int)e.m_data["m_createrid"];
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_dirx = (float)(double)e.m_data["m_dirx"];
		msg.m_diry = (float)(double)e.m_data["m_diry"];
		msg.m_dirz = (float)(double)e.m_data["m_dirz"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicerecovercmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.recovercmd_s msg = new Protocol.fightModule.recovercmd_s();
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_attackerid = (int)e.m_data["m_attackerid"];
		msg.m_recovernumber = (int)e.m_data["m_recovernumber"];
		msg.m_isautorecover = (bool)e.m_data["m_isautorecover"];
		msg.m_skillid = e.m_data["m_skillid"].ToString();
		msg.m_buffid = e.m_data["m_buffid"].ToString();
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicepickupitemcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.pickupitemcmd_s msg = new Protocol.fightModule.pickupitemcmd_s();
		msg.m_itemid = (int)e.m_data["m_itemid"];
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceattackcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.attackcmd_s msg = new Protocol.fightModule.attackcmd_s();
		msg.m_dirx = (float)(double)e.m_data["m_dirx"];
		msg.m_diry = (float)(double)e.m_data["m_diry"];
		msg.m_dirz = (float)(double)e.m_data["m_dirz"];
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_creatcomandtime = (int)e.m_data["m_creatcomandtime"];
		msg.m_characterid = (int)e.m_data["m_characterid"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicechangeweaponcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.changeweaponcmd_s msg = new Protocol.fightModule.changeweaponcmd_s();
		msg.m_weaponid = e.m_data["m_weaponid"].ToString();
		msg.m_creatcomandtime = (int)e.m_data["m_creatcomandtime"];
		msg.m_characterid = (int)e.m_data["m_characterid"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceremovebuffcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.removebuffcmd_s msg = new Protocol.fightModule.removebuffcmd_s();
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_buffid = e.m_data["m_buffid"].ToString();
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceresurgencecmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.resurgencecmd_s msg = new Protocol.fightModule.resurgencecmd_s();
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_creatcomandtime = (int)e.m_data["m_creatcomandtime"];
		msg.m_characterid = (int)e.m_data["m_characterid"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefight_use_item_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.fight_use_item_s msg = new Protocol.fightModule.fight_use_item_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicefight_relive_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.fight_relive_s msg = new Protocol.fightModule.fight_relive_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicedestroyflyobjectcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.destroyflyobjectcmd_s msg = new Protocol.fightModule.destroyflyobjectcmd_s();
		msg.m_flyid = (int)e.m_data["m_flyid"];
		msg.m_isshowhiteffect = (bool)e.m_data["m_isshowhiteffect"];
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceblowflycmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.blowflycmd_s msg = new Protocol.fightModule.blowflycmd_s();
		msg.m_flyerid = (int)e.m_data["m_flyerid"];
		msg.m_attackerid = (int)e.m_data["m_attackerid"];
		msg.m_shiftid = e.m_data["m_shiftid"].ToString();
		msg.m_attackerposx = (float)(double)e.m_data["m_attackerposx"];
		msg.m_attackerposy = (float)(double)e.m_data["m_attackerposy"];
		msg.m_attackerposz = (float)(double)e.m_data["m_attackerposz"];
		msg.m_hurterposx = (float)(double)e.m_data["m_hurterposx"];
		msg.m_hurterposy = (float)(double)e.m_data["m_hurterposy"];
		msg.m_hurterposz = (float)(double)e.m_data["m_hurterposz"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicedestroyitemcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.destroyitemcmd_s msg = new Protocol.fightModule.destroyitemcmd_s();
		msg.m_itemid = (int)e.m_data["m_itemid"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicedestroybloodvialcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.destroybloodvialcmd_s msg = new Protocol.fightModule.destroybloodvialcmd_s();
		msg.m_bloodvialid = (int)e.m_data["m_bloodvialid"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicediecmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.diecmd_s msg = new Protocol.fightModule.diecmd_s();
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_killerid = (int)e.m_data["m_killerid"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicepickupbloodvialcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.pickupbloodvialcmd_s msg = new Protocol.fightModule.pickupbloodvialcmd_s();
		msg.m_bloodvialid = (int)e.m_data["m_bloodvialid"];
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicecreatebloodvialcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.createbloodvialcmd_s msg = new Protocol.fightModule.createbloodvialcmd_s();
		msg.m_bloodvialid = (int)e.m_data["m_bloodvialid"];
		msg.m_camp = (int)e.m_data["m_camp"];
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Receviceaddbuffcmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.addbuffcmd_s msg = new Protocol.fightModule.addbuffcmd_s();
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_attackerid = (int)e.m_data["m_attackerid"];
		msg.m_buffid = e.m_data["m_buffid"].ToString();
		msg.m_skillid = e.m_data["m_skillid"].ToString();
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicedamagecmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.damagecmd_s msg = new Protocol.fightModule.damagecmd_s();
		msg.m_characterid = (int)e.m_data["m_characterid"];
		msg.m_attackerid = (int)e.m_data["m_attackerid"];
		msg.m_damagenumber = (int)e.m_data["m_damagenumber"];
		msg.m_skillid = e.m_data["m_skillid"].ToString();
		msg.m_buffid = e.m_data["m_buffid"].ToString();
		msg.m_crit = (bool)e.m_data["m_crit"];
		msg.m_disrupting = (bool)e.m_data["m_disrupting"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicecreateskilltokencmd_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.fightModule.createskilltokencmd_s msg = new Protocol.fightModule.createskilltokencmd_s();
		msg.m_skillid = e.m_data["m_skillid"].ToString();
		msg.m_createrid = (int)e.m_data["m_createrid"];
		msg.m_camp = (int)e.m_data["m_camp"];
		msg.m_posx = (float)(double)e.m_data["m_posx"];
		msg.m_posy = (float)(double)e.m_data["m_posy"];
		msg.m_posz = (float)(double)e.m_data["m_posz"];
		msg.m_dirx = (float)(double)e.m_data["m_dirx"];
		msg.m_diry = (float)(double)e.m_data["m_diry"];
		msg.m_dirz = (float)(double)e.m_data["m_dirz"];
		msg.m_executetime = (float)(double)e.m_data["m_executetime"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicechest_open_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.chestModule.chest_open_s msg = new Protocol.chestModule.chest_open_s();
		msg.id = (int)e.m_data["id"];
		
		EventService.DispatchTypeEvent(session,msg);
	}
	static void Recevicechest_init_s(SyncSession session ,ProtocolRequestBase e)
	{
		Protocol.chestModule.chest_init_s msg = new Protocol.chestModule.chest_init_s();
		
		EventService.DispatchTypeEvent(session,msg);
	}
	#endregion
}
