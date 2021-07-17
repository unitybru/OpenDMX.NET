using OpenDMX.NET.FTDI;
using System;

namespace OpenDMX.NET
{
    public class OpenDMXException : Exception
    {
        public Status Status { get; private set; }

        public OpenDMXException(string message, Status status) : base(message)
        {
            Status = status;
        }
    }
}
