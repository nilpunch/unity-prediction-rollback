namespace UPR
{
    public class CommandRouter<TCommand> : ICommandRouter<TCommand>
    {
        public void AddTarget(ICommandTarget<TCommand> target, EntityId entityId)
        {
            throw new System.NotImplementedException();
        }

        public void ForwardCommand(in TCommand command, EntityId entityId)
        {
            throw new System.NotImplementedException();
        }
    }
}
