namespace UPR.Samples
{
    public class EntityFinderAdapter<TFrom, TTo> : IEntityFinder<TTo> where TTo : TFrom
    {
        private readonly IEntityFinder<TFrom> _entityFinder;

        public EntityFinderAdapter(IEntityFinder<TFrom> entityFinder)
        {
            _entityFinder = entityFinder;
        }

        public TTo FindAliveEntity(EntityId entityId)
        {
            return (TTo)_entityFinder.FindAliveEntity(entityId);
        }

        public bool IsAlive(EntityId entityId)
        {
            return _entityFinder.IsAlive(entityId);
        }
    }
}
