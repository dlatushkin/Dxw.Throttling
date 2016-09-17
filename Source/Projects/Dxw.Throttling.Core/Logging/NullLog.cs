namespace Dxw.Throttling.Core.Logging
{
    using System.Runtime.CompilerServices;

    public class NullLog : ILog
    {
        public readonly static ILog Instance = new NullLog();

        private NullLog() {}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(string msg) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(LogLevel lvl, string msg) { }
    }
}
