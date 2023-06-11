namespace UPR.PredictionRollback
{
    public class CommandRouter<TCommand> : ICommandRouter<TCommand>
    {
        private readonly ICommandTargetFinder<ICommandTarget<TCommand>> _commandTargetWorld;

        public CommandRouter(ICommandTargetFinder<ICommandTarget<TCommand>> commandTargetWorld)
        {
            _commandTargetWorld = commandTargetWorld;
        }

        public void ForwardCommand(in TCommand command, TargetId targetId)
        {
            if (_commandTargetWorld.IsTargetIdExists(targetId))
            {
                _commandTargetWorld.GetTarget(targetId).ExecuteCommand(command);
            }
        }
    }
}
