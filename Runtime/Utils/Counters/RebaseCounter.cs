using UPR.PredictionRollback;

namespace UPR.Utils
{
    public class RebaseCounter : IRebase
    {
        private readonly ITickCounter _worldTickCounter;

        public RebaseCounter(ITickCounter worldTickCounter)
        {
            _worldTickCounter = worldTickCounter;
            HistoryBeginningTick = _worldTickCounter.CurrentTick;
        }

        public int StepsSaved => _worldTickCounter.CurrentTick - HistoryBeginningTick;

        private int HistoryBeginningTick { get; set; }

        public void ForgetFromBeginning(int steps)
        {
            HistoryBeginningTick += steps;
        }
    }
}
