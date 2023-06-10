using UPR.PredictionRollback;
using UPR.Serialization;

namespace UPR.Utils
{
    public class EntityWorldDeserializer<TEntity> : IDeserializer<EntityWorld<TEntity>> where TEntity : ITickCounter
    {
        private readonly IDeserializer<int> _intDeserializer = new IntDeserializer();
        private readonly IDeserializer<TEntity> _entityDeserializer;

        public EntityWorldDeserializer(IDeserializer<TEntity> entityDeserializer)
        {
            _entityDeserializer = entityDeserializer;
        }

        public EntityWorld<TEntity> Deserialize(IReadHandle readHandle)
        {
            var entityWorld = new EntityWorld<TEntity>();

            int entitiesCount = _intDeserializer.Deserialize(readHandle);
            for (int i = 0; i < entitiesCount; i++)
            {
                var entityId = new EntityId(_intDeserializer.Deserialize(readHandle));
                var entity = _entityDeserializer.Deserialize(readHandle);
                entityWorld.RegisterEntity(entity, entityId);
            }

            return entityWorld;
        }
    }
}
