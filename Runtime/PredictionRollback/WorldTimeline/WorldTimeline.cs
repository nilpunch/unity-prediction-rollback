using System;

namespace UPR.PredictionRollback
{
    public class WorldTimeline : IWorldTimeline
    {
        private readonly IHistory _worldHistory;
        private readonly ISimulation _worldSimulation;
        private readonly IRollback _worldRollback;
        private readonly ICommandPlayer _worldCommandPlayer;

        private int CurrentTick { get; set; }
        private int EarliestApprovedTick { get; set; }

        public WorldTimeline(IHistory worldHistory, ISimulation worldSimulation, IRollback worldRollback, ICommandPlayer worldCommandPlayer)
        {
            _worldHistory = worldHistory;
            _worldSimulation = worldSimulation;
            _worldRollback = worldRollback;
            _worldCommandPlayer = worldCommandPlayer;
        }

        public void UpdateEarliestApprovedTick(int tick)
        {
            if (EarliestApprovedTick > tick)
            {
                EarliestApprovedTick = tick;
            }
        }

        public void FastForwardToTick(int targetTick)
        {
            if (targetTick < 0)
                throw new ArgumentOutOfRangeException(nameof(targetTick), "Target tick should not be negative!");

            int earliestTick = Math.Min(targetTick, EarliestApprovedTick);
            int stepsToRollback = Math.Max(CurrentTick - earliestTick, 0);

            _worldRollback.Rollback(stepsToRollback);
            CurrentTick -= stepsToRollback;

            while (CurrentTick < targetTick)
            {
                _worldCommandPlayer.PlayCommands(CurrentTick);
                _worldSimulation.StepForward();
                _worldHistory.SaveStep();
                CurrentTick += 1;
            }

            EarliestApprovedTick = CurrentTick;
        }
    }
}
