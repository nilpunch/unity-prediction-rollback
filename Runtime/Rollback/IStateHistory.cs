namespace UPR
{
    public interface IStateHistory
    {
        void SaveStep();

        void Rollback(int steps);
    }
}
