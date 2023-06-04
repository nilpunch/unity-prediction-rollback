namespace UPR
{
    public interface ICommandTimeline<TCommand>
    {
        int GetLatestTickWithCommand(int tick);
        void ExecuteCommand(int tick);
        void RemoveAllCommandsDownTo(int tick);
        void RemoveCommand(int tick);
        void InsertCommand(int tick, in TCommand command);
    }
}
