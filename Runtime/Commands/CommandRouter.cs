namespace UPR
{
    public class CommandRouter<TCommand> : ICommandRouter<TCommand>
    {
        private readonly IEntityWorld _entityWorld;

        public CommandRouter(IEntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
        }

        public void ForwardCommand(in TCommand command, EntityId entityId)
        {
            var entity = _entityWorld.FindAliveEntity(entityId);

            if (entity is ICommandTarget<TCommand> target)
            {
                target.ExecuteCommand(command);
            }
        }
    }
}
