using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.Protocol
{
    class ProtocolReceiveFilter : IReceiveFilter<ProtocolRequestBase>
    {
        /// <summary>
        /// Gets the size of the left buffer.
        /// </summary>
        /// <value>
        /// The size of the left buffer.
        /// </value>
        int LeftBufferSize { get; }

        /// <summary>
        /// Gets the next receive filter.
        /// </summary>
        IReceiveFilter<ProtocolRequestBase> NextReceiveFilter { get; }

        int IReceiveFilter<ProtocolRequestBase>.LeftBufferSize => throw new NotImplementedException();

        IReceiveFilter<ProtocolRequestBase> IReceiveFilter<ProtocolRequestBase>.NextReceiveFilter => throw new NotImplementedException();

        public FilterState State => throw new NotImplementedException();

        /// <summary>
        /// Resets this instance to initial state.
        /// </summary>
        void Reset()
        {

        }

        ProtocolRequestBase IReceiveFilter<ProtocolRequestBase>.Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            throw new NotImplementedException();
        }

        void IReceiveFilter<ProtocolRequestBase>.Reset()
        {
            throw new NotImplementedException();
        }
    }
}
