namespace UPR.Networking
{
    public interface ICommandTimelineRegistry<in TTarget>
    {
        void Add(TTarget target, CommandTimelineId commandTimelineId);
        void Remove(CommandTimelineId commandTimelineId);
        void Remove(int entryIndex);
        CommandTimelineId GetTargetId(TTarget target);
    }
}
