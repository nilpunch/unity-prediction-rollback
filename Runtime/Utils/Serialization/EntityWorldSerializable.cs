using UPR.PredictionRollback;
using UPR.Serialization;

namespace UPR.Utils
{
    public class EntityWorldSerializable<TEntity> : ISerializable where TEntity : IEntity, ISerializable
    {
        private readonly IEntityWorld<TEntity> _entityWorld;

        public EntityWorldSerializable(IEntityWorld<TEntity> entityWorld)
        {
            _entityWorld = entityWorld;
        }

        public void Serialize(IWriteHandle writeHandle)
        {
            new IntSerializable(_entityWorld.CurrentTick).Serialize(writeHandle);
            new IntSerializable(_entityWorld.Entities.Count).Serialize(writeHandle);
            foreach (TEntity entity in _entityWorld.Entities)
            {
                new IntSerializable(_entityWorld.GetEntityId(entity).Value).Serialize(writeHandle);
                entity.Serialize(writeHandle);
            }
        }
    }
}
