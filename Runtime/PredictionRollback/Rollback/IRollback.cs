namespace UPR.PredictionRollback
{
    public interface IRollback
    {
        void Rollback(int steps);
    }
}
