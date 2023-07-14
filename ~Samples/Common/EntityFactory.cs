using System.Collections.Generic;
using UPR.Common;
using UPR.PredictionRollback;
using UPR.Useful;

namespace UPR.Samples
{
    /// <summary>
    /// Use this to create entities at any time.
    /// </summary>
    public class EntityFactory<TEntity> : IFactory<TEntity>, IMispredictionCleanup where TEntity : ITickCounter, IReusableEntity
    {
        private readonly IContainer<TEntity> _entities;
        private readonly IPool<TEntity> _pool;

        private readonly List<TEntity> _createdEntities = new List<TEntity>();

        public EntityFactory(IContainer<TEntity> entities, IFactory<TEntity> factory)
        {
            _entities = entities;
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

            TEntity entity = _pool.Get();
            _createdEntities.Add(entity);
            _entities.Entries.Add(entity);
            return entity;
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
