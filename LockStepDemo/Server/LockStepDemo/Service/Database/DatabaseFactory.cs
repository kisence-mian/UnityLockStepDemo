using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase
{
    public class DatabaseFactory
    {
        public static IDatabase CreateDatabase(DbConfig config, DbConfig.DbType type)
        {
            IDatabase instance = null;

            switch(type)
            {
                case DbConfig.DbType.MYSQL:
                    if(config.Port == 0)
                    {
                        config.Port = 3306;
                    }
                    instance = MySqlDatabase.GetInstance(config);
                    break;
            }

            return instance;
        }
    }
}
