using UPR.PredictionRollback;
using UPR.Serialization;

namespace UPR.Utils
{
    public class EntityWorldSerializable<TEntity> : ISerializable where TEntity : ITickCounter, ISerializable
    {
        private readonly ICommandTargetRegistry<TEntity> _commandTargetRegistry;

        public EntityWorldSerializable(ICommandTargetRegistry<TEntity> commandTargetRegistry)
        {
            _commandTargetRegistry = commandTargetRegistry;
        }

        public void Serialize(IWriteHandle writeHandle)
        {
            new IntSerializable(_commandTargetRegistry.Entries.Count).Serialize(writeHandle);
            foreach (TEntity entity in _commandTargetRegistry.Entries)
            {
                new IntSerializable(_commandTargetRegistry.GetTargetId(entity).Value).Serialize(writeHandle);
                entity.Serialize(writeHandle);
            }
        }
    }
}
