using NLog;
using IMilestoneLogger = Milestone.Utility.IMilestoneLogger; 

namespace Milestone.Utility
{
    public class NLogLogger : IMilestoneLogger 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }
    }
}