namespace UPR
{
    public interface IStateHistory
    {
        int HistoryLength { get; }

        void SaveStep();

        void Rollback(int ticks);
    }
}
