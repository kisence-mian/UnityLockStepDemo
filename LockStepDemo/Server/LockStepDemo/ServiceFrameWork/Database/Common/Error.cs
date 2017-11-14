using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase.Common
{
    public class Error
    {
        public static class ErrorCode
        {
            public static int UNKNOWN_DATABASE = 1;
            public static int UNKNOWN_SERVER = 2;
            public static int ACCESS_DENIED = 3;
        }

        public static class ErrorMessage
        {
            public static string UNKNOWN_DATABASE = "No Database";
            public static string UNKNOWN_Server = "No Database Server";
            public static string ACCESS_DENIED = "Access Denied";
        }
    }
}
