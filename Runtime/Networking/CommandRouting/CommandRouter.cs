namespace UPR.Networking
{
    public class CommandRouter<TCommand> : ICommandRouter<TCommand>
    {
        private readonly ITargetFinder<ICommandTarget<TCommand>> _targetFinder;

        public CommandRouter(ITargetFinder<ICommandTarget<TCommand>> targetFinder)
        {
            _targetFinder = targetFinder;
        }

        public void ForwardCommand(TCommand command, TargetId targetId, int tick)
        {
            if (_targetFinder.IsTargetIdExists(targetId))
            {
                _targetFinder.GetTarget(targetId).CommandTimeline.InsertCommand(tick, command);
            }
        }
    }
}
