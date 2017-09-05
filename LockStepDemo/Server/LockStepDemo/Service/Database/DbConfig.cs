using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase
{
    public class DbConfig
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        private string server = "";

        /// <summary>
        /// 服务器端口
        /// </summary>
        private int port = 0;

        /// <summary>
        /// 登录用户名
        /// </summary>
        private string user = "";

        /// <summary>
        /// 登录密码
        /// </summary>
        private string password = "";

        /// <summary>
        /// 打开的数据库名称
        /// </summary>
        private string database = "";

        /// <summary>
        /// 数据库类型
        /// </summary>
        public enum DbType
        {
            UNKNOWN = 0,
            MYSQL = 1,
        }

        public string Server
        {
            get
            {
                return server;
            }

            set
            {
                server = value;
            }
        }

        public int Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }

        public string User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Database
        {
            get
            {
                return database;
            }

            set
            {
                database = value;
            }
        }
    }
}
