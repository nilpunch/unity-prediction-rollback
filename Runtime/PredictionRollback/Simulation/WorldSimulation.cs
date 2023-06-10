namespace UPR.PredictionRollback
{
    public class WorldSimulation<TEntity> : ISimulation where TEntity : ITickCounter, ISimulation
    {
        private readonly IEntityCollection<TEntity> _entityCollection;

        public WorldSimulation(IEntityCollection<TEntity> entityCollection)
        {
            _entityCollection = entityCollection;
        }

        public void StepForward()
        {
            for (int i = 0; i < _entityCollection.Entities.Count; i++)
            {
                _entityCollection.Entities[i].StepForward();
            }
        }
    }
}
