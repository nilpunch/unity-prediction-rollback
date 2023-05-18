using System.Collections.Generic;

namespace UPR.Samples
{
    /// <summary>
    /// Use this to create entities at any time;
    /// </summary>
    public class EntityFactory<TEntity> : IFactory<TEntity>, IRollback where TEntity : UnityEntity
    {
        private readonly IEntityWorld<TEntity> _entityWorld;
        private readonly IPool<TEntity> _pool;
        private readonly IIdGenerator _idGenerator;

        private readonly Dictionary<EntityId, TEntity> _presentEntities = new Dictionary<EntityId, TEntity>();

        public EntityFactory(IEntityWorld<TEntity> entityWorld, IIdGenerator idGenerator, IFactory<TEntity> pool)
        {
            _entityWorld = entityWorld;
            _idGenerator = idGenerator;
            _pool = new Pool<TEntity>(pool);
        }

        public TEntity Create()
        {
            var entityId = _idGenerator.Generate();

            TEntity entity;
            if (_presentEntities.ContainsKey(entityId))
            {
                entity = _presentEntities[entityId];
            }
            else
            {
                entity = _pool.Get();
                _presentEntities.Add(entityId, entity);
            }

            entity.ResetEntity();
            entity.Id = entityId;

            _entityWorld.RegisterEntity(entity);
            return entity;
        }

        public void Rollback(int steps)
        {
            RepurposeVolatileEntities();
        }

        private readonly List<EntityId> _repurposeEntitiesBuffer = new List<EntityId>();

        private void RepurposeVolatileEntities()
        {
            foreach (var entity in _presentEntities)
            {
                if (entity.Value.IsVolatile)
                {
                    _repurposeEntitiesBuffer.Add(entity.Key);
                }
            }
            foreach (var entityId in _repurposeEntitiesBuffer)
            {
                _pool.Return(_presentEntities[entityId]);
                _presentEntities.Remove(entityId);
            }
            _repurposeEntitiesBuffer.Clear();
        }
    }
}
