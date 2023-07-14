using UPR.PredictionRollback;

namespace UPR.Networking
{
    public class CommandRouter<TCommand> : ICommandRouter<TCommand>
    {
        private readonly ICommandTimelineFinder<ICommandTimeline<TCommand>> _commandTimelineFinder;

        public CommandRouter(ICommandTimelineFinder<ICommandTimeline<TCommand>> commandTimelineFinder)
        {
            _commandTimelineFinder = commandTimelineFinder;
        }

        public void ForwardCommand(CommandTimelineId commandTimelineId, TCommand command, int tick)
        {
            if (_commandTimelineFinder.IsCommandTimelineExists(commandTimelineId))
            {
                _commandTimelineFinder.GetCommandTimeline(commandTimelineId).InsertCommand(tick, command);
            }
        }
    }
}
