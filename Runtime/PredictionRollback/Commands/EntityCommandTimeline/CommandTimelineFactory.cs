namespace UPR
{
    public class CommandTimelineFactory<TCommand> : IEntityCommandTimelineFactory<TCommand>
    {
        private readonly ICommandRouter<TCommand> _commandRouter;

        public CommandTimelineFactory(ICommandRouter<TCommand> commandRouter)
        {
            _commandRouter = commandRouter;
        }

        public IEntityCommandTimeline<TCommand> CreateForEntity(EntityId entityId)
        {
            return new EntityCommandTimeline<TCommand>(_commandRouter, entityId);
        }
    }
}
