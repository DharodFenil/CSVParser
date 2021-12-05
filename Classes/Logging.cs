using log4net;

namespace CSVParse.Classes
{
    public static class Logging
    {
        private static ILog _logger = null;
        private static log4net.ILog Logger
        {
            get
            {
                if(_logger == null)
                {
                    _logger = LogManager.GetLogger(typeof(Logging));
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _logger;
            }
        }

        public static void LogError(string message)
        {
            Logger.Error(message);
        }
    }
}
