﻿namespace UPR
{
    public class PredictionCommandTimelineFactory<TCommand> : IEntityCommandTimelineFactory<TCommand>
    {
        private readonly ICommandRouter<TCommand> _commandRouter;

        public PredictionCommandTimelineFactory(ICommandRouter<TCommand> commandRouter)
        {
            _commandRouter = commandRouter;
        }

        public IEntityCommandTimeline<TCommand> CreateForEntity(EntityId entityId)
        {
            return new EntityCommandPrediction<TCommand>(new EntityCommandTimeline<TCommand>(_commandRouter, entityId));
        }
    }
}
