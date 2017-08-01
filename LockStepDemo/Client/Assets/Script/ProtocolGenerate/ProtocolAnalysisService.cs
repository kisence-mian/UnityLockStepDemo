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
		InputManager.AddListener<InputNetworkMessageEvent>("user_heartbeat",Receviceuser_heartbeat_c);
		InputManager.AddListener<InputNetworkMessageEvent>("role_attr_change",Recevicerole_attr_change_c);
		InputManager.AddListener<InputNetworkMessageEvent>("role_attr",Recevicerole_attr_c);
		InputManager.AddListener<InputNetworkMessageEvent>("role_kick",Recevicerole_kick_c);
		InputManager.AddListener<InputNetworkMessageEvent>("role_auth",Recevicerole_auth_c);
		InputManager.AddListener<InputNetworkMessageEvent>("role_money",Recevicerole_money_c);
		InputManager.AddListener<InputNetworkMessageEvent>("role_login",Recevicerole_login_c);
		InputManager.AddListener<InputNetworkMessageEvent>("role_create",Recevicerole_create_c);
		InputManager.AddListener<InputNetworkMessageEvent>("bag_changedgoods",Recevicebag_changedgoods_c);
		InputManager.AddListener<InputNetworkMessageEvent>("bag_add",Recevicebag_add_c);
		InputManager.AddListener<InputNetworkMessageEvent>("bag_diamondnum",Recevicebag_diamondnum_c);
		InputManager.AddListener<InputNetworkMessageEvent>("bag_info",Recevicebag_info_c);
		InputManager.AddListener<InputNetworkMessageEvent>("bag_use",Recevicebag_use_c);
		InputManager.AddListener<InputNetworkMessageEvent>("bag_sell",Recevicebag_sell_c);
		InputManager.AddListener<InputNetworkMessageEvent>("mail_attach",Recevicemail_attach_c);
		InputManager.AddListener<InputNetworkMessageEvent>("mail_del",Recevicemail_del_c);
		InputManager.AddListener<InputNetworkMessageEvent>("mail_list",Recevicemail_list_c);
		InputManager.AddListener<InputNetworkMessageEvent>("mail_read",Recevicemail_read_c);
		InputManager.AddListener<InputNetworkMessageEvent>("mail_new",Recevicemail_new_c);
		InputManager.AddListener<InputNetworkMessageEvent>("task_cat",Recevicetask_cat_c);
		InputManager.AddListener<InputNetworkMessageEvent>("task_ach",Recevicetask_ach_c);
		InputManager.AddListener<InputNetworkMessageEvent>("task_award",Recevicetask_award_c);
		InputManager.AddListener<InputNetworkMessageEvent>("task_list",Recevicetask_list_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_inlay",Receviceequip_inlay_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_enchanting",Receviceequip_enchanting_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_syn",Receviceequip_syn_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_wear",Receviceequip_wear_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_bag",Receviceequip_bag_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_list",Receviceequip_list_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_strength",Receviceequip_strength_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_skill",Receviceequip_skill_c);
		InputManager.AddListener<InputNetworkMessageEvent>("equip_fuse",Receviceequip_fuse_c);
		InputManager.AddListener<InputNetworkMessageEvent>("shop_buy",Receviceshop_buy_c);
		InputManager.AddListener<InputNetworkMessageEvent>("shop_list",Receviceshop_list_c);
		InputManager.AddListener<InputNetworkMessageEvent>("friend_search",Recevicefriend_search_c);
		InputManager.AddListener<InputNetworkMessageEvent>("friend_request",Recevicefriend_request_c);
		InputManager.AddListener<InputNetworkMessageEvent>("friend_new",Recevicefriend_new_c);
		InputManager.AddListener<InputNetworkMessageEvent>("friend_info",Recevicefriend_info_c);
		InputManager.AddListener<InputNetworkMessageEvent>("friend_op",Recevicefriend_op_c);
		InputManager.AddListener<InputNetworkMessageEvent>("chat_msg",Recevicechat_msg_c);
		InputManager.AddListener<InputNetworkMessageEvent>("guide_add",Receviceguide_add_c);
		InputManager.AddListener<InputNetworkMessageEvent>("guide_list",Receviceguide_list_c);
		InputManager.AddListener<InputNetworkMessageEvent>("signin_online",Recevicesignin_online_c);
		InputManager.AddListener<InputNetworkMessageEvent>("signin_award",Recevicesignin_award_c);
		InputManager.AddListener<InputNetworkMessageEvent>("signin_get",Recevicesignin_get_c);
		InputManager.AddListener<InputNetworkMessageEvent>("signin_add",Recevicesignin_add_c);
		InputManager.AddListener<InputNetworkMessageEvent>("signin_info",Recevicesignin_info_c);
		InputManager.AddListener<InputNetworkMessageEvent>("signin_rep",Recevicesignin_rep_c);
		InputManager.AddListener<InputNetworkMessageEvent>("liveness_list",Receviceliveness_list_c);
		InputManager.AddListener<InputNetworkMessageEvent>("liveness_award",Receviceliveness_award_c);
		InputManager.AddListener<InputNetworkMessageEvent>("time_resync",Recevicetime_resync_c);
		InputManager.AddListener<InputNetworkMessageEvent>("time_syncreturn",Recevicetime_syncreturn_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_users",Receviceroom2_users_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_info",Receviceroom2_info_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_timesync",Receviceroom2_timesync_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_citydel",Receviceroom2_citydel_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_changeserver",Receviceroom2_changeserver_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_citymove",Receviceroom2_citymove_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_cityadd",Receviceroom2_cityadd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_level",Receviceroom2_level_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_task",Receviceroom2_task_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_pool",Receviceroom2_pool_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_exit",Receviceroom2_exit_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_reflashp",Receviceroom2_reflashp_c);
		InputManager.AddListener<InputNetworkMessageEvent>("room2_join",Receviceroom2_join_c);
		InputManager.AddListener<InputNetworkMessageEvent>("addbuffcmd",Receviceaddbuffcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("recovercmd",Recevicerecovercmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("createskilltokencmd",Recevicecreateskilltokencmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("damagecmd",Recevicedamagecmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("pickupbloodvialcmd",Recevicepickupbloodvialcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("destroyitemcmd",Recevicedestroyitemcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("pickupitemcmd",Recevicepickupitemcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("createbloodvialcmd",Recevicecreatebloodvialcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("destroybloodvialcmd",Recevicedestroybloodvialcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_item_list",Recevicefight_item_list_c);
		InputManager.AddListener<InputNetworkMessageEvent>("blowflycmd",Receviceblowflycmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_relive",Recevicefight_relive_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_use_item",Recevicefight_use_item_c);
		InputManager.AddListener<InputNetworkMessageEvent>("removebuffcmd",Receviceremovebuffcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("resurgencecmd",Receviceresurgencecmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("diecmd",Recevicediecmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("changeweaponcmd",Recevicechangeweaponcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("attackcmd",Receviceattackcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_match",Recevicefight_match_c);
		InputManager.AddListener<InputNetworkMessageEvent>("createflyobjectcmd",Recevicecreateflyobjectcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_setelement",Recevicefight_setelement_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_loading",Recevicefight_loading_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_cancel",Recevicefight_cancel_c);
		InputManager.AddListener<InputNetworkMessageEvent>("createitemcmd",Recevicecreateitemcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("createcharactercmd",Recevicecreatecharactercmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_rank",Recevicefight_rank_c);
		InputManager.AddListener<InputNetworkMessageEvent>("destroyflyobjectcmd",Recevicedestroyflyobjectcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_item_num",Recevicefight_item_num_c);
		InputManager.AddListener<InputNetworkMessageEvent>("rotationcmd",Recevicerotationcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("traptriggercmd",Recevicetraptriggercmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("skillcmd",Receviceskillcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("movecmd",Recevicemovecmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_end",Recevicefight_end_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_element",Recevicefight_element_c);
		InputManager.AddListener<InputNetworkMessageEvent>("fight_setrole",Recevicefight_setrole_c);
		InputManager.AddListener<InputNetworkMessageEvent>("createtrapcmd",Recevicecreatetrapcmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("removecharactercmd",Receviceremovecharactercmd_c);
		InputManager.AddListener<InputNetworkMessageEvent>("chest_open",Recevicechest_open_c);
		InputManager.AddListener<InputNetworkMessageEvent>("chest_init",Recevicechest_init_c);
	}

	public static void Dispose()
	{
		InputManager.RemoveListener<InputNetworkMessageEvent>("user_heartbeat",Receviceuser_heartbeat_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("role_attr_change",Recevicerole_attr_change_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("role_attr",Recevicerole_attr_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("role_kick",Recevicerole_kick_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("role_auth",Recevicerole_auth_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("role_money",Recevicerole_money_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("role_login",Recevicerole_login_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("role_create",Recevicerole_create_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("bag_changedgoods",Recevicebag_changedgoods_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("bag_add",Recevicebag_add_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("bag_diamondnum",Recevicebag_diamondnum_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("bag_info",Recevicebag_info_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("bag_use",Recevicebag_use_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("bag_sell",Recevicebag_sell_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("mail_attach",Recevicemail_attach_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("mail_del",Recevicemail_del_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("mail_list",Recevicemail_list_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("mail_read",Recevicemail_read_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("mail_new",Recevicemail_new_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("task_cat",Recevicetask_cat_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("task_ach",Recevicetask_ach_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("task_award",Recevicetask_award_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("task_list",Recevicetask_list_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_inlay",Receviceequip_inlay_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_enchanting",Receviceequip_enchanting_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_syn",Receviceequip_syn_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_wear",Receviceequip_wear_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_bag",Receviceequip_bag_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_list",Receviceequip_list_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_strength",Receviceequip_strength_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_skill",Receviceequip_skill_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("equip_fuse",Receviceequip_fuse_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("shop_buy",Receviceshop_buy_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("shop_list",Receviceshop_list_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("friend_search",Recevicefriend_search_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("friend_request",Recevicefriend_request_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("friend_new",Recevicefriend_new_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("friend_info",Recevicefriend_info_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("friend_op",Recevicefriend_op_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("chat_msg",Recevicechat_msg_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("guide_add",Receviceguide_add_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("guide_list",Receviceguide_list_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("signin_online",Recevicesignin_online_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("signin_award",Recevicesignin_award_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("signin_get",Recevicesignin_get_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("signin_add",Recevicesignin_add_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("signin_info",Recevicesignin_info_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("signin_rep",Recevicesignin_rep_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("liveness_list",Receviceliveness_list_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("liveness_award",Receviceliveness_award_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("time_resync",Recevicetime_resync_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("time_syncreturn",Recevicetime_syncreturn_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_users",Receviceroom2_users_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_info",Receviceroom2_info_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_timesync",Receviceroom2_timesync_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_citydel",Receviceroom2_citydel_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_changeserver",Receviceroom2_changeserver_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_citymove",Receviceroom2_citymove_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_cityadd",Receviceroom2_cityadd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_level",Receviceroom2_level_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_task",Receviceroom2_task_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_pool",Receviceroom2_pool_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_exit",Receviceroom2_exit_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_reflashp",Receviceroom2_reflashp_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("room2_join",Receviceroom2_join_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("addbuffcmd",Receviceaddbuffcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("recovercmd",Recevicerecovercmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("createskilltokencmd",Recevicecreateskilltokencmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("damagecmd",Recevicedamagecmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("pickupbloodvialcmd",Recevicepickupbloodvialcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("destroyitemcmd",Recevicedestroyitemcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("pickupitemcmd",Recevicepickupitemcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("createbloodvialcmd",Recevicecreatebloodvialcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("destroybloodvialcmd",Recevicedestroybloodvialcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_item_list",Recevicefight_item_list_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("blowflycmd",Receviceblowflycmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_relive",Recevicefight_relive_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_use_item",Recevicefight_use_item_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("removebuffcmd",Receviceremovebuffcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("resurgencecmd",Receviceresurgencecmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("diecmd",Recevicediecmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("changeweaponcmd",Recevicechangeweaponcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("attackcmd",Receviceattackcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_match",Recevicefight_match_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("createflyobjectcmd",Recevicecreateflyobjectcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_setelement",Recevicefight_setelement_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_loading",Recevicefight_loading_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_cancel",Recevicefight_cancel_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("createitemcmd",Recevicecreateitemcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("createcharactercmd",Recevicecreatecharactercmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_rank",Recevicefight_rank_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("destroyflyobjectcmd",Recevicedestroyflyobjectcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_item_num",Recevicefight_item_num_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("rotationcmd",Recevicerotationcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("traptriggercmd",Recevicetraptriggercmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("skillcmd",Receviceskillcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("movecmd",Recevicemovecmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_end",Recevicefight_end_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_element",Recevicefight_element_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("fight_setrole",Recevicefight_setrole_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("createtrapcmd",Recevicecreatetrapcmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("removecharactercmd",Receviceremovecharactercmd_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("chest_open",Recevicechest_open_c);
		InputManager.RemoveListener<InputNetworkMessageEvent>("chest_init",Recevicechest_init_c);
	}
	public static void SendCommand (IProtocolMessageInterface cmd)
	{
		if(cmd is Protocol.user_heartbeat_s )
		{
			Senduser_heartbeat_s(cmd);
		}
		else if(cmd is Protocol.roleModule.role_auth_s )
		{
			Sendrole_auth_s(cmd);
		}
		else if(cmd is Protocol.roleModule.role_create_s )
		{
			Sendrole_create_s(cmd);
		}
		else if(cmd is Protocol.roleModule.role_money_s )
		{
			Sendrole_money_s(cmd);
		}
		else if(cmd is Protocol.roleModule.role_login_s )
		{
			Sendrole_login_s(cmd);
		}
		else if(cmd is Protocol.bagModule.bag_changedgoods_s )
		{
			Sendbag_changedgoods_s(cmd);
		}
		else if(cmd is Protocol.bagModule.bag_diamondnum_s )
		{
			Sendbag_diamondnum_s(cmd);
		}
		else if(cmd is Protocol.bagModule.bag_sell_s )
		{
			Sendbag_sell_s(cmd);
		}
		else if(cmd is Protocol.bagModule.bag_info_s )
		{
			Sendbag_info_s(cmd);
		}
		else if(cmd is Protocol.bagModule.bag_use_s )
		{
			Sendbag_use_s(cmd);
		}
		else if(cmd is Protocol.mailModule.mail_new_s )
		{
			Sendmail_new_s(cmd);
		}
		else if(cmd is Protocol.mailModule.mail_attach_s )
		{
			Sendmail_attach_s(cmd);
		}
		else if(cmd is Protocol.mailModule.mail_del_s )
		{
			Sendmail_del_s(cmd);
		}
		else if(cmd is Protocol.mailModule.mail_list_s )
		{
			Sendmail_list_s(cmd);
		}
		else if(cmd is Protocol.mailModule.mail_read_s )
		{
			Sendmail_read_s(cmd);
		}
		else if(cmd is Protocol.taskModule.task_cat_s )
		{
			Sendtask_cat_s(cmd);
		}
		else if(cmd is Protocol.taskModule.task_ach_s )
		{
			Sendtask_ach_s(cmd);
		}
		else if(cmd is Protocol.taskModule.task_award_s )
		{
			Sendtask_award_s(cmd);
		}
		else if(cmd is Protocol.taskModule.task_list_s )
		{
			Sendtask_list_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_enchanting_s )
		{
			Sendequip_enchanting_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_skill_s )
		{
			Sendequip_skill_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_inlay_s )
		{
			Sendequip_inlay_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_syn_s )
		{
			Sendequip_syn_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_wear_s )
		{
			Sendequip_wear_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_bag_s )
		{
			Sendequip_bag_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_list_s )
		{
			Sendequip_list_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_fuse_s )
		{
			Sendequip_fuse_s(cmd);
		}
		else if(cmd is Protocol.equipModule.equip_strength_s )
		{
			Sendequip_strength_s(cmd);
		}
		else if(cmd is Protocol.shopModule.shop_buy_s )
		{
			Sendshop_buy_s(cmd);
		}
		else if(cmd is Protocol.shopModule.shop_list_s )
		{
			Sendshop_list_s(cmd);
		}
		else if(cmd is Protocol.friendModule.friend_search_s )
		{
			Sendfriend_search_s(cmd);
		}
		else if(cmd is Protocol.friendModule.friend_request_s )
		{
			Sendfriend_request_s(cmd);
		}
		else if(cmd is Protocol.friendModule.friend_info_s )
		{
			Sendfriend_info_s(cmd);
		}
		else if(cmd is Protocol.friendModule.friend_op_s )
		{
			Sendfriend_op_s(cmd);
		}
		else if(cmd is Protocol.chatModule.chat_msg_s )
		{
			Sendchat_msg_s(cmd);
		}
		else if(cmd is Protocol.guideModule.guide_add_s )
		{
			Sendguide_add_s(cmd);
		}
		else if(cmd is Protocol.guideModule.guide_msg_s )
		{
			Sendguide_msg_s(cmd);
		}
		else if(cmd is Protocol.guideModule.guide_list_s )
		{
			Sendguide_list_s(cmd);
		}
		else if(cmd is Protocol.signinModule.signin_award_s )
		{
			Sendsignin_award_s(cmd);
		}
		else if(cmd is Protocol.signinModule.signin_get_s )
		{
			Sendsignin_get_s(cmd);
		}
		else if(cmd is Protocol.signinModule.signin_online_s )
		{
			Sendsignin_online_s(cmd);
		}
		else if(cmd is Protocol.signinModule.signin_info_s )
		{
			Sendsignin_info_s(cmd);
		}
		else if(cmd is Protocol.signinModule.signin_rep_s )
		{
			Sendsignin_rep_s(cmd);
		}
		else if(cmd is Protocol.signinModule.signin_add_s )
		{
			Sendsignin_add_s(cmd);
		}
		else if(cmd is Protocol.livenessModule.liveness_list_s )
		{
			Sendliveness_list_s(cmd);
		}
		else if(cmd is Protocol.livenessModule.liveness_award_s )
		{
			Sendliveness_award_s(cmd);
		}
		else if(cmd is Protocol.timeModule.time_resync_s )
		{
			Sendtime_resync_s(cmd);
		}
		else if(cmd is Protocol.timeModule.time_sync_s )
		{
			Sendtime_sync_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_info_s )
		{
			Sendroom2_info_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_cityadd_s )
		{
			Sendroom2_cityadd_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_timesync_s )
		{
			Sendroom2_timesync_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_report_s )
		{
			Sendroom2_report_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_cityexit_s )
		{
			Sendroom2_cityexit_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_citymove_s )
		{
			Sendroom2_citymove_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_pool_s )
		{
			Sendroom2_pool_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_level_s )
		{
			Sendroom2_level_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_task_s )
		{
			Sendroom2_task_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_join_s )
		{
			Sendroom2_join_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_exit_s )
		{
			Sendroom2_exit_s(cmd);
		}
		else if(cmd is Protocol.room2Module.room2_reflashp_s )
		{
			Sendroom2_reflashp_s(cmd);
		}
		else if(cmd is Protocol.fightModule.recovercmd_s )
		{
			Sendrecovercmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.pickupbloodvialcmd_s )
		{
			Sendpickupbloodvialcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.damagecmd_s )
		{
			Senddamagecmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.addbuffcmd_s )
		{
			Sendaddbuffcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.destroyitemcmd_s )
		{
			Senddestroyitemcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.pickupitemcmd_s )
		{
			Sendpickupitemcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.createbloodvialcmd_s )
		{
			Sendcreatebloodvialcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.destroybloodvialcmd_s )
		{
			Senddestroybloodvialcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.createskilltokencmd_s )
		{
			Sendcreateskilltokencmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.blowflycmd_s )
		{
			Sendblowflycmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.removebuffcmd_s )
		{
			Sendremovebuffcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.fight_relive_s )
		{
			Sendfight_relive_s(cmd);
		}
		else if(cmd is Protocol.fightModule.fight_use_item_s )
		{
			Sendfight_use_item_s(cmd);
		}
		else if(cmd is Protocol.fightModule.resurgencecmd_s )
		{
			Sendresurgencecmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.diecmd_s )
		{
			Senddiecmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.changeweaponcmd_s )
		{
			Sendchangeweaponcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.attackcmd_s )
		{
			Sendattackcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.fight_match_s )
		{
			Sendfight_match_s(cmd);
		}
		else if(cmd is Protocol.fightModule.createflyobjectcmd_s )
		{
			Sendcreateflyobjectcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.fight_setelement_s )
		{
			Sendfight_setelement_s(cmd);
		}
		else if(cmd is Protocol.fightModule.fight_loading_s )
		{
			Sendfight_loading_s(cmd);
		}
		else if(cmd is Protocol.fightModule.createcharactercmd_s )
		{
			Sendcreatecharactercmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.fight_cancel_s )
		{
			Sendfight_cancel_s(cmd);
		}
		else if(cmd is Protocol.fightModule.createitemcmd_s )
		{
			Sendcreateitemcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.destroyflyobjectcmd_s )
		{
			Senddestroyflyobjectcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.rotationcmd_s )
		{
			Sendrotationcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.traptriggercmd_s )
		{
			Sendtraptriggercmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.skillcmd_s )
		{
			Sendskillcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.movecmd_s )
		{
			Sendmovecmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.createtrapcmd_s )
		{
			Sendcreatetrapcmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.fight_setrole_s )
		{
			Sendfight_setrole_s(cmd);
		}
		else if(cmd is Protocol.fightModule.removecharactercmd_s )
		{
			Sendremovecharactercmd_s(cmd);
		}
		else if(cmd is Protocol.fightModule.fight_end_s )
		{
			Sendfight_end_s(cmd);
		}
		else if(cmd is Protocol.chestModule.chest_open_s )
		{
			Sendchest_open_s(cmd);
		}
		else if(cmd is Protocol.chestModule.chest_init_s )
		{
			Sendchest_init_s(cmd);
		}
		else
		{
			throw new Exception("SendCommand Exception : 不支持的消息类型!" + cmd.GetType());
		}
	}
	static void Senduser_heartbeat_s(IProtocolMessageInterface msg)
	{
		Protocol.user_heartbeat_s e = (Protocol.user_heartbeat_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("user_heartbeat",data);
	}
	static void Sendrole_auth_s(IProtocolMessageInterface msg)
	{
		Protocol.roleModule.role_auth_s e = (Protocol.roleModule.role_auth_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("role_auth",data);
	}
	static void Sendrole_create_s(IProtocolMessageInterface msg)
	{
		Protocol.roleModule.role_create_s e = (Protocol.roleModule.role_create_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("nick", e.nick);
		data.Add("sex", e.sex);
		data.Add("model_id", e.model_id);
		data.Add("head", e.head);
		NetworkManager.SendMessage("role_create",data);
	}
	static void Sendrole_money_s(IProtocolMessageInterface msg)
	{
		Protocol.roleModule.role_money_s e = (Protocol.roleModule.role_money_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("role_money",data);
	}
	static void Sendrole_login_s(IProtocolMessageInterface msg)
	{
		Protocol.roleModule.role_login_s e = (Protocol.roleModule.role_login_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("account", e.account);
		NetworkManager.SendMessage("role_login",data);
	}
	static void Sendbag_changedgoods_s(IProtocolMessageInterface msg)
	{
		Protocol.bagModule.bag_changedgoods_s e = (Protocol.bagModule.bag_changedgoods_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("bag_changedgoods",data);
	}
	static void Sendbag_diamondnum_s(IProtocolMessageInterface msg)
	{
		Protocol.bagModule.bag_diamondnum_s e = (Protocol.bagModule.bag_diamondnum_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("gold", e.gold);
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <e.goods.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", e.goods[i2].id);
				data2.Add("num", e.goods[i2].num);
				list2.Add( data2);
			}
			data.Add("goods",list2);
		}
		NetworkManager.SendMessage("bag_diamondnum",data);
	}
	static void Sendbag_sell_s(IProtocolMessageInterface msg)
	{
		Protocol.bagModule.bag_sell_s e = (Protocol.bagModule.bag_sell_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
			for(int i2 = 0;i2 <e.sellgoods.Count ; i2++)
			{
				Dictionary<string, object> data2 = new Dictionary<string, object>();
				data2.Add("id", e.sellgoods[i2].id);
				data2.Add("num", e.sellgoods[i2].num);
				list2.Add( data2);
			}
			data.Add("sellgoods",list2);
		}
		NetworkManager.SendMessage("bag_sell",data);
	}
	static void Sendbag_info_s(IProtocolMessageInterface msg)
	{
		Protocol.bagModule.bag_info_s e = (Protocol.bagModule.bag_info_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("bag_info",data);
	}
	static void Sendbag_use_s(IProtocolMessageInterface msg)
	{
		Protocol.bagModule.bag_use_s e = (Protocol.bagModule.bag_use_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		data.Add("num", e.num);
		NetworkManager.SendMessage("bag_use",data);
	}
	static void Sendmail_new_s(IProtocolMessageInterface msg)
	{
		Protocol.mailModule.mail_new_s e = (Protocol.mailModule.mail_new_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("mail_new",data);
	}
	static void Sendmail_attach_s(IProtocolMessageInterface msg)
	{
		Protocol.mailModule.mail_attach_s e = (Protocol.mailModule.mail_attach_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("mail_attach",data);
	}
	static void Sendmail_del_s(IProtocolMessageInterface msg)
	{
		Protocol.mailModule.mail_del_s e = (Protocol.mailModule.mail_del_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<System.Int32> list = new List<System.Int32>();
			for(int i = 0;i <e.ids.Count ; i++)
			{
				list.Add( e.ids[i]);
			}
			data.Add("ids",list);
		}
		NetworkManager.SendMessage("mail_del",data);
	}
	static void Sendmail_list_s(IProtocolMessageInterface msg)
	{
		Protocol.mailModule.mail_list_s e = (Protocol.mailModule.mail_list_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("mail_list",data);
	}
	static void Sendmail_read_s(IProtocolMessageInterface msg)
	{
		Protocol.mailModule.mail_read_s e = (Protocol.mailModule.mail_read_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("mail_read",data);
	}
	static void Sendtask_cat_s(IProtocolMessageInterface msg)
	{
		Protocol.taskModule.task_cat_s e = (Protocol.taskModule.task_cat_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("task_cat",data);
	}
	static void Sendtask_ach_s(IProtocolMessageInterface msg)
	{
		Protocol.taskModule.task_ach_s e = (Protocol.taskModule.task_ach_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("type", e.type);
		NetworkManager.SendMessage("task_ach",data);
	}
	static void Sendtask_award_s(IProtocolMessageInterface msg)
	{
		Protocol.taskModule.task_award_s e = (Protocol.taskModule.task_award_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("task_award",data);
	}
	static void Sendtask_list_s(IProtocolMessageInterface msg)
	{
		Protocol.taskModule.task_list_s e = (Protocol.taskModule.task_list_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("task_list",data);
	}
	static void Sendequip_enchanting_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_enchanting_s e = (Protocol.equipModule.equip_enchanting_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("item_id", e.item_id);
		NetworkManager.SendMessage("equip_enchanting",data);
	}
	static void Sendequip_skill_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_skill_s e = (Protocol.equipModule.equip_skill_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		data.Add("skill_id", e.skill_id);
		NetworkManager.SendMessage("equip_skill",data);
	}
	static void Sendequip_inlay_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_inlay_s e = (Protocol.equipModule.equip_inlay_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("item_id", e.item_id);
		data.Add("diamondid", e.diamondid);
		data.Add("diamond_pos", e.diamond_pos);
		NetworkManager.SendMessage("equip_inlay",data);
	}
	static void Sendequip_syn_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_syn_s e = (Protocol.equipModule.equip_syn_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		data.Add("num", e.num);
		NetworkManager.SendMessage("equip_syn",data);
	}
	static void Sendequip_wear_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_wear_s e = (Protocol.equipModule.equip_wear_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("pos", e.pos);
		data.Add("id", e.id);
		data.Add("item_id", e.item_id);
		NetworkManager.SendMessage("equip_wear",data);
	}
	static void Sendequip_bag_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_bag_s e = (Protocol.equipModule.equip_bag_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("equip_bag",data);
	}
	static void Sendequip_list_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_list_s e = (Protocol.equipModule.equip_list_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("equip_list",data);
	}
	static void Sendequip_fuse_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_fuse_s e = (Protocol.equipModule.equip_fuse_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<System.Int32> list = new List<System.Int32>();
			for(int i = 0;i <e.ids.Count ; i++)
			{
				list.Add( e.ids[i]);
			}
			data.Add("ids",list);
		}
		NetworkManager.SendMessage("equip_fuse",data);
	}
	static void Sendequip_strength_s(IProtocolMessageInterface msg)
	{
		Protocol.equipModule.equip_strength_s e = (Protocol.equipModule.equip_strength_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("equip_strength",data);
	}
	static void Sendshop_buy_s(IProtocolMessageInterface msg)
	{
		Protocol.shopModule.shop_buy_s e = (Protocol.shopModule.shop_buy_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("shop_buy",data);
	}
	static void Sendshop_list_s(IProtocolMessageInterface msg)
	{
		Protocol.shopModule.shop_list_s e = (Protocol.shopModule.shop_list_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("type", e.type);
		NetworkManager.SendMessage("shop_list",data);
	}
	static void Sendfriend_search_s(IProtocolMessageInterface msg)
	{
		Protocol.friendModule.friend_search_s e = (Protocol.friendModule.friend_search_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("name", e.name);
		NetworkManager.SendMessage("friend_search",data);
	}
	static void Sendfriend_request_s(IProtocolMessageInterface msg)
	{
		Protocol.friendModule.friend_request_s e = (Protocol.friendModule.friend_request_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("friend_request",data);
	}
	static void Sendfriend_info_s(IProtocolMessageInterface msg)
	{
		Protocol.friendModule.friend_info_s e = (Protocol.friendModule.friend_info_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("friend_info",data);
	}
	static void Sendfriend_op_s(IProtocolMessageInterface msg)
	{
		Protocol.friendModule.friend_op_s e = (Protocol.friendModule.friend_op_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("role_id", e.role_id);
		data.Add("type", e.type);
		NetworkManager.SendMessage("friend_op",data);
	}
	static void Sendchat_msg_s(IProtocolMessageInterface msg)
	{
		Protocol.chatModule.chat_msg_s e = (Protocol.chatModule.chat_msg_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("to", e.to);
		data.Add("channel", e.channel);
		data.Add("msg", e.msg);
		NetworkManager.SendMessage("chat_msg",data);
	}
	static void Sendguide_add_s(IProtocolMessageInterface msg)
	{
		Protocol.guideModule.guide_add_s e = (Protocol.guideModule.guide_add_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("guide_add",data);
	}
	static void Sendguide_msg_s(IProtocolMessageInterface msg)
	{
		Protocol.guideModule.guide_msg_s e = (Protocol.guideModule.guide_msg_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("guide_msg",data);
	}
	static void Sendguide_list_s(IProtocolMessageInterface msg)
	{
		Protocol.guideModule.guide_list_s e = (Protocol.guideModule.guide_list_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("guide_list",data);
	}
	static void Sendsignin_award_s(IProtocolMessageInterface msg)
	{
		Protocol.signinModule.signin_award_s e = (Protocol.signinModule.signin_award_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("signin_award",data);
	}
	static void Sendsignin_get_s(IProtocolMessageInterface msg)
	{
		Protocol.signinModule.signin_get_s e = (Protocol.signinModule.signin_get_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("signin_get",data);
	}
	static void Sendsignin_online_s(IProtocolMessageInterface msg)
	{
		Protocol.signinModule.signin_online_s e = (Protocol.signinModule.signin_online_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("signin_online",data);
	}
	static void Sendsignin_info_s(IProtocolMessageInterface msg)
	{
		Protocol.signinModule.signin_info_s e = (Protocol.signinModule.signin_info_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("signin_info",data);
	}
	static void Sendsignin_rep_s(IProtocolMessageInterface msg)
	{
		Protocol.signinModule.signin_rep_s e = (Protocol.signinModule.signin_rep_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("day", e.day);
		NetworkManager.SendMessage("signin_rep",data);
	}
	static void Sendsignin_add_s(IProtocolMessageInterface msg)
	{
		Protocol.signinModule.signin_add_s e = (Protocol.signinModule.signin_add_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("signin_add",data);
	}
	static void Sendliveness_list_s(IProtocolMessageInterface msg)
	{
		Protocol.livenessModule.liveness_list_s e = (Protocol.livenessModule.liveness_list_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("liveness_list",data);
	}
	static void Sendliveness_award_s(IProtocolMessageInterface msg)
	{
		Protocol.livenessModule.liveness_award_s e = (Protocol.livenessModule.liveness_award_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("pos", e.pos);
		NetworkManager.SendMessage("liveness_award",data);
	}
	static void Sendtime_resync_s(IProtocolMessageInterface msg)
	{
		Protocol.timeModule.time_resync_s e = (Protocol.timeModule.time_resync_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("time_resync",data);
	}
	static void Sendtime_sync_s(IProtocolMessageInterface msg)
	{
		Protocol.timeModule.time_sync_s e = (Protocol.timeModule.time_sync_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("sendtime", e.sendtime);
		NetworkManager.SendMessage("time_sync",data);
	}
	static void Sendroom2_info_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_info_s e = (Protocol.room2Module.room2_info_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		{
			List<System.String> list = new List<System.String>();
			for(int i = 0;i <e.roominfo.Count ; i++)
			{
				list.Add( e.roominfo[i]);
			}
			data.Add("roominfo",list);
		}
		data.Add("gameplay", e.gameplay);
		NetworkManager.SendMessage("room2_info",data);
	}
	static void Sendroom2_cityadd_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_cityadd_s e = (Protocol.room2Module.room2_cityadd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("room2_cityadd",data);
	}
	static void Sendroom2_timesync_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_timesync_s e = (Protocol.room2Module.room2_timesync_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("client_time", e.client_time);
		NetworkManager.SendMessage("room2_timesync",data);
	}
	static void Sendroom2_report_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_report_s e = (Protocol.room2Module.room2_report_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("time", e.time);
		NetworkManager.SendMessage("room2_report",data);
	}
	static void Sendroom2_cityexit_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_cityexit_s e = (Protocol.room2Module.room2_cityexit_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("room2_cityexit",data);
	}
	static void Sendroom2_citymove_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_citymove_s e = (Protocol.room2Module.room2_citymove_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("role_id", e.role_id);
		data.Add("x", e.x);
		data.Add("z", e.z);
		data.Add("time", e.time);
		NetworkManager.SendMessage("room2_citymove",data);
	}
	static void Sendroom2_pool_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_pool_s e = (Protocol.room2Module.room2_pool_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("room2_pool",data);
	}
	static void Sendroom2_level_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_level_s e = (Protocol.room2Module.room2_level_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("op", e.op);
		data.Add("val", e.val);
		NetworkManager.SendMessage("room2_level",data);
	}
	static void Sendroom2_task_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_task_s e = (Protocol.room2Module.room2_task_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("room2_task",data);
	}
	static void Sendroom2_join_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_join_s e = (Protocol.room2Module.room2_join_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("room2_join",data);
	}
	static void Sendroom2_exit_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_exit_s e = (Protocol.room2Module.room2_exit_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("room2_exit",data);
	}
	static void Sendroom2_reflashp_s(IProtocolMessageInterface msg)
	{
		Protocol.room2Module.room2_reflashp_s e = (Protocol.room2Module.room2_reflashp_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("room2_reflashp",data);
	}
	static void Sendrecovercmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.recovercmd_s e = (Protocol.fightModule.recovercmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_attackerid", e.m_attackerid);
		data.Add("m_recovernumber", e.m_recovernumber);
		data.Add("m_isautorecover", e.m_isautorecover);
		data.Add("m_skillid", e.m_skillid);
		data.Add("m_buffid", e.m_buffid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("recovercmd",data);
	}
	static void Sendpickupbloodvialcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.pickupbloodvialcmd_s e = (Protocol.fightModule.pickupbloodvialcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_bloodvialid", e.m_bloodvialid);
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("pickupbloodvialcmd",data);
	}
	static void Senddamagecmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.damagecmd_s e = (Protocol.fightModule.damagecmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_attackerid", e.m_attackerid);
		data.Add("m_damagenumber", e.m_damagenumber);
		data.Add("m_skillid", e.m_skillid);
		data.Add("m_buffid", e.m_buffid);
		data.Add("m_crit", e.m_crit);
		data.Add("m_disrupting", e.m_disrupting);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("damagecmd",data);
	}
	static void Sendaddbuffcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.addbuffcmd_s e = (Protocol.fightModule.addbuffcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_attackerid", e.m_attackerid);
		data.Add("m_buffid", e.m_buffid);
		data.Add("m_skillid", e.m_skillid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("addbuffcmd",data);
	}
	static void Senddestroyitemcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.destroyitemcmd_s e = (Protocol.fightModule.destroyitemcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_itemid", e.m_itemid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("destroyitemcmd",data);
	}
	static void Sendpickupitemcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.pickupitemcmd_s e = (Protocol.fightModule.pickupitemcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_itemid", e.m_itemid);
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("pickupitemcmd",data);
	}
	static void Sendcreatebloodvialcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.createbloodvialcmd_s e = (Protocol.fightModule.createbloodvialcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_bloodvialid", e.m_bloodvialid);
		data.Add("m_camp", e.m_camp);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("createbloodvialcmd",data);
	}
	static void Senddestroybloodvialcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.destroybloodvialcmd_s e = (Protocol.fightModule.destroybloodvialcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_bloodvialid", e.m_bloodvialid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("destroybloodvialcmd",data);
	}
	static void Sendcreateskilltokencmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.createskilltokencmd_s e = (Protocol.fightModule.createskilltokencmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_skillid", e.m_skillid);
		data.Add("m_createrid", e.m_createrid);
		data.Add("m_camp", e.m_camp);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_dirx", e.m_dirx);
		data.Add("m_diry", e.m_diry);
		data.Add("m_dirz", e.m_dirz);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("createskilltokencmd",data);
	}
	static void Sendblowflycmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.blowflycmd_s e = (Protocol.fightModule.blowflycmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_flyerid", e.m_flyerid);
		data.Add("m_attackerid", e.m_attackerid);
		data.Add("m_shiftid", e.m_shiftid);
		data.Add("m_attackerposx", e.m_attackerposx);
		data.Add("m_attackerposy", e.m_attackerposy);
		data.Add("m_attackerposz", e.m_attackerposz);
		data.Add("m_hurterposx", e.m_hurterposx);
		data.Add("m_hurterposy", e.m_hurterposy);
		data.Add("m_hurterposz", e.m_hurterposz);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("blowflycmd",data);
	}
	static void Sendremovebuffcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.removebuffcmd_s e = (Protocol.fightModule.removebuffcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_buffid", e.m_buffid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("removebuffcmd",data);
	}
	static void Sendfight_relive_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.fight_relive_s e = (Protocol.fightModule.fight_relive_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("fight_relive",data);
	}
	static void Sendfight_use_item_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.fight_use_item_s e = (Protocol.fightModule.fight_use_item_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("fight_use_item",data);
	}
	static void Sendresurgencecmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.resurgencecmd_s e = (Protocol.fightModule.resurgencecmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_creatcomandtime", e.m_creatcomandtime);
		data.Add("m_characterid", e.m_characterid);
		NetworkManager.SendMessage("resurgencecmd",data);
	}
	static void Senddiecmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.diecmd_s e = (Protocol.fightModule.diecmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_killerid", e.m_killerid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("diecmd",data);
	}
	static void Sendchangeweaponcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.changeweaponcmd_s e = (Protocol.fightModule.changeweaponcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_weaponid", e.m_weaponid);
		data.Add("m_creatcomandtime", e.m_creatcomandtime);
		data.Add("m_characterid", e.m_characterid);
		NetworkManager.SendMessage("changeweaponcmd",data);
	}
	static void Sendattackcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.attackcmd_s e = (Protocol.fightModule.attackcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_dirx", e.m_dirx);
		data.Add("m_diry", e.m_diry);
		data.Add("m_dirz", e.m_dirz);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_creatcomandtime", e.m_creatcomandtime);
		data.Add("m_characterid", e.m_characterid);
		NetworkManager.SendMessage("attackcmd",data);
	}
	static void Sendfight_match_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.fight_match_s e = (Protocol.fightModule.fight_match_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("fight_match",data);
	}
	static void Sendcreateflyobjectcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.createflyobjectcmd_s e = (Protocol.fightModule.createflyobjectcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_flyname", e.m_flyname);
		data.Add("m_flyid", e.m_flyid);
		data.Add("m_skillid", e.m_skillid);
		data.Add("m_createrid", e.m_createrid);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_dirx", e.m_dirx);
		data.Add("m_diry", e.m_diry);
		data.Add("m_dirz", e.m_dirz);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("createflyobjectcmd",data);
	}
	static void Sendfight_setelement_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.fight_setelement_s e = (Protocol.fightModule.fight_setelement_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("item1", e.item1);
		data.Add("item2", e.item2);
		NetworkManager.SendMessage("fight_setelement",data);
	}
	static void Sendfight_loading_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.fight_loading_s e = (Protocol.fightModule.fight_loading_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("fight_loading",data);
	}
	static void Sendcreatecharactercmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.createcharactercmd_s e = (Protocol.fightModule.createcharactercmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_charactertype", e.m_charactertype);
		data.Add("m_charactername", e.m_charactername);
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_camp", e.m_camp);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_dirx", e.m_dirx);
		data.Add("m_diry", e.m_diry);
		data.Add("m_dirz", e.m_dirz);
		data.Add("m_amplification", e.m_amplification);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("createcharactercmd",data);
	}
	static void Sendfight_cancel_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.fight_cancel_s e = (Protocol.fightModule.fight_cancel_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("fight_cancel",data);
	}
	static void Sendcreateitemcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.createitemcmd_s e = (Protocol.fightModule.createitemcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_itemid", e.m_itemid);
		data.Add("m_itemname", e.m_itemname);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("createitemcmd",data);
	}
	static void Senddestroyflyobjectcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.destroyflyobjectcmd_s e = (Protocol.fightModule.destroyflyobjectcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_flyid", e.m_flyid);
		data.Add("m_isshowhiteffect", e.m_isshowhiteffect);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("destroyflyobjectcmd",data);
	}
	static void Sendrotationcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.rotationcmd_s e = (Protocol.fightModule.rotationcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_dirx", e.m_dirx);
		data.Add("m_diry", e.m_diry);
		data.Add("m_dirz", e.m_dirz);
		data.Add("m_creatcomandtime", e.m_creatcomandtime);
		data.Add("m_characterid", e.m_characterid);
		NetworkManager.SendMessage("rotationcmd",data);
	}
	static void Sendtraptriggercmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.traptriggercmd_s e = (Protocol.fightModule.traptriggercmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_trapid", e.m_trapid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("traptriggercmd",data);
	}
	static void Sendskillcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.skillcmd_s e = (Protocol.fightModule.skillcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_skillid", e.m_skillid);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_skilldirx", e.m_skilldirx);
		data.Add("m_skilldiry", e.m_skilldiry);
		data.Add("m_skilldirz", e.m_skilldirz);
		data.Add("m_creatcomandtime", e.m_creatcomandtime);
		data.Add("m_characterid", e.m_characterid);
		NetworkManager.SendMessage("skillcmd",data);
	}
	static void Sendmovecmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.movecmd_s e = (Protocol.fightModule.movecmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_dirx", e.m_dirx);
		data.Add("m_dirz", e.m_dirz);
		data.Add("m_test", e.m_test);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_isonlyturn", e.m_isonlyturn);
		data.Add("m_creatcomandtime", e.m_creatcomandtime);
		data.Add("m_characterid", e.m_characterid);
		NetworkManager.SendMessage("movecmd",data);
	}
	static void Sendcreatetrapcmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.createtrapcmd_s e = (Protocol.fightModule.createtrapcmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_trapid", e.m_trapid);
		data.Add("m_trapname", e.m_trapname);
		data.Add("m_posx", e.m_posx);
		data.Add("m_posy", e.m_posy);
		data.Add("m_posz", e.m_posz);
		data.Add("m_dirx", e.m_dirx);
		data.Add("m_diry", e.m_diry);
		data.Add("m_dirz", e.m_dirz);
		data.Add("m_camp", e.m_camp);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("createtrapcmd",data);
	}
	static void Sendfight_setrole_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.fight_setrole_s e = (Protocol.fightModule.fight_setrole_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("fight_setrole",data);
	}
	static void Sendremovecharactercmd_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.removecharactercmd_s e = (Protocol.fightModule.removecharactercmd_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("m_characterid", e.m_characterid);
		data.Add("m_executetime", e.m_executetime);
		NetworkManager.SendMessage("removecharactercmd",data);
	}
	static void Sendfight_end_s(IProtocolMessageInterface msg)
	{
		Protocol.fightModule.fight_end_s e = (Protocol.fightModule.fight_end_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("fight_end",data);
	}
	static void Sendchest_open_s(IProtocolMessageInterface msg)
	{
		Protocol.chestModule.chest_open_s e = (Protocol.chestModule.chest_open_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("id", e.id);
		NetworkManager.SendMessage("chest_open",data);
	}
	static void Sendchest_init_s(IProtocolMessageInterface msg)
	{
		Protocol.chestModule.chest_init_s e = (Protocol.chestModule.chest_init_s)msg;
		Dictionary<string, object> data = new Dictionary<string, object>();
		NetworkManager.SendMessage("chest_init",data);
	}
	#endregion

	#region 事件接收
	static void Receviceuser_heartbeat_c(InputNetworkMessageEvent e)
	{
		Protocol.user_heartbeat_c msg = new Protocol.user_heartbeat_c();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerole_attr_change_c(InputNetworkMessageEvent e)
	{
		Protocol.roleModule.role_attr_change_c msg = new Protocol.roleModule.role_attr_change_c();
		msg.role_id = (int)e.Data["role_id"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["changes"];
			List<Protocol.p_role_attr_change> list2 = new List<Protocol.p_role_attr_change>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_role_attr_change tmp2 = new Protocol.p_role_attr_change();
				tmp2.key = data2[i2]["key"].ToString();
				tmp2.value = (int)data2[i2]["value"];
				list2.Add(tmp2);
			}
			msg.changes =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerole_attr_c(InputNetworkMessageEvent e)
	{
		Protocol.roleModule.role_attr_c msg = new Protocol.roleModule.role_attr_c();
		msg.role_id = (int)e.Data["role_id"];
		msg.gold = (int)e.Data["gold"];
		msg.diamond = (int)e.Data["diamond"];
		msg.lv = (int)e.Data["lv"];
		msg.exp = (int)e.Data["exp"];
		msg.expneed = (int)e.Data["expneed"];
		msg.phy = (int)e.Data["phy"];
		msg.renown = (int)e.Data["renown"];
		msg.power = (int)e.Data["power"];
		msg.att = (int)e.Data["att"];
		msg.def = (int)e.Data["def"];
		msg.hp = (int)e.Data["hp"];
		msg.hprecover = (int)e.Data["hprecover"];
		msg.crit = (int)e.Data["crit"];
		msg.critdamage = (int)e.Data["critdamage"];
		msg.ignoredef = (int)e.Data["ignoredef"];
		msg.hpabsorb = (int)e.Data["hpabsorb"];
		msg.movespeed = (int)e.Data["movespeed"];
		msg.tough = (int)e.Data["tough"];
		msg.model_id = e.Data["model_id"].ToString();
		msg.oid = (int)e.Data["oid"];
		msg.nick = e.Data["nick"].ToString();
		msg.head = e.Data["head"].ToString();
		msg.sex = (int)e.Data["sex"];
		msg.weapon = e.Data["weapon"].ToString();
		msg.hero = e.Data["hero"].ToString();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerole_kick_c(InputNetworkMessageEvent e)
	{
		Protocol.roleModule.role_kick_c msg = new Protocol.roleModule.role_kick_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerole_auth_c(InputNetworkMessageEvent e)
	{
		Protocol.roleModule.role_auth_c msg = new Protocol.roleModule.role_auth_c();
		msg.key = e.Data["key"].ToString();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerole_money_c(InputNetworkMessageEvent e)
	{
		Protocol.roleModule.role_money_c msg = new Protocol.roleModule.role_money_c();
		msg.gold = (int)e.Data["gold"];
		msg.diamond = (int)e.Data["diamond"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerole_login_c(InputNetworkMessageEvent e)
	{
		Protocol.roleModule.role_login_c msg = new Protocol.roleModule.role_login_c();
		msg.code = (int)e.Data["code"];
		msg.create = (int)e.Data["create"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerole_create_c(InputNetworkMessageEvent e)
	{
		Protocol.roleModule.role_create_c msg = new Protocol.roleModule.role_create_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicebag_changedgoods_c(InputNetworkMessageEvent e)
	{
		Protocol.bagModule.bag_changedgoods_c msg = new Protocol.bagModule.bag_changedgoods_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["del"];
			List<Protocol.goods1> list2 = new List<Protocol.goods1>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.goods1 tmp2 = new Protocol.goods1();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.del =  list2;
		}
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["changed"];
			List<Protocol.goods1> list2 = new List<Protocol.goods1>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.goods1 tmp2 = new Protocol.goods1();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.changed =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicebag_add_c(InputNetworkMessageEvent e)
	{
		Protocol.bagModule.bag_add_c msg = new Protocol.bagModule.bag_add_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["bag"];
			List<Protocol.goods1> list2 = new List<Protocol.goods1>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.goods1 tmp2 = new Protocol.goods1();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.bag =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicebag_diamondnum_c(InputNetworkMessageEvent e)
	{
		Protocol.bagModule.bag_diamondnum_c msg = new Protocol.bagModule.bag_diamondnum_c();
		msg.diamondnum = (int)e.Data["diamondnum"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicebag_info_c(InputNetworkMessageEvent e)
	{
		Protocol.bagModule.bag_info_c msg = new Protocol.bagModule.bag_info_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.goods1> list2 = new List<Protocol.goods1>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.goods1 tmp2 = new Protocol.goods1();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicebag_use_c(InputNetworkMessageEvent e)
	{
		Protocol.bagModule.bag_use_c msg = new Protocol.bagModule.bag_use_c();
		msg.result = (int)e.Data["result"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicebag_sell_c(InputNetworkMessageEvent e)
	{
		Protocol.bagModule.bag_sell_c msg = new Protocol.bagModule.bag_sell_c();
		msg.result = (int)e.Data["result"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicemail_attach_c(InputNetworkMessageEvent e)
	{
		Protocol.mailModule.mail_attach_c msg = new Protocol.mailModule.mail_attach_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicemail_del_c(InputNetworkMessageEvent e)
	{
		Protocol.mailModule.mail_del_c msg = new Protocol.mailModule.mail_del_c();
		msg.ids = (List<Int32>)e.Data["ids"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicemail_list_c(InputNetworkMessageEvent e)
	{
		Protocol.mailModule.mail_list_c msg = new Protocol.mailModule.mail_list_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.p_mail_info> list2 = new List<Protocol.p_mail_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_mail_info tmp2 = new Protocol.p_mail_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.title = data2[i2]["title"].ToString();
				tmp2.content = data2[i2]["content"].ToString();
				tmp2.isread = (int)data2[i2]["isread"];
				{
					List<Dictionary<string, object>> data4 = (List<Dictionary<string, object>>)data2[i2]["items"];
					List<Protocol.p_mail_item> list4 = new List<Protocol.p_mail_item>();
					for (int i4 = 0; i4 < data4.Count; i4++)
					{
						Protocol.p_mail_item tmp4 = new Protocol.p_mail_item();
						tmp4.id = (int)data4[i4]["id"];
						tmp4.num = (int)data4[i4]["num"];
						list4.Add(tmp4);
					}
					tmp2.items =  list4;
				}
				tmp2.time = (int)data2[i2]["time"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicemail_read_c(InputNetworkMessageEvent e)
	{
		Protocol.mailModule.mail_read_c msg = new Protocol.mailModule.mail_read_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicemail_new_c(InputNetworkMessageEvent e)
	{
		Protocol.mailModule.mail_new_c msg = new Protocol.mailModule.mail_new_c();
		msg.type = (int)e.Data["type"];
		msg.num = (int)e.Data["num"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicetask_cat_c(InputNetworkMessageEvent e)
	{
		Protocol.taskModule.task_cat_c msg = new Protocol.taskModule.task_cat_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["data"];
			List<Protocol.p_task_cat> list2 = new List<Protocol.p_task_cat>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_task_cat tmp2 = new Protocol.p_task_cat();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.point = (int)data2[i2]["point"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.data =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicetask_ach_c(InputNetworkMessageEvent e)
	{
		Protocol.taskModule.task_ach_c msg = new Protocol.taskModule.task_ach_c();
		msg.type = (int)e.Data["type"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["data"];
			List<Protocol.task_info> list2 = new List<Protocol.task_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.task_info tmp2 = new Protocol.task_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				tmp2.max = (int)data2[i2]["max"];
				tmp2.status = (int)data2[i2]["status"];
				list2.Add(tmp2);
			}
			msg.data =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicetask_award_c(InputNetworkMessageEvent e)
	{
		Protocol.taskModule.task_award_c msg = new Protocol.taskModule.task_award_c();
		msg.code = (int)e.Data["code"];
		msg.id = (int)e.Data["id"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["items"];
			List<Protocol.p_item_info> list2 = new List<Protocol.p_item_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_item_info tmp2 = new Protocol.p_item_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.items =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicetask_list_c(InputNetworkMessageEvent e)
	{
		Protocol.taskModule.task_list_c msg = new Protocol.taskModule.task_list_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["data"];
			List<Protocol.task_info> list2 = new List<Protocol.task_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.task_info tmp2 = new Protocol.task_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				tmp2.max = (int)data2[i2]["max"];
				tmp2.status = (int)data2[i2]["status"];
				list2.Add(tmp2);
			}
			msg.data =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_inlay_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_inlay_c msg = new Protocol.equipModule.equip_inlay_c();
		msg.code = (int)e.Data["code"];
		msg.item_id = (int)e.Data["item_id"];
		msg.diamondid = (int)e.Data["diamondid"];
		msg.diamond_pos = (int)e.Data["diamond_pos"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_enchanting_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_enchanting_c msg = new Protocol.equipModule.equip_enchanting_c();
		msg.code = (int)e.Data["code"];
		msg.item_id = (int)e.Data["item_id"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["attrs"];
			List<Protocol.p_equip_attr> list2 = new List<Protocol.p_equip_attr>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_equip_attr tmp2 = new Protocol.p_equip_attr();
				tmp2.name = data2[i2]["name"].ToString();
				tmp2.value = (int)data2[i2]["value"];
				list2.Add(tmp2);
			}
			msg.attrs =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_syn_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_syn_c msg = new Protocol.equipModule.equip_syn_c();
		msg.code = (int)e.Data["code"];
		msg.id = (int)e.Data["id"];
		msg.new_id = (int)e.Data["new_id"];
		msg.new_num = (int)e.Data["new_num"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_wear_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_wear_c msg = new Protocol.equipModule.equip_wear_c();
		msg.code = (int)e.Data["code"];
		msg.pos = (int)e.Data["pos"];
		msg.id = (int)e.Data["id"];
		msg.item_id = e.Data["item_id"].ToString();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_bag_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_bag_c msg = new Protocol.equipModule.equip_bag_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.p_equip_item> list2 = new List<Protocol.p_equip_item>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_equip_item tmp2 = new Protocol.p_equip_item();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.item_id = (int)data2[i2]["item_id"];
				tmp2.lv = (int)data2[i2]["lv"];
				tmp2.gem = (List<Int32>)data2[i2]["gem"];
				{
					List<Dictionary<string, object>> data4 = (List<Dictionary<string, object>>)data2[i2]["ench"];
					List<Protocol.p_equip_attr> list4 = new List<Protocol.p_equip_attr>();
					for (int i4 = 0; i4 < data4.Count; i4++)
					{
						Protocol.p_equip_attr tmp4 = new Protocol.p_equip_attr();
						tmp4.name = data4[i4]["name"].ToString();
						tmp4.value = (int)data4[i4]["value"];
						list4.Add(tmp4);
					}
					tmp2.ench =  list4;
				}
				tmp2.skills = (List<String>)data2[i2]["skills"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_list_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_list_c msg = new Protocol.equipModule.equip_list_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.p_equip_wear> list2 = new List<Protocol.p_equip_wear>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_equip_wear tmp2 = new Protocol.p_equip_wear();
				tmp2.pos = (int)data2[i2]["pos"];
				tmp2.id = (int)data2[i2]["id"];
				tmp2.item_id = (int)data2[i2]["item_id"];
				tmp2.gem = (List<Int32>)data2[i2]["gem"];
				{
					List<Dictionary<string, object>> data4 = (List<Dictionary<string, object>>)data2[i2]["ench"];
					List<Protocol.p_equip_attr> list4 = new List<Protocol.p_equip_attr>();
					for (int i4 = 0; i4 < data4.Count; i4++)
					{
						Protocol.p_equip_attr tmp4 = new Protocol.p_equip_attr();
						tmp4.name = data4[i4]["name"].ToString();
						tmp4.value = (int)data4[i4]["value"];
						list4.Add(tmp4);
					}
					tmp2.ench =  list4;
				}
				tmp2.lv = (int)data2[i2]["lv"];
				tmp2.skills = (List<String>)data2[i2]["skills"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_strength_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_strength_c msg = new Protocol.equipModule.equip_strength_c();
		msg.code = (int)e.Data["code"];
		msg.lv = (int)e.Data["lv"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_skill_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_skill_c msg = new Protocol.equipModule.equip_skill_c();
		msg.code = (int)e.Data["code"];
		msg.id = (int)e.Data["id"];
		msg.skill_id = e.Data["skill_id"].ToString();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceequip_fuse_c(InputNetworkMessageEvent e)
	{
		Protocol.equipModule.equip_fuse_c msg = new Protocol.equipModule.equip_fuse_c();
		msg.code = (int)e.Data["code"];
		msg.gold = (int)e.Data["gold"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["items"];
			List<Protocol.p_item_info> list2 = new List<Protocol.p_item_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_item_info tmp2 = new Protocol.p_item_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.items =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceshop_buy_c(InputNetworkMessageEvent e)
	{
		Protocol.shopModule.shop_buy_c msg = new Protocol.shopModule.shop_buy_c();
		msg.code = (int)e.Data["code"];
		msg.id = (int)e.Data["id"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceshop_list_c(InputNetworkMessageEvent e)
	{
		Protocol.shopModule.shop_list_c msg = new Protocol.shopModule.shop_list_c();
		msg.type = (int)e.Data["type"];
		msg.list = (List<Int32>)e.Data["list"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefriend_search_c(InputNetworkMessageEvent e)
	{
		Protocol.friendModule.friend_search_c msg = new Protocol.friendModule.friend_search_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.friend> list2 = new List<Protocol.friend>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.friend tmp2 = new Protocol.friend();
				tmp2.role_id = (int)data2[i2]["role_id"];
				tmp2.nick = data2[i2]["nick"].ToString();
				tmp2.head = data2[i2]["head"].ToString();
				tmp2.lv = (int)data2[i2]["lv"];
				tmp2.power = (int)data2[i2]["power"];
				tmp2.time = (int)data2[i2]["time"];
				tmp2.online = (int)data2[i2]["online"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefriend_request_c(InputNetworkMessageEvent e)
	{
		Protocol.friendModule.friend_request_c msg = new Protocol.friendModule.friend_request_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.friend> list2 = new List<Protocol.friend>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.friend tmp2 = new Protocol.friend();
				tmp2.role_id = (int)data2[i2]["role_id"];
				tmp2.nick = data2[i2]["nick"].ToString();
				tmp2.head = data2[i2]["head"].ToString();
				tmp2.lv = (int)data2[i2]["lv"];
				tmp2.power = (int)data2[i2]["power"];
				tmp2.time = (int)data2[i2]["time"];
				tmp2.online = (int)data2[i2]["online"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefriend_new_c(InputNetworkMessageEvent e)
	{
		Protocol.friendModule.friend_new_c msg = new Protocol.friendModule.friend_new_c();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefriend_info_c(InputNetworkMessageEvent e)
	{
		Protocol.friendModule.friend_info_c msg = new Protocol.friendModule.friend_info_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.friend> list2 = new List<Protocol.friend>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.friend tmp2 = new Protocol.friend();
				tmp2.role_id = (int)data2[i2]["role_id"];
				tmp2.nick = data2[i2]["nick"].ToString();
				tmp2.head = data2[i2]["head"].ToString();
				tmp2.lv = (int)data2[i2]["lv"];
				tmp2.power = (int)data2[i2]["power"];
				tmp2.time = (int)data2[i2]["time"];
				tmp2.online = (int)data2[i2]["online"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefriend_op_c(InputNetworkMessageEvent e)
	{
		Protocol.friendModule.friend_op_c msg = new Protocol.friendModule.friend_op_c();
		msg.role_id = (int)e.Data["role_id"];
		msg.type = (int)e.Data["type"];
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicechat_msg_c(InputNetworkMessageEvent e)
	{
		Protocol.chatModule.chat_msg_c msg = new Protocol.chatModule.chat_msg_c();
		msg.code = (int)e.Data["code"];
		msg.from_id = (int)e.Data["from_id"];
		msg.from_name = e.Data["from_name"].ToString();
		msg.channel = (int)e.Data["channel"];
		msg.msg = e.Data["msg"].ToString();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceguide_add_c(InputNetworkMessageEvent e)
	{
		Protocol.guideModule.guide_add_c msg = new Protocol.guideModule.guide_add_c();
		msg.code = (int)e.Data["code"];
		msg.id = (int)e.Data["id"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceguide_list_c(InputNetworkMessageEvent e)
	{
		Protocol.guideModule.guide_list_c msg = new Protocol.guideModule.guide_list_c();
		msg.value = (int)e.Data["value"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicesignin_online_c(InputNetworkMessageEvent e)
	{
		Protocol.signinModule.signin_online_c msg = new Protocol.signinModule.signin_online_c();
		msg.code = (int)e.Data["code"];
		msg.id = (int)e.Data["id"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicesignin_award_c(InputNetworkMessageEvent e)
	{
		Protocol.signinModule.signin_award_c msg = new Protocol.signinModule.signin_award_c();
		msg.code = (int)e.Data["code"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["items"];
			List<Protocol.p_item_info> list2 = new List<Protocol.p_item_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_item_info tmp2 = new Protocol.p_item_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.items =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicesignin_get_c(InputNetworkMessageEvent e)
	{
		Protocol.signinModule.signin_get_c msg = new Protocol.signinModule.signin_get_c();
		msg.time = (int)e.Data["time"];
		msg.id = (int)e.Data["id"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicesignin_add_c(InputNetworkMessageEvent e)
	{
		Protocol.signinModule.signin_add_c msg = new Protocol.signinModule.signin_add_c();
		msg.code = (int)e.Data["code"];
		msg.num = (int)e.Data["num"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["items"];
			List<Protocol.p_item_info> list2 = new List<Protocol.p_item_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_item_info tmp2 = new Protocol.p_item_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.items =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicesignin_info_c(InputNetworkMessageEvent e)
	{
		Protocol.signinModule.signin_info_c msg = new Protocol.signinModule.signin_info_c();
		msg.num = (int)e.Data["num"];
		msg.ok = (bool)e.Data["ok"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicesignin_rep_c(InputNetworkMessageEvent e)
	{
		Protocol.signinModule.signin_rep_c msg = new Protocol.signinModule.signin_rep_c();
		msg.code = (int)e.Data["code"];
		msg.day = (int)e.Data["day"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceliveness_list_c(InputNetworkMessageEvent e)
	{
		Protocol.livenessModule.liveness_list_c msg = new Protocol.livenessModule.liveness_list_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["task"];
			List<Protocol.p_daily> list2 = new List<Protocol.p_daily>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_daily tmp2 = new Protocol.p_daily();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				tmp2.status = (int)data2[i2]["status"];
				list2.Add(tmp2);
			}
			msg.task =  list2;
		}
		msg.box = (List<Int32>)e.Data["box"];
		msg.score = (int)e.Data["score"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceliveness_award_c(InputNetworkMessageEvent e)
	{
		Protocol.livenessModule.liveness_award_c msg = new Protocol.livenessModule.liveness_award_c();
		msg.code = (int)e.Data["code"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["items"];
			List<Protocol.p_item_info> list2 = new List<Protocol.p_item_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_item_info tmp2 = new Protocol.p_item_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.items =  list2;
		}
		msg.pos = (int)e.Data["pos"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicetime_resync_c(InputNetworkMessageEvent e)
	{
		Protocol.timeModule.time_resync_c msg = new Protocol.timeModule.time_resync_c();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicetime_syncreturn_c(InputNetworkMessageEvent e)
	{
		Protocol.timeModule.time_syncreturn_c msg = new Protocol.timeModule.time_syncreturn_c();
		msg.sendtime = (float)(double)e.Data["sendtime"];
		msg.servicetime = (float)(double)e.Data["servicetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_users_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_users_c msg = new Protocol.room2Module.room2_users_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.room_user> list2 = new List<Protocol.room_user>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.room_user tmp2 = new Protocol.room_user();
				tmp2.nick = data2[i2]["nick"].ToString();
				tmp2.head = data2[i2]["head"].ToString();
				tmp2.lv = (int)data2[i2]["lv"];
				tmp2.role_id = (int)data2[i2]["role_id"];
				tmp2.model_id = data2[i2]["model_id"].ToString();
				tmp2.weapon = data2[i2]["weapon"].ToString();
				tmp2.camp = (int)data2[i2]["camp"];
				tmp2.att = (int)data2[i2]["att"];
				tmp2.def = (int)data2[i2]["def"];
				tmp2.hp = (int)data2[i2]["hp"];
				tmp2.hprecover = (int)data2[i2]["hprecover"];
				tmp2.crit = (int)data2[i2]["crit"];
				tmp2.critdamage = (int)data2[i2]["critdamage"];
				tmp2.ignoredef = (int)data2[i2]["ignoredef"];
				tmp2.hpabsorb = (int)data2[i2]["hpabsorb"];
				tmp2.movespeed = (int)data2[i2]["movespeed"];
				tmp2.tough = (int)data2[i2]["tough"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_info_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_info_c msg = new Protocol.room2Module.room2_info_c();
		msg.roominfo = (List<String>)e.Data["roominfo"];
		msg.gameplay = (int)e.Data["gameplay"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_timesync_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_timesync_c msg = new Protocol.room2Module.room2_timesync_c();
		msg.client_time = (float)(double)e.Data["client_time"];
		msg.server_time = (float)(double)e.Data["server_time"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_citydel_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_citydel_c msg = new Protocol.room2Module.room2_citydel_c();
		msg.list = (List<Int32>)e.Data["list"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_changeserver_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_changeserver_c msg = new Protocol.room2Module.room2_changeserver_c();
		msg.server = (bool)e.Data["server"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_citymove_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_citymove_c msg = new Protocol.room2Module.room2_citymove_c();
		msg.role_id = (int)e.Data["role_id"];
		msg.x = (int)e.Data["x"];
		msg.z = (int)e.Data["z"];
		msg.time = (int)e.Data["time"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_cityadd_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_cityadd_c msg = new Protocol.room2Module.room2_cityadd_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.city_player> list2 = new List<Protocol.city_player>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.city_player tmp2 = new Protocol.city_player();
				tmp2.nick = data2[i2]["nick"].ToString();
				tmp2.role_id = (int)data2[i2]["role_id"];
				tmp2.model_id = data2[i2]["model_id"].ToString();
				tmp2.weapon = data2[i2]["weapon"].ToString();
				tmp2.x = (int)data2[i2]["x"];
				tmp2.z = (int)data2[i2]["z"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_level_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_level_c msg = new Protocol.room2Module.room2_level_c();
		msg.op = (int)e.Data["op"];
		msg.val = (int)e.Data["val"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_task_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_task_c msg = new Protocol.room2Module.room2_task_c();
		msg.code = (int)e.Data["code"];
		msg.num = (int)e.Data["num"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_pool_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_pool_c msg = new Protocol.room2Module.room2_pool_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.pool_data> list2 = new List<Protocol.pool_data>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.pool_data tmp2 = new Protocol.pool_data();
				tmp2.type = (int)data2[i2]["type"];
				tmp2.id = data2[i2]["id"].ToString();
				tmp2.start = (int)data2[i2]["start"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		msg.time = (int)e.Data["time"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_exit_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_exit_c msg = new Protocol.room2Module.room2_exit_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_reflashp_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_reflashp_c msg = new Protocol.room2Module.room2_reflashp_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceroom2_join_c(InputNetworkMessageEvent e)
	{
		Protocol.room2Module.room2_join_c msg = new Protocol.room2Module.room2_join_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceaddbuffcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.addbuffcmd_c msg = new Protocol.fightModule.addbuffcmd_c();
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_attackerid = (int)e.Data["m_attackerid"];
		msg.m_buffid = e.Data["m_buffid"].ToString();
		msg.m_skillid = e.Data["m_skillid"].ToString();
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerecovercmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.recovercmd_c msg = new Protocol.fightModule.recovercmd_c();
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_attackerid = (int)e.Data["m_attackerid"];
		msg.m_recovernumber = (int)e.Data["m_recovernumber"];
		msg.m_isautorecover = (bool)e.Data["m_isautorecover"];
		msg.m_skillid = e.Data["m_skillid"].ToString();
		msg.m_buffid = e.Data["m_buffid"].ToString();
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicecreateskilltokencmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.createskilltokencmd_c msg = new Protocol.fightModule.createskilltokencmd_c();
		msg.m_skillid = e.Data["m_skillid"].ToString();
		msg.m_createrid = (int)e.Data["m_createrid"];
		msg.m_camp = (int)e.Data["m_camp"];
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_dirx = (float)(double)e.Data["m_dirx"];
		msg.m_diry = (float)(double)e.Data["m_diry"];
		msg.m_dirz = (float)(double)e.Data["m_dirz"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicedamagecmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.damagecmd_c msg = new Protocol.fightModule.damagecmd_c();
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_attackerid = (int)e.Data["m_attackerid"];
		msg.m_damagenumber = (int)e.Data["m_damagenumber"];
		msg.m_skillid = e.Data["m_skillid"].ToString();
		msg.m_buffid = e.Data["m_buffid"].ToString();
		msg.m_crit = (bool)e.Data["m_crit"];
		msg.m_disrupting = (bool)e.Data["m_disrupting"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicepickupbloodvialcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.pickupbloodvialcmd_c msg = new Protocol.fightModule.pickupbloodvialcmd_c();
		msg.m_bloodvialid = (int)e.Data["m_bloodvialid"];
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicedestroyitemcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.destroyitemcmd_c msg = new Protocol.fightModule.destroyitemcmd_c();
		msg.m_itemid = (int)e.Data["m_itemid"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicepickupitemcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.pickupitemcmd_c msg = new Protocol.fightModule.pickupitemcmd_c();
		msg.m_itemid = (int)e.Data["m_itemid"];
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicecreatebloodvialcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.createbloodvialcmd_c msg = new Protocol.fightModule.createbloodvialcmd_c();
		msg.m_bloodvialid = (int)e.Data["m_bloodvialid"];
		msg.m_camp = (int)e.Data["m_camp"];
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicedestroybloodvialcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.destroybloodvialcmd_c msg = new Protocol.fightModule.destroybloodvialcmd_c();
		msg.m_bloodvialid = (int)e.Data["m_bloodvialid"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_item_list_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_item_list_c msg = new Protocol.fightModule.fight_item_list_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["lists"];
			List<Protocol.p_item_info> list2 = new List<Protocol.p_item_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_item_info tmp2 = new Protocol.p_item_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.lists =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceblowflycmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.blowflycmd_c msg = new Protocol.fightModule.blowflycmd_c();
		msg.m_flyerid = (int)e.Data["m_flyerid"];
		msg.m_attackerid = (int)e.Data["m_attackerid"];
		msg.m_shiftid = e.Data["m_shiftid"].ToString();
		msg.m_attackerposx = (float)(double)e.Data["m_attackerposx"];
		msg.m_attackerposy = (float)(double)e.Data["m_attackerposy"];
		msg.m_attackerposz = (float)(double)e.Data["m_attackerposz"];
		msg.m_hurterposx = (float)(double)e.Data["m_hurterposx"];
		msg.m_hurterposy = (float)(double)e.Data["m_hurterposy"];
		msg.m_hurterposz = (float)(double)e.Data["m_hurterposz"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_relive_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_relive_c msg = new Protocol.fightModule.fight_relive_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_use_item_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_use_item_c msg = new Protocol.fightModule.fight_use_item_c();
		msg.code = (int)e.Data["code"];
		msg.id = (int)e.Data["id"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceremovebuffcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.removebuffcmd_c msg = new Protocol.fightModule.removebuffcmd_c();
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_buffid = e.Data["m_buffid"].ToString();
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceresurgencecmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.resurgencecmd_c msg = new Protocol.fightModule.resurgencecmd_c();
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_creatcomandtime = (int)e.Data["m_creatcomandtime"];
		msg.m_characterid = (int)e.Data["m_characterid"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicediecmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.diecmd_c msg = new Protocol.fightModule.diecmd_c();
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_killerid = (int)e.Data["m_killerid"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicechangeweaponcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.changeweaponcmd_c msg = new Protocol.fightModule.changeweaponcmd_c();
		msg.m_weaponid = e.Data["m_weaponid"].ToString();
		msg.m_creatcomandtime = (int)e.Data["m_creatcomandtime"];
		msg.m_characterid = (int)e.Data["m_characterid"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceattackcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.attackcmd_c msg = new Protocol.fightModule.attackcmd_c();
		msg.m_dirx = (float)(double)e.Data["m_dirx"];
		msg.m_diry = (float)(double)e.Data["m_diry"];
		msg.m_dirz = (float)(double)e.Data["m_dirz"];
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_creatcomandtime = (int)e.Data["m_creatcomandtime"];
		msg.m_characterid = (int)e.Data["m_characterid"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_match_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_match_c msg = new Protocol.fightModule.fight_match_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicecreateflyobjectcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.createflyobjectcmd_c msg = new Protocol.fightModule.createflyobjectcmd_c();
		msg.m_flyname = e.Data["m_flyname"].ToString();
		msg.m_flyid = (int)e.Data["m_flyid"];
		msg.m_skillid = e.Data["m_skillid"].ToString();
		msg.m_createrid = (int)e.Data["m_createrid"];
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_dirx = (float)(double)e.Data["m_dirx"];
		msg.m_diry = (float)(double)e.Data["m_diry"];
		msg.m_dirz = (float)(double)e.Data["m_dirz"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_setelement_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_setelement_c msg = new Protocol.fightModule.fight_setelement_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_loading_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_loading_c msg = new Protocol.fightModule.fight_loading_c();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_cancel_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_cancel_c msg = new Protocol.fightModule.fight_cancel_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicecreateitemcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.createitemcmd_c msg = new Protocol.fightModule.createitemcmd_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.p_map_item> list2 = new List<Protocol.p_map_item>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_map_item tmp2 = new Protocol.p_map_item();
				tmp2.m_itemid = (int)data2[i2]["m_itemid"];
				tmp2.m_itemname = data2[i2]["m_itemname"].ToString();
				tmp2.m_posx = (float)(double)data2[i2]["m_posx"];
				tmp2.m_posy = (float)(double)data2[i2]["m_posy"];
				tmp2.m_posz = (float)(double)data2[i2]["m_posz"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicecreatecharactercmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.createcharactercmd_c msg = new Protocol.fightModule.createcharactercmd_c();
		msg.m_charactertype = (int)e.Data["m_charactertype"];
		msg.m_charactername = e.Data["m_charactername"].ToString();
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_camp = (int)e.Data["m_camp"];
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_dirx = (float)(double)e.Data["m_dirx"];
		msg.m_diry = (float)(double)e.Data["m_diry"];
		msg.m_dirz = (float)(double)e.Data["m_dirz"];
		msg.m_amplification = (float)(double)e.Data["m_amplification"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_rank_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_rank_c msg = new Protocol.fightModule.fight_rank_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["lists"];
			List<Protocol.p_rank> list2 = new List<Protocol.p_rank>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_rank tmp2 = new Protocol.p_rank();
				tmp2.nick = data2[i2]["nick"].ToString();
				tmp2.score = (int)data2[i2]["score"];
				list2.Add(tmp2);
			}
			msg.lists =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicedestroyflyobjectcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.destroyflyobjectcmd_c msg = new Protocol.fightModule.destroyflyobjectcmd_c();
		msg.m_flyid = (int)e.Data["m_flyid"];
		msg.m_isshowhiteffect = (bool)e.Data["m_isshowhiteffect"];
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_item_num_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_item_num_c msg = new Protocol.fightModule.fight_item_num_c();
		msg.id = (int)e.Data["id"];
		msg.num = (int)e.Data["num"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicerotationcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.rotationcmd_c msg = new Protocol.fightModule.rotationcmd_c();
		msg.m_dirx = (float)(double)e.Data["m_dirx"];
		msg.m_diry = (float)(double)e.Data["m_diry"];
		msg.m_dirz = (float)(double)e.Data["m_dirz"];
		msg.m_creatcomandtime = (int)e.Data["m_creatcomandtime"];
		msg.m_characterid = (int)e.Data["m_characterid"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicetraptriggercmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.traptriggercmd_c msg = new Protocol.fightModule.traptriggercmd_c();
		msg.m_trapid = (int)e.Data["m_trapid"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceskillcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.skillcmd_c msg = new Protocol.fightModule.skillcmd_c();
		msg.m_skillid = e.Data["m_skillid"].ToString();
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_skilldirx = (float)(double)e.Data["m_skilldirx"];
		msg.m_skilldiry = (float)(double)e.Data["m_skilldiry"];
		msg.m_skilldirz = (float)(double)e.Data["m_skilldirz"];
		msg.m_creatcomandtime = (int)e.Data["m_creatcomandtime"];
		msg.m_characterid = (int)e.Data["m_characterid"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicemovecmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.movecmd_c msg = new Protocol.fightModule.movecmd_c();
		msg.m_dirx = (int)e.Data["m_dirx"];
		msg.m_dirz = (int)e.Data["m_dirz"];
		msg.m_test = (int)e.Data["m_test"];
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_isonlyturn = (bool)e.Data["m_isonlyturn"];
		msg.m_creatcomandtime = (int)e.Data["m_creatcomandtime"];
		msg.m_characterid = (int)e.Data["m_characterid"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_end_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_end_c msg = new Protocol.fightModule.fight_end_c();
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_element_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_element_c msg = new Protocol.fightModule.fight_element_c();
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["list"];
			List<Protocol.p_item_info> list2 = new List<Protocol.p_item_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_item_info tmp2 = new Protocol.p_item_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.list =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicefight_setrole_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.fight_setrole_c msg = new Protocol.fightModule.fight_setrole_c();
		msg.code = (int)e.Data["code"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicecreatetrapcmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.createtrapcmd_c msg = new Protocol.fightModule.createtrapcmd_c();
		msg.m_trapid = (int)e.Data["m_trapid"];
		msg.m_trapname = e.Data["m_trapname"].ToString();
		msg.m_posx = (float)(double)e.Data["m_posx"];
		msg.m_posy = (float)(double)e.Data["m_posy"];
		msg.m_posz = (float)(double)e.Data["m_posz"];
		msg.m_dirx = (float)(double)e.Data["m_dirx"];
		msg.m_diry = (float)(double)e.Data["m_diry"];
		msg.m_dirz = (float)(double)e.Data["m_dirz"];
		msg.m_camp = (int)e.Data["m_camp"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Receviceremovecharactercmd_c(InputNetworkMessageEvent e)
	{
		Protocol.fightModule.removecharactercmd_c msg = new Protocol.fightModule.removecharactercmd_c();
		msg.m_characterid = (int)e.Data["m_characterid"];
		msg.m_executetime = (float)(double)e.Data["m_executetime"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicechest_open_c(InputNetworkMessageEvent e)
	{
		Protocol.chestModule.chest_open_c msg = new Protocol.chestModule.chest_open_c();
		msg.code = (int)e.Data["code"];
		msg.id = (int)e.Data["id"];
		msg.time = (int)e.Data["time"];
		{
			List<Dictionary<string, object>> data2 = (List<Dictionary<string, object>>)e.Data["items"];
			List<Protocol.p_item_info> list2 = new List<Protocol.p_item_info>();
			for (int i2 = 0; i2 < data2.Count; i2++)
			{
				Protocol.p_item_info tmp2 = new Protocol.p_item_info();
				tmp2.id = (int)data2[i2]["id"];
				tmp2.num = (int)data2[i2]["num"];
				list2.Add(tmp2);
			}
			msg.items =  list2;
		}
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	static void Recevicechest_init_c(InputNetworkMessageEvent e)
	{
		Protocol.chestModule.chest_init_c msg = new Protocol.chestModule.chest_init_c();
		msg.id = (int)e.Data["id"];
		msg.time = (int)e.Data["time"];
		
		GlobalEvent.DispatchTypeEvent(msg);
	}
	#endregion
}
