using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.Protocol
{
    class ProtocolRequestBase : IRequestInfo
    {
        private string m_key;

        public string Key
        {
            get => m_key;
            set => m_key = value;
        }

        public Dictionary<string, object> m_data;
    }
}
