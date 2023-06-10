namespace UPR.PredictionRollback
{
    public class EntityWorldCleanup<TEntity> : IMispredictionCleanup where TEntity : ITickCounter
    {
        private readonly IEntityWorld<TEntity> _entityWorld;

        public EntityWorldCleanup(IEntityWorld<TEntity> entityWorld)
        {
            _entityWorld = entityWorld;
        }

        public void Cleanup()
        {
            for (int i = _entityWorld.Entities.Count - 1; i >= 0; i--)
            {
                var entity = _entityWorld.Entities[i];
                if (entity.CurrentTick <= 0)
                {
                    _entityWorld.DeregisterEntity(_entityWorld.GetEntityId(entity));
                }
            }
        }
    }
}
