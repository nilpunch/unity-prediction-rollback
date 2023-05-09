namespace UPR
{
    public interface IStateHistory
    {
        int HistoryLength { get; }

        void SaveState();

        void Rollback(int ticks);
    }
}
