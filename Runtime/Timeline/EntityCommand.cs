namespace UPR
{
    public struct EntityCommand<TCommand>
    {
        public TCommand Command { get; }
        public EntityId Entity { get; }

        public EntityCommand(TCommand command, EntityId entity)
        {
            Command = command;
            Entity = entity;
        }
    }
}
