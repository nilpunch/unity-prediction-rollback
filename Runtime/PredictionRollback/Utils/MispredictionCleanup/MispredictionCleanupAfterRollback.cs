namespace UPR.PredictionRollback
{
    public class MispredictionCleanupAfterRollback : IRollback
    {
        private readonly IMispredictionCleanup _mispredictionCleanup;

        public MispredictionCleanupAfterRollback(IMispredictionCleanup mispredictionCleanup)
        {
            _mispredictionCleanup = mispredictionCleanup;
        }

        public void Rollback(int steps)
        {
            _mispredictionCleanup.Cleanup();
        }
    }
}
