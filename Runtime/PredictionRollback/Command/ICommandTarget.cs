namespace UPR.PredictionRollback
{
    public interface ICommandTarget<TCommand>
    {
        void ExecuteCommand(in TCommand command);
    }
}
