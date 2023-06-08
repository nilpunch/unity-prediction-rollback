using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class WorldCommandTimeline<TCommand> : IWorldCommandTimeline<TCommand>
    {
        private readonly ICommandTimelineFactory<TCommand> _commandTimelineFactory;
        private readonly Dictionary<EntityId, ICommandTimeline<TCommand>> _entityCommandTimelines = new Dictionary<EntityId, ICommandTimeline<TCommand>>();

        public WorldCommandTimeline(ICommandTimelineFactory<TCommand> commandTimelineFactory)
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
            foreach (var commandTimeline in _entityCommandTimelines.Values)
            {
                commandTimeline.ExecuteCommand(tick);
            }
        }

        public void RemoveAllCommandsDownTo(int tick)
        {
            foreach (var commandTimeline in _entityCommandTimelines.Values)
            {
                commandTimeline.RemoveAllCommandsDownTo(tick);
            }

            if (tick + 1 < EarliestCommandChange)
            {
                EarliestCommandChange = tick + 1;
            }
        }

        public void RemoveAllCommandsForEntityDownTo(int tick, EntityId entityId)
        {
            if (_entityCommandTimelines.TryGetValue(entityId, out var commandTimeline))
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
            foreach (var commandTimeline in _entityCommandTimelines.Values)
            {
                commandTimeline.RemoveCommand(tick);
            }

            if (tick < EarliestCommandChange)
            {
                EarliestCommandChange = tick;
            }
        }

        public void RemoveCommandForEntityAt(int tick, EntityId entityId)
        {
            if (_entityCommandTimelines.TryGetValue(entityId, out var commandTimeline))
            {
                commandTimeline.RemoveCommand(tick);

                if (tick < EarliestCommandChange)
                {
                    EarliestCommandChange = tick;
                }
            }
        }

        public void InsertCommand(int tick, in TCommand command, EntityId entityId)
        {
            if (!_entityCommandTimelines.TryGetValue(entityId, out var commandTimeline))
            {
                commandTimeline = _commandTimelineFactory.CreateForEntity(entityId);
                _entityCommandTimelines.Add(entityId, commandTimeline);
            }

            commandTimeline.InsertCommand(tick, command);

            if (tick < EarliestCommandChange)
            {
                EarliestCommandChange = tick;
            }
        }
    }
}
