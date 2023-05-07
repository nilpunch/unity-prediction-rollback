namespace UPR
{
    public interface ICommandRouter<TCommand>
    {
        void AddTarget(ICommandTarget<TCommand> target, EntityId entityId);

        void ForwardCommand(in TCommand command, EntityId entityId);
    }
}
