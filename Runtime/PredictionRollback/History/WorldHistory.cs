namespace UPR.PredictionRollback
{
    public class WorldHistory<TEntity> : IHistory where TEntity : ITickCounter, IHistory
    {
        private readonly IEntityWorld<TEntity> _entityWorld;

        public WorldHistory(IEntityWorld<TEntity> entityWorld)
        {
            _entityWorld = entityWorld;
        }

        public void SaveStep()
        {
            foreach (var entity in _entityWorld.Entities)
            {
                entity.SaveStep();
            }
        }
    }
}
