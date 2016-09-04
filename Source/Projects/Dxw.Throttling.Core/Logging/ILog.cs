namespace Dxw.Throttling.Core.Logging
{
    public interface ILog
    {
        void Log(string msg);

        void Log(LogLevel lvl, string msg);
    }
}
