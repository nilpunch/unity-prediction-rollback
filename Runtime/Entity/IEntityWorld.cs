namespace UPR
{
    public interface IEntityWorld
    {
        public void RegisterEntity(IEntity entity);
        public void RegisterEntityAtStep(int step, IEntity entity);
        public void KillEntity(EntityId entityId);
        public IEntity FindAliveEntity(EntityId entityId);
    }
}
