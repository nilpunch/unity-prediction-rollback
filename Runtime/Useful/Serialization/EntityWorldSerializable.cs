using UPR.Networking;
using UPR.PredictionRollback;
using UPR.Serialization;

namespace UPR.Useful
{
    public class EntityWorldSerializable<TEntity> : ISerializable where TEntity : ITickCounter, ISerializable
    {
        private readonly ITargetRegistry<TEntity> _targetRegistry;

        public EntityWorldSerializable(ITargetRegistry<TEntity> targetRegistry)
        {
            _targetRegistry = targetRegistry;
        }

        public void Serialize(IWriteHandle writeHandle)
        {
            new IntSerializable(_targetRegistry.Entries.Count).Serialize(writeHandle);
            foreach (TEntity entity in _targetRegistry.Entries)
            {
                new IntSerializable(_targetRegistry.GetTargetId(entity).Value).Serialize(writeHandle);
                entity.Serialize(writeHandle);
            }
        }
    }
}
