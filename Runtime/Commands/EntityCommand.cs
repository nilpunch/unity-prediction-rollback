namespace UPR
{
    public struct EntityCommand<TCommand>
    {
        public EntityCommand(TCommand command, EntityId entity)
        {
            Command = command;
            Entity = entity;
        }

        public TCommand Command { get; }
        public EntityId Entity { get; }
    }
}
