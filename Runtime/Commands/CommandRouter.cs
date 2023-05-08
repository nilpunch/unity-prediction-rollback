namespace UPR
{
    public class CommandRouter<TCommand> : ICommandRouter<TCommand>
    {
        private readonly ITargetsCollection<TCommand> _targetsCollection;

        public CommandRouter(ITargetsCollection<TCommand> targetsCollection)
        {
            _targetsCollection = targetsCollection;
        }

        public void ForwardCommand(in TCommand command, EntityId entityId)
        {
            _targetsCollection.FindTarget(entityId).ExecuteCommand(command);
        }
    }
}
