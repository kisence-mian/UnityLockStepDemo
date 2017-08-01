using System.Collections.Generic;
namespace Protocol
{
	//Protocol消息文件
	//该文件自动生成，请勿修改，以避免不必要的损失
	[MessageMode(SendMode.ToClient)] 
	public class user_heartbeat_c : IProtocolMessageInterface 
	{
	}
	[MessageMode(SendMode.ToServer)] 
	public class user_heartbeat_s : IProtocolMessageInterface 
	{
	}
	#region Module role
	namespace roleModule
	{
	[Module(12 , "role")]
	public abstract class roleModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class role_money_c : roleModule 
		{
			public int gold;
			public int diamond;
		}
		[MessageMode(SendMode.ToServer)] 
		public class role_money_s : roleModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class role_login_c : roleModule 
		{
			public int code;
			public int create;
		}
		[MessageMode(SendMode.ToServer)] 
		public class role_login_s : roleModule 
		{
			public string account;
		}
		[MessageMode(SendMode.ToClient)] 
		public class role_create_c : roleModule 
		{
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class role_create_s : roleModule 
		{
			public string nick;
			public int sex;
			public string model_id;
			public string head;
		}
		[MessageMode(SendMode.ToClient)] 
		public class role_attr_c : roleModule 
		{
			public int role_id;
			public int gold;
			public int diamond;
			public int lv;
			public int exp;
			public int expneed;
			public int phy;
			public int renown;
			public int power;
			public int att;
			public int def;
			public int hp;
			public int hprecover;
			public int crit;
			public int critdamage;
			public int ignoredef;
			public int hpabsorb;
			public int movespeed;
			public int tough;
			public string model_id;
			public int oid;
			public string nick;
			public string head;
			public int sex;
			public string weapon;
			public string hero;
		}
		[MessageMode(SendMode.ToClient)] 
		public class role_attr_change_c : roleModule 
		{
			public int role_id;
			public List<p_role_attr_change> changes;
		}
		[MessageMode(SendMode.ToClient)] 
		public class role_kick_c : roleModule 
		{
			public int code;
		}
		[MessageMode(SendMode.ToClient)] 
		public class role_auth_c : roleModule 
		{
			public string key;
		}
		[MessageMode(SendMode.ToServer)] 
		public class role_auth_s : roleModule 
		{
		}
	}
	#endregion 

	#region Module bag
	namespace bagModule
	{
		[Module(15 , "bag")]
		public abstract class bagModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class bag_info_c : bagModule 
		{
			public List<goods1> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class bag_info_s : bagModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class bag_use_c : bagModule 
		{
			public int result;
		}
		[MessageMode(SendMode.ToServer)] 
		public class bag_use_s : bagModule 
		{
			public int id;
			public int num;
		}
		[MessageMode(SendMode.ToClient)] 
		public class bag_sell_c : bagModule 
		{
			public int result;
		}
		[MessageMode(SendMode.ToServer)] 
		public class bag_sell_s : bagModule 
		{
			public List<goods1> sellgoods;
		}
		[MessageMode(SendMode.ToClient)] 
		public class bag_changedgoods_c : bagModule 
		{
			public List<goods1> del;
			public List<goods1> changed;
		}
		[MessageMode(SendMode.ToServer)] 
		public class bag_changedgoods_s : bagModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class bag_add_c : bagModule 
		{
			public List<goods1> bag;
		}
		[MessageMode(SendMode.ToClient)] 
		public class bag_diamondnum_c : bagModule 
		{
			public int diamondnum;
		}
		[MessageMode(SendMode.ToServer)] 
		public class bag_diamondnum_s : bagModule 
		{
			public int gold;
			public List<goods1> goods;
		}
	}
	#endregion 

	#region Module mail
	namespace mailModule
	{
		[Module(16 , "mail")]
		public abstract class mailModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class mail_list_c : mailModule 
		{
			public List<p_mail_info> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class mail_list_s : mailModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class mail_read_c : mailModule 
		{
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class mail_read_s : mailModule 
		{
			public int id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class mail_new_c : mailModule 
		{
			public int type;
			public int num;
		}
		[MessageMode(SendMode.ToServer)] 
		public class mail_new_s : mailModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class mail_attach_c : mailModule 
		{
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class mail_attach_s : mailModule 
		{
			public int id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class mail_del_c : mailModule 
		{
			public List<int> ids;
		}
		[MessageMode(SendMode.ToServer)] 
		public class mail_del_s : mailModule 
		{
			public List<int> ids;
		}
	}
	#endregion 

	#region Module task
	namespace taskModule
	{
		[Module(19 , "task")]
		public abstract class taskModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class task_award_c : taskModule 
		{
			public int code;
			public int id;
			public List<p_item_info> items;
		}
		[MessageMode(SendMode.ToServer)] 
		public class task_award_s : taskModule 
		{
			public int id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class task_list_c : taskModule 
		{
			public List<task_info> data;
		}
		[MessageMode(SendMode.ToServer)] 
		public class task_list_s : taskModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class task_cat_c : taskModule 
		{
			public List<p_task_cat> data;
		}
		[MessageMode(SendMode.ToServer)] 
		public class task_cat_s : taskModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class task_ach_c : taskModule 
		{
			public int type;
			public List<task_info> data;
		}
		[MessageMode(SendMode.ToServer)] 
		public class task_ach_s : taskModule 
		{
			public int type;
		}
	}
	#endregion 

	#region Module equip
	namespace equipModule
	{
		[Module(20 , "equip")]
		public abstract class equipModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class equip_list_c : equipModule 
		{
			public List<p_equip_wear> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_list_s : equipModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class equip_bag_c : equipModule 
		{
			public List<p_equip_item> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_bag_s : equipModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class equip_strength_c : equipModule 
		{
			public int code;
			public int lv;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_strength_s : equipModule 
		{
			public int id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class equip_fuse_c : equipModule 
		{
			public int code;
			public int gold;
			public List<p_item_info> items;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_fuse_s : equipModule 
		{
			public List<int> ids;
		}
		[MessageMode(SendMode.ToClient)] 
		public class equip_skill_c : equipModule 
		{
			public int code;
			public int id;
			public string skill_id;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_skill_s : equipModule 
		{
			public int id;
			public string skill_id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class equip_enchanting_c : equipModule 
		{
			public int code;
			public int item_id;
			public List<p_equip_attr> attrs;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_enchanting_s : equipModule 
		{
			public int item_id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class equip_inlay_c : equipModule 
		{
			public int code;
			public int item_id;
			public int diamondid;
			public int diamond_pos;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_inlay_s : equipModule 
		{
			public int item_id;
			public int diamondid;
			public int diamond_pos;
		}
		[MessageMode(SendMode.ToClient)] 
		public class equip_wear_c : equipModule 
		{
			public int code;
			public int pos;
			public int id;
			public string item_id;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_wear_s : equipModule 
		{
			public int pos;
			public int id;
			public string item_id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class equip_syn_c : equipModule 
		{
			public int code;
			public int id;
			public int new_id;
			public int new_num;
		}
		[MessageMode(SendMode.ToServer)] 
		public class equip_syn_s : equipModule 
		{
			public int id;
			public int num;
		}
	}
	#endregion 

	#region Module shop
	namespace shopModule
	{
		[Module(21 , "shop")]
		public abstract class shopModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class shop_list_c : shopModule 
		{
			public int type;
			public List<int> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class shop_list_s : shopModule 
		{
			public int type;
		}
		[MessageMode(SendMode.ToClient)] 
		public class shop_buy_c : shopModule 
		{
			public int code;
			public int id;
		}
		[MessageMode(SendMode.ToServer)] 
		public class shop_buy_s : shopModule 
		{
			public int id;
		}
	}
	#endregion 

	#region Module friend
	namespace friendModule
	{
		[Module(23 , "friend")]
		public abstract class friendModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class friend_info_c : friendModule 
		{
			public List<friend> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class friend_info_s : friendModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class friend_op_c : friendModule 
		{
			public int role_id;
			public int type;
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class friend_op_s : friendModule 
		{
			public int role_id;
			public int type;
		}
		[MessageMode(SendMode.ToClient)] 
		public class friend_new_c : friendModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class friend_search_c : friendModule 
		{
			public List<friend> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class friend_search_s : friendModule 
		{
			public string name;
		}
		[MessageMode(SendMode.ToClient)] 
		public class friend_request_c : friendModule 
		{
			public List<friend> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class friend_request_s : friendModule 
		{
		}
	}
	#endregion 

	#region Module chat
	namespace chatModule
	{
		[Module(24 , "chat")]
		public abstract class chatModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class chat_msg_c : chatModule 
		{
			public int code;
			public int from_id;
			public string from_name;
			public int channel;
			public string msg;
		}
		[MessageMode(SendMode.ToServer)] 
		public class chat_msg_s : chatModule 
		{
			public int to;
			public int channel;
			public string msg;
		}
	}
	#endregion 

	#region Module guide
	namespace guideModule
	{
		[Module(26 , "guide")]
		public abstract class guideModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class guide_list_c : guideModule 
		{
			public int value;
		}
		[MessageMode(SendMode.ToServer)] 
		public class guide_list_s : guideModule 
		{
		}
		[MessageMode(SendMode.ToServer)] 
		public class guide_msg_s : guideModule 
		{
			public int id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class guide_add_c : guideModule 
		{
			public int code;
			public int id;
		}
		[MessageMode(SendMode.ToServer)] 
		public class guide_add_s : guideModule 
		{
			public int id;
		}
	}
	#endregion 

	#region Module daily
	namespace dailyModule
	{
		[Module(28 , "daily")]
		public abstract class dailyModule : IProtocolMessageInterface {}

	}
	#endregion 

	#region Module signin
	namespace signinModule
	{
		[Module(30 , "signin")]
		public abstract class signinModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class signin_info_c : signinModule 
		{
			public int num;
			public bool ok;
		}
		[MessageMode(SendMode.ToServer)] 
		public class signin_info_s : signinModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class signin_add_c : signinModule 
		{
			public int code;
			public int num;
			public List<p_item_info> items;
		}
		[MessageMode(SendMode.ToServer)] 
		public class signin_add_s : signinModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class signin_rep_c : signinModule 
		{
			public int code;
			public int day;
		}
		[MessageMode(SendMode.ToServer)] 
		public class signin_rep_s : signinModule 
		{
			public int day;
		}
		[MessageMode(SendMode.ToClient)] 
		public class signin_award_c : signinModule 
		{
			public int code;
			public List<p_item_info> items;
		}
		[MessageMode(SendMode.ToServer)] 
		public class signin_award_s : signinModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class signin_online_c : signinModule 
		{
			public int code;
			public int id;
		}
		[MessageMode(SendMode.ToServer)] 
		public class signin_online_s : signinModule 
		{
			public int id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class signin_get_c : signinModule 
		{
			public int time;
			public int id;
		}
		[MessageMode(SendMode.ToServer)] 
		public class signin_get_s : signinModule 
		{
		}
	}
	#endregion 

	#region Module liveness
	namespace livenessModule
	{
		[Module(31 , "liveness")]
		public abstract class livenessModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class liveness_award_c : livenessModule 
		{
			public int code;
			public List<p_item_info> items;
			public int pos;
		}
		[MessageMode(SendMode.ToServer)] 
		public class liveness_award_s : livenessModule 
		{
			public int pos;
		}
		[MessageMode(SendMode.ToClient)] 
		public class liveness_list_c : livenessModule 
		{
			public List<p_daily> task;
			public List<int> box;
			public int score;
		}
		[MessageMode(SendMode.ToServer)] 
		public class liveness_list_s : livenessModule 
		{
		}
	}
	#endregion 

	#region Module time
	namespace timeModule
	{
		[Module(40 , "time")]
		public abstract class timeModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class time_syncreturn_c : timeModule 
		{
			public float sendtime;
			public float servicetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class time_sync_s : timeModule 
		{
			public float sendtime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class time_resync_c : timeModule 
		{
		}
		[MessageMode(SendMode.ToServer)] 
		public class time_resync_s : timeModule 
		{
		}
	}
	#endregion 

	#region Module room2
	namespace room2Module
	{
		[Module(42 , "room2")]
		public abstract class room2Module : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class room2_task_c : room2Module 
		{
			public int code;
			public int num;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_task_s : room2Module 
		{
			public string id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_pool_c : room2Module 
		{
			public List<pool_data> list;
			public int time;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_pool_s : room2Module 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_level_c : room2Module 
		{
			public int op;
			public int val;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_level_s : room2Module 
		{
			public int op;
			public int val;
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_reflashp_c : room2Module 
		{
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_reflashp_s : room2Module 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_join_c : room2Module 
		{
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_join_s : room2Module 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_exit_c : room2Module 
		{
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_exit_s : room2Module 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_cityadd_c : room2Module 
		{
			public List<city_player> list;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_cityadd_s : room2Module 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_timesync_c : room2Module 
		{
			public float client_time;
			public float server_time;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_timesync_s : room2Module 
		{
			public float client_time;
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_users_c : room2Module 
		{
			public List<room_user> list;
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_info_c : room2Module 
		{
			public List<string> roominfo;
			public int gameplay;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_info_s : room2Module 
		{
			public List<string> roominfo;
			public int gameplay;
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_changeserver_c : room2Module 
		{
			public bool server;
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_citymove_c : room2Module 
		{
			[Int16]
			public int role_id;
			[Int16]
			public int x;
			[Int16]
			public int z;
			public int time;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_citymove_s : room2Module 
		{
			[Int16]
			public int role_id;
			[Int16]
			public int x;
			[Int16]
			public int z;
			public int time;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_report_s : room2Module 
		{
			public float time;
		}
		[MessageMode(SendMode.ToServer)] 
		public class room2_cityexit_s : room2Module 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class room2_citydel_c : room2Module 
		{
			public List<int> list;
		}
	}
	#endregion 

	#region Module fight
	namespace fightModule
	{
		[Module(43 , "fight")]
		public abstract class fightModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class createitemcmd_c : fightModule 
		{
			public List<p_map_item> list;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class createitemcmd_s : fightModule 
		{
			public int m_itemid;
			public string m_itemname;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_cancel_c : fightModule 
		{
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class fight_cancel_s : fightModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_rank_c : fightModule 
		{
			public List<p_rank> lists;
		}
		[MessageMode(SendMode.ToClient)] 
		public class destroyflyobjectcmd_c : fightModule 
		{
			public int m_flyid;
			public bool m_isshowhiteffect;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class destroyflyobjectcmd_s : fightModule 
		{
			public int m_flyid;
			public bool m_isshowhiteffect;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class createcharactercmd_c : fightModule 
		{
			[Int8]
			public int m_charactertype;
			public string m_charactername;
			public int m_characterid;
			[Int8]
			public int m_camp;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			public float m_amplification;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class createcharactercmd_s : fightModule 
		{
			[Int8]
			public int m_charactertype;
			public string m_charactername;
			public int m_characterid;
			[Int8]
			public int m_camp;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			public float m_amplification;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class createflyobjectcmd_c : fightModule 
		{
			public string m_flyname;
			public int m_flyid;
			public string m_skillid;
			public int m_createrid;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class createflyobjectcmd_s : fightModule 
		{
			public string m_flyname;
			public int m_flyid;
			public string m_skillid;
			public int m_createrid;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_match_c : fightModule 
		{
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class fight_match_s : fightModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_loading_c : fightModule 
		{
		}
		[MessageMode(SendMode.ToServer)] 
		public class fight_loading_s : fightModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_setelement_c : fightModule 
		{
			[Int16]
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class fight_setelement_s : fightModule 
		{
			[Int16]
			public int item1;
			[Int16]
			public int item2;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_item_num_c : fightModule 
		{
			public int id;
			[Int8]
			public int num;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_element_c : fightModule 
		{
			public List<p_item_info> list;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_setrole_c : fightModule 
		{
			[Int16]
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class fight_setrole_s : fightModule 
		{
			public string id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_end_c : fightModule 
		{
		}
		[MessageMode(SendMode.ToServer)] 
		public class fight_end_s : fightModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class removecharactercmd_c : fightModule 
		{
			public int m_characterid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class removecharactercmd_s : fightModule 
		{
			public int m_characterid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class createtrapcmd_c : fightModule 
		{
			public int m_trapid;
			public string m_trapname;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			[Int8]
			public int m_camp;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class createtrapcmd_s : fightModule 
		{
			public int m_trapid;
			public string m_trapname;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			[Int8]
			public int m_camp;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class traptriggercmd_c : fightModule 
		{
			public int m_trapid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class traptriggercmd_s : fightModule 
		{
			public int m_trapid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class rotationcmd_c : fightModule 
		{
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToServer)] 
		public class rotationcmd_s : fightModule 
		{
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToClient)] 
		public class movecmd_c : fightModule 
		{
			[Int16]
			public int m_dirx;
			[Int16]
			public int m_dirz;
			public int m_test;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public bool m_isonlyturn;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToServer)] 
		public class movecmd_s : fightModule 
		{
			[Int16]
			public int m_dirx;
			[Int16]
			public int m_dirz;
			public int m_test;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public bool m_isonlyturn;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToClient)] 
		public class skillcmd_c : fightModule 
		{
			public string m_skillid;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_skilldirx;
			public float m_skilldiry;
			public float m_skilldirz;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToServer)] 
		public class skillcmd_s : fightModule 
		{
			public string m_skillid;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_skilldirx;
			public float m_skilldiry;
			public float m_skilldirz;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToClient)] 
		public class pickupitemcmd_c : fightModule 
		{
			public int m_itemid;
			public int m_characterid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class pickupitemcmd_s : fightModule 
		{
			public int m_itemid;
			public int m_characterid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class destroyitemcmd_c : fightModule 
		{
			public int m_itemid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class destroyitemcmd_s : fightModule 
		{
			public int m_itemid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class destroybloodvialcmd_c : fightModule 
		{
			public int m_bloodvialid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class destroybloodvialcmd_s : fightModule 
		{
			public int m_bloodvialid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class createbloodvialcmd_c : fightModule 
		{
			public int m_bloodvialid;
			[Int8]
			public int m_camp;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class createbloodvialcmd_s : fightModule 
		{
			public int m_bloodvialid;
			[Int8]
			public int m_camp;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class pickupbloodvialcmd_c : fightModule 
		{
			public int m_bloodvialid;
			public int m_characterid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class pickupbloodvialcmd_s : fightModule 
		{
			public int m_bloodvialid;
			public int m_characterid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class recovercmd_c : fightModule 
		{
			public int m_characterid;
			public int m_attackerid;
			public int m_recovernumber;
			public bool m_isautorecover;
			public string m_skillid;
			public string m_buffid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class recovercmd_s : fightModule 
		{
			public int m_characterid;
			public int m_attackerid;
			public int m_recovernumber;
			public bool m_isautorecover;
			public string m_skillid;
			public string m_buffid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class addbuffcmd_c : fightModule 
		{
			public int m_characterid;
			public int m_attackerid;
			public string m_buffid;
			public string m_skillid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class addbuffcmd_s : fightModule 
		{
			public int m_characterid;
			public int m_attackerid;
			public string m_buffid;
			public string m_skillid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class damagecmd_c : fightModule 
		{
			public int m_characterid;
			public int m_attackerid;
			public int m_damagenumber;
			public string m_skillid;
			public string m_buffid;
			public bool m_crit;
			public bool m_disrupting;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class damagecmd_s : fightModule 
		{
			public int m_characterid;
			public int m_attackerid;
			public int m_damagenumber;
			public string m_skillid;
			public string m_buffid;
			public bool m_crit;
			public bool m_disrupting;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class createskilltokencmd_c : fightModule 
		{
			public string m_skillid;
			public int m_createrid;
			[Int8]
			public int m_camp;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class createskilltokencmd_s : fightModule 
		{
			public string m_skillid;
			public int m_createrid;
			[Int8]
			public int m_camp;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class diecmd_c : fightModule 
		{
			public int m_characterid;
			public int m_killerid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class diecmd_s : fightModule 
		{
			public int m_characterid;
			public int m_killerid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class resurgencecmd_c : fightModule 
		{
			public float m_posx;
			public float m_posy;
			public float m_posz;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToServer)] 
		public class resurgencecmd_s : fightModule 
		{
			public float m_posx;
			public float m_posy;
			public float m_posz;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToClient)] 
		public class attackcmd_c : fightModule 
		{
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToServer)] 
		public class attackcmd_s : fightModule 
		{
			public float m_dirx;
			public float m_diry;
			public float m_dirz;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToClient)] 
		public class changeweaponcmd_c : fightModule 
		{
			public string m_weaponid;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToServer)] 
		public class changeweaponcmd_s : fightModule 
		{
			public string m_weaponid;
			[Int16]
			public int m_creatcomandtime;
			public int m_characterid;
		}
		[MessageMode(SendMode.ToClient)] 
		public class removebuffcmd_c : fightModule 
		{
			public int m_characterid;
			public string m_buffid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class removebuffcmd_s : fightModule 
		{
			public int m_characterid;
			public string m_buffid;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class blowflycmd_c : fightModule 
		{
			public int m_flyerid;
			public int m_attackerid;
			public string m_shiftid;
			public float m_attackerposx;
			public float m_attackerposy;
			public float m_attackerposz;
			public float m_hurterposx;
			public float m_hurterposy;
			public float m_hurterposz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToServer)] 
		public class blowflycmd_s : fightModule 
		{
			public int m_flyerid;
			public int m_attackerid;
			public string m_shiftid;
			public float m_attackerposx;
			public float m_attackerposy;
			public float m_attackerposz;
			public float m_hurterposx;
			public float m_hurterposy;
			public float m_hurterposz;
			public float m_executetime;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_item_list_c : fightModule 
		{
			public List<p_item_info> lists;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_use_item_c : fightModule 
		{
			[Int16]
			public int code;
			public int id;
		}
		[MessageMode(SendMode.ToServer)] 
		public class fight_use_item_s : fightModule 
		{
			public int id;
		}
		[MessageMode(SendMode.ToClient)] 
		public class fight_relive_c : fightModule 
		{
			[Int16]
			public int code;
		}
		[MessageMode(SendMode.ToServer)] 
		public class fight_relive_s : fightModule 
		{
		}
	}
	#endregion 

	#region Module chest
	namespace chestModule
	{
		[Module(44 , "chest")]
		public abstract class chestModule : IProtocolMessageInterface {}

		[MessageMode(SendMode.ToClient)] 
		public class chest_init_c : chestModule 
		{
			public int id;
			public int time;
		}
		[MessageMode(SendMode.ToServer)] 
		public class chest_init_s : chestModule 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class chest_open_c : chestModule 
		{
			public int code;
			public int id;
			public int time;
			public List<p_item_info> items;
		}
		[MessageMode(SendMode.ToServer)] 
		public class chest_open_s : chestModule 
		{
			public int id;
		}
	}
	#endregion 

	#region Struct
		public class p_role_attr_change : IProtocolStructInterface 
		{
			public string key;
			public int value;
		}
		public class goods1 : IProtocolStructInterface 
		{
			public int id;
			public int num;
		}
		public class p_mail_info : IProtocolStructInterface 
		{
			public int id;
			public string title;
			public string content;
			public int isread;
			public List<p_mail_item> items;
			public int time;
		}
		public class p_item_info : IProtocolStructInterface 
		{
			public int id;
			public int num;
		}
		public class task_info : IProtocolStructInterface 
		{
			public int id;
			public int num;
			public int max;
			public int status;
		}
		public class p_task_cat : IProtocolStructInterface 
		{
			public int id;
			public int point;
			public int num;
		}
		public class p_equip_wear : IProtocolStructInterface 
		{
			public int pos;
			public int id;
			public int item_id;
			public List<int> gem;
			public List<p_equip_attr> ench;
			public int lv;
			public List<string> skills;
		}
		public class p_equip_item : IProtocolStructInterface 
		{
			public int id;
			public int item_id;
			public int lv;
			public List<int> gem;
			public List<p_equip_attr> ench;
			public List<string> skills;
		}
		public class p_equip_attr : IProtocolStructInterface 
		{
			public string name;
			public int value;
		}
		public class friend : IProtocolStructInterface 
		{
			public int role_id;
			public string nick;
			public string head;
			public int lv;
			public int power;
			public int time;
			public int online;
		}
		public class p_daily : IProtocolStructInterface 
		{
			public int id;
			public int num;
			public int status;
		}
		public class pool_data : IProtocolStructInterface 
		{
			public int type;
			public string id;
			public int start;
		}
		public class city_player : IProtocolStructInterface 
		{
			public string nick;
			[Int16]
			public int role_id;
			public string model_id;
			public string weapon;
			[Int16]
			public int x;
			[Int16]
			public int z;
		}
		public class room_user : IProtocolStructInterface 
		{
			public string nick;
			public string head;
			public int lv;
			public int role_id;
			public string model_id;
			public string weapon;
			public int camp;
			public int att;
			public int def;
			public int hp;
			public int hprecover;
			public int crit;
			public int critdamage;
			public int ignoredef;
			public int hpabsorb;
			public int movespeed;
			public int tough;
		}
		public class p_map_item : IProtocolStructInterface 
		{
			public int m_itemid;
			public string m_itemname;
			public float m_posx;
			public float m_posy;
			public float m_posz;
			[Int8]
			public int num;
		}
		public class p_rank : IProtocolStructInterface 
		{
			public string nick;
			public int score;
		}
		public class p_mail_item : IProtocolStructInterface 
		{
			public int id;
			public int num;
		}
	#endregion 
}
