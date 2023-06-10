namespace UPR.PredictionRollback
{
    public class Time : ISimulation, IRollback, ITime
    {
        public Time(int currentTick = 0)
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
