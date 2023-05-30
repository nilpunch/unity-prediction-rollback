namespace UPR
{
    public interface ICommandRouter<TCommand>
    {
        void ForwardCommand(in TCommand command, EntityId entityId);
    }
}
