using System;
using System.Collections.Generic;
using System.Linq;

namespace UPR
{
    public class TimeTravelMachine
    {
        private readonly IHistory _worldHistory;
        private readonly ISimulation _worldSimulation;
        private readonly IRollback _worldRollback;
        private readonly List<ICommandTimeline> _commandTimelines = new List<ICommandTimeline>();

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
                throw new ArgumentOutOfRangeException(nameof(targetTick), "Target tick should not be negative!");

            int earliestCommandChange = EarliestCommandChange();
            int earliestTick = Math.Min(targetTick, earliestCommandChange);
            int stepsToRollback = _worldHistory.CurrentStep - earliestTick;

            _worldRollback.Rollback(stepsToRollback);

            for (int currentTick = _worldHistory.CurrentStep; currentTick <= targetTick; currentTick++)
            {
                foreach (ICommandTimeline commandTimeline in _commandTimelines)
                    commandTimeline.ExecuteCommands(currentTick);

                _worldSimulation.StepForward();
                _worldHistory.SaveStep();
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
