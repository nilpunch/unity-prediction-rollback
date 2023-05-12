namespace UPR
{
    public interface IEntityWorld
    {
        void RegisterEntity(IEntity entity);
        void RegisterEntityAtStep(int step, IEntity entity);
        void KillEntity(EntityId entityId);

        public void SubmitEntities();

        public IEntity FindAliveEntity(EntityId entityId);
        bool IsAlive(EntityId entityId);
    }
}
