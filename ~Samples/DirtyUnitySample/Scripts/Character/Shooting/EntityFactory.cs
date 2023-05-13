using System.Collections.Generic;
using Tools;

namespace UPR.Samples
{
    public class EntityFactory<TEntity> : IFactory<TEntity>, ISimulation where TEntity : IEntity, ICachedEntity
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
            if (!_presentEntities.TryGetValue(entityId, out TEntity entity))
            {
                entity = _pool.Get();
                _presentEntities.Add(entityId, entity);
            }
            entity.ResetHistory();
            entity.ChangeId(entityId);
            _entityWorld.RegisterEntity(entity);
            return entity;
        }

        public void StepForward()
        {
            RepurposeNonExistEntities();
        }

        private readonly List<EntityId> _repurposeEntitiesBuffer = new List<EntityId>();

        private void RepurposeNonExistEntities()
        {
            foreach (var entity in _presentEntities)
            {
                if (!_entityWorld.IsExistsInHistory(entity.Key))
                {
                    _repurposeEntitiesBuffer.Add(entity.Key);
                }
            }
            foreach (var entityId in _repurposeEntitiesBuffer)
            {
                var entity = _presentEntities[entityId];
                entity.ResetHistory();
                _pool.Return(_presentEntities[entityId]);
                _presentEntities.Remove(entityId);
            }
            _repurposeEntitiesBuffer.Clear();
        }
    }
}
