using System;

namespace UPR.PredictionRollback
{
    public class WorldRollback<TEntity> : IRollback where TEntity : ITickCounter, IRollback
    {
        private readonly IEntityCollection<TEntity> _entityCollection;

        public WorldRollback(IEntityCollection<TEntity> entityCollection)
        {
            _entityCollection = entityCollection;
        }

        public void Rollback(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps));

            foreach (var entity in _entityCollection.Entities)
            {
                entity.Rollback(steps);
            }
        }
    }
}
