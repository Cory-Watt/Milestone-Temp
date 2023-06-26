using NLog;

namespace Milestone.Utility
{
    public class MyLogger
    {

        private static MyLogger instance;
        private static Logger logger;

        public static MyLogger GetInstance()
        {
            if (instance == null)
            {
                instance = new MyLogger();
            }
            return instance;
        }

        public Logger GetLogger()
        {
            if (logger == null)
            {
                logger = LogManager.GetLogger("LoginAppRule");
            }
            return logger;
        }

        public void Debug(string message)
        {
            GetLogger().Debug(message);
        }

        public void Error(string message)
        {
            GetLogger().Error(message);
        }

        public void Info(string message)
        {
            GetLogger().Info(message);
        }

        public void Warn(string message)
        {
            GetLogger().Warn(message);
        }
    }
}