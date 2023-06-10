using System;

namespace UPR.PredictionRollback
{
    public class WorldRebase<TEntity> : IRebase where TEntity : ITickCounter, IRebase
    {
        private readonly IEntityCollection<TEntity> _entityCollection;
        private readonly ITickCounter _worldTickCounter;

        public WorldRebase(IEntityCollection<TEntity> entityCollection, ITickCounter worldTickCounter)
        {
            _entityCollection = entityCollection;
            _worldTickCounter = worldTickCounter;
            HistoryBeginningTick = _worldTickCounter.CurrentTick;
        }

        public int StepsSaved => _worldTickCounter.CurrentTick - HistoryBeginningTick;

        private int HistoryBeginningTick { get; set; }

        public void ForgetFromBeginning(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            if (steps > StepsSaved)
                throw new Exception($"Can't forget that far. {nameof(StepsSaved)}: {StepsSaved}, Forgetting: {steps}.");

            HistoryBeginningTick += steps;

            foreach (var entity in _entityCollection.Entities)
            {
                int entityHistoryBegin = _worldTickCounter.CurrentTick - entity.CurrentTick;
                int canForgetSteps = Math.Max(HistoryBeginningTick - entityHistoryBegin, 0);
                entity.ForgetFromBeginning(Math.Min(canForgetSteps, steps));
            }
        }
    }
}
