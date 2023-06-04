namespace UPR
{
    public class PredictionCommandTimelineFactory<TCommand> : ICommandTimelineFactory<TCommand>
    {
        private readonly ICommandRouter<TCommand> _commandRouter;

        public PredictionCommandTimelineFactory(ICommandRouter<TCommand> commandRouter)
        {
            _commandRouter = commandRouter;
        }

        public ICommandTimeline<TCommand> CreateForEntity(EntityId entityId)
        {
            return new PredictionCommandTimeline<TCommand>(new CommandTimeline<TCommand>(_commandRouter, entityId));
        }
    }
}
