namespace UPR.PredictionRollback
{
    public interface ICommandRouter<TCommand>
    {
        void ForwardCommand(in TCommand command, TargetId targetId);
    }
}
