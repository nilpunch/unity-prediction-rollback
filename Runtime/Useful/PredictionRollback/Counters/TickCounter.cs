using UPR.PredictionRollback;

namespace UPR.Useful
{
    public class TickCounter : ISimulation, IRollback, ITickCounter
    {
        public TickCounter(int currentTick = 0)
        {
            CurrentTick = currentTick;
        }

        public int CurrentTick { get; private set; }

        public void StepForward()
        {
            CurrentTick += 1;
        }

        public void Rollback(int steps)
        {
            CurrentTick -= steps;
        }
    }
}
