using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class RollbackMispredictionCleanup : IRollback
    {
        private readonly IMispredictionCleanup _mispredictionCleanup;

        public RollbackMispredictionCleanup(IMispredictionCleanup mispredictionCleanup)
        {
            _mispredictionCleanup = mispredictionCleanup;
        }

        public void Rollback(int steps)
        {
            _mispredictionCleanup.CleanUp();
        }
    }
}
