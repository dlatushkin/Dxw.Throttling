namespace Dxw.Throttling.Core.Logging
{
    using System;

    [Flags]
    public enum LogLevel
    {
        Off = 0x00,

        Fatal = 0x01,
        Error = 0x02,
        Warning = 0x03,
        Debug = 0x04,
        Info = 0x05,

        All = 0x10
    }
}
