using System;
using UPR.Common;
using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class CollectionRebase<TEntity> : IRebase where TEntity : ITickCounter, IRebase
    {
        private readonly IReadOnlyCollection<TEntity> _collection;
        private readonly ITickCounter _worldTickCounter;

        public CollectionRebase(IReadOnlyCollection<TEntity> collection, ITickCounter worldTickCounter)
        {
            _collection = collection;
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

            foreach (var entity in _collection.Entries)
            {
                int entityHistoryBegin = _worldTickCounter.CurrentTick - entity.CurrentTick;
                int canForgetSteps = Math.Max(HistoryBeginningTick - entityHistoryBegin, 0);
                entity.ForgetFromBeginning(Math.Min(canForgetSteps, steps));
            }
        }
    }
}
