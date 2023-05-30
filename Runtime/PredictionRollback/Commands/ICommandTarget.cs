namespace UPR
{
    public interface ICommandTarget<TCommand>
    {
        void ExecuteCommand(in TCommand command);
    }
}
