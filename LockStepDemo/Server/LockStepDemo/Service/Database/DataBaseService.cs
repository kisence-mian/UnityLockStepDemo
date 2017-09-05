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
        DbConfig config = new DbConfig();
        config.Database = "ElementCraft";
        config.Server = "54.191.174.49";
        config.User = "root";
        config.Password = "83dd961d3ce758ce";

        database = DatabaseFactory.CreateDatabase(config, DbConfig.DbType.MYSQL);

        try
        {
            database.Open();
        }
        catch (DatabaseException e)
        {
            Debug.LogError("错误代码：" + e.GetErrorCode() + "，错误信息：" + e.GetErrorMsg());
        }
    }


}
