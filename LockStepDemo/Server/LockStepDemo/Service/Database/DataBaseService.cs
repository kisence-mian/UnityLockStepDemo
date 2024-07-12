//using CDatabase;
using CDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataBaseService
{
    public static IDatabase database;
    public static void Init()
    {
        Debug.Log("开始连接数据库~~~");
        long time = ServiceTime.GetServiceTime();

        DbConfig config = new DbConfig();
        config.Server = "192.168.3.210";
        config.User = "root";
        config.Password = "83dd961d3ce758ce";
        config.Database = "ElementCraft";

        database = DatabaseFactory.CreateDatabase(config, DbConfig.DbType.MYSQL);

        try
        {
            database.Open();

            time = ServiceTime.GetServiceTime() - time;

            Debug.Log("数据库连接成功 用时" + time +"ms");
        }
        catch (DatabaseException e)
        {
            Debug.LogError("错误代码：" + e.GetErrorCode() + "，错误信息：" + e.GetErrorMsg());
        }
    }
}
