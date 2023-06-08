namespace UPR.PredictionRollback
{
    public class CommandTimelineFactory<TCommand> : ICommandTimelineFactory<TCommand>
    {
        private readonly ICommandRouter<TCommand> _commandRouter;

        public CommandTimelineFactory(ICommandRouter<TCommand> commandRouter)
        {
            _commandRouter = commandRouter;
        }

        public ICommandTimeline<TCommand> CreateForEntity(EntityId entityId)
        {
            return new CommandTimeline<TCommand>(_commandRouter, entityId);
        }
    }
}
