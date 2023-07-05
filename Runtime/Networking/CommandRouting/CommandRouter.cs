using UPR.PredictionRollback;

namespace UPR.Networking
{
    public class CommandRouter<TCommand> : ICommandRouter<TCommand>
    {
        private readonly ITargetFinder<ICommandTimeline<TCommand>> _targetFinder;

        public CommandRouter(ITargetFinder<ICommandTimeline<TCommand>> targetFinder)
        {
            _targetFinder = targetFinder;
        }

        public void ForwardCommand(TargetId targetId, TCommand command, int tick)
        {
            if (_targetFinder.IsTargetIdExists(targetId))
            {
                _targetFinder.GetTarget(targetId).InsertCommand(tick, command);
            }
        }
    }
}
