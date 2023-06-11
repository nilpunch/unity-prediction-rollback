using UPR.PredictionRollback;
using UPR.Serialization;

namespace UPR.Utils
{
    public class EntityWorldDeserializer<TEntity> : IDeserializer<CommandTargetRegistry<TEntity>> where TEntity : ITickCounter
    {
        private readonly IDeserializer<int> _intDeserializer = new IntDeserializer();
        private readonly IDeserializer<TEntity> _entityDeserializer;

        public EntityWorldDeserializer(IDeserializer<TEntity> entityDeserializer)
        {
            _entityDeserializer = entityDeserializer;
        }

        public CommandTargetRegistry<TEntity> Deserialize(IReadHandle readHandle)
        {
            var entityWorld = new CommandTargetRegistry<TEntity>();

            int entitiesCount = _intDeserializer.Deserialize(readHandle);
            for (int i = 0; i < entitiesCount; i++)
            {
                var entityId = new TargetId(_intDeserializer.Deserialize(readHandle));
                var entity = _entityDeserializer.Deserialize(readHandle);
                entityWorld.Add(entity, entityId);
            }

            return entityWorld;
        }
    }
}
