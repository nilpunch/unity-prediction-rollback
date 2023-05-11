using System;
using System.Collections.Generic;

namespace UPR
{
    public class WorldTimeline : IWorldTimeline
    {
        private readonly IHistory _worldHistory;
        private readonly ISimulation _worldSimulation;
        private readonly IRollback _worldRollback;
        private readonly Dictionary<Type, ICommandTimeline> _commandTimelines = new Dictionary<Type, ICommandTimeline>();
        private readonly List<ICommandTimeline> _commandTimelinesInOrder = new List<ICommandTimeline>();

        private int _latestApprovedTick;

        public WorldTimeline(IHistory worldHistory, ISimulation worldSimulation, IRollback worldRollback)
        {
            _worldHistory = worldHistory;
            _worldSimulation = worldSimulation;
            _worldRollback = worldRollback;
        }

        public void RegisterTimeline<TCommand>(ICommandTimeline<TCommand> commandTimeline)
        {
            _commandTimelines.Add(typeof(TCommand), commandTimeline);
            _commandTimelinesInOrder.Add(commandTimeline);
        }

        public void RemoveCommand<TCommand>(int tick, EntityId entityId)
        {
            ICommandTimeline<TCommand> commandTimeline = (ICommandTimeline<TCommand>)_commandTimelines[typeof(TCommand)];

            commandTimeline.RemoveCommand(tick, entityId);

            if (_latestApprovedTick > tick)
            {
                _latestApprovedTick = tick;
            }
        }

        public void InsertCommand<TCommand>(int tick, in TCommand command, EntityId entityId)
        {
            ICommandTimeline<TCommand> commandTimeline = (ICommandTimeline<TCommand>)_commandTimelines[typeof(TCommand)];

            commandTimeline.InsertCommand(tick, command, entityId);

            if (_latestApprovedTick > tick)
            {
                _latestApprovedTick = tick;
            }
        }

        public void FastForwardToTick(int targetTick)
        {
            if (targetTick < 0)
                throw new ArgumentOutOfRangeException(nameof(targetTick), "Target tick should not be negative!");

            int earliestTick = Math.Min(targetTick, _latestApprovedTick);
            int stepsToRollback = _worldHistory.CurrentStep - earliestTick;

            _worldRollback.Rollback(stepsToRollback);

            for (int currentTick = _worldHistory.CurrentStep; currentTick <= targetTick; currentTick++)
            {
                foreach (var commandTimeline in _commandTimelinesInOrder)
                    commandTimeline.ExecuteCommands(currentTick);

                _worldSimulation.StepForward();
                _worldHistory.SaveStep();
            }

            _latestApprovedTick = targetTick;
        }
    }
}
