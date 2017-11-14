using CDatabase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase.Exception
{
    class MySqlException : IException
    {
        private static MySqlException instance = null;

        private int error = 0;

        private MySqlException(int error)
        {
            this.error = error;
        }

        public static MySqlException GetInstance(int error)
        {
            if (instance == null)
            {
                instance = new MySqlException(error);
            }

            return instance;
        }

        public int GetErrorCode()
        {
            int code = 0;

            switch(error)
            {
                case 0:
                    code = Error.ErrorCode.ACCESS_DENIED;
                    break;
                case 1049:
                    code = Error.ErrorCode.UNKNOWN_DATABASE;
                    break;
                case 1042:
                    code = Error.ErrorCode.UNKNOWN_SERVER;
                    break;
            }

            return code;
        }

        public string GetErrorMsg()
        {
            string msg = "";

            switch (error)
            {
                case 0:
                    msg = Error.ErrorMessage.ACCESS_DENIED;
                    break;
                case 1049:
                    msg = Error.ErrorMessage.UNKNOWN_DATABASE;
                    break;
                case 1042:
                    msg = Error.ErrorMessage.UNKNOWN_Server;
                    break;
            }

            return msg;
        }
    }
}
