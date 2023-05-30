using System;
using System.Collections.Generic;

namespace UPR
{
    public class TimeTravelMachine
    {
        private readonly IHistory _worldHistory;
        private readonly ISimulation _worldSimulation;
        private readonly IRollback _worldRollback;
        private readonly List<ICommandTimeline> _commandTimelines = new List<ICommandTimeline>();

        private int _currentTick;

        public TimeTravelMachine(IHistory worldHistory, ISimulation worldSimulation, IRollback worldRollback)
        {
            _worldHistory = worldHistory;
            _worldSimulation = worldSimulation;
            _worldRollback = worldRollback;
        }

        public void AddCommandsTimeline(ICommandTimeline commandTimeline)
        {
            _commandTimelines.Add(commandTimeline);
        }

        public void FastForwardToTick(int targetTick)
        {
            if (targetTick < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetTick), "Target tick should not be negative!");
            }

            int earliestCommandChange = EarliestCommandChange();
            int earliestTick = Math.Min(targetTick, earliestCommandChange);
            int stepsToRollback = _currentTick - earliestTick;

            _worldRollback.Rollback(stepsToRollback);
            _currentTick -= stepsToRollback;

            while (_currentTick <= targetTick)
            {
                foreach (ICommandTimeline commandTimeline in _commandTimelines)
                {
                    commandTimeline.ExecuteCommands(_currentTick);
                }

                _worldSimulation.StepForward();
                _worldHistory.SaveStep();
                _currentTick += 1;
            }

            foreach (ICommandTimeline commandTimeline in _commandTimelines)
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
