namespace Dxw.Throttling.Core.Logging
{
    using System;
    using System.IO;
    using System.Xml;
    using Configuration;

    public class FileLog : ILog, IDisposable, IXmlConfigurable
    {
        private string _fileName;

        private LogLevel _logLevel;

        private LogLevel _defaultLogLevel;

        private string _dateTimePattern;

        private StreamWriter _writer;

        private readonly object _lockObj = new object();

        public FileLog() {}

        public FileLog(string fileName, LogLevel logLevel = LogLevel.Warning, LogLevel defaultLogLevel = LogLevel.Warning, string dateTimePattern = "s")
        {
            _fileName = fileName;

            _logLevel = logLevel;
            _defaultLogLevel = defaultLogLevel;
            _dateTimePattern = dateTimePattern;
        }

        public void Log(string msg)
        {
            Log(_defaultLogLevel, msg);
        }

        public void Log(LogLevel logLevel, string msg)
        {
            if (logLevel > _logLevel) return;

            var dt = DateTime.Now.ToString(_dateTimePattern);
            EnsureWriter();
            _writer.WriteLine(dt + ": " + logLevel + ": " + msg);
        }

        private void EnsureWriter()
        {
            if (_writer == null)
            {
                lock (_lockObj)
                {
                    if (_writer == null)
                    {
                        var fileStream = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                        _writer = new StreamWriter(fileStream);
                    }
                }
            }
        }

        #region IConfigurable

        public void Configure(XmlNode node, IConfiguration context)
        {
            var fileAttr = node.Attributes["file"];
            if (fileAttr != null)
                _fileName = fileAttr.Value;

            var logLevelAttr = node.Attributes["logLevel"];
            if (logLevelAttr != null)
            {
                _logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logLevelAttr.Value, true);
            }

            var defaultLogLevelAttr = node.Attributes["defaultLogLevel"];
            if (defaultLogLevelAttr != null)
            {
                _defaultLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), defaultLogLevelAttr.Value, true);
            }

            var dateTimePatternAttr = node.Attributes["dateTimePattern"];
            if (dateTimePatternAttr != null)
                _dateTimePattern = dateTimePatternAttr.Value;
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_writer != null)
                {
                    _writer.Close();
                    _writer = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
