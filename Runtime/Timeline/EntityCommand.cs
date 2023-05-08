namespace UPR
{
    public struct EntityCommand<TCommand>
    {
        public TCommand Command { get; }
        public EntityId Target { get; }

        public EntityCommand(TCommand command, EntityId target)
        {
            Command = command;
            Target = target;
        }
    }
}
