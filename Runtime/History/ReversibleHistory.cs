namespace UPR
{
    public class ReversibleHistory : IReversibleHistory
    {
        private readonly IHistory _history;
        private readonly IRollback _rollback;

        public ReversibleHistory(IHistory history, IRollback rollback)
        {
            _history = history;
            _rollback = rollback;
        }

        public int StepsSaved { get; set; }

        public void SubmitStep()
        {
            _history.SubmitStep();
            StepsSaved += 1;
        }

        public void Rollback(int steps)
        {
            _rollback.Rollback(steps);
            StepsSaved -= steps;
        }
    }
}
