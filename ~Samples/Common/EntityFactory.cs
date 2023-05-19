﻿using System.Collections.Generic;

namespace UPR.Samples
{
    /// <summary>
    /// Use this to create entities at any time;
    /// </summary>
    public class EntityFactory<TEntity> : IFactory<TEntity>, IRollback where TEntity : IEntity, IReusableEntity
    {
        private readonly IEntityWorld<TEntity> _entityWorld;
        private readonly IPool<TEntity> _pool;
        private readonly IIdGenerator _idGenerator;

        private readonly List<TEntity> _createdEntities = new List<TEntity>();

        public EntityFactory(IEntityWorld<TEntity> entityWorld, IIdGenerator idGenerator, IFactory<TEntity> pool)
        {
            _entityWorld = entityWorld;
            _idGenerator = idGenerator;
            _pool = new Pool<TEntity>(pool);
        }

        public TEntity Create()
        {
            var entityId = _idGenerator.Generate();
            TEntity entity = _pool.Get();
            entity.ResetLife();
            _createdEntities.Add(entity);
            _entityWorld.RegisterEntity(entity, entityId);
            return entity;
        }

        public void Rollback(int steps)
        {
            ReturnVolatileEntitiesToPool();
        }

        private void ReturnVolatileEntitiesToPool()
        {
            for (int i = _createdEntities.Count - 1; i >= 0; i--)
            {
                var entity = _createdEntities[i];
                if (entity.Age <= 0)
                {
                    _pool.Return(entity);
                    _createdEntities.RemoveAt(i);
                }
            }
        }
    }
}