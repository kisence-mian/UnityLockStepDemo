using CDatabase.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase
{
    public class DatabaseException : ApplicationException
    {
        private IException instance = null;

        public DatabaseException(DbConfig.DbType type, int code)
        {
            switch(type)
            {
                case DbConfig.DbType.MYSQL:
                    instance = MySqlException.GetInstance(code);
                    break;
            }
        }

        public int GetErrorCode()
        {
            return instance.GetErrorCode();
        }

        public string GetErrorMsg()
        {
            return instance.GetErrorMsg();
        }
    }
}
