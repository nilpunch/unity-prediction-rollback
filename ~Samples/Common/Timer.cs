namespace UPR.Samples
{
    public class Timer : ISimulation, IRollback, IHistory, IRebase
    {
        private readonly RarelyChangingValue<int> _resetTick = new RarelyChangingValue<int>(0);

        private int _elapsedTicks;

        public int CurrentTick => _elapsedTicks - _resetTick.Value;

        public void Reset()
        {
            _resetTick.Value = _elapsedTicks;
        }

        public void StepForward()
        {
            _elapsedTicks += 1;
        }

        public void Rollback(int steps)
        {
            _resetTick.Rollback(steps);
            _elapsedTicks -= steps;
        }

        public void SaveStep()
        {
            _resetTick.SaveStep();
        }

        public void ForgetFromBeginning(int steps)
        {
            _resetTick.ForgetFromBeginning(steps);
        }
    }
}
