namespace UPR
{
    public struct EntityWorldRegisterEntity
    {
        public EntityWorldRegisterEntity(IEntity entity)
        {
            Entity = entity;
        }

        public IEntity Entity { get; }
    }
}
