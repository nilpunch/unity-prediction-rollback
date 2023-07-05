namespace UPR.PredictionRollback
{
    public interface IReadOnlyCommandTimeline<TCommand>
    {
        int GetLatestTickWithCommandBefore(int tickInclusive);
        bool HasCommand(int tick);
        bool HasExactCommand(int tick, TCommand command);
        TCommand GetCommand(int tick);
    }
}
