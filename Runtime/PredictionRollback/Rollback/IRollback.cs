namespace UPR
{
    public interface IRollback
    {
        void Rollback(int steps);
    }
}