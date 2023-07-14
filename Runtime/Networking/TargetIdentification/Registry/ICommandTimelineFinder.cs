namespace UPR.Networking
{
    public interface ICommandTimelineFinder<out TTarget>
    {
        TTarget GetCommandTimeline(CommandTimelineId commandTimelineId);
        bool IsCommandTimelineExists(CommandTimelineId commandTimelineId);
    }
}
