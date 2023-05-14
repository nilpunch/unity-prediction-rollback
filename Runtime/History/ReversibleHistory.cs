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

        public int CurrentStep => _history.CurrentStep;

        public void SaveStep()
        {
            _history.SaveStep();
        }

        public void Rollback(int steps)
        {
            _rollback.Rollback(steps);
        }
    }
}
