using System.Collections.Generic;

namespace UPR.Samples
{
    /// <summary>
    /// Use this to create entities at any time;
    /// </summary>
    public class TimelessFactory<TEntity> : IFactory<TEntity>, ISimulation where TEntity : IEntity, IReusableEntity
    {
        private readonly IEntityWorld<TEntity> _entityWorld;
        private readonly IPool<TEntity> _pool;
        private readonly IIdGenerator _idGenerator;

        private readonly Dictionary<EntityId, TEntity> _presentEntities = new Dictionary<EntityId, TEntity>();

        public TimelessFactory(IEntityWorld<TEntity> entityWorld, IIdGenerator idGenerator, IFactory<TEntity> pool)
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
                entity.ResetHistory();
            }
            else
            {
                entity = _pool.Get();
                entity.ChangeId(entityId);
                _presentEntities.Add(entityId, entity);
            }

            _entityWorld.RegisterEntity(entity);
            return entity;
        }

        public void StepForward()
        {
            RepurposeLostEntities();
        }

        private readonly List<EntityId> _repurposeEntitiesBuffer = new List<EntityId>();

        private void RepurposeLostEntities()
        {
            foreach (var entity in _presentEntities)
            {
                if (_entityWorld.IsLostInHistory(entity.Key))
                {
                    _repurposeEntitiesBuffer.Add(entity.Key);
                }
            }
            foreach (var entityId in _repurposeEntitiesBuffer)
            {
                var entity = _presentEntities[entityId];
                entity.ResetHistory();
                entity.ChangeId(new EntityId(-1));
                _pool.Return(entity);
                _presentEntities.Remove(entityId);
            }
            _repurposeEntitiesBuffer.Clear();
        }
    }
}
