using System.Collections.Generic;

namespace UPR
{
    public class CommandTimeline<TCommand> : ICommandTimeline<TCommand>
    {
        private readonly ICommandRouter<TCommand> _commandRouter;

        private readonly Dictionary<int, List<EntityCommand>> _timeline = new Dictionary<int, List<EntityCommand>>();

        public CommandTimeline(ICommandRouter<TCommand> commandRouter)
        {
            _commandRouter = commandRouter;
        }

        public int EarliestCommandChange { get; private set; }

        public void ApproveChangesUpTo(int tick)
        {
            EarliestCommandChange = tick;
        }

        public void ExecuteCommands(int tick)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                foreach (var command in commands)
                {
                    _commandRouter.ForwardCommand(command.Command, command.Entity);
                }
            }
        }

        public void RemoveAllCommandsDownTo(int tick)
        {
            foreach (var tickCommandPair in _timeline)
            {
                if (tickCommandPair.Key > tick)
                {
                    tickCommandPair.Value.Clear();
                }
            }

            if (tick + 1 < EarliestCommandChange)
            {
                EarliestCommandChange = tick + 1;
            }
        }

        public void RemoveAllCommandsAt(int tick)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                commands.Clear();

                if (tick < EarliestCommandChange)
                {
                    EarliestCommandChange = tick;
                }
            }
        }

        public void RemoveCommand(int tick, EntityId entityId)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                commands.RemoveAll(command => command.Entity == entityId);

                if (tick < EarliestCommandChange)
                {
                    EarliestCommandChange = tick;
                }
            }
        }

        public void InsertCommand(int tick, in TCommand command, EntityId entityId)
        {
            if (_timeline.TryGetValue(tick, out var commands))
            {
                commands.Add(new EntityCommand(command, entityId));
            }
            else
            {
                _timeline.Add(tick, new List<EntityCommand>()
                {
                    new EntityCommand(command, entityId)
                });
            }

            if (tick < EarliestCommandChange)
            {
                EarliestCommandChange = tick;
            }
        }

        private readonly struct EntityCommand
        {
            public EntityCommand(TCommand command, EntityId entity)
            {
                Command = command;
                Entity = entity;
            }

            public TCommand Command { get; }
            public EntityId Entity { get; }
        }
    }
}
