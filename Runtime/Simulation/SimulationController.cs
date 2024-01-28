using System;

namespace UPR.PredictionRollback
{
    public class SimulationController
    {
        private readonly ISimulation _simulation;

        public SimulationController(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public int CurrentTick { get; private set; }

        public void FastForwardToTick(int targetTick, int rollbackTo = int.MaxValue)
        {
            if (targetTick < 0)
                throw new ArgumentOutOfRangeException(nameof(targetTick), "Target tick should not be negative!");

            int earliestTick = Math.Min(targetTick, rollbackTo);
            int stepsToRollback = Math.Max(CurrentTick - earliestTick, 0);

            _simulation.Rollback(stepsToRollback);
            CurrentTick -= stepsToRollback;

            while (CurrentTick < targetTick)
            {
                _simulation.PlayCommands(CurrentTick);
                _simulation.Update(CurrentTick);
                _simulation.SaveChanges();
                CurrentTick += 1;
            }
        }
    }
}
