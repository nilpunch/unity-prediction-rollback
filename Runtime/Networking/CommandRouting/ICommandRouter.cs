namespace UPR.Networking
{
    public interface ICommandRouter<TCommand>
    {
        public void ForwardCommand(CommandTimelineId commandTimelineId, TCommand command, int tick);
    }
}
