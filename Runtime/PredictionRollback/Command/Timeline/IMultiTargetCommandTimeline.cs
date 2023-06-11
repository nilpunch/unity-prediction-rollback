namespace UPR.PredictionRollback
{
    public interface IMultiTargetCommandTimeline
    {
        int EarliestCommandChange { get; }

        void ApproveChangesUpTo(int tick);

        void ExecuteCommands(int tick);
    }

    public interface IMultiTargetCommandTimeline<TCommand> : IMultiTargetCommandTimeline
    {
        void RemoveAllCommandsDownTo(int tick);
        void RemoveAllCommandsForEntityDownTo(int tick, TargetId targetId);

        void RemoveAllCommandsAt(int tick);
        void RemoveCommandForEntityAt(int tick, TargetId targetId);

        void InsertCommand(int tick, in TCommand command, TargetId targetId);
    }
}
