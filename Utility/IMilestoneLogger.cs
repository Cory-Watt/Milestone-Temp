namespace Milestone.Utility
{
    public interface IMilestoneLogger
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}