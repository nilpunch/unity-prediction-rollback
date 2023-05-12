namespace UPR
{
    public interface IEntityWorld
    {
        public void RegisterEntity(IEntity entity);
        public void RegisterEntityAtStep(int step, IEntity entity);

        public void SubmitEntities();

        public IEntity FindAliveEntity(EntityId entityId);
        public bool IsExistsInHistory(EntityId entityId);
    }
}
