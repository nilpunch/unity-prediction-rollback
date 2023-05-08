using System;
using System.Collections.Generic;

namespace UPR
{
    public class WorldTimeline
    {
        private readonly IStateHistory _worldStateHistory;
        private readonly ISimulation _worldSimulation;
        private readonly Dictionary<Type, ICommandTimeline> _commandTimelines;

        private long _latestApprovedTick;

        public WorldTimeline(IStateHistory worldStateHistory, ISimulation worldSimulation)
        {
            _worldStateHistory = worldStateHistory;
            _worldSimulation = worldSimulation;
        }

        public void RegisterTimeline<TCommand>(ICommandTimeline<TCommand> commandTimeline)
        {
            _commandTimelines.Add(typeof(TCommand), commandTimeline);
        }

        public void RemoveCommand<TCommand>(long tick, EntityId entityId)
        {
            ICommandTimeline<TCommand> commandTimeline = (ICommandTimeline<TCommand>)_commandTimelines[typeof(TCommand)];

            commandTimeline.RemoveCommand(tick, entityId);

            if (_latestApprovedTick > tick)
            {
                _latestApprovedTick = tick;
            }
        }

        public void InsertCommand<TCommand>(long tick, in TCommand command, EntityId entityId)
        {
            ICommandTimeline<TCommand> commandTimeline = (ICommandTimeline<TCommand>)_commandTimelines[typeof(TCommand)];

            commandTimeline.InsertCommand(tick, command, entityId);

            if (_latestApprovedTick > tick)
            {
                _latestApprovedTick = tick;
            }
        }

        public void Simulate(long currentTick)
        {
            if (currentTick < 0)
                throw new ArgumentOutOfRangeException(nameof(currentTick), "Simulated tick should not be negative!");

            long ticksDelta = currentTick - _latestApprovedTick;
            _worldStateHistory.Rollback((int)ticksDelta);

            for (long tick = _latestApprovedTick; tick <= currentTick; tick++)
            {
                foreach (var commandTimeline in _commandTimelines.Values)
                    commandTimeline.ExecuteCommands(tick);

                _worldSimulation.StepForward(tick);
                _worldStateHistory.SaveStep();
            }

            _latestApprovedTick = currentTick;
        }
    }
}
