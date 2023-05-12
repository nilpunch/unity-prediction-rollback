namespace UPR
{
    public class CommandRouter<TCommand> : ICommandRouter<TCommand>
    {
        private readonly IEntityFinder<ICommandTarget<TCommand>> _entityWorld;

        public CommandRouter(IEntityFinder<ICommandTarget<TCommand>> entityWorld)
        {
            _entityWorld = entityWorld;
        }

        public void ForwardCommand(in TCommand command, EntityId entityId)
        {
            if (_entityWorld.IsAlive(entityId))
            {
                _entityWorld.FindAliveEntity(entityId).ExecuteCommand(command);
            }
        }
    }
}
