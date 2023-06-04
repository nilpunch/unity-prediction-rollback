namespace UPR.Samples
{
    public class Timer : ISimulation, IRollback, IHistory
    {
        private readonly ReversibleValue<int> _resetTick = new ReversibleValue<int>(0);

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
    }
}
