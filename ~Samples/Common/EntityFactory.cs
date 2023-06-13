using System.Collections.Generic;
using UPR.Networking;
using UPR.PredictionRollback;
using UPR.Utils;

namespace UPR.Samples
{
    /// <summary>
    /// Use this to create entities at any time.
    /// </summary>
    public class EntityFactory<TEntity> : IFactory<TEntity>, IMispredictionCleanup where TEntity : ITickCounter, IReusableEntity
    {
        private readonly ITargetRegistry<TEntity> _targetRegistry;
        private readonly IIdGenerator _idGenerator;
        private readonly IPool<TEntity> _pool;

        private readonly List<TEntity> _createdEntities = new List<TEntity>();

        public EntityFactory(ITargetRegistry<TEntity> targetRegistry, IIdGenerator idGenerator, IFactory<TEntity> factory)
        {
            _targetRegistry = targetRegistry;
            _idGenerator = idGenerator;
            _pool = new Pool<TEntity>(factory);
        }

        public TEntity Create()
        {
            foreach (var createdEntity in _createdEntities)
            {
                if (createdEntity.CanBeReused)
                {
                    return createdEntity;
                }
            }

            var entityId = _idGenerator.Generate();
            TEntity entity = _pool.Get();
            _createdEntities.Add(entity);
            _targetRegistry.Add(entity, entityId);
            return entity;
        }

        public void Rollback(int steps)
        {
            ReturnNonExistedEntitiesToPool();
        }

        public void CleanUp()
        {
            ReturnNonExistedEntitiesToPool();
        }

        private void ReturnNonExistedEntitiesToPool()
        {
            for (int i = _createdEntities.Count - 1; i >= 0; i--)
            {
                var entity = _createdEntities[i];
                if (entity.CurrentTick <= 0)
                {
                    entity.FullyResetEntity();
                    _pool.Return(entity);
                    _createdEntities.RemoveAt(i);
                }
            }
        }
    }
}
