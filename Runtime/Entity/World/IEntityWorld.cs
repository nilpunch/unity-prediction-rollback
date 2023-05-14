namespace UPR
{
    public interface IEntityWorld<TEntity> : IEntityFinder<TEntity> where TEntity : IEntity
    {
        void RegisterEntity(TEntity entity);
        void RegisterEntityAtStep(int step, TEntity entity);
        void KillEntity(EntityId entityId);

        bool IsLostInHistory(EntityId entityId);

        public void SubmitRegistration();
    }
}
