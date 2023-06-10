using System;

namespace UPR.PredictionRollback
{
    public class WorldRebase<TEntity> : IRebase where TEntity : IEntity, IRebase
    {
        private readonly IEntityCollection<TEntity> _entityCollection;
        private readonly ITime _worldTime;

        public WorldRebase(IEntityCollection<TEntity> entityCollection, ITime worldTime)
        {
            _entityCollection = entityCollection;
            _worldTime = worldTime;
            HistoryBeginningTick = _worldTime.CurrentTick;
        }

        public int StepsSaved => _worldTime.CurrentTick - HistoryBeginningTick;

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
                int entityHistoryBegin = _worldTime.CurrentTick - entity.SavedSteps;
                int canForgetSteps = Math.Max(HistoryBeginningTick - entityHistoryBegin, 0);
                entity.ForgetFromBeginning(Math.Min(canForgetSteps, steps));
            }
        }
    }
}
