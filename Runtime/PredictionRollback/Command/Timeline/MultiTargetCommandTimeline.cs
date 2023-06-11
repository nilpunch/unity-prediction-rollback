using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class MultiTargetCommandTimeline<TCommand> : IMultiTargetCommandTimeline<TCommand>
    {
        private readonly ICommandTimelineFactory<TCommand> _commandTimelineFactory;
        private readonly Dictionary<TargetId, ICommandTimeline<TCommand>> _targetCommandTimelines = new Dictionary<TargetId, ICommandTimeline<TCommand>>();

        public MultiTargetCommandTimeline(ICommandTimelineFactory<TCommand> commandTimelineFactory)
        {
            _commandTimelineFactory = commandTimelineFactory;
        }

        public int EarliestCommandChange { get; private set; }

        public void ApproveChangesUpTo(int tick)
        {
            EarliestCommandChange = tick;
        }

        public void ExecuteCommands(int tick)
        {
            foreach (var commandTimeline in _targetCommandTimelines.Values)
            {
                commandTimeline.ExecuteCommand(tick);
            }
        }

        public void RemoveAllCommandsDownTo(int tick)
        {
            foreach (var commandTimeline in _targetCommandTimelines.Values)
            {
                commandTimeline.RemoveAllCommandsDownTo(tick);
            }

            if (tick + 1 < EarliestCommandChange)
            {
                EarliestCommandChange = tick + 1;
            }
        }

        public void RemoveAllCommandsForEntityDownTo(int tick, TargetId targetId)
        {
            if (_targetCommandTimelines.TryGetValue(targetId, out var commandTimeline))
            {
                commandTimeline.RemoveAllCommandsDownTo(tick);

                if (tick + 1 < EarliestCommandChange)
                {
                    EarliestCommandChange = tick + 1;
                }
            }
        }

        public void RemoveAllCommandsAt(int tick)
        {
            foreach (var commandTimeline in _targetCommandTimelines.Values)
            {
                commandTimeline.RemoveCommand(tick);
            }

            if (tick < EarliestCommandChange)
            {
                EarliestCommandChange = tick;
            }
        }

        public void RemoveCommandForEntityAt(int tick, TargetId targetId)
        {
            if (_targetCommandTimelines.TryGetValue(targetId, out var commandTimeline))
            {
                commandTimeline.RemoveCommand(tick);

                if (tick < EarliestCommandChange)
                {
                    EarliestCommandChange = tick;
                }
            }
        }

        public void InsertCommand(int tick, in TCommand command, TargetId targetId)
        {
            if (!_targetCommandTimelines.TryGetValue(targetId, out var commandTimeline))
            {
                commandTimeline = _commandTimelineFactory.CreateForEntity(targetId);
                _targetCommandTimelines.Add(targetId, commandTimeline);
            }

            commandTimeline.InsertCommand(tick, command);

            if (tick < EarliestCommandChange)
            {
                EarliestCommandChange = tick;
            }
        }
    }
}
