using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UPR.Samples
{
    /// <summary>
    /// Use this to create entities at any time.
    /// </summary>
    public class EntityFactory<TEntity> : IFactory<TEntity>, IRollback where TEntity : IEntity, IReusableEntity
    {
        private readonly IEntityWorld<TEntity> _entityWorld;
        private readonly IIdGenerator _idGenerator;
        private readonly IPool<TEntity> _pool;

        private readonly List<TEntity> _createdEntities = new List<TEntity>();

        public EntityFactory(IEntityWorld<TEntity> entityWorld, IIdGenerator idGenerator, IFactory<TEntity> factory)
        {
            _entityWorld = entityWorld;
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
            _entityWorld.RegisterEntity(entity, entityId);
            return entity;
        }

        public void Rollback(int steps)
        {
            ReturnMissingEntitiesToPool();
        }

        private void ReturnMissingEntitiesToPool()
        {
            for (int i = _createdEntities.Count - 1; i >= 0; i--)
            {
                var entity = _createdEntities[i];
                if (!_entityWorld.IsEntityExists(entity))
                {
                    entity.FullyResetEntity();
                    _pool.Return(entity);
                    _createdEntities.RemoveAt(i);
                }
            }
        }
    }
}
