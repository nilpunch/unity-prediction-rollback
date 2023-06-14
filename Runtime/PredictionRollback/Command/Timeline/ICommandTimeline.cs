namespace UPR.PredictionRollback
{
    public interface ICommandTimeline<TCommand>
    {
        int GetLatestTickWithCommandBefore(int tickInclusive);
        bool HasCommand(int tick);
        TCommand GetCommand(int tick);

        void RemoveAllCommandsDownTo(int tick);
        void RemoveCommand(int tick);
        void InsertCommand(int tick, in TCommand command);
    }
}
