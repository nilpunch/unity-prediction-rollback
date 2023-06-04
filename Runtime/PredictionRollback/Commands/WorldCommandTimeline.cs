using System.Collections.Generic;

namespace UPR
{
    public class WorldCommandTimeline<TCommand> : IWorldCommandTimeline<TCommand>
    {
        private readonly IEntityCommandTimelineFactory<TCommand> _entityCommandTimelineFactory;
        private readonly Dictionary<EntityId, IEntityCommandTimeline<TCommand>> _entityCommandTimelines = new Dictionary<EntityId, IEntityCommandTimeline<TCommand>>();

        public WorldCommandTimeline(IEntityCommandTimelineFactory<TCommand> entityCommandTimelineFactory)
        {
            _entityCommandTimelineFactory = entityCommandTimelineFactory;
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

        public void InsertCommand(int tick, in TCommand command, EntityId entityId)
        {
            if (!_entityCommandTimelines.TryGetValue(entityId, out var commandTimeline))
            {
                commandTimeline = _entityCommandTimelineFactory.CreateForEntity(entityId);
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
