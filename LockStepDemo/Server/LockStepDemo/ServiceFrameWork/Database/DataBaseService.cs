//using CDatabase;
using CDatabase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataBaseService : ServiceBase
{
    public static IDatabase database;
    protected override void OnInit(IServerConfig config)
    {
        Debug.Log("开始连接数据库~~~");
        long time = ServiceTime.GetServiceTime();

        DbConfig dConfig = new DbConfig();
        dConfig.Server = config.Options.Get("DataBaseURL");
        dConfig.User = config.Options.Get("DataBaseUser");
        dConfig.Password = config.Options.Get("DataBasePassword");
        dConfig.Database = config.Options.Get("DataBaseName");

        database = DatabaseFactory.CreateDatabase(dConfig, DbConfig.DbType.MYSQL);

        try
        {
            database.Open();

            time = ServiceTime.GetServiceTime() - time;

            Debug.Log("数据库连接成功 用时" + time + "ms");
        }
        catch (DatabaseException e)
        {
            Debug.LogError("错误代码：" + e.GetErrorCode() + "，错误信息：" + e.GetErrorMsg());
        }
    }
}
