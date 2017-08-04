using System.Collections.Generic;
namespace Protocol
{
	//Protocol消息文件
	//该文件自动生成，请勿修改，以避免不必要的损失
	#region Module GameSync
	namespace GameSyncModule
	{
		[MessageMode(SendMode.ToClient)] 
		public class playercomponent_c : ComponentBase 
		{
		}
		[MessageMode(SendMode.ToServer)] 
		public class playercomponent_s : ComponentBase 
		{
		}
		[MessageMode(SendMode.ToClient)] 
		public class perfabcomponent_c : ComponentBase 
		{
			public string perfab;
		}
		[MessageMode(SendMode.ToServer)] 
		public class perfabcomponent_s : ComponentBase 
		{
			public string perfab;
		}
	}
	#endregion 

	#region Struct
	#endregion 
}
