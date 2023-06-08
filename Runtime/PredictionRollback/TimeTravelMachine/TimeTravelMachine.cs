using System;
using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class TimeTravelMachine
    {
        private readonly IHistory _worldHistory;
        private readonly ISimulation _worldSimulation;
        private readonly IRollback _worldRollback;
        private readonly List<IWorldCommandTimeline> _commandTimelines = new List<IWorldCommandTimeline>();

        private int CurrentTick { get; set; }

        public TimeTravelMachine(IHistory worldHistory, ISimulation worldSimulation, IRollback worldRollback)
        {
            _worldHistory = worldHistory;
            _worldSimulation = worldSimulation;
            _worldRollback = worldRollback;
        }

        public void AddCommandsTimeline(IWorldCommandTimeline worldCommandTimeline)
        {
            _commandTimelines.Add(worldCommandTimeline);
        }

        public void FastForwardToTick(int targetTick)
        {
            if (targetTick < 0)
                throw new ArgumentOutOfRangeException(nameof(targetTick), "Target tick should not be negative!");

            int earliestTick = Math.Min(targetTick, EarliestCommandChange());
            int stepsToRollback = Math.Max(CurrentTick - earliestTick, 0);

            _worldRollback.Rollback(stepsToRollback);
            CurrentTick -= stepsToRollback;

            while (CurrentTick < targetTick)
            {
                foreach (var commandTimeline in _commandTimelines)
                {
                    commandTimeline.ExecuteCommands(CurrentTick);
                }

                _worldSimulation.StepForward();
                _worldHistory.SaveStep();
                CurrentTick += 1;
            }

            foreach (var commandTimeline in _commandTimelines)
            {
                commandTimeline.ApproveChangesUpTo(targetTick);
            }
        }

        private int EarliestCommandChange()
        {
            int earliestCommandChange = int.MaxValue;
            foreach (var commandTimeline in _commandTimelines)
            {
                earliestCommandChange = Math.Min(earliestCommandChange, commandTimeline.EarliestCommandChange);
            }
            return earliestCommandChange;
        }
    }
}
